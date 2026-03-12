using Godot;
using System;

public partial class Hand : MarginContainer
{
	[Export] private Curve _handCurve;
	[Export] private Curve _cardRotations;
	
	[Export] private float _cardW = 64;
	
	[Export] private float _maxRotationDegree;
	[Export] private float _cardSep;
	[Export] private float _yCardMin;
	[Export] private float _yCardMax;
	
	private SpecialCard _currentCardTop;
	
	
	private System.Collections.Generic.List<SpecialCard> _cards = new();
	
	private System.Collections.Generic.SortedList<int, SpecialCard> _overlapping = new();
	
	public override void _Ready() {
		ForceUpdateTransform();
		UpdateCards();
	}
	
	public void PrintCards() {
		foreach(var card in _cards) {
			GD.Print(card.GetData().Name);
		}
	}
	
	public void Add(SpecialCard card) {
		
		// Connect signals
		card.Hovered += OnCardHovered;
		card.Unhovered += OnCardUnhovered;
		card.ZIndex = _cards.Count;
		_cards.Add(card);
		AddChild(card);
		UpdateCards();
		// PrintCards();
	}
	
	public void Remove(SpecialCard card) {
		// if (GetChildCount() < 1) return;
		_cards.Remove(card);
		UpdateCards();
	}
	
	public void UpdateCards() {
		int nbCards = _cards.Count; 
		float AllCardsSize = (_cardW * nbCards) + (_cardSep * (nbCards - 1));
		float finalCardSep = _cardSep;
		
		if (AllCardsSize > this.Size.X) {
			finalCardSep = (this.Size.X - _cardW * nbCards) / (nbCards - 1);
			AllCardsSize = this.Size.X;
		}
		
		float offset = (Size.X - AllCardsSize + _cardW) / 2;
		
		for (int i=0; i < nbCards; i++) {
			SpecialCard card = _cards[i];
			float yMultiplier = _handCurve.Sample(1.0f / (nbCards-1) * i);
			float rotMultiplier = _cardRotations.Sample(1.0f / (nbCards-1) * i);
			
			if (nbCards == 1) {
				yMultiplier = 0.0f;
				rotMultiplier = 0.0f;
			}
			
			float finalX = offset + _cardW * i + finalCardSep * i;
			float finalY = _yCardMin + _yCardMax * yMultiplier;
			
			card.Position = new Vector2(finalX, finalY);
			card.RotationDegrees = _maxRotationDegree * rotMultiplier;
		}
		
		//int marginValue = (int) (nbCards * _cardW);
		//GD.Print(marginValue);
		//AddThemeConstantOverride("margin_right", marginValue);
	}
	
	// ---------- Signals ------------ //

	public void OnCardHovered(SpecialCard card)
	{
		if (_currentCardTop != null && _currentCardTop._isDragging) return;
		
		if (card.IsHidden()) return;
		
		if (!_overlapping.ContainsKey(card.ZIndex))
		{
			_overlapping.Add(card.ZIndex, card);
		}
		UpdateTopCard();
	}

	public void OnCardUnhovered(SpecialCard card)
	{
		// Test if we are not dragging the current top card
		if (_currentCardTop != null && _currentCardTop._isDragging) return;
		
		// Test if the card is hidden (opponent card)
		if (card.IsHidden()) return;
		
		int index = _overlapping.IndexOfValue(card);
		if (index != -1)
		{
			_overlapping.RemoveAt(index);
			card.Unhover();
			
			if (_currentCardTop == card)
			{
				_currentCardTop = null;
			}

			UpdateTopCard();
		}
	}

	private void UpdateTopCard()
	{
		if (_overlapping.Count == 0)
		{
			_currentCardTop = null;
			UICardInfo.Instance.HideTooltip();
			return;
		}

		SpecialCard newTop = _overlapping.Values[_overlapping.Count - 1];

		if (newTop != _currentCardTop)
		{
			_currentCardTop?.Unhover();
			_currentCardTop = newTop;
			_currentCardTop.Hover();
			UICardInfo.Instance.ShowTooltip(_currentCardTop.GetData());
		}
	}
	
	// -------------- Dragging -------------- //

	public override void _Input(InputEvent @event)
	{
		if (_currentCardTop == null || _currentCardTop.IsSetted()) return;
		
		// Grad the card
		if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Left)
		{
			if (mouseEvent.Pressed && _currentCardTop != null)
			{
				_currentCardTop.StartDragging();
			}
			else if (!mouseEvent.Pressed && _currentCardTop._isDragging)
			{
				try {
					_currentCardTop.StopDragging();
					Remove(_currentCardTop);
				}
				catch (Exception e) {
					// Can't drag card into an emplacement
					GD.PushError(e);
				}
				finally {
					OnCardUnhovered(_currentCardTop);
				}
			}
		}

		// Move the card
		if (@event is InputEventMouseMotion && _currentCardTop._isDragging)
		{
			_currentCardTop.GlobalPosition = GetGlobalMousePosition() - _currentCardTop._dragOffset;
		}
	}
}
