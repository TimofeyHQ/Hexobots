using Godot;
using System;
using static System.Math;

public class unit : KinematicBody
{
    public bool stats_set {get; private set;}
    public int action_points_cap {get; private set;} 
    public int health_points_cap {get; private set;} 
    public int attack_points {get; private set;} 
    public int defence_points {get; private set;} 
    public int health_points_current {get; set;}
    public int action_points_current {get; set;}  
    public TileTest tile_underneath {get; private set;}
    [Export]
    public int speed = 10;
    [Export]
    public int rot_speed = 5;
    [Signal]
    public delegate void _unit_selected(TileTest one_underneath);
    [Signal]
    public delegate void _unit_died();
    [Signal]
    public delegate void _connect_to_UI(unit unt);
    [Signal]
    public delegate void _selected_to_UI();
    [Signal]
    public delegate void _deselected_to_UI();
    [Signal]
    public delegate void _stat_change(unit unt);

    Vector3 velocity = Vector3.Zero;
    Vector3 direction;
    Vector3 point;
    KinematicBody body;

    AnimationPlayer anim_player = new AnimationPlayer();
    PackedScene _bullet_scene = GD.Load<PackedScene>("res://Scenes/bullet.tscn");

    int x = 6;
    int z = 0;

    bool is_shooting = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        body = (KinematicBody)GetNode(".");
     //   move(new Vector3(3, 0, 3));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        bool is_moving = false;

        if (point.DistanceTo(Transform.origin) > 0.1)
        {
            direction = point - Transform.origin;
            direction = direction.Normalized() * speed;
            is_moving = true;
        }
        else
        {
            direction = point - Transform.origin;
            is_moving = false;
        }

        MoveAndSlide(direction);

        string anim = "Robot_Idle";
        if (is_moving)
        {
            anim = "Robot_Walking";

            float angle = (float)Atan2(direction.x, direction.z);
            Vector3 char_rot = body.Rotation;
            char_rot.y = angle;
            body.Rotation = char_rot;

        }
        else if (is_shooting)
        {
            anim = "Robot_Punch";
        }
        else if (health_points_current <= 0)
        {
            anim = "Robot_Death";
        }

        string curr_anim = anim_player.CurrentAnimation;
        if (anim != curr_anim)
            anim_player.Play(anim);

        // if (Input.IsActionJustPressed("ui_down"))
        // {
        //     Vector3 kek = new Vector3(x, 0, z);
        //     deal_damage(kek);
        //     x -= 1;
        //     z += 1;
        //     GD.Print("x is ", x);
        //     GD.Print("z is ", z);
        // }
    }

    public void set_player(int player)
    {
        string name;
        string path;

        if (player == 1)
        {
            name = "BlueRobot";
            path = "res://Scenes/BlueRobot.tscn";
        }
        else
        {
            name = "RedRobot";
            path = "res://Scenes/RedRobot.tscn";
        }
        Node robot = GD.Load<PackedScene>(path).Instance();
        AddChild(robot);
        anim_player = (AnimationPlayer)GetNode(name+"/AnimationPlayer");
        anim_player.Connect("animation_finished", GetNode("."), "_on_AnimationPlayer_animation_finished");
        if (player == 2)
        {
            Vector3 char_rot = body.Rotation;
            char_rot.y = 135;
            body.Rotation = char_rot;
        }
    }

    public void move(Vector3 dest)
    {
        point = dest;
        EmitSignal("_stat_change", this);
    }

    public void deal_damage(Vector3 target)
    {
        Vector3 dir = target - Transform.origin;
        float angle = (float)Atan2(dir.x, dir.z);
        Vector3 char_rot = body.Rotation;
        char_rot.y = angle;
        body.Rotation = char_rot;
        
        var bullet = _bullet_scene.Instance() as bullet;
        AddChild(bullet);
        bullet.create_bullet(dir);
        is_shooting = true;
    }

    public void _on_AnimationPlayer_animation_finished(string anim_name)
    {
        if (anim_name == "Robot_Punch")
        {
            is_shooting = false;
            GetNode("bullet").QueueFree();
        }
        else if (anim_name == "Robot_Death")
        {
            EmitSignal("_unit_died");
            this.tile_underneath.unit_on_tile = null;
            QueueFree();
        }
    }

    public void _Set_Stats(int AP, int HP, int damage, int defence)
    {
        if (stats_set == false)
        {
            action_points_cap = AP;
            health_points_cap = HP;
            attack_points = damage;
            defence_points = defence; 
            health_points_current = HP;
            action_points_current = AP;      
        }
    }

    public void _Receive_Damage(int damage)
    {
        Random rand = new Random();
        for (int i = 0; i < defence_points; i ++)
            if (damage > 0) 
            {
                if (rand.Next(10) > 2)
                    health_points_current --;
                damage --;
            }
       // GD.Print("Health: " + health_points_current.ToString()); 
       EmitSignal("_stat_change", this);           
    }

    public void _Teleport_unit(Vector3 new_pos)
    {
        this.Transform = Transform.Translated(new_pos);
        this.point = new_pos;
    }

    public void _Change_Tile(TileTest new_tile)
    {   if(this.tile_underneath != null)
            this.tile_underneath.unit_on_tile = null;
        this.tile_underneath = null;
        move(new_tile.Transform.origin);
        this.tile_underneath = new_tile;
        new_tile.unit_on_tile = this;
    }

    public void _Refresh_AP()
    {
        action_points_current = action_points_cap;
    }
    public void _on_Mouse_click(Node a, InputEvent inputEvent, Vector3 click_pos, Vector3 click_norm, int shape_idx)
    {   
        if (inputEvent is InputEventMouseButton eventMB)
            if (eventMB.Pressed && eventMB.ButtonIndex == 1)
            {
                EmitSignal(nameof(_unit_selected), this.tile_underneath);
            }
    }
}
