using Godot;
using System;

public partial class enemy : CharacterBody3D
{
	private globals _globals;
	public const float Speed = 5.0f;
	[Export] public player Player;
	private Vector3 _directionToPlayer;

	public override void _Ready()
	{
		_globals = GetNode<globals>("/root/Globals");
		_globals.Connect(nameof(globals.PlayerAddedEventHandler), new Callable(this, nameof(FindPlayer)));
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		// Calculate direction towards the player
		_directionToPlayer = (Player.GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
		
		//float XChange = Mathf.Lerp(Position.X, _playerPosition.X);
		//float ZChange = Mathf.Lerp(Position.Z, _playerPosition.Z);

		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= Gravity * (float)delta;

		velocity.X = _directionToPlayer.X * Speed;
		velocity.Z = _directionToPlayer.Z * Speed;

		Velocity = velocity;
		MoveAndSlide();
	}

	private void FindPlayer(string scenePath)
	{
		Player = GetNode<player>(scenePath);
	}

	public player GetPlayer()
	{
		return Player;
	}
}
