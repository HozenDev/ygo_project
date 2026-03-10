using Godot;
using System;

public enum MonsterAttribute {
	Dark,
	Divine,
	Earth,
	Fire,
	Light,
	Water,
	Wind
}

public enum MonsterType {
	Aqua,
	Beast,
	BeastWarrior,
	CreatorGod,
	Cyberse,
	Dinosaur,
	DivineBeast,
	Dragon,
	Fairy,
	Fiend,
	Fish,
	Insect,
	Illusion,
	Machine,
	Plant,
	Psychic,
	Pyro,
	Reptile,
	Rock,
	SeaSerpent,
	Spellcaster,
	Thunder,
	Warrior,
	WingedBeast,
	Wyrm,
	Zombie
}

[GlobalClass]
public partial class MonsterData : CardData
{
	// Information
	[Export] public Texture2D BattleSprite { get; set; }
	[Export] public MonsterAttribute Attribute { get; set; }
	[Export] public MonsterType Type { get; set; }
	
	// Base Stats
	[Export] public int AttackBase { get; set; }
	[Export] public int DefenseBase { get; set; }
	[Export] public int LifeBase { get; set; }
	[Export] public int SpeedBase { get; set; }
}
