using Godot;
using System;

public class Map : Spatial
{
    // Declare member variables here. Examples:

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var _tile_scene = GD.Load<PackedScene>("res://Scenes/TileTest.tscn");
        var _tile = _tile_scene.Instance() as TileTest;
        AddChild(_tile);
        _tile._Move_tile(1, 1);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
