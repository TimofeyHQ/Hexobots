using Godot;
using System;

public class TileTest : MeshInstance
{
    // Declare member variables here. Examples:
    public int movement{get; private set;}
    // private unit unit_on_tile;
    public bool is_unit_on_tile{get; private set;}
    private TileTest []touching_tiles = new TileTest[6];

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
