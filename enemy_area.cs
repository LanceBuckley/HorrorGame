using Godot;
using System;
using System.Transactions;

public partial class enemy_area : Area3D
{
	private player _player;
	private enemy _enemy;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_enemy = GetParent<enemy>();
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnBodyEntered(Node3D body)
	{
		// ?? is the null-coalescing operator and can be used to assign a default value to something in the case that it is null such as [result = expression1 ?? expression2] is 1 is null result will become 2, if 1 has a non-null value, result will be 1
		// ??= is the null-coalescing assignment operator and can be used to assign a new value to something only in the case that the current value is null, which we are doing here
		_player ??= _enemy.GetPlayer();
		if (body != _player)
		{
			GD.Print(body);
		}
		else
		{
			GD.Print("Tag!");
		}
	}
}
