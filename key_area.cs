using Godot;
using System;

public partial class key_area : Area3D
{

	[Export]
	public string InteractText = "Pick up key";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// AddUserSignal(nameof(globals.InteractEventHandler));
		// _interactArea = GetNode<Area3D>("Area3D");
		// Cannot use Callable.From for signals that have parameters
		// _interactArea.Connect("body_entered", new Callable(this, nameof(_On_Area_3D_Body_Entered)));
		// Connect("mouse_entered", Callable.From(_Mouse_Entered));
		
	}


	

	public string GetText()
	{
		return InteractText;
	}

	public ulong GetInteractable()
	{
		return GetInstanceId();
	}


}
