using Godot;

public partial class new_enemy : CharacterBody3D
{
    private globals _globals;
    [Export] private NodePath _playerPath;
    private const float Speed = 5.0f;
    private player _player;
    private NavigationAgent3D _navAgent;

    public override void _Ready()
    {
        _globals = GetNode<globals>("/root/Globals");
        _globals.Connect(nameof(globals.PlayerAddedEventHandler), new Callable(this, nameof(FindPlayer)));
        _navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        // This prevents the physics process from running long enough for the NavigationServer to be set up
        SetPhysicsProcess(false);
        // This waits a frame to call the ActorSetUp function
        CallDeferred(nameof(ActorSetUp));
    }

    public override void _PhysicsProcess(double delta)
    {
        // We set the target position to the player
        _navAgent.TargetPosition = _player.GlobalTransform.Origin;
        // We find the next point on the path
        var nextNavPoint = _navAgent.GetNextPathPosition();
        // By subtracting the enemy's current position from the next point, and normalizing it, we can generate the direction, multiplied by the speed gives us the velocity
        Velocity = (nextNavPoint - GlobalTransform.Origin).Normalized() * Speed;
        // Turns and faces the direction of the player
        var newTransform = GlobalTransform.LookingAt(new Vector3(_player.GlobalPosition.X, GlobalPosition.Y, _player.GlobalPosition.Z), Vector3.Up);
        Transform = Transform.InterpolateWith(newTransform, (float)(Speed * delta));
        MoveAndSlide();
    }
    
    private void FindPlayer(string scenePath)
    {
        _player = GetNode<player>(scenePath);
    }

    private async void ActorSetUp()
    {
        // This is an asynchronous function that waits for the physics frame signal and then enables the physics process
        await ToSignal(GetTree(), "physics_frame");
        SetPhysicsProcess(true);
    }
}
