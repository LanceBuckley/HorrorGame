using Godot;
using System;

public partial class camera_joint : SpringArm3D
{
	[Export]
	private double _mouseSensitivity = 0.05;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TopLevel = true;
		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
