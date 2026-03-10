using Godot;
using System;

public partial class UICardInfo : MarginContainer
{
	public static UICardInfo Instance { get; private set; }
	
	[Export] public NinePatchRect _cardSprite;
	[Export] public Label _cardName;
	[Export] public Label _cardDesc;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		Hide();
	}

	public void ShowTooltip(CardData data)
	{
		_cardName.Text = data.Name;
		_cardDesc.Text = data.Description;
		UpdateCardTexture(data.CardCoordinates);
		Show();
	}

	public void HideTooltip()
	{
		Hide();
	}
	
	private void UpdateCardTexture(Vector2 cardPosition) {
		Rect2 regionRect = _cardSprite.RegionRect;
		regionRect.Position = cardPosition;
		_cardSprite.RegionRect = regionRect;
	}
}
