using Godot;
using System;

public partial class scene_manager : Node
{

	private PackedScene _levelScene = (PackedScene)ResourceLoader.Load("res://level.tscn");
	private PackedScene _playerScene = (PackedScene)ResourceLoader.Load("res://player.tscn");
	private main_menu _mainMenu;
	private globals _globals;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_globals = GetNode<globals>("/root/Globals");
		_mainMenu = GetNode<main_menu>("/root/SceneManager/MainMenu");
		
		// Connect the signal from MainMenu to the method in this class
		_mainMenu.Connect(nameof(main_menu.StartGameEventHandler), Callable.From(OnStartGame));
	}
	
	// Method to handle the signal
	private void OnStartGame()
	{
		Console.WriteLine("Start game signal received");
		var levelInstance = _levelScene.Instantiate<level>();
		AddChild(levelInstance);
		var playerInstance = _playerScene.Instantiate<player>();
		var playerPosition = levelInstance.GetNode<Node3D>("PlayerPosition");
		playerInstance.Position = playerPosition.Position;
		AddChild(playerInstance);
		_globals.EmitSignal(nameof(globals.PlayerAddedEventHandler), playerInstance.GetPath());
		_mainMenu.QueueFree();
	}
}
