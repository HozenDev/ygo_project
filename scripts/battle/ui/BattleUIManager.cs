using Godot;
using System;

public partial class BattleUIManager : Control
{
	[Export] public MonsterHUD _playerHUD;
	[Export] public MonsterHUD _enemyHUD;
	[Export] public UIActionMenu _actionMenu;
	[Export] public TextureRect _background;
	
	[Signal] public delegate void ActionForwardedEventHandler(ActionType action);
	
	public void SetTheme(BattleTheme theme)
	{
		try {
			_background.Texture = theme.BackgroundTexture;
		}
		catch (Exception e) {
			GD.PushError("Error when loading theme background texture:\n", e);
		}
	}
	
	private void UpdatePlayerUI(BattlePlayer player) {
		_playerHUD.UpdateUI(player.ActiveMonster);
	}
	
	private void UpdateEnemyUI(BattlePlayer player) {
		_enemyHUD.UpdateUI(player.ActiveMonster);
	}
	
	public void UpdateUI(BattlePlayer player, BattlePlayer enemy, BattleTheme theme) {
		SetTheme(theme);
		UpdatePlayerUI(player);
		UpdateEnemyUI(enemy);
	}
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_actionMenu.ActionSelected += (action) => EmitSignal(SignalName.ActionForwarded, Variant.From(action));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
