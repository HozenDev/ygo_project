using Godot;
using System;

public enum MonsterLocation {
	Stock,
	Party,
	Field
}

public enum MonsterStatus {
	None,
	KO,
	Paralysed,
	Asleep,
	Burned,
	Poisonned
}

public partial class MonsterCard : Card<MonsterData>
{
	// ---------- Monster Info ------------ //
	
	public int CurrentLife { get; set; }
	public int CurrentAttack { get; set; }
	public int CurrentDefense { get; set; }
	public int CurrentSpeed { get; set; }
	
	public int Level { get; set; }
	public int Experience { get; set; }
	public string Nickname { get; set; }

	// todo: create a curve stats (see tutorial)
	// Manage experience
	public int MaxLife { get; set; }
	public int Attack { get; set; }
	public int Defense { get; set; }
	public int Speed { get; set; }
	
	public void Init() {
		MaxLife = GetData().LifeBase + (Level * 5);
		Attack = GetData().DefenseBase + (Level * 3);
		Defense = GetData().AttackBase + (Level * 3);
		Speed = GetData().SpeedBase + (Level * 3);
		
		CurrentLife = MaxLife;
		CurrentAttack = Attack;
		CurrentDefense = Defense;
		CurrentSpeed = Speed;
		
		if (string.IsNullOrEmpty(Nickname)) {
			Nickname = GetData().Name;
		}
	}
}
