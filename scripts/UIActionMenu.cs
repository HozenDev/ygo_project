using Godot;
using System;

public enum ActionType {
	ATTACK,
	BAG,
	TEAM,
	QUIT
};

public partial class UIActionMenu : MarginContainer
{
	[Export] public Button ActionButton;
	[Export] public Button BagButton;
	[Export] public Button TeamButton;
	[Export] public Button QuitButton;
	
	[Signal] public delegate void ActionSelectedEventHandler(ActionType type);
	
	public void ActionPressed() {
		GD.Print("Action pressed");
		EmitSignal(SignalName.ActionSelected, Variant.From(ActionType.ATTACK));
	}
	
	public void BagPressed() {
		GD.Print("Bag pressed");
		EmitSignal(SignalName.ActionSelected, Variant.From(ActionType.BAG));
	}
	
	public void TeamPressed() {
		GD.Print("Team pressed");
		EmitSignal(SignalName.ActionSelected, Variant.From(ActionType.TEAM));
	}
	
	public void QuitPressed() {
		GD.Print("Quit pressed");
		EmitSignal(SignalName.ActionSelected, Variant.From(ActionType.QUIT));
	}
}
