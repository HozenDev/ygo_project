using Godot;
using System;

public partial class NPC : Character<CharacterData>
{
	[Export] public CharacterData NPCResource {
		get => Data; 
		set => Data = value; 
	}
	[Export] public IInteractable InteractableZone;
	
	public override void _Ready() {
		base._Ready();
		InteractableZone.Interact = new Callable(this, MethodName.Interact);
	}
	
	public virtual void Interact(Player player)
	{
		FacePlayer(player);
		
		//GD.Print($"Bonjour {player.Data.Name}, je suis {Data.Name} !");
		if (DialogueManager.Instance.IsActive) {
			DialogueManager.Instance.AdvanceDialogue(player);
		}
		else {
			DialogueManager.Instance.StartDialogue(player, Data.Dialogue);
		}
	}
}
