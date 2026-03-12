using Godot;
using System;

public partial class SpecialCardSlot : CardSlot<SpecialCard, SpecialCardData>
{
	[Export] private Texture2D _emptyTexture; // ygo_battle_card_empty.png
	[Export] private Texture2D _cardBackTexture; // ygo_battle_card.png
	
	public const int CARD_SLOT_OFFSET = 16;

	public override void _Ready()
	{
		base._Ready();
		RefreshVisual();
		UpdateCollision();
	}
	
	public void Init(int index) {
		int x = ((IsFlipped()) ? -1 : 1 ) * CARD_SLOT_OFFSET * index;
		int y = - CARD_SLOT_OFFSET * index;
		UpdateOffset(new Vector2(x, y));
	}
	
	public override void DropCard() 
	{
		base.DropCard();
		RefreshVisual();
	}

	public override void SetCard(SpecialCard card)
	{
		base.SetCard(card);
		card.Set();
		var tween = card.CreateTween();
		tween.TweenProperty(card, "global_position", GlobalPosition, 0.1f);
		RefreshVisual();
	}

	public void ActivateCard()
	{
		if (!IsOccupied) {
			throw new Exception("There is no card to activate in this slot.");
		}
		
		GetCard().Activate();
		GetCard().Visible = true;
		GetSprite().Visible = false;
	}

	private void RefreshVisual()
	{
		if (!IsOccupied)
		{
			GetSprite().Texture = _emptyTexture;
			GetSprite().Visible = true;
		}
		else if (GetCard().GetState() == SpecialCardState.Setted)
		{
			GetSprite().Texture = _cardBackTexture;
			// On cache la carte pour montrer le dos via le slot
			GetCard().Visible = false; 
		}
		else if (GetCard().GetState() == SpecialCardState.Activated) {
			switch (GetCard().GetData().Type) {
				case SpecialType.Magic:
					// load magic activation sprite
					break;
				default:
					break;
			}
		}
		else {
			// do nothing
		}
	}
}
