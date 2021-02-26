using Godot;
using System;

public class Map : Spatial
{
    // Declare member variables here. Examples:

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var _tile_scene = GD.Load<PackedScene>("res://Scenes/TileTest.tscn");
        int amount_of_columns;
        for (int i = -4; i < 5; i ++)
        {
            if (i % 2 == 0) amount_of_columns = 9;
            else amount_of_columns = 8;
            for (int j = -(amount_of_columns - 5); j <= amount_of_columns - 5; j += 2)
            {
                var _tile = _tile_scene.Instance() as TileTest;
                AddChild(_tile);
                _tile._Move_tile(i, j);
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
