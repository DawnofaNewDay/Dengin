using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Dengin.GameObjects;

public class Player : GameObject
{
    public float MoveSpeed = 3.5f;

    public Player(RenderWindow? window) : base(window)
    {
        CurrentSprite.Texture = new Texture("./Resources/Images/Player/sprite.jpg");
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
}
