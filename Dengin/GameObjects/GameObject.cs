using SFML.Graphics;
using SFML.System;

namespace Dengin.GameObjects;

public class GameObject
{
    protected Vector2f Pos = new Vector2f(0, 0);
    public readonly Sprite CurrentSprite = new Sprite();
    protected readonly RenderWindow? Win;
    public Vector2f Size = new Vector2f(Game.TileSizePx, Game.TileSizePx);

    protected GameObject(RenderWindow? window)
    {
        Win = window; 
    }

    public virtual void Move(Vector2f offset)
    {
        Pos += offset;
        CurrentSprite.Position = Pos;
    }

    public virtual void Update()
    {
        Win.Draw(CurrentSprite);
    }
}