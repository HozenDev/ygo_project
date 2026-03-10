using Godot;
using System;

public partial class CardData : Resource
{
	[Export] public int Id { get; set; }
	// Card cost to be played
	[Export] public int Cost { get; set; }
	// Card name
	[Export] public string Name { get; set; }
	// Card description printed when hovered
	[Export(PropertyHint.MultilineText)] public string Description;
	// Coordinates in the card tileset
	[Export] public Vector2 CardCoordinates { get; set; }
	private Vector2 Size { get; set; } = new Vector2(60, 84);
}
