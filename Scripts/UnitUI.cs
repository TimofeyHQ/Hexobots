using Godot;
using System;

public class UnitUI : VBoxContainer
{
    TextureRect UnitPic;
    ProgressBar HPBar;
    Label APLabel;
    Label SelectionLabel;
    unit connected_unit;

    // Called when the node enters the scene tree for the first time.
    public void _Stat_change(unit unt)
    {   
        connected_unit = unt;
        HPBar.MaxValue = unt.health_points_cap;
        HPBar.Value = unt.health_points_current;
        APLabel.Text = unt.action_points_current.ToString() + "Action(s)";
    }

    public void _Texture_set(unit unt)
    {
        Texture temp2 = GD.Load<Texture>("res://Resources/Unit/rsz_red_ava.png");
        Texture temp1 = GD.Load<Texture>("res://Resources/Unit/rsz_blue_ava.png");
        if (unt.IsInGroup("Player2")) UnitPic.Texture = temp2;
        if (unt.IsInGroup("Player1")) UnitPic.Texture = temp1;
    }
    public void _Selected()
    {
        SelectionLabel.Text = "Is selected";
    }
    public void _Deselected()
    {
        SelectionLabel.Text = "Not selected";
    }
        public override void _Ready()
    {
        UnitPic = GetNode("UnitIcon") as TextureRect;
        HPBar = GetNode("HealthBar") as ProgressBar;
        APLabel = GetNode("ActionLabel") as Label;
        SelectionLabel = GetNode("SelectionLabel") as Label;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (connected_unit != null)
        {
            HPBar.Value = connected_unit.health_points_current;
            APLabel.Text = connected_unit.action_points_current.ToString() + " action(s)";
        }
    }
}
