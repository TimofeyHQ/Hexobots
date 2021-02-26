using Godot;
using System;

public class TileTest : MeshInstance
{
    // Declare member variables here. Examples:
    public void _Move_tile(int row, int column)
    {
        coordinats[0] = row;
        coordinats[1] = column;
        Vector3 new_coords;
        new_coords = new Vector3(row*(float)Math.Sqrt(3)/2, 0, column*1.5F);
        this.Transform = Transform.Translated(new_coords);
    }
    public int movement{get; private set;}
    // private unit unit_on_tile;
    public bool is_unit_on_tile{get; private set;}
    private int []coordinats = new int[2];
    TileTest(){
        coordinats[0] = 0;
        coordinats[1] = 0;
    }

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
