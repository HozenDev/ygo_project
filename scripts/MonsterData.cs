using Godot;
using System;

public partial class MonsterData : Node
{
	private string m_name;
	private int m_attack;
	private string m_battleSpritePath;
	
	public MonsterData(string name, int attack, string battleSpritePath) {
		m_name = name;
		m_attack = attack;
		m_battleSpritePath = battleSpritePath;
	}
	
	public string GetBattleSpritePath() {
		return m_battleSpritePath;
	}
}
