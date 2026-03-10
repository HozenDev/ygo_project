using Godot;
using System;

public partial class BattleUIManager : Control
{
	[Export] public MonsterHUD _playerHUD;
	[Export] public MonsterHUD _enemyHUD;
	[Export] public BattleSideScene _playerSideScene;
	[Export] public BattleSideScene _enemySideScene;
	[Export] public UIActionMenu _actionMenu;
	[Export] public TextureRect _background;
	
	[Signal] public delegate void ActionForwardedEventHandler(ActionType action);
	
	// --------------- Actions --------------- //
	
	private void ActivateSpecialCard(bool isPlayer, int index)
	{
		var sideScene = (isPlayer) ? _playerSideScene : _enemySideScene;
		if (sideScene.GetSlot(index).GetCard().GetState() == SpecialCardState.Setted) {
			sideScene.ActivateCard(index);
		}
		else {
			GD.PushError("Cannot activate the card: No card here.");
		}
	}
		
	private void SetSpecialCard(SpecialCard card, bool isPlayer, int index) {
		var sideScene = (isPlayer) ? _playerSideScene : _enemySideScene;
		if (sideScene.GetSlot(index).GetCard().GetState() == SpecialCardState.None) {
			sideScene.SetCard(card, index);
		}
		else {
			GD.PushError("The special card place is already taken.");
		}
	}
	
	public void SetPlayerSpecialCard(SpecialCard card, int index) 
		=> SetSpecialCard(card, true, index);
	public void SetOpponentSpecialCard(SpecialCard card, int index) 
		=> SetSpecialCard(card, false, index);
	public void ActivatePlayerSpecialCard(int index)
		=> ActivateSpecialCard(true, index);
	public void ActivateOpponentSpecialCard(int index)
		=> ActivateSpecialCard(false, index);
	
	// --------------- Initialization --------------- //
	
	private void SetTheme(BattleTheme theme) {
		try {
			_background.Texture = theme.BackgroundTexture;
		}
		catch (Exception e) {
			GD.PushError("Error when loading theme background texture:\n", e);
		}
	}
	
	private void UpdatePlayerUI(IDuellist duellist) {
		_playerHUD.UpdateUI(duellist.GetActiveMonster());
		_playerSideScene.UpdateUI(duellist);
	}
	
	private void UpdateEnemyUI(IDuellist duellist) {
		_enemyHUD.UpdateUI(duellist.GetActiveMonster());
		_enemySideScene.UpdateUI(duellist);
	}
	
	public void UpdateUI(IDuellist player, IDuellist enemy) {
		UpdatePlayerUI(player);
		UpdateEnemyUI(enemy);
	}
	
	public void Init(IDuellist player, IDuellist enemy, BattleTheme theme) {
		SetTheme(theme);
		_playerSideScene.Init(player, BattleSideSceneType.Player);
		_enemySideScene.Init(enemy, BattleSideSceneType.Opponent);
	}
		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		_actionMenu.ActionSelected += (action) => EmitSignal(SignalName.ActionForwarded, Variant.From(action));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
	}
}
