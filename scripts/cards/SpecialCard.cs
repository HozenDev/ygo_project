using Godot;
using System;

public enum SpecialCardState {
	None,
	InDeck,
	InHand,
	Setted,
	Activated
}

public partial class SpecialCard : Card<SpecialCardData>
{	
	// Card State
	private SpecialCardState _state;
	
	// Handle card hover in hand
	private Tween _tween;
	private Vector2 _originalPosition;
	private const float HOVER_OFFSET = 10f; // Hauteur de levée
	private const float HOVER_SCALE = 1.2f;  // Augmentation de taille
	[Signal] public delegate void HoveredEventHandler(SpecialCard card);
	[Signal] public delegate void UnhoveredEventHandler(SpecialCard card);
	
	// ------------ Main Actions -------------- //
	
	public void Activate() {
		if (GetData() == null) return; // No card to be activated
		
		// todo: handle activation effect
		_state = SpecialCardState.Activated;
				
		// Always make the card visible for info
		SetVisibility(true);
	}
	
	public override void Destroy() {
		_state = SpecialCardState.None;
		SetData(null);
	} 
	
	public void Set() {
		_state = SpecialCardState.Setted;
	}
	
	// ------------ Informations -------------- //
	
	public SpecialCardState GetState() => _state;
	
	// -------------- Signals ------------ //
	
	public void Hover() {
		// Save original position
		_originalPosition = Position;

		// Scale and up
		if (_tween != null && _tween.IsRunning()) _tween.Kill();
		_tween = CreateTween().SetParallel(true).SetTrans(Tween.TransitionType.Cubic);
		
		_tween.TweenProperty(this, "position:y", _originalPosition.Y - HOVER_OFFSET, 0.2f);
		_tween.TweenProperty(this, "scale", Vector2.One * HOVER_SCALE, 0.2f);
	}
	
	public void Unhover() {
		if (_tween != null && _tween.IsRunning()) _tween.Kill();
		_tween = CreateTween().SetParallel(true).SetTrans(Tween.TransitionType.Cubic);
		
		_tween.TweenProperty(this, "position:y", _originalPosition.Y, 0.2f);
		_tween.TweenProperty(this, "scale", Vector2.One, 0.2f);
	}
	
	public override void OnMouseEntered()
	{
		EmitSignal(SignalName.Hovered, this);
	}

	public override void OnMouseExited()
	{
		EmitSignal(SignalName.Unhovered, this);
	}
}
