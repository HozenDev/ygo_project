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
	
	private PackedScene CardScene = GD.Load<PackedScene>("uid://bvcjerfb0hlfr") as PackedScene;
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
	
	public void Add(SpecialCardData cardData) {
		SpecialCard card = CardScene.Instantiate<SpecialCard>();
		card.SetData(cardData);
		
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
	
	private SpecialCard _currentCardTop;

	public void OnCardHovered(SpecialCard card)
	{
		if (!_overlapping.ContainsKey(card.ZIndex))
		{
			_overlapping.Add(card.ZIndex, card);
		}
		UpdateTopCard();
	}

	public void OnCardUnhovered(SpecialCard card)
	{
		int index = _overlapping.IndexOfValue(card);
		if (index != -1)
		{
			_overlapping.RemoveAt(index);
			card.Unhover();
			
			// Si on quitte la carte qui était au sommet, on l'oublie
			if (_currentCardTop == card)
			{
				_currentCardTop = null;
			}

			UpdateTopCard();
		}
	}

	private void UpdateTopCard()
	{
		// CAS 1 : Plus aucune carte n'est survolée
		if (_overlapping.Count == 0)
		{
			_currentCardTop = null;
			UICardInfo.Instance.HideTooltip();
			return;
		}

		// Récupérer la carte avec le plus haut ZIndex (la dernière)
		SpecialCard newTop = _overlapping.Values[_overlapping.Count - 1];

		// CAS 2 : La carte prioritaire a changé
		if (newTop != _currentCardTop)
		{
			// 1. On remet l'ancienne carte en état normal
			_currentCardTop?.Unhover();
			
			// 2. On change la référence
			_currentCardTop = newTop;
			
			// 3. On active la nouvelle
			_currentCardTop.Hover();

			// 4. ON FORCE LE RE-SHOW (cela écrase l'ancien contenu du tooltip)
			UICardInfo.Instance.ShowTooltip(_currentCardTop.GetData());
		}
	}
}
