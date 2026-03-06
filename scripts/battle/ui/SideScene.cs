using Godot;
using System;

public partial class SideScene : MarginContainer
{
	[Export] public Sprite2D _duellistSprite;
	[Export] public Sprite2D _cardSprite1;
	[Export] public Sprite2D _cardSprite2;
	[Export] public Sprite2D _monsterSprite;
	
	public void CenterMonster() {
		_monsterSprite.Offset = new Vector2(0, -_monsterSprite.Texture.GetSize().Y / 2);
	}
	
	private void ActivateCard(Sprite2D cardSprite, int cardType)
	{
		// todo: put the activate sprite to the card
		// cardSprite.Texture = ...
	}
	
	private void DropCard(Sprite2D cardSprite)
	{
		// todo: drop card texture from the field
		// cardSprite.Texture = ...
	}
	
	private void SetCard(Sprite2D cardSprite) {
		// todo: update card texture to be setted
		// cardSprite.Texture = ...
	}
	
	public void SetCard1() => SetCard(_cardSprite1);
	public void SetCard2() => SetCard(_cardSprite2);
	public void DropCard1() => DropCard(_cardSprite2);
	public void DropCard2() => DropCard(_cardSprite2);
	
	public void UpdateUI(IDuellist duellist) {
		_duellistSprite.Texture = duellist.GetDuelData().BattleSprite;
		_monsterSprite.Texture = duellist.GetActiveMonster().Data.BattleSprite;
		CenterMonster();
		// Todo: manage card played and posed by the duellist;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
