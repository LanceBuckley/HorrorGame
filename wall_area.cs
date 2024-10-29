using Godot;
using System;

public partial class wall_area : Area3D
{

    [Export]
    public string InteractText = "Unlock Door";
    

    public string GetText()
    {
        return InteractText;
    }

    public ulong GetInteractable()
    {
        return GetInstanceId();
    }

    // This is just a test to see if the ssh key stuff is working

}