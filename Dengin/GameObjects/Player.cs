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
    Right = 0,
    Left = 1
}

public class Player : GameObject
{
    public Vector2i TilePos;
    private float ratio;
    
    public float MoveSpeed = 3f;
    public float GravitySpeed = 1f * Game.Scale;
    public float JumpHeight = 2 * Game.TileSizePx;
    public Vector2f BaseSize;

    public JumpState JumpState = JumpState.Falling;
    public float JumpTime = 4;
    public float JumpCounter;
    public Vector2f JumpStart;
    
    public Textures CurrentTexture = Textures.Right;
    public Texture[] MyTextures =
    {
        new Texture("./Resources/Images/Player/right.png"),
        new Texture("./Resources/Images/Player/left.png"),
    };

    public Player(RenderWindow? window) : base(window)
    {
        CurrentSprite.Texture = MyTextures[(int)Textures.Right];
        BaseSize = Size;
        Pos.Y = 6 * Game.TileSizePx;
        ratio = (float)CurrentSprite.Texture.Size.X / (float)CurrentSprite.Texture.Size.Y;
        
        Size = new Vector2f(ratio * Game.TileSizePx, Game.TileSizePx);
    }

    public override void Move(Vector2f offset)
    {
        Vector2f hypoPos = Pos + offset;

        if (hypoPos.X + Size.X > Win.Size.X)
        {
            offset.X = Win.Size.X - (Pos.X + Size.X);
            base.Move(offset);
            return;
        }
        if (hypoPos.X < 0)
        {
            offset.X = -Pos.X;
            base.Move(offset);
            return;
        }
        if (hypoPos.Y + Size.Y > Win.Size.Y) 
            offset.Y = Win.Size.Y - (Pos.Y + Size.Y);
        if (hypoPos.Y < 0) 
        {
            offset.X = -Pos.Y;
            base.Move(offset);
            return;
        }
        
        if (Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X, hypoPos.Y + Size.Y - 1)).Y, 
                Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X, hypoPos.Y + Size.Y - 1)).X] != 0
            || Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X, hypoPos.Y)).Y, 
                Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X, hypoPos.Y)).X] != 0)
        {
            Pos.X = Game.TileSizePx * Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X, hypoPos.Y)).X - Size.X;
            return;
        }

        if (Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y + Size.Y - 1)).Y,
                Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y + Size.Y - 1)).X] != 0
            || Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y)).Y,
                Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y)).X] != 0)
        {
            Pos.X = Game.TileSizePx * (Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y)).X + 1);
            return;
        }
        
        base.Move(offset);
    }

    public override void Update()
    {
        TilePos = new Vector2i((int)Math.Floor(Pos.X / Game.TileSizePx), (int)Math.Floor(Pos.Y / Game.TileSizePx));
        
        Vector2f offset = new Vector2f();
        if (JumpState != JumpState.Jumping)
        {
            if (Pos.Y + GravitySpeed + Size.Y > Win.Size.Y)
                offset.Y = Win.Size.Y - (Pos.Y + Size.Y);
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

        Vector2f hypoPos = Pos + offset;

        if (Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y)).Y,
                Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y)).X] != 0
            || Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X - 1, hypoPos.Y)).Y,
                Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X - 1, hypoPos.Y)).X] != 0)
        {
            offset.Y = Game.TileSizePx * Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y + Size.Y)).Y - Pos.Y;
            JumpState = JumpState.Falling;
        }

        if (JumpState != JumpState.Jumping)
        {
            if (Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y + Size.Y - 1)).Y,
                    Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y - 1)).X] != 0
                || Game.Map[Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X - 1, hypoPos.Y + Size.Y - 1)).Y,
                    Utility.ToTilePos(new Vector2f(hypoPos.X + Size.X - 1, hypoPos.Y - 1)).X] != 0)
            {
                offset.Y = (Game.TileSizePx * Utility.ToTilePos(new Vector2f(hypoPos.X, hypoPos.Y)).Y - Pos.Y) * Size.Y / BaseSize.Y;
                JumpState = JumpState.Grounded;
            }
            else
                JumpState = JumpState.Falling;   
        }

        CurrentSprite.Scale = new Vector2f(
            Size.X / CurrentSprite.GetLocalBounds().Width,
            Size.Y / CurrentSprite.GetLocalBounds().Height);
        
        
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
