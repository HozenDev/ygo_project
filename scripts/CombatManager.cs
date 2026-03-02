using Godot;
using System;

public partial class CombatManager : Node2D
{
	public static readonly PackedScene CombatScene = ResourceLoader.Load<PackedScene>("res://scenes/combat_scene.tscn");
	
	public enum CombatState { START, CHOICE_TURN, RESOLVE, WON, LOST }
	private CombatState m_currentState;
	
	[Export] public Node2D BattlePlayerSide; 	// Player visualization
	[Export] public Node2D BattleEnemySide; 	// Enemy visualization
	[Export] public Sprite2D _background;		// Battle background
	[Export] public BattleTheme _theme;			// Battle Theme
	
	// Players data
	public BattlePlayer m_playerData;
	public BattlePlayer m_enemyData;
	
	public const int PlayerDistFromOriginX = 260; // In pixel
	public const int PlayerDistFromOriginY = 80; // In pixel
	public const int MonsterDistFromOriginX = 100; // In pixel
	public const int MonsterDistFromOriginY = 110; // In pixel
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		m_currentState = CombatState.START;
	}
	
	private void SetActiveMonster(MonsterData monster, bool isEnemy) 
	{
		Node2D BattleSide = (isEnemy) ? BattleEnemySide : BattlePlayerSide;
		Sprite2D ActiveMonsterSprite = BattleSide.GetNode<Sprite2D>("ActiveMonster");
		ActiveMonsterSprite.Texture = ResourceLoader.Load<Texture2D>(monster.GetBattleSpritePath());
		
		int XPosition = ((isEnemy) ? 1 : -1 ) * MonsterDistFromOriginX;
		int YPosition = MonsterDistFromOriginY;
		ActiveMonsterSprite.Position = new Vector2(XPosition, YPosition);
		ActiveMonsterSprite.FlipH = isEnemy;
		
		// Make it on the same line even if sprite sizes are different.
		ActiveMonsterSprite.Offset = new Vector2(0, -ActiveMonsterSprite.Texture.GetSize().Y / 2);
	}
	
	private void SetPlayer(BattlePlayer player, bool isEnemy) 
	{
		Node2D BattleSide = null;
		
		if (isEnemy) {
			m_enemyData = player;
			BattleSide = BattleEnemySide;
		}
		else {
			m_playerData = player;
			BattleSide = BattlePlayerSide;
		}
		
		Sprite2D PlayerSprite = BattleSide.GetNode<Sprite2D>("Player");
		PlayerSprite.Texture = ResourceLoader.Load<Texture2D>(player.SpritePath);
		PlayerSprite.FlipH = isEnemy;
		
		int XPosition = ((isEnemy) ? 1 : -1 ) * PlayerDistFromOriginX;
		int YPosition = PlayerDistFromOriginY;
		PlayerSprite.Position = new Vector2(XPosition, YPosition);
	}
	
	public void SetTheme(ThemeType theme) 
	{
		try {
			_theme = ThemeRegistry.Get(theme);
			_background.Texture = _theme.BackgroundTexture;
		}
		catch (Exception e) {
			GD.PushError("Error in loading battle theme: ", e);
		}
	}
	
	public void SetMainPlayer(BattlePlayer player) => SetPlayer(player, false);
	public void SetEnemyPlayer(BattlePlayer player) => SetPlayer(player, true);
	public void SetPlayerActiveMonster(MonsterData monster) => SetActiveMonster(monster, false);
	public void SetEnemyActiveMonster(MonsterData monster) => SetActiveMonster(monster, true);
	
	
	public static CombatManager NewCombat(BattlePlayer playerData, BattlePlayer enemyData, ThemeType theme) {
		CombatManager _combat = CombatScene.Instantiate<CombatManager>();
		
		// Set theme
		_combat.SetTheme(theme);
		
		// Initialize Players
		_combat.SetMainPlayer(playerData);
		_combat.SetEnemyPlayer(enemyData);
		
		// Initialize Active Monsters
		_combat.SetPlayerActiveMonster(_combat.m_playerData.ActiveMonster);
		_combat.SetEnemyActiveMonster(_combat.m_enemyData.ActiveMonster);

		return _combat;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
