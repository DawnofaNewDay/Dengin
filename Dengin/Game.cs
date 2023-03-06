using SFML.Graphics;
using SFML.Window;
using Dengin.GameObjects;
using SFML.System;

namespace Dengin;

public static class Game
{
    public static readonly uint TileSize = 8;
    public static readonly uint Scale = 4;
    public static readonly uint TileSizePx = TileSize * Scale;
    
    private static readonly RenderWindow Win = new RenderWindow(new VideoMode(8 * TileSizePx, 8 * TileSizePx), "Good Morning", Styles.Close);
    private static readonly Player MainPlayer = new Player(Win);
    private static List<GameObject> _levelObjects = new List<GameObject>()
    {
        MainPlayer
    };

    private static int[] map =
    {
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        1, 1, 1, 1, 1, 1, 1, 1,
    };
    private static Tilemap tmap = new Tilemap(map, 8, 8, new Vector2f(1, 1));

    public static void Main()
    {
        Win.SetFramerateLimit(60);
        
        Win.Closed += (_, _) => Win.Close();
        
        tmap.Scale = new Vector2f(Scale, Scale);
        
        while (Win.IsOpen) Loop();
    }

    private static void Loop()
    {
        Win.DispatchEvents();
        Win.Clear(Color.Cyan);
        
        Win.Draw(tmap);
        PlayerControl();
        _levelObjects.ForEach((obj) => obj.Update());

        Win.Display();
    }

    private static void PlayerControl()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
        {
            MainPlayer.Move(new Vector2f(-MainPlayer.MoveSpeed, 0));
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
        {
            MainPlayer.Move(new Vector2f(MainPlayer.MoveSpeed, 0));
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Z) && MainPlayer.JumpState == JumpState.Grounded)
        {
            MainPlayer.Jump();
        }
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
        {
            if (MainPlayer.Size.Y == MainPlayer.BaseSize.Y && MainPlayer.JumpState == JumpState.Grounded)
            {
                MainPlayer.Size.Y = MainPlayer.BaseSize.Y / 2;
                MainPlayer.Move(new Vector2f(0, MainPlayer.BaseSize.Y));
            }
            else if (MainPlayer.JumpState == JumpState.Grounded)
                MainPlayer.Size.Y = MainPlayer.BaseSize.Y / 2;
        }
        else
        {
            MainPlayer.Size.Y = MainPlayer.BaseSize.Y;
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
        {
            MainPlayer.MoveSpeed = 6f;
        }
        else
        {
            MainPlayer.MoveSpeed = 3f;
        }
    }
}