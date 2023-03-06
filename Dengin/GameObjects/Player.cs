using SFML.Graphics;
using SFML.System;

namespace Dengin.GameObjects;

public enum JumpState
{
    Grounded = 0,
    Jumping = 1,
    Falling = 2
}

public class Player : GameObject
{
    public float MoveSpeed = 3f;
    public float GravitySpeed = 1f * Game.Scale;
    public float JumpHeight = 20f * Game.Scale;
    public Vector2f BaseSize;

    public JumpState JumpState = JumpState.Falling;
    public float JumpTime = 3;
    public float JumpCounter;
    public Vector2f JumpStart;

    public Player(RenderWindow? window) : base(window)
    {
        CurrentSprite.Texture = new Texture("./Resources/Images/Player/sprite.jpg");
        BaseSize = Size;
    }

    public override void Move(Vector2f offset)
    {
        Vector2f hypoPos = Pos + offset;

        if (hypoPos.X + Size.X > _win.Size.X) 
            offset.X = _win.Size.X - (Pos.X + Size.X);
        if (hypoPos.X < 0) 
            offset.X = -Pos.X;
        if (hypoPos.Y + Size.Y > _win.Size.Y) 
            offset.Y = _win.Size.Y - (Pos.Y + Size.Y);
        if (hypoPos.Y < 0) 
            offset.Y = -Pos.Y;
            
        base.Move(offset);
    }

    public override void Update()
    {
        Vector2f offset = new Vector2f();
        if (JumpState != JumpState.Jumping)
        {
            if (Pos.Y + GravitySpeed + Size.Y > _win.Size.Y)
                offset.Y = _win.Size.Y - (Pos.Y + Size.Y);
            else
                offset.Y = GravitySpeed;
        }

        if (JumpState == JumpState.Jumping)
        {
            if (JumpCounter < JumpTime)
            {
                JumpCounter += 0.1f;
                float t = JumpCounter / JumpTime;
                t = (float)Math.Sin(t * Math.PI * 0.5f);
                Pos.Y = Utility.Lerp(JumpStart.Y, JumpStart.Y - JumpHeight, t);
            }
            else
            {
                JumpState = JumpState.Falling;
                JumpCounter = 0;
            }
        }
        
        if (Pos.Y + Size.Y == _win.Size.Y)
            JumpState = JumpState.Grounded;
        
        base.Move(offset);
        base.Update();
    }

    public void Jump()
    {
        JumpState = JumpState.Jumping;
        JumpCounter = 0.1f;
        JumpStart = Pos;
    }
}
