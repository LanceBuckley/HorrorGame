using Godot;
using System;

public partial class globals : Node
{
	[Signal]
	public delegate void InteractEventHandler(string type);

	[Signal]
	public delegate void UseItemEventHandler(ulong itemId);
	
	// Register the signal when the node is added to the scene tree
	public override void _EnterTree()
	{
		AddUserSignal(nameof(InteractEventHandler));
		AddUserSignal(nameof(UseItemEventHandler));
	}
}
