using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;

public partial class inventory : Node
{
	
	private List<Tuple<ulong, string>> _inventory = new List<Tuple<ulong, string>>();

	public List<Tuple<ulong, string>> InventoryItems
	{
		get { return _inventory; }
		set { _inventory = value; }
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void AddItem(ulong itemId, string itemName)
	{
		var newItem = new Tuple<ulong, string>(itemId, itemName);
		_inventory.Add(newItem);
		GD.Print(newItem);
	}
}
