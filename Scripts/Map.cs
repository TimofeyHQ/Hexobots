using Godot;
using System;

public class Map : Spatial
{
    // Declare member variables here. Examples:
    private TileTest[,] map;
    // Called when the node enters the scene tree for the first time.
    [Signal]
    public delegate void map_selector_connection(Map mp);
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

    private void _Unit_Configuration(unit unt, int player_number,int column_number, int row_number, int UI_number)
    {
        unt.set_player(player_number);
        unt.AddToGroup("Player" + player_number.ToString());
        unt._Change_Tile(map[column_number, row_number]);
        GetNode("../NextTurnButton").Connect("pressed", unt, "_Refresh_AP");
        unt.Connect("_unit_selected", GetNode("../Selector"), "_on_Tile_selected");
        unt.Connect("_unit_died", GetNode("../TurnLabel"), "_On_P" + player_number.ToString() + "_unit_death");
        unt.Connect("_connect_to_UI", GetNode("../UnitUI_P"+ player_number.ToString() +"_" + UI_number.ToString()), "_Texture_set");
        unt.EmitSignal("_connect_to_UI", this);
        unt.Connect("_selected_to_UI", GetNode("../UnitUI_P"+ player_number.ToString() +"_" + UI_number.ToString()), "_Selected");
        unt.Connect("_deselected_to_UI", GetNode("../UnitUI_P"+ player_number.ToString() +"_" + UI_number.ToString()), "_Deselected");
        unt.Connect("_stat_change", GetNode("../UnitUI_P"+ player_number.ToString() +"_" + UI_number.ToString()), "_Stat_change");
    }
    private void _Spawn_units()
    {
        var _unit_scene = GD.Load<PackedScene>("res://Scenes/unit.tscn");
        int unit_player1_number = 1;
        for (int i = 1; i < 10; i += 4)
        {
            var _unit = _unit_scene.Instance() as unit;
            _unit._Teleport_unit(map[i, 0].Transform.origin);
            _unit._Set_Stats(4,10,4,4);
            AddChild(_unit);
            _Unit_Configuration(_unit, 1, i, 0, unit_player1_number);
            unit_player1_number ++;
        }
        int unit_player2_number = 1;
        for (int i = 1; i < 10; i += 4)
        {
            var _unit = _unit_scene.Instance() as unit;
            _unit._Teleport_unit(map[i, 10].Transform.origin);
            _unit._Set_Stats(4,10,4,4);
            AddChild(_unit);
            _Unit_Configuration(_unit, 2, i, 10, unit_player2_number);
            unit_player2_number ++;
        }
    }
    public TileTest _Get_Tile_from_Map(int rows, int columns)
    {
        return map[rows, columns];
    }

    public override void _Ready()
    {
        _Map_Generation();
        _Spawn_units();
        EmitSignal("map_selector_connection", this);
    }

    public bool _Pathfind(TileTest one, TileTest two)
    {
        if (Math.Abs(one.coord(0) - two.coord(0)) % 2 == 0 && one.coord(1) == two.coord(1))
        {
            int j = one.coord(1) + 5, imax, imin;
            if (one.coord(0) > two.coord(0))
            {
                imin = two.coord(0) + 5;
                imax = one.coord(0) + 5;
            }
            else
            {
                imax = two.coord(0) + 5;
                imin = one.coord(0) + 5;
            }
            GD.Print("imin: " + imin + " imax: " + imax);
            for (int i = imin; i < imax; i+=2)
                if (map[i,j] != null)
                {
                    if (map[i, j].movement == -1488) return false;
                }
            return true;
            
        }
        else if (Math.Abs(one.coord(0) - two.coord(0)) == Math.Abs(one.coord(1) - two.coord(1)))
        {   
            int jmin, jmax, imin, imax;
            if (one.coord(0) > two.coord(0))
            {
                imin = two.coord(0) + 5;
                imax = one.coord(0) + 5;
            }
            else
            {
                imax = two.coord(0) + 5;
                imin = one.coord(0) + 5;
            }
            if (one.coord(1) > two.coord(1))
            {
                jmin = two.coord(1) + 5;
                jmax = one.coord(1) + 5;
            }
            else
            {
                jmax = two.coord(1) + 5;
                jmin = one.coord(1) + 5;
            }
            int i = imin;
            int j = jmin;
            while (i < imax && j < jmax)
            {
                if (map[i,j] != null)
                    {
                    if (map[i, j].movement == -1488) return false;
                    }
                i++;
                j++;
            }
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
