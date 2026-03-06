using Godot;
using System;

[GlobalClass]
public partial class DuellistData : CharacterData 
{
	[Export] public Godot.Collections.Array<Monster> Party { get; set; } = new();
	[Export] public Texture2D BattleSprite { get; set; }
	
	// Todo: duel dialogues
}
