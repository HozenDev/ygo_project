using Godot;
using System;

[GlobalClass]
public partial class MonsterData : Resource
{
	// Information
	[Export] public int Id { get; set; }
	[Export] public string Name { get; set; }
	[Export] public Texture2D BattleSprite { get; set; }
	[Export] public Vector2 CardCoordinates { get; set; } // Coordinates in the card tileset
	
	// Base Stats
	[Export] public int AttackBase { get; set; }
	[Export] public int DefenseBase { get; set; }
	[Export] public int LifeBase { get; set; }
	[Export] public int SpeedBase { get; set; }
}
