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

	private void OnInteract(ulong interactable, Node3D heldItem)
	{
		if (interactable != _keyArea.GetInstanceId()) return;
		_inventory.AddItem(interactable, "Key1");
		ChangeParent(heldItem);
		Position = Vector3.Zero;
		ProcessMode = ProcessModeEnum.Disabled;
	}

	private void OnUse(ulong itemId)
	{
		if (itemId != _keyArea.GetInstanceId()) return;
		QueueFree();
	}

	private void ChangeParent(Node3D heldItem)
	{
		GetParent().RemoveChild(this);
		var newParent = heldItem.GetChild(0);
		newParent.AddChild(this);
	}
}
