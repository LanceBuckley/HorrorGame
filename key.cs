using Godot;
using System;

public partial class key : StaticBody3D
{

	private globals _globals;
	private inventory _inventory;
	private key_area _keyArea;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_keyArea = GetNode<key_area>("KeyArea");
		_globals = GetNode<globals>("/root/Globals");
		_inventory = GetNode<inventory>("/root/Inventory");
		_globals.Connect(nameof(globals.UseItemEventHandler), new Callable(this, nameof(OnUse)));
		_globals.Connect(nameof(globals.InteractEventHandler), new Callable(this, nameof(OnInteract)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnInteract(ulong interactable, Vector3 heldPosition)
	{
		if (interactable != _keyArea.GetInstanceId()) return;
		Position = heldPosition;
		_inventory.AddItem(interactable, "Key1");
	}

	private void OnUse(ulong itemId)
	{
		if (itemId != _keyArea.GetInstanceId()) return;
		QueueFree();
	}
}
