using Godot;
using System;

public partial class CombatManager : Node2D
{	
	public enum CombatState { START, CHOICE_TURN, RESOLVE, WON, LOST }
	private CombatState m_currentState;
	
	[Export] public Node2D BattlePlayerSide; 	// Player visualization
	[Export] public Node2D BattleEnemySide; 	// Enemy visualization
	[Export] public Sprite2D _background;		// Battle background
	[Export] public BattleTheme _theme;			// Battle Theme
	[Export] public BattleUIManager _battleUI;

	// Players data
	public IDuellist m_playerData;
	public IDuellist m_enemyData;
	
	public const int PlayerDistFromOriginX = 260; // In pixel
	public const int PlayerDistFromOriginY = 80; // In pixel
	public const int MonsterDistFromOriginX = 100; // In pixel
	public const int MonsterDistFromOriginY = 110; // In pixel
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_currentState = CombatState.START;
		_battleUI.ActionForwarded += OnActionSelected;
	}
	
	private void SetActiveMonster(Monster monster, bool isEnemy) 
	{
		Node2D BattleSide = (isEnemy) ? BattleEnemySide : BattlePlayerSide;
		Sprite2D ActiveMonsterSprite = BattleSide.GetNode<Sprite2D>("ActiveMonster");
		ActiveMonsterSprite.Texture = monster.Data.BattleSprite;
		
		int XPosition = ((isEnemy) ? 1 : -1 ) * MonsterDistFromOriginX;
		int YPosition = MonsterDistFromOriginY;
		ActiveMonsterSprite.Position = new Vector2(XPosition, YPosition);
		ActiveMonsterSprite.FlipH = isEnemy;
		
		// Make it on the same line even if sprite sizes are different.
		ActiveMonsterSprite.Offset = new Vector2(0, -ActiveMonsterSprite.Texture.GetSize().Y / 2);
	}
	
	private void SetPlayer(IDuellist duellist, bool isEnemy) 
	{
		Node2D BattleSide = null;
		
		if (isEnemy) {
			m_enemyData = duellist;
			BattleSide = BattleEnemySide;
		}
		else {
			m_playerData = duellist;
			BattleSide = BattlePlayerSide;
		}
		
		Sprite2D PlayerSprite = BattleSide.GetNode<Sprite2D>("Player");
		PlayerSprite.Texture = duellist.GetDuelData().BattleSprite;
		PlayerSprite.FlipH = isEnemy;
		
		int XPosition = ((isEnemy) ? 1 : -1 ) * PlayerDistFromOriginX;
		int YPosition = PlayerDistFromOriginY;
		PlayerSprite.Position = new Vector2(XPosition, YPosition);
	}
	
	public void SetMainPlayer(IDuellist duellist) => SetPlayer(duellist, false);
	public void SetEnemyPlayer(IDuellist duellist) => SetPlayer(duellist, true);
	public void SetPlayerActiveMonster(Monster monster) => SetActiveMonster(monster, false);
	public void SetEnemyActiveMonster(Monster monster) => SetActiveMonster(monster, true);
	
	private void MonsterAttack(Monster attacker, Monster receiver)
	{
		GD.Print($"'{attacker.Nickname}' is attacking '{receiver.Nickname}'!");
		receiver.CurrentLife -= attacker.Attack;
		UpdateUI();
	}
	
	private void OpenBag() {
		GD.Print("Opening the bag...");
		// TODO: Bag manager
	}
	
	private void Quit() {
		SceneLoader.Instance.LoadScene(SceneLoader._worldScene);
	}
	
	private void OnActionSelected(ActionType action)
	{
		switch (action)
		{
			case ActionType.ATTACK:
				MonsterAttack(m_playerData.GetFirstValidMonster(), m_enemyData.GetFirstValidMonster());
				break;
			case ActionType.BAG:
				OpenBag();
				break;
			case ActionType.QUIT:
				Quit();
				break;
		}
		
		CheckEnd();
	}
	
	private void SetTheme(ThemeType theme) {
		try {
			_theme = ThemeRegistry.Get(theme);
		}
		catch (Exception e) {
			GD.PushError("Error when loading theme:\n", e);
		}
	}
	
	private void InitMonsters() {
		foreach(var monster in m_playerData.GetDuelData().Party)
		{
			monster.Init();
		}
		
		foreach(var monster in m_enemyData.GetDuelData().Party)
		{
			monster.Init();
		}
	}
	
	public void Initialize(IDuellist playerData, IDuellist enemyData, ThemeType theme) {
		
		// Initiliaze theme
		SetTheme(theme);
		
		// Initialize Players
		SetMainPlayer(playerData);
		SetEnemyPlayer(enemyData);
		
		// Initialize Monsters
		InitMonsters();
		
		// Initialize Active Monsters
		SetPlayerActiveMonster(m_playerData.GetActiveMonster());
		SetEnemyActiveMonster(m_enemyData.GetActiveMonster());
		
		UpdateUI();
	}
	
	public void UpdateUI() {
		_battleUI.UpdateUI(m_playerData, m_enemyData, _theme);
	}
	
	public void CheckEnd()
	{
		bool win = false;
		bool lose = false;
		
		if (!m_enemyData.CanFight())
		{
			GD.Print("You win!");
			win = true;
			// Win animation
		}
		else if (!m_playerData.CanFight()) 
		{
			GD.Print("You lose!");
			lose = true;
			// Lose animation
		}
		
		bool end = win || lose;
		
		if (end)
		{
			// End animation
			SceneLoader.Instance.LoadScene(SceneLoader._worldScene);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
