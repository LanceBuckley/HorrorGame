using Godot;
using System;

public partial class main_menu : CanvasLayer
{
    private const string Pressed = "pressed";
    
    [Signal]
    public delegate void StartGameEventHandler();
    
    public override void _Ready()
    {
        // Ensure the signal is properly registered
        AddUserSignal(nameof(StartGameEventHandler));
    }
    
    private void _on_button_pressed()
    {
        Console.WriteLine(Pressed);
        // Can also use GD.Print(Pressed); for Godot's console output
        EmitSignal(nameof(StartGameEventHandler));
    }
}
