using Godot;
using System;

public partial class key : StaticBody3D
{

	private globals _globals;
	private key_area _keyArea;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_keyArea = GetNode<key_area>("KeyArea");
		_globals = GetNode<globals>("/root/Globals");
		_globals.Connect(nameof(globals.InteractEventHandler), new Callable(this, nameof(OnInteract)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnInteract(ulong interactable)
	{
		if (interactable != _keyArea.GetInstanceId()) return;
		Position = new Vector3(Position.X, Position.Y + 1, Position.Z);
	}
}
