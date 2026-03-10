using Godot;
using System;

public partial class BattleManager : Node2D
{	
	// UI Management
	[Export] public BattleTheme _theme;			
	[Export] public BattleUIManager _battleUI;

	// Duellists data
	public IDuellist _playerData;
	public IDuellist _opponentData;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		_battleUI.ActionForwarded += OnActionSelected;
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {}
	
	// ---------------- Battle Actions ----------------- //
	
	private void SetSpecialCard(SpecialCard card, int index, bool isPlayer) 
	{
		// todo: handle set a special card on the field
		// link the duellist special card list and the field
		if (!isPlayer) {
			_battleUI.SetOpponentSpecialCard(card, index);
		}
		else {
			_battleUI.SetPlayerSpecialCard(card, index);
		}
	}
	
	private void ActivateSpecialCard(int index, bool isPlayer)
	{
		if (!isPlayer) {
			_battleUI.ActivateOpponentSpecialCard(index);
		}
		else {
			_battleUI.ActivatePlayerSpecialCard(index);
		}
	}
	
	private void SetMonster(MonsterCard monster, bool isPlayer)
	{
		// todo: handle changing monster
	}
	
	private void MonsterAttack(MonsterCard attacker, MonsterCard receiver) {
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
	
	private void OnActionSelected(ActionType action) {
		switch (action)
		{
			case ActionType.ATTACK:
				MonsterAttack(_playerData.GetFirstValidMonster(), _opponentData.GetFirstValidMonster());
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
	
	// ---------------- Initialiations ----------------- //
	
	private void SetTheme(ThemeType theme) {
		try {
			_theme = ThemeRegistry.Get(theme);
		}
		catch (Exception e) {
			GD.PushError("Error when loading theme:\n", e);
		}
	}
	
	private void InitMonsters() {
		foreach(var monster in _playerData.GetDuelData().Party)
		{
			monster.Init();
		}
		
		foreach(var monster in _opponentData.GetDuelData().Party)
		{
			monster.Init();
		}
	}
	
	private void SetMainPlayer(IDuellist duellist) => _playerData = duellist;
	private void SetEnemyPlayer(IDuellist duellist) => _opponentData = duellist;
	
	public void Initialize(IDuellist playerData, IDuellist enemyData, ThemeType theme) {
		
		// Initiliaze theme
		SetTheme(theme);
		
		// Hack: dump player and enemy monster
		PackedScene MonsterCardScene = GD.Load<PackedScene>("uid://c1qrdkfq4jgoj") as PackedScene;
		MonsterData celticGuardian = GD.Load<MonsterData>("res://resources/monsters/1.tres");
		MonsterData dragonBlue = GD.Load<MonsterData>("res://resources/monsters/0.tres");
		
		playerData.GetDuelData().Party.Clear();
		MonsterCard mcardp1 = MonsterCardScene.Instantiate<MonsterCard>();
		mcardp1.SetData(celticGuardian);
		mcardp1.Init();
		playerData.GetDuelData().Party.Add(mcardp1);
		
		enemyData.GetDuelData().Party.Clear();
		MonsterCard mcardp2 = MonsterCardScene.Instantiate<MonsterCard>();
		mcardp2.SetData(dragonBlue);
		mcardp2.Init();
		enemyData.GetDuelData().Party.Add(mcardp2);
		
		// GD.Print(playerData.GetDuelData().Party[0].Nickname);
		// GD.Print(enemyData.GetDuelData().Party[0].Nickname);
		
		// Initialize Players
		SetMainPlayer(playerData);
		SetEnemyPlayer(enemyData);
		
		// Initialize Monsters
		InitMonsters();

		// Init and update UI
		InitUI();
		
		UpdateUI();
	}
	
	// ---------------- UI Handle ----------------- //
	
	public void InitUI() {
		_battleUI.Init(_playerData, _opponentData, _theme);
	}
	
	public void UpdateUI() {
		_battleUI.UpdateUI(_playerData, _opponentData);
	}
	
	// ---------------- Battle End Handle ----------------- //
	
	public void CheckEnd()
	{
		bool win = false;
		bool lose = false;
		
		if (!_opponentData.CanFight())
		{
			GD.Print("You win!");
			win = true;
			// Win animation
		}
		else if (!_playerData.CanFight()) 
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
}
