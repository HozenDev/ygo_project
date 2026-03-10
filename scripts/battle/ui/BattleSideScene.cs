using Godot;
using System;

public enum BattleSideSceneType { 
	Player,
	Opponent
}

public partial class BattleSideScene : MarginContainer
{
	// Duesllist
	[Export] public Sprite2D _duellistSprite;
	
	// Monster
	[Export] public MonsterCardSlot _monsterCardSlot;

	// Hand
	[Export] public Hand _hand;
	
	// --- Info
	private BattleSideSceneType _side;
	
	// --- Special Cards
	[Export] public VBoxContainer _specialCardSlotsContainer;
	public const int MAX_NB_SPECIAL_CARDS = 2;
	public PackedScene SpecialCardSlotScene = GD.Load<PackedScene>("uid://0ubfetw3ig3g") as PackedScene;
	private Godot.Collections.Array<SpecialCardSlot> _specialCardSlots = new();
	
	// --- Deck management
	public const int INIT_NB_HAND_CARDS = 3;
	// public const int MAX_NB_HAND_CARDS = 5;
	private Godot.Collections.Array<SpecialCardData> _drawPile = new();
	
	// ------------------ Deck ------------------ //
	
	// [Signal] public delegate void CardDrawnEventHandler(SpecialCardData card);

	public void InitializeDuel(IDuellist duellist)
	{
		SetActiveMonsterCard(duellist.GetActiveMonster());
		// Copy deck when start duel
		_drawPile = new Godot.Collections.Array<SpecialCardData>(duellist.GetDuelData().Deck);
		ShuffleDeck();
		GD.Print(_drawPile);
		for (int i = 0; i < INIT_NB_HAND_CARDS; i++) {
			DrawCard();
		}
	}
	
	public void DrawCard()
	{
		if (_drawPile.Count > 0)
		{
			// Draw the card (only data)
			SpecialCardData drawnCardData = _drawPile[0];
			_drawPile.RemoveAt(0);
			
			// Instanciate the card
			_hand.Add(drawnCardData);
			
			// Signal
			// EmitSignal(SignalName.CardDrawn, drawnCardData);
		}
		else
		{
			GD.Print("No card in deck anymore.");
		}
	}
	
	private void ShuffleDeck()
	{
		GD.Randomize();
		_drawPile.Shuffle();
	}
	
	// ------------------ Card Actions ------------------ //
	
	public void ActivateCard(int index) 
		=> _specialCardSlots[VerifyIndex(index)].ActivateCard();
	public void DropCard(int index) 
		=> _specialCardSlots[VerifyIndex(index)].DropCard();	
	public SpecialCardSlot GetSlot(int index)
		=> _specialCardSlots[VerifyIndex(index)];
	
	private int VerifyIndex(int index) {
		if (index < 0 || index > MAX_NB_SPECIAL_CARDS ) {
			throw new IndexOutOfRangeException ($"Index = {index} need to be between 0 and {MAX_NB_SPECIAL_CARDS}");
		}
		return index;
	}
	
	public void DropCards() {
		for (int i = 0; i < MAX_NB_SPECIAL_CARDS; i++) {
			DropCard(i);
		}
	}
	
	public void SetCard(SpecialCard cardData, int index) {
		SpecialCardSlot slot = _specialCardSlots[VerifyIndex(index)];
		slot.SetCard(cardData);
		if (_side == BattleSideSceneType.Opponent) {
			slot.GetCard().SetVisibility(false);
		}
	}
	
	public void SetActiveMonsterCard(MonsterCard monster) 
	{
		_monsterCardSlot.SetCard(monster);
	}
	
	// ------------------ Update UI ------------------ //
	
	public void UpdateUI(IDuellist duellist) {
		// SetActiveMonsterCard(duellist.GetActiveMonster());
	}
	
	public void Init(IDuellist duellist, BattleSideSceneType side) {
		_duellistSprite.Texture = duellist.GetDuelData().BattleSprite;
		_side = side;
		for (int i = 0; i < MAX_NB_SPECIAL_CARDS; i++) {
			SpecialCardSlot slot = SpecialCardSlotScene.Instantiate<SpecialCardSlot>();
			_specialCardSlots.Add(slot);
			
			_specialCardSlotsContainer.AddChild(slot);

			if (side == BattleSideSceneType.Opponent) {
				slot.Flip();
			}
			slot.Init(i);
		}
		
		if (side == BattleSideSceneType.Opponent) {
			_monsterCardSlot.Flip();
		}
		
		InitializeDuel(duellist);
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
