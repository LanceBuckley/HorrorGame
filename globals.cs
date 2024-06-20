using Godot;
using System;

public partial class globals : Node
{
	[Signal]
	public delegate void InteractEventHandler(ulong itemId, Node3D heldItem);

	[Signal]
	public delegate void UseItemEventHandler(ulong itemId);

	[Signal]
	public delegate void PlayerAddedEventHandler(string path);
	
	// Register the signal when the node is added to the scene tree
	public override void _EnterTree()
	{
		AddUserSignal(nameof(InteractEventHandler));
		AddUserSignal(nameof(UseItemEventHandler));
		AddUserSignal(nameof(PlayerAddedEventHandler));
	}
}
