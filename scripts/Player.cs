using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 60.0f;
	public Vector2 m_direction;
	
	private AnimationTree m_animationTree;
	private AnimationNodeStateMachinePlayback m_moveStateMachine;
	
	public override void _Ready() {
		m_animationTree = GetNode<AnimationTree>("Animation/AnimationTree");
		m_moveStateMachine = m_animationTree.Get("parameters/MoveStateMachine/playback").As<AnimationNodeStateMachinePlayback>();
		//GD.Print(m_animationTree);
		//GD.Print(m_moveStateMachine);
	}
	
	private void Animate() {
		if (m_direction != Vector2.Zero) {
			m_moveStateMachine.Travel("Walk");
			Vector2 animation_direction = new Vector2(Mathf.Round(m_direction.X), Mathf.Round(m_direction.Y));
			m_animationTree.Set("parameters/MoveStateMachine/Walk/blend_position", animation_direction);
			m_animationTree.Set("parameters/MoveStateMachine/Idle/blend_position", animation_direction);
		}
		else {
			m_moveStateMachine.Travel("Idle");
		}
	}
	
	private void StartBattle() {
		// 1. Préparer les données du Joueur (souvent stockées dans un Global/Save)
		var playerBattleInfo = new BattlePlayer {
			Name = "Yugi",
			IsPlayer = true,
			ActiveMonster = new MonsterData("Player Monster", 20, "res://assets/sprites/battle_celtic_warrior.png"),
			SpritePath = "res://assets/sprites/battle_sprite_yugi.png",
		};

		// 2. Préparer les données de l'Adversaire (le PNJ)
		var enemyBattleInfo = new BattlePlayer {
			Name = "NPC",
			IsPlayer = false,
			ActiveMonster = new MonsterData("Enemy Monster", 20, "res://assets/sprites/battle_blue_eyes_dragon.png"), // Le monstre principal du PNJ
			SpritePath = "res://assets/sprites/battle_sprite_kaiba.png",
		};

		// 3. Charger et Injecter
		var combatInstance = CombatManager.NewCombat(playerBattleInfo, enemyBattleInfo, ThemeType.CITY);

		// 4. Switcher de scène proprement
		GetTree().Root.AddChild(combatInstance);
		GetTree().CurrentScene.QueueFree();
		GetTree().CurrentScene = combatInstance;
	}
	
	private void LaunchBattle() {
		bool launch = Input.IsActionJustPressed("launch_battle");
		if (launch) {
			GD.Print("launch");
			StartBattle();
		}
	}
	
	private void Move() {
		Vector2 velocity = Velocity;
		
		m_direction = Input.GetVector("left", "right", "up", "down");
		velocity = m_direction * Speed;
		
		Velocity = velocity;
		
		MoveAndSlide();
	}

	public override void _PhysicsProcess(double delta)
	{
		Move();
		Animate();
		LaunchBattle();
	}
}
