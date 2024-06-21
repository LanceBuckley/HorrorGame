using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class enemy : CharacterBody3D
{
	private globals _globals;
	public const float Speed = 5.0f;
	[Export] public player Player;
	private Vector3 _directionToPlayer;
	private enemy_area _enemyArea;
	private Dictionary<ulong, Vector3> _colliders = new();
	private bool _isCorner;
	private float _greaterX;
	private float _greaterZ;
	private int _repeatedXCollision = 0;
	private int _repeatedZCollision = 0;

	public override void _Ready()
	{
		_enemyArea = GetNode<enemy_area>("EnemyArea");
		_globals = GetNode<globals>("/root/Globals");
		_globals.Connect(nameof(globals.PlayerAddedEventHandler), new Callable(this, nameof(FindPlayer)));
		_enemyArea.Connect(nameof(enemy_area.CollisionEventHandler), new Callable(this, nameof(OnColliding)));
		_enemyArea.Connect(nameof(enemy_area.LeavingEventHandler), new Callable(this, nameof(OnLeaving)));
	}

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
		
		// Calculate direction towards the player
		_directionToPlayer = (Player.GlobalTransform.Origin - GlobalTransform.Origin).Normalized();
		
		

		if (_colliders.Count == 0)
		{
			_isCorner = false;
			velocity.X = _directionToPlayer.X * Speed;
			velocity.Z = _directionToPlayer.Z * Speed;
		}
		
		if (_colliders.Count == 1 && !_isCorner)
		{
			var collidersList = _colliders.ToList();
			_greaterX = Mathf.Max(collidersList[0].Value.X, Mathf.Abs(_directionToPlayer.X));
			_greaterZ = Mathf.Max(collidersList[0].Value.Z, Mathf.Abs(_directionToPlayer.Z));
			if (_greaterX == Mathf.Abs(_directionToPlayer.X))
			{
				velocity.Z = -1 * Speed;
			}
			if (_greaterZ == Mathf.Abs(_directionToPlayer.Z))
			{
				velocity.X= -1 * Speed;
			}
		}
		
		if (_colliders.Count == 2)
		{
			_isCorner = true;
			
			var collidersList = _colliders.ToList();
			_greaterX = Mathf.Max(Mathf.Abs(collidersList[0].Value.X), Mathf.Abs(collidersList[1].Value.X));
			_greaterZ = Mathf.Max(Mathf.Abs(collidersList[0].Value.Z), Mathf.Abs(collidersList[1].Value.Z));
			if (_repeatedXCollision > 20)
			{
				velocity.Z = -1 * Speed;
			}
			if (_repeatedZCollision > 20)
			{
				velocity.X = -1 * Speed;
			}
			else if (_greaterX > _greaterZ)
			{
				_repeatedXCollision += 1;
				_repeatedZCollision = 0;
				velocity.Z = 1 * Speed;
			}
			else if (_greaterZ > _greaterX)
			{
				_repeatedZCollision += 1;
				_repeatedXCollision = 0;
				velocity.X = 1 * Speed;
			}
		}


		GD.Print("Player Direction" + _directionToPlayer);

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= Gravity * (float)delta;

		//velocity.X = (_directionToPlayer.X * _collider.X) * Speed;
		//velocity.Z = (_directionToPlayer.Z * _collider.Z) * Speed;
		
		//velocity.X = Mathf.MoveToward(_collider.X, _directionToPlayer.X, Speed);
		//velocity.Z = Mathf.MoveToward(_collider.Z, _directionToPlayer.Z, Speed);
		//velocity.X = -_collider.X * Speed;
		//velocity.Z = -_collider.Z * Speed;
			// velocity.X = Mathf.MoveToward(_collider.X, _directionToPlayer.X, Speed);
			// velocity.Z = Mathf.MoveToward(_collider.Z, _directionToPlayer.Z, Speed);
		/*else
		{
			velocity.X = _directionToPlayer.X + _collider.Z * Speed;
			velocity.Z = _directionToPlayer.Z + _collider.X * Speed;
		}*/
		
		

		Velocity = velocity;
		GD.Print("Velocity: " + Velocity);
		GD.Print("Colliders: " + _colliders.Count);
		foreach (var collider in _colliders)
		{
			GD.Print("Collider" + collider.Key);
		}
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

	private void OnColliding(ulong colliderId, Vector3 colliderPosition)
	{
		_colliders.Add(colliderId, colliderPosition);
	}
	
	private void OnLeaving(ulong colliderId)
	{
		_colliders.Remove(colliderId);
	}
}
