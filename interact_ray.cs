using Godot;
using System;
using System.Net.Mime;

public partial class interact_ray : RayCast3D
{
	[Export]
	private double _mouseSensitivity = 0.05;

	[Signal]
	public delegate void TouchingEventHandler(string text, ulong interactable);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Ensure the signal is properly registered
		AddUserSignal(nameof(TouchingEventHandler));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (IsColliding())
		{
			GodotObject interactable = GetCollider();
			if (interactable == null) return;
			if (interactable.HasMethod("GetText"))
			{
				var interactText = interactable.Call("GetText");
				var interactId = interactable.Call("GetInteractable");
				EmitSignal(nameof(TouchingEventHandler), interactText, interactId);
			}
		}
		else
		{
			const string noText = "null";
			const string noType = "null";
			EmitSignal(nameof(TouchingEventHandler), noText, noType);
		}
	}
	
	public override void _UnhandledInput(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouseMotion mouseMotion)
		{
			// Adjust rotation based on mouse motion
			RotationDegrees -= new Vector3((float)(mouseMotion.Relative.Y * _mouseSensitivity), (float)(mouseMotion.Relative.X * _mouseSensitivity), 0);
            
			// Clamp the X rotation angle between -60.0 and 60.0
			RotationDegrees = new Vector3(
				Mathf.Clamp(RotationDegrees.X, -60.0f, 60.0f),
				Mathf.Wrap(RotationDegrees.Y, 0.0f, 360.0f),
				RotationDegrees.Z
			);
		}
	}
}
