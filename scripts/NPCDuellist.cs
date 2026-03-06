using Godot;
using System;

public partial class NPCDuellist : Character<DuellistData>, IDuellist
{
	[Export] public DuellistData NPCResource {
		get => Data; 
		set => Data = value; 
	}
	[Export] public IInteractable InteractableZone;
	
	public override void _Ready() {
		base._Ready();
		InteractableZone.Interact = new Callable(this, MethodName.Interact);
	}
	
	public virtual void Interact(Player player) {
		
		if (DialogueManager.Instance.IsActive) {
			DialogueManager.Instance.AdvanceDialogue(player);
			player.StartDuel(this);
		}
		else {
			DialogueManager.Instance.StartDialogue(player, Data.Dialogue);
		}
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
