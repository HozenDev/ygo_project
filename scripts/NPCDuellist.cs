using Godot;
using System;

public partial class NPCDuellist : Character<DuellistData>, IDuellist, IInteractable
{
	[Export] public DuellistData NPCResource {
		get => Data; 
		set => Data = value; 
	}
	
	public virtual void Interact(Player player) {
		GD.Print($"Get ready {player.Data.Name}!");
		player.StartDuel(this);
	}
	
	public DuellistData GetDuelData() => Data;
	
	public void OnDuelStarted() {
		// Handle the start of the duel
	}
	
	public void OnDuelFinished(bool victory) {
		// Handle the end of the duel
		// Reset monster stats
	}
}
