using Godot;
using System;

public enum SpecialType { Magic, Trap }

[GlobalClass]
public partial class SpecialCardData : CardData
{
	[Export] public SpecialType Type { get; set; }
}
