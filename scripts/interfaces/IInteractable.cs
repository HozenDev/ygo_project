using Godot;
using System;

public partial class IInteractable : Area2D
{
	public Callable Interact { get; set; }
}
