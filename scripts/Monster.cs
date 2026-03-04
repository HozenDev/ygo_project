using Godot;
using System;

[GlobalClass]
public partial class Monster : Resource
{
	[Export] public MonsterData Data { get; set; }

	public int CurrentLife { get; set; }
	public int CurrentAttack { get; set; }
	public int CurrentDefense { get; set; }
	public int CurrentSpeed { get; set; }
	
	[Export] public int Level { get; set; }
	// [Export] public int Experience { get; set; }
	[Export] public string Nickname { get; set; }

	public int MaxLife => Data.LifeBase + (Level * 5);
	public int Attack => Data.DefenseBase + (Level * 3);
	public int Defense => Data.AttackBase + (Level * 3);
	public int Speed => Data.SpeedBase + (Level * 3);
	
	public void Init() {
		CurrentLife = MaxLife;
		CurrentAttack = Attack;
		CurrentDefense = Defense;
		CurrentSpeed = Speed;
		if (string.IsNullOrEmpty(Nickname)) {
			Nickname = Data.Name;
		}
	}
}
