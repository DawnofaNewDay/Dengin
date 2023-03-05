using System.Diagnostics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Dengin.GameObjects;

public class Player : GameObject
{
    public float MoveSpeed = 1;

    public Player(RenderWindow? window) : base(window)
    {
        CurrentSprite.Texture = new Texture("./Resources/Images/Player/sprite.jpg");
    }

    private void ReceiveInput()
    {
        Console.WriteLine(Pos);
        if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
            Pos.Y -= MoveSpeed;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            Pos.Y += MoveSpeed;

        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            Pos.X -= MoveSpeed;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
            Pos.X += MoveSpeed;
    }
    
    public override void Update() 
    {
        Console.WriteLine("hi");
        ReceiveInput();
        base.Update();
    }
}