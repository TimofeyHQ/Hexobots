using Godot;
using System;

public class Selector : Spatial
{
    // Declare member variables here. Examples:
    TileTest first;
    TileTest second;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private void _on_Tile_selected(TileTest tile)
    {
        if (first == null && tile.unit_on_tile == null)
            {first = tile; tile._Set_tiletype("Mountain");}
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
