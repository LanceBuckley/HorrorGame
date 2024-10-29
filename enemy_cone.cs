using Godot;
using System;

public partial class enemy_cone : Area3D
{
	
	[Signal]
	public delegate void OnBodyEnteredEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Ensure the signal is properly registered
		Console.WriteLine("BodyEntered Signal Added");
		AddUserSignal(nameof(OnBodyEnteredEventHandler));
	}
}
