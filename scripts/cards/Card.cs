using Godot;
using System;

public partial class Card<T> : Area2D where T: CardData
{
	// Collision Handle
	[Export] private Sprite2D _sprite;
	[Export] private CollisionShape2D _collision;
	
	private T _data;
	
	// Card State
	private bool _isHidden = false;
	public bool IsHidden() => _isHidden;
	public void SetVisibility(bool isVisible) => _isHidden = !isVisible;
	
	public override void _Ready()
	{
	}
	
	// ------------ Info -------------- //
	
	public virtual void SetData(T data)
	{
		_data = data;
		UpdateTexture();
	}
	
	public T GetData() => _data;
	
	private void UpdateTexture() {
		Rect2 currentRegion = _sprite.RegionRect;
		currentRegion.Position = _data.CardCoordinates;
		_sprite.RegionRect = currentRegion;
	}
	
	public virtual void Destroy() {
		// clean card
	}
	
	// ------------ Collision -------------- //
	
	// ------------ UI Handlers -------------- //
	
	public virtual void OnMouseEntered()
	{
		if (_data == null) {
			// GD.PushError("Card Data not set for this sprite");
			return;
		}
		
		if (!_isHidden) {
			UICardInfo.Instance.ShowTooltip(_data);
		}
	}

	public virtual void OnMouseExited()
	{
		if (!_isHidden) {
			UICardInfo.Instance.HideTooltip();
		}
	}
	
	public virtual void UpdateUI() {
		
	}
}
