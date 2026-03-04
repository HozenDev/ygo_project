using Godot;
using System;

public partial class NPC : Character<CharacterData>, IInteractable
{
	[Export] public CharacterData NPCResource {
		get => Data; 
		set => Data = value; 
	}
	
	public virtual void Interact(Player player)
	{
		GD.Print($"Bonjour {player.Data.Name}, je suis {Data.Name} !");
	}
}
