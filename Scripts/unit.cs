using Godot;
using System;
using static System.Math;

public class unit : KinematicBody
{
    public bool stats_set {get; private set;}
    public uint action_points_cap {get; private set;} 
    public uint health_points_cap {get; private set;} 
    public uint attack_points {get; private set;} 
    public uint defence_points {get; private set;} 
    public uint health_points_current {get; set;}
    public uint action_points_current {get; set;}  
    [Export]
    public int speed = 10;
    [Export]
    public int rot_speed = 5;

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
        anim_player = (AnimationPlayer)GetNode("model/AnimationPlayer");
        body = (KinematicBody)GetNode(".");
     //   move(new Vector3(3, 0, 3));
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        bool is_moving = false;

        if (point.DistanceTo(Transform.origin) > 0.05)
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
        if (anim == "Robot_Idle")
        {
            body.Rotation = Vector3.Zero;
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

    public void move(Vector3 dest)
    {
        point = dest;
    }

    public void deal_damage(Vector3 target)
    {
        float angle = (float)Atan2(target.x, target.z);
        Vector3 char_rot = body.Rotation;
        char_rot.y = angle;
        body.Rotation = char_rot;
        is_shooting = true;
        var bullet = _bullet_scene.Instance() as bullet;
        AddChild(bullet);
        bullet.create_bullet(target);
    }

    public void _on_AnimationPlayer_animation_finished(string anim_name)
    {
        if (anim_name == "Robot_Punch")
        {
            is_shooting = false;
            GetNode("bullet").QueueFree();
        }
    }

    public void _Set_Stats(uint AP, uint HP, uint damage, uint defence)
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

    public void _Receive_Damage(uint damage)
    {

    }

    public void _Death()
    {

    }

    public void _Teleport_unit(Vector3 new_pos)
    {
        this.Transform = Transform.Translated(new_pos);
        this.point = new_pos;
    }
}
