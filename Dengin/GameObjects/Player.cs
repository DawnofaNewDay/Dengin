using SFML.Audio;
using SFML.Graphics;
using SFML.System;

namespace Dengin.GameObjects;

public enum JumpState
{
    Grounded = 0,
    Jumping = 1,
    Falling = 2
}

public enum Textures
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public enum SFX
{
    Bump = 0
}

public class Player : GameObject
{
    public bool Moving = false;
    public float MoveTime = 0.03f;
    public float LerpTime = 0f;
    
    public Vector2f OldPos;
    public Vector2f Destination;
    public Vector2f Center => new(Pos.X + Size.X / 2, Pos.Y + Size.Y / 2);

    public Textures CurrentTexture = Textures.Right;
    public float Ratio => ((float)CurrentSprite.Texture.Size.Y / (float)CurrentSprite.Texture.Size.X);
    public Vector2f SpriteSize => new(Size.X, Size.Y * Ratio);
    public readonly Texture[] MyTextures =
    {
        new Texture("./Data/Images/Player/up.png"),
        new Texture("./Data/Images/Player/down.png"),
        new Texture("./Data/Images/Player/left.png"),
        new Texture("./Data/Images/Player/right.png")
    };
    
    public readonly Sound[] SfXs =
    {
        new(new SoundBuffer("./Data/Sound/SFX/bump.wav"))
    };

    public Player()
    {
        CurrentSprite.Texture = MyTextures[(int)Textures.Right];
        Pos = new Vector2f(Game.SpawnPos.X * Game.TileSizePx, Game.SpawnPos.Y * Game.TileSizePx);
    }

    public override void Move(Vector2f offset)
    {
        offset = new Vector2f(offset.X * Game.TileSizePx, offset.Y * Game.TileSizePx);
        OldPos = Pos;
        Destination = Pos + offset;
        if (!Utility.IsWalkable(Utility.PullTile(Destination)))
        {
            PlaySfx(SFX.Bump, 20);
            return;
        }
        Moving = true;
        LerpTime = 0;
    }

    public override void Update()
    {
        if (Moving)
        {
            LerpTime += 0.1f * Game.DeltaTime;
            Vector2f predictedPos = new Vector2f(
                Utility.Lerp(OldPos.X, Destination.X, LerpTime / MoveTime), 
                Utility.Lerp(OldPos.Y, Destination.Y, LerpTime / MoveTime)
            );
            if (LerpTime >= MoveTime)
            {
                Pos = Destination;
                Moving = false;
                LerpTime = 0;
            }
            else
            {
                Pos = predictedPos;
            }
        }
        
        CurrentSprite.Texture = MyTextures[(int)CurrentTexture];
        Game.MainCam.Center = new Vector2f((int)Math.Round(Center.X), (int)Math.Round(Center.Y));
        Game.Win.SetView(Game.MainCam);

        CurrentSprite.Scale = new Vector2f(SpriteSize.X / CurrentSprite.Texture.Size.X, SpriteSize.Y / CurrentSprite.Texture.Size.Y);
        Pos = new Vector2f((int)Math.Round(Pos.X), (int)Math.Round(Pos.Y));
        CurrentSprite.Position = new Vector2f((int)Math.Round(Pos.X), (int)Math.Round(Pos.Y - (SpriteSize.Y - SpriteSize.X)));
        
        Game.Win.Draw(CurrentSprite);
    }

    public void PlaySfx(SFX index, int volume = 20)
    {
        if (SfXs[(int)index].Status != SoundStatus.Playing)
        {
            SfXs[(int)index].Volume = volume;
            SfXs[(int)index].Play();
        }
    }
}
