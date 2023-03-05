using SFML.Graphics;
using SFML.System;

namespace Dengin;

public class GameObject
{
    protected Vector2f Pos = new Vector2f(0, 0);
    protected Sprite CurrentSprite = new Sprite();
    protected Vector2f Size = new Vector2f(64, 64);
    private readonly RenderWindow? _win;

    protected GameObject(RenderWindow? window)
    {
        _win = window;
    }

    public virtual void Update()
    {
        CurrentSprite.Position = Pos;
        CurrentSprite.Scale = new Vector2f(
            Size.X / CurrentSprite.GetLocalBounds().Width,
            Size.Y / CurrentSprite.GetLocalBounds().Height);
        _win.Draw(CurrentSprite);
    }
}