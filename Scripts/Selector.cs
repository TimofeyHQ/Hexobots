using Godot;
using System;

public class Selector : Spatial
{
    // Declare member variables here. Examples:
    TileTest first;
    Map map;
    int curr_player = 3;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    private void _on_Tile_selected(TileTest tile)
    {
        if (first == null && tile.unit_on_tile != null && tile?.unit_on_tile.action_points_current >= 0)
        {
            if ((curr_player == 1 && tile.unit_on_tile.IsInGroup("Player1"))||(curr_player == 2 && tile.unit_on_tile.IsInGroup("Player2")))
            { 
                first = tile;
                first.unit_on_tile.EmitSignal("_selected_to_UI");
                GD.Print("first " + tile.coord(0).ToString() + ';' + tile.coord(1).ToString());
            }
        }
        else if (first != null && tile.unit_on_tile == null && (tile.movement + first.unit_on_tile.action_points_current >-1))
        {
            int x1 = first.coord(0), x2 = tile.coord(0), z1 = first.coord(1), z2 = tile.coord(1);
            if ((Math.Abs(x1 - x2) == 2 && z1 == z2) || (Math.Abs(x1 - x2) == 1 && Math.Abs(z1 - z2) == 1))
            {
                GD.Print("second "+ tile.coord(0).ToString() + ';' + tile.coord(1).ToString());
                first.unit_on_tile.EmitSignal("_deselected_to_UI");
                first.unit_on_tile._Change_Tile(tile);
                first = null;
                tile.unit_on_tile.action_points_current += tile.movement;
            }
        }
        else if (first != null && tile.unit_on_tile != null && ((first.unit_on_tile.IsInGroup("Player1") && tile.unit_on_tile.IsInGroup("Player1"))||(first.unit_on_tile.IsInGroup("Player2") && tile.unit_on_tile.IsInGroup("Player2"))))
        {
            first.unit_on_tile.EmitSignal("_deselected_to_UI");
            first = tile;
            first.unit_on_tile.EmitSignal("_selected_to_UI");
        }    
        else if (first != null && tile.unit_on_tile != null && first?.unit_on_tile.action_points_current > 0 && ((first.unit_on_tile.IsInGroup("Player1") && tile.unit_on_tile.IsInGroup("Player2"))||(first.unit_on_tile.IsInGroup("Player2") && tile.unit_on_tile.IsInGroup("Player1"))))
        {   
            if (map._Pathfind(first, tile)) first.unit_on_tile.deal_damage(tile.Transform.origin);
            tile.unit_on_tile._Receive_Damage(first.unit_on_tile.attack_points);
            first.unit_on_tile.action_points_current --;
        }
    }

    public void _Change_player()
    {
        if (curr_player == 1) curr_player ++;
        else curr_player = 1;
        GD.Print("Player"+ curr_player.ToString());
        if (first != null) first.unit_on_tile.EmitSignal("_deselected_to_UI");
        first = null;
    }

    public void _Get_Map(Map mp)
    {
        map = mp;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
