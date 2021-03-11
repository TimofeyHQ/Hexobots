using Godot;
using System;

public class bullet : KinematicBody
{
    private int speed = 200;
    Vector3 target = Vector3.Zero;
    Vector3 velocity = Vector3.Zero;
    Vector3 direction = Vector3.Zero;

    public override void _Ready()
    {
        
    }
    public override void _Process(float delta)
    {
        MoveAndSlide(velocity);
    }

    public void create_bullet(Vector3 dest)
    {
        velocity = dest;
    }
}