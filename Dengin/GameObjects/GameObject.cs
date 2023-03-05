using SFML.Graphics;
using SFML.System;

namespace Dengin.GameObjects;

public class GameObject
{
    protected Vector2f Pos = new Vector2f(0, 0);
    public readonly Sprite CurrentSprite = new Sprite();
    public Vector2f Size = new Vector2f(64, 64);
    protected readonly RenderWindow? _win;

    protected GameObject(RenderWindow? window)
    {
        _win = window;
    }

    public virtual void Move(Vector2f offset)
    {
        Pos += offset;
        CurrentSprite.Position = Pos;
    }

    public virtual void Update()
    {
        CurrentSprite.Scale = new Vector2f(
            Size.X / CurrentSprite.GetLocalBounds().Width,
            Size.Y / CurrentSprite.GetLocalBounds().Height);
        _win.Draw(CurrentSprite);
    }
}