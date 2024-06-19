using Godot;

public partial class options_menu : CanvasLayer
{

	private Button _resume;
	private Button _retry;
	private inventory _inventory;
    public override void _Ready()
    {
	    // We initialize the vBoxContainer up here so that it can be appropriately referenced below in the switch case
	    VBoxContainer vBoxContainer;
	    switch(OS.GetName())
		    {
                case "Windows":
	                Button quitButton = new Button();
	                quitButton.Text = "Quit Game";
	                vBoxContainer = GetNode<VBoxContainer>("Control/VBoxContainer");
	                vBoxContainer.AddChild(quitButton);
	                quitButton.Connect("pressed", Callable.From(_OnQuitPressed));
	                break;
                case "Web":
	                Button restartButton = new Button();
	                restartButton.Text = "Restart Game";
	                vBoxContainer = GetNode<VBoxContainer>("Control/VBoxContainer");
	                vBoxContainer.AddChild(restartButton);
	                restartButton.Connect("pressed", Callable.From(_OnRestartPressed));
	                break;
		    }
	    _resume = GetNode<Button>("Control/VBoxContainer/Resume");
	    _retry = GetNode<Button>("Control/VBoxContainer/Retry");
	    _resume.Connect("pressed", Callable.From(_OnResumePressed));
	    _retry.Connect("pressed", Callable.From(_OnRetryPressed));
	    _inventory = GetNode<inventory>("/root/Inventory");
    }

    public override void _Input(InputEvent @event)
    {
	    // Unless the Options button was just pressed this function immediately returns
	    if (!Input.IsActionJustPressed("Options")) return;
	    if (!Visible)
	    {
		    _ShowMenu();
	    }
	    else
	    {
		    _HideMenu();
	    }
    }

    private void _ShowMenu()
    {
	    Show();
	    GetTree().Paused = true;
	    Input.MouseMode = Input.MouseModeEnum.Confined;
    }

    private void _HideMenu()
    {
	    GetTree().Paused = false;
	    Hide();
	    Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    private void _OnResumePressed()
    {
	    GetTree().Paused = false;
	    _HideMenu();
    }

    private void _OnRetryPressed()
    {
	    GetTree().Paused = false;
	    // This stops duplicate items from being stored in the inventory when the game is restarted
	    _inventory.InventoryItems.Clear();
	    GetTree().ReloadCurrentScene();
    }

    private void _OnQuitPressed()
    {
	    GetTree().Quit();
    }

    private void _OnRestartPressed()
    {
	    // Be aware that this path might need to change
	    GetTree().Paused = false;
	    GetTree().ChangeSceneToFile("res://scene_manager.tscn");
    }
}
