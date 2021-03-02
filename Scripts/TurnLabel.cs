using Godot;
using System;

public class TurnLabel : Label
{
    // Declare member variables here. Examples:
    int current_player = 2;
    string current_controller = "AI";
    string gamemode = "PvP";
    public void _on_NextTurnButton_pressed()
    {   
        
        switch (gamemode)
        {
            case "PvP":
                current_controller = "Player";
                if (current_player == 1) current_player++;
                else current_player--;
                this.Text = current_controller + " " + current_player.ToString() + "'s turn";
                break;
            case "PvE":
                if (current_controller == "Player") current_controller = "AI";
                else current_controller = "Player";
                this.Text = current_controller + " " + "'s turn";
                break;
        }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Text = "Press Next Turn Button to start";
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
