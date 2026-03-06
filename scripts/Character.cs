using Godot;
using System;

public partial class Character<T> : CharacterBody2D where T : CharacterData
{
	public T Data { get; set; }
	
	public Vector2 _direction;
	public bool CanMove { get; set; } = true;
	
	[Export] public AnimationTree _animationTree;
	[Export] public Sprite2D _sprite;
	
	public AnimationNodeStateMachinePlayback _moveStateMachine;
	
	private void Animate() {
		try {
			if (CanMove && _direction != Vector2.Zero) {
				_moveStateMachine.Travel("Walk");
				Vector2 animation_direction = new Vector2(Mathf.Round(_direction.X), Mathf.Round(_direction.Y));
				_animationTree.Set("parameters/MoveStateMachine/Walk/blend_position", animation_direction);
				_animationTree.Set("parameters/MoveStateMachine/Idle/blend_position", animation_direction);
			}
			else {
				_moveStateMachine.Travel("Idle");
			}
		}
		catch (Exception e)
		{
			GD.PushError("No animation tree set for the character:\n", e);
		}
	}
	
	private void Move(Vector2 direction) {
		if (direction != Vector2.Zero) {
			Vector2 velocity = Velocity;
			velocity = direction * Data.Speed;
			Velocity = velocity;
			MoveAndSlide();
		}
	}
	
	public virtual void UpdateDirection() {
		// _direction = new Vector2(0, 0);
	}

	public override void _Ready() {
		_direction = new Vector2(0, 0);
		if (Data != null) _sprite.Texture = Data.Sprite;
		if (_animationTree != null) _moveStateMachine = _animationTree.Get("parameters/MoveStateMachine/playback").As<AnimationNodeStateMachinePlayback>();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (CanMove) {
			UpdateDirection();
			Move(_direction);
		}
		Animate();
	}
}
