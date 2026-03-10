using Godot;
using System;

public partial class Player : Character<PlayerData>, IDuellist
{
	[Export] public PlayerData PlayerResource { 
		get => Data; 
		set => Data = value; 
	}
	[Export] public RayCast2D _rayCast;
	
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
		SceneLoader.Instance.LoadScene(SceneLoader._battleScene, (node) => 
		{
			if (node is BattleManager battleInstance)
			{
				battleInstance.Initialize(this, opponent, ThemeType.NONE);
			}
		});
	}
	
	public override void _Input(InputEvent @event)
	{
		if (!@event.IsActionPressed("interact")) return;
		CheckInteraction();
	}
	
	private void CheckInteraction()
	{
		if (_rayCast.IsColliding())
		{
			var collider = _rayCast.GetCollider();

			if (collider is IInteractable interactable)
			{
				interactable.Interact.Call(this);
			}
			else {
				GD.Print(collider);
			}
		}
	}
	
	public override void UpdateDirection() {
		_direction = Input.GetVector("left", "right", "up", "down");
		
		if (_direction != Vector2.Zero) {
			_rayCast.TargetPosition = _direction.Normalized() * 20;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
	}
}
