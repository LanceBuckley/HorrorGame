using Godot;
using System;

public partial class player : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	private camera_joint _springArm;
	private CollisionShape3D _body;
	private interact_ray _interactRay;
	private CanvasLayer _interactWindow;
	private globals _globals;
	private inventory _inventory;
	private Node3D _heldItem;
	private ulong _touching;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_springArm = GetNode<camera_joint>("/root/SceneManager/Player/CameraJoint");
		_body = GetNode<CollisionShape3D>("/root/SceneManager/Player/CollisionShape3D");
		_interactWindow = GetNode<CanvasLayer>("InteractWindow");
		_interactRay = GetNode<interact_ray>("InteractRay");
		_globals = GetNode<globals>("/root/Globals");
		_inventory = GetNode<inventory>("/root/Inventory");
		_heldItem = GetNode<Node3D>("HeldItem");
		_interactRay.Connect(nameof(interact_ray.TouchingEventHandler), new Callable(this, nameof(OnTouch)));
	}

	public override void _PhysicsProcess(double delta)
	{
		
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity.Y -= Gravity * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		if (Input.IsActionJustPressed("Inventory"))
		{
			// Assuming _inventory.InventoryItems is a List<string>
			foreach (var item in _inventory.InventoryItems)
			{
				GD.Print(item);
			}
		}

		if (_interactWindow.Visible)
		{
			if (Input.IsActionJustPressed("Interact"))
			{
				_globals.EmitSignal(nameof(globals.InteractEventHandler), _touching, _heldItem);
			}
		}

		// Get the input direction
		Vector2 inputDir = Input.GetVector("Left", "Right", "Up", "Down");
		// Alter the input direction by the current rotation of the player, converting it into a local value
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		// Rotate the direction further around the Y-axis by the springArm's rotation, thereby making it align with the visual direction of the camera
		direction = direction.Rotated(Vector3.Up, _springArm.Rotation.Y).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		if (Velocity.Length() > 0.2)
		{
			// Change the player's rotation to match direction changes above
			var lookDirection = new Vector2(Velocity.Z, Velocity.X);
			var angle = lookDirection.Angle();
			_body.RotationDegrees = new Vector3(_body.RotationDegrees.X, angle, _body.RotationDegrees.Z);
		}
	}

	public override void _Process(double delta)
	{
		_springArm.Position = Position;
	}

	private void OnTouch(string text, ulong interactable)
	{
		_touching = interactable;
		if (text == "null")
		{
			_interactWindow.Visible = false;
			return;
		}
		_interactWindow.Visible = true;
		var label = _interactWindow.GetChild<Label>(0);
		label.Text = text;
	}
}
