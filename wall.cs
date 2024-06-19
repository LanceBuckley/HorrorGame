using Godot;
using System;

public partial class wall : StaticBody3D
{

    private globals _globals;
    private inventory _inventory;
    private wall_area _wallArea;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _wallArea = GetNode<wall_area>("WallArea");
        _globals = GetNode<globals>("/root/Globals");
        _inventory = GetNode<inventory>("/root/Inventory");
        _globals.Connect(nameof(globals.InteractEventHandler), new Callable(this, nameof(OnInteract)));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    private void OnInteract(ulong interactable, Vector3 heldPosition)
    {
        if (interactable != _wallArea.GetInstanceId()) return;
        // This searches InventoryItems to find an item matching the criteria of Item2(the string) being Key1
        var doorKey = _inventory.InventoryItems.Find(item => item.Item2 == "Key1");
        if (doorKey != null)
        {
            _inventory.InventoryItems.Remove(doorKey);
            _globals.EmitSignal(nameof(globals.UseItemEventHandler), doorKey.Item1);
            QueueFree();
        }
        else
        {
            GD.Print("The door is locked");
        }
    }
}