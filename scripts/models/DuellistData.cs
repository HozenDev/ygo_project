using Godot;
using System;

[GlobalClass]
public partial class DuellistData : CharacterData 
{
	public Godot.Collections.Array<MonsterCard> Party { get; set; } = new();
	[Export] public Texture2D BattleSprite { get; set; }
	
	// Todo: duel dialogues
	
	// Todo: special card hands
	
	// Todo: special card deck
	[Export] public Godot.Collections.Array<SpecialCardData> Deck { get; set; } = new();
}
