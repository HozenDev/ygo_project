using Godot;
using System.Collections.Generic;

public partial class BattlePlayer : Node
{
	// Infos du Dresseur
	public string PlayerName { get; set; }
	public bool IsPlayer { get; set; }

	// Le monstre actuellement sur le terrain
	public MonsterData ActiveMonster { get; set; }

	// La réserve (pour le système de 15 étoiles / Soul Power)
	public List<MonsterData> Bench { get; set; } = new List<MonsterData>();
	
	// Visual info
	public string SpritePath { get; set; }
}
