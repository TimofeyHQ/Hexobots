using Godot;
using System;

public class TileTest : MeshInstance
{
    // Declare member variables here. Examples:
    public int movement{get; private set;}
    public unit unit_on_tile;
    private int []coords = new int[2];
    private string tile_type = "None";
    
    [Signal]
    public delegate void _Tile_selected(TileTest this_one);
    public void _Move_tile(int row, int column)
    {
        coords[0] = row;
        coords[1] = column;
        Vector3 new_coords;
        new_coords = new Vector3(row*(float)Math.Sqrt(3)/2, 0, column*1.5F);
        this.Transform = Transform.Translated(new_coords);
    }
    
    public void _Set_tiletype(string type)
    {
        Mesh new_mesh;
        switch (type)
        {
            case "Grass":
                tile_type = type;
                movement = 0;
                new_mesh = GD.Load<Mesh>("res://Resources/Tiles/GrassTile.mesh");
                break;
            case "Mountain":
                tile_type = type;
                movement = -1488;
                new_mesh = GD.Load<Mesh>("res://Resources/Tiles/MountainTile.mesh");
                break;
            case "Lake":
                movement = -1;
                new_mesh = GD.Load<Mesh>("res://Resources/Tiles/LakeTile.mesh");
                break;
            default:
                tile_type = "Grass";
                movement = 0;
                new_mesh = GD.Load<Mesh>("res://Resources/Tiles/GrassTile.mesh");
                break;
        }
        this.Mesh = new_mesh;
    }
    
    public void _Set_Random_Tiletype(int amount_of_variants)
    {
        string type;
        Random rand = new Random();
        switch (rand.Next(amount_of_variants))
        {
            case 0: type = "Grass"; break;
            case 1: type = "Lake"; break;
            case 2: type = "Mountain"; break;
            default: type = "Grass"; break;
        }
        _Set_tiletype(type);
    }
    public int coord(int number)
    {
        return coords[number];
    }
    TileTest(){
        coords[0] = 0;
        coords[1] = 0;
    }

    public void _on_Mouse_click(Node a, InputEvent inputEvent, Vector3 click_pos, Vector3 click_norm, int shape_idx)
    {   
        if (inputEvent is InputEventMouseButton eventMB)
            if (eventMB.Pressed && eventMB.ButtonIndex == 1)
            {
                GD.Print("Tile works");
                EmitSignal(nameof(_Tile_selected), this);
            }
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
