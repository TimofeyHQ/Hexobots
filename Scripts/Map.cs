using Godot;
using System;

public class Map : Spatial
{
    // Declare member variables here. Examples:
    private TileTest[,] map;
    // Called when the node enters the scene tree for the first time.
    [Signal]
    public delegate void map_selector_connection(Map map);
    private void _Map_Generation()
    {
        var _tile_scene = GD.Load<PackedScene>("res://Scenes/TileTest.tscn");
        int amount_of_columns;
        map = new TileTest[11, 11];
        for (int i = -5; i < 6; i ++)
        {
            if (i % 2 == 0) amount_of_columns = 11;
            else amount_of_columns = 10;
            for (int j = -(amount_of_columns - 6); j <= amount_of_columns - 6; j += 2)
            {
                var _tile = _tile_scene.Instance() as TileTest;
                AddChild(_tile);
                _tile._Move_tile(i, j);
                map[_tile.coord(0) + 5, _tile.coord(1) + 5] = _tile;
                _tile.Connect("_Tile_selected", GetNode("../Selector"), "_on_Tile_selected");
            }
        }
        for (int i = 0; i < 11; i++)
            for (int j = 0; j < 11; j ++)
            {
            if (map[i, j] == null) continue;
            if ((j <= 1) || (j >= 9) || (i <= 1) || (i >= 9)) map[i, j]._Set_Random_Tiletype(2);
            else map[i, j]._Set_Random_Tiletype(3);
            }
    }

    private void _Spawn_units()
    {
        var _unit_scene = GD.Load<PackedScene>("res://Scenes/unit.tscn");
        for (int i = 1; i < 10; i += 4)
        {
            var _unit = _unit_scene.Instance() as unit;
            _unit._Teleport_unit(map[i, 0].Transform.origin);
            _unit._Set_Stats(4,10,4,4);
            AddChild(_unit);
            _unit.AddToGroup("Player1");
            _unit._Change_Tile(map[i, 0]);
            GetNode("../NextTurnButton").Connect("pressed", _unit, "_Refresh_AP");
        }
        for (int i = 1; i < 10; i += 4)
        {
            var _unit = _unit_scene.Instance() as unit;
            _unit._Teleport_unit(map[i, 10].Transform.origin);
            _unit._Set_Stats(4,10,4,4);
            AddChild(_unit);
            _unit.AddToGroup("Player2");
            _unit._Change_Tile(map[i, 10]);
            GetNode("../NextTurnButton").Connect("pressed", _unit, "_Refresh_AP");
        }
    }
    public TileTest _Get_Tile_from_Map(int rows, int columns)
    {
        return map[rows, columns];
    }

    public override void _Ready()
    {
        EmitSignal("map_selector_connection", this);
        _Map_Generation();
        _Spawn_units();
    }

    public bool _Pathfind(TileTest one, TileTest two)
    {
        if (Math.Abs(one.coord(0) - two.coord(0)) == 2 && one.coord(1) == two.coord(1))
        {
            int j = one.coord(1);
            for (int i = one.coord(0) + 5; i < two.coord(0) + 5; i+=2)
                if (map[i,j] != null)
                    if (map[i, j].movement == -1488) return false;
            return true;
        }
        else if (Math.Abs(one.coord(0) - two.coord(0)) == Math.Abs(one.coord(1) - two.coord(1)))
        {
            int di = two.coord(0) - one.coord(0), dj = two.coord(1) - one.coord(1);
            for (int i = one.coord(0) + 5; i < two.coord(0) + 5; i += di)
                for (int j = one.coord(1) + 5; j < two.coord(1) + 5; j += dj)
                    if (map[i,j] != null)    
                        if (map[i, j].movement == -1488) return false;
            return true;
        }
        return false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
