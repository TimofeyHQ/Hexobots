using Godot;
using System;

public class Map : Spatial
{
    // Declare member variables here. Examples:
    private TileTest[,] map;
    // Called when the node enters the scene tree for the first time.
    private void _Map_Generation()
    {
        var _tile_scene = GD.Load<PackedScene>("res://Scenes/TileTest.tscn");
        int amount_of_columns;
        map = new TileTest[9, 9];
        for (int i = -4; i < 5; i ++)
        {
            if (i % 2 == 0) amount_of_columns = 9;
            else amount_of_columns = 8;
            for (int j = -(amount_of_columns - 5); j <= amount_of_columns - 5; j += 2)
            {
                var _tile = _tile_scene.Instance() as TileTest;
                AddChild(_tile);
                _tile._Move_tile(i, j);
                map[_tile.coord(0) + 4, _tile.coord(1) + 4] = _tile;
            }
        }
        for (int i = 0; i < 9; i++)
            for (int j = 0; j < 9; j ++)
            {
            if (map[i, j] == null) continue;
            if ((j <= 1) || (j >= 7)) map[i, j]._Set_Random_Tiletype(2);
            else map[i, j]._Set_Random_Tiletype(3);
            }
    }
    public override void _Ready()
    {
        _Map_Generation();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
