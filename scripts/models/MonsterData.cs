using Godot;
using System;

public partial class MonsterData : Node
{
	public string MonsterName { get; set; }
	public int Attack { get; set; }
	public int Level { get; set; }
	public int MaxHp { get; set; }
	public int CurrentHp { get; set; }
	public string BattleSpritePath { get; set; }
	public Vector2 CardCoordinates { get; set; }
}
