using Godot;
using System;

public partial class DialogueManager : CanvasLayer
{
	public static DialogueManager Instance { get; private set; }
	
	[Export] private Control DialogueBox;
	[Export] private Label DialogueText;
	
	public bool IsActive { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {	
		DialogueBox.Visible = false;
		IsActive = false;
		Instance = this;
	}
	
	public void StartDialogue(Player player, string dialogue) {
		IsActive = true;
		DialogueBox.Visible = true;
		DialogueText.Text = dialogue;
		player.CanMove = false;    
	}
	
	public void AdvanceDialogue(Player player) {
		IsActive = false;
		DialogueBox.Visible = false;
		player.CanMove = true;
	}
}
