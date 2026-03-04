using Godot;
using System;

public partial class Player : Character<PlayerData>, IDuellist
{
	[Export] public PlayerData PlayerResource { 
		get => Data; 
		set => Data = value; 
	}
	[Export] public Area2D InteractionZone;
	
	public DuellistData GetDuelData() => Data;
	
	public void OnDuelStarted() {
		// Handle the start of the duel
	}
	
	public void OnDuelFinished(bool victory) {
		// Handle the end of the duel
		// Reset monster stats
	}
	
	public override void _Ready() {
		base._Ready();
	}
	
	public void StartDuel(IDuellist opponent)
	{
		SceneLoader.Instance.LoadScene(SceneLoader._combatScene, (node) => 
		{
			if (node is CombatManager combatInstance)
			{
				combatInstance.Initialize(this, opponent, ThemeType.CITY);
			}
		});
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("interact")) // Assure-toi de créer cette action dans Input Map
		{
			CheckInteraction();
		}
	}
	
	private void CheckInteraction()
	{
		var overlappingBodies = InteractionZone.GetOverlappingBodies();

		foreach (var body in overlappingBodies)
		{
			// On vérifie si le corps touché possède l'interface IInteractable
			if (body is IInteractable interactable)
			{
				interactable.Interact(this);
				break; // On interagit avec le premier objet trouvé
			}
		}
	}
	
	public override void UpdateDirection() {
		_direction = Input.GetVector("left", "right", "up", "down");
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}
}
