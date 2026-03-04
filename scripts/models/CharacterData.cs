using Godot;
using System;

[GlobalClass]
public partial class CharacterData : Resource
{
	[Export] public string Name { get; set; }
	[Export] public int Id { get; set; }
	[Export] public float Speed { get; set; }
	[Export] public Texture2D Sprite;
	
	[Export(PropertyHint.MultilineText)] public string Dialogue { get; set; }
}
