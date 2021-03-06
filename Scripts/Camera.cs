using Godot;
using System;

public class Camera : Godot.Camera
{
    private int curr_player = 2;
    private bool switching = false;
    private Vector3 player1_pos = new Vector3(0, 7, -11);
    private Vector3 player1_rot = new Vector3(-40, 180, 0);

    private Vector3 player2_pos = new Vector3(0, 7, 11);
    private Vector3 player2_rot = new Vector3(-40, 0, 0);

    private int curr_pos = 0;
    private int size = 79;
    private Vector3[] arr_pos;
    private Vector3[] arr_rot;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (switching)
        {
            this.Translation = arr_pos[curr_pos];
            this.RotationDegrees = arr_rot[curr_pos];
            curr_pos++;
            if (curr_pos >= size)
            {
                switching = false;
                curr_pos = 0;
            }
            curr_player = (curr_player == 1) ? 2 : 1;
        }
    }

    public void switch_sides()
    {
        arr_pos = init_arr_pos();
        arr_rot = init_arr_rot();
        switching = true;
    }
    public Vector3[] init_arr_pos()
    {
        Vector3[] arr = new Vector3[size];
        float z;
        float z_inc;
        float radius;

        z = (curr_player == 1) ? player1_pos.z : player2_pos.z;
        z_inc = (player2_pos.z - player1_pos.z) / size;
        z_inc *= (curr_player == 1) ? 1 : -1;
        radius = z;
        arr[0] = (curr_player == 1) ? player1_pos : player2_pos;
        arr[size - 1] = (curr_player == 1) ? player2_pos : player1_pos;
        for (int i = 1; i < size - 1; i++)
        {
            z += z_inc;
            arr[i] = new Vector3(Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(z, 2)), player1_pos.y, z);
        }
        /* for (int i = 0; i < size; i++)
             GD.Print(i, ": ", arr[i]);*/
        return (arr);
    }


    public Vector3[] init_arr_rot()
    {
        Vector3[] arr = new Vector3[size];
        float y;
        float y_inc;

        y = (curr_player == 1) ? player1_rot.y : player2_rot.y;
        y_inc = (player2_rot.y - player1_rot.y) / size;
        y_inc *= (curr_player == 2) ? -1 : 1;
        arr[0] = (curr_player == 1) ? player1_rot : player2_rot;
        arr[size - 1] = (curr_player == 1) ? player2_rot : player1_rot;
        for (int i = 1; i < size - 1; i++)
        {
            y += y_inc;
            arr[i] = new Vector3(player1_rot.x, y, player1_rot.z);
        }
        // for (int i = 0; i < size; i++)
        //     GD.Print(i, ": ", arr[i]);
        return (arr);
    }
}
