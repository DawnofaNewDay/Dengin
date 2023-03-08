using SFML.Graphics;
using SFML.System;

namespace Dengin.GameObjects;

public class GameObject
{
    public Vector2f Pos = new Vector2f(0, 0);
    public readonly Sprite CurrentSprite = new Sprite();
    public Vector2f Size = new Vector2f(Game.TileSizePx, Game.TileSizePx);

    public virtual void Move(Vector2f offset)
    {
        Pos += offset;
        CurrentSprite.Position = Pos;
    }

    public virtual void Update()
    {
        CurrentSprite.Scale = new Vector2f(Size.X / CurrentSprite.Texture.Size.X, Size.Y / CurrentSprite.Texture.Size.Y);
        CurrentSprite.Position = Pos;
        Game.Win.Draw(CurrentSprite);
    }
}