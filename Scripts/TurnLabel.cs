using Godot;
using System;

public class TurnLabel : Label
{
    // Declare member variables here. Examples:
    int current_player = 2;
    int P1_units = 3;
    int P2_units = 3;
    string current_controller = "AI";
    string gamemode = "PvP";
    [Signal]
    public delegate void Victory();
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
        this.Text = "Press Button to start";
    }

    public void _On_P1_unit_death()
    {
        P1_units --;
        if (P1_units == 0)
        {
            this.Text = "Player 2 wins";
            EmitSignal(nameof(Victory));
        }
    }
    public void _On_P2_unit_death()
    {
        P2_units --;
        if (P2_units == 0)
        {
            this.Text = "Player 1 wins";
            EmitSignal(nameof(Victory));    
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
