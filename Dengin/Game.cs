using System.Drawing;
using SFML.Graphics;
using SFML.Window;
using Dengin.GameObjects;
using SFML.System;
using Color = SFML.Graphics.Color;

namespace Dengin;

public static class Game
{
    public static readonly uint TileSize = 16;
    public static float Scale = 4;
    public static float TileSizePx => TileSize * Scale;

    private static readonly Clock DeltaClock = new();
    public static float DeltaTime;
    
    public static readonly RenderWindow Win = new(new VideoMode((uint)Math.Round(15 * TileSizePx), (uint)Math.Round(10 * TileSizePx)), "Good Morning", Styles.Close);
    public static uint FrameRate = 144;
    
    public static readonly Player MainPlayer = new();
    public static readonly View MainCam = new(MainPlayer.Center, (Vector2f)Win.Size);
    public static List<GameObject> _levelObjects = new()
    {
        MainPlayer
    };

    public static int[] WalkableTiles =
    {
        0, 1, 2, 3, 4, 5, 6, 7, 25, 26, 27, 18, 29, 30, 31, 32, 50, 51, 250, 175, 725, 675, 225, 275, 650, 200, 700
    };
    public static int[,] Map =
    {
        { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 6, 29, 50, 6, 6, 50, 50, 50, 50, 30, 30, 6, 30, 6, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 6, 29, 50, 6, 6, 50, 50, 50, 50, 30, 30, 6, 30, 6, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 6, 29, 50, 6, 6, 50, 50, 50, 50, 30, 30, 6, 30, 6, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 6, 29, 50, 6, 51, 314, 315, 316, 51, 30, 30, 6, 30, 6, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 700, 250, 250, 250, 250, 250, 250, 250, 725, 30, 30, 6, 30, 6, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 200, 175, 175, 175, 175, 175, 175, 175, 225, 578, 603, 6, 30, 6, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 650, 275, 275, 275, 275, 275, 275, 275, 675, 268, 269, 6, 30, 6, 8 },
        { 8, 30, 6, 30, 29, 29, 6, 50, 50, 6, 29, 50, 6, 6, 50, 50, 50, 50, 629, 654, 6, 30, 6, 8 },
        { 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8 },
    };
    public static Vector2u MapDimensions = new(24, 10);
    
    public static Tilemap Tmap = new Tilemap(Utility.ToOneDimArray(Map), MapDimensions.X, MapDimensions.Y, new Vector2u(TileSize, TileSize));

    public static void Main()
    {
        Win.SetFramerateLimit(FrameRate);
        Win.SetView(MainCam);
        
        Win.Closed += (_, _) => Win.Close();
        Win.KeyPressed += (_, e) =>
        {
            if (e.Code == Keyboard.Key.Equal)
            {
                Resize(0.5f);
            }
        };
        Tmap.Scale = new Vector2f(Scale, Scale);
        Console.WriteLine(Tmap.Width * TileSizePx);
        Console.WriteLine(Win.Size);
        
        while (Win.IsOpen) Loop();
    }

    private static void Loop()
    {
        Win.DispatchEvents();
        Win.Clear(Color.Black);
        
        Win.Draw(Tmap);
        if (!MainPlayer.Moving) PlayerControl();
        _levelObjects.ForEach((obj) => obj.Update());
        
        Win.Display();
        
        DeltaTime = DeltaClock.Restart().AsSeconds();
    }

    private static void PlayerControl()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
        {
            MainPlayer.CurrentTexture = Textures.Up;
            MainPlayer.Move(new Vector2f(0, -1));
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            MainPlayer.CurrentTexture = Textures.Down;
            MainPlayer.Move(new Vector2f(0, 1));
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            MainPlayer.CurrentTexture = Textures.Left;
            MainPlayer.Move(new Vector2f(-1, 0));
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            MainPlayer.CurrentTexture = Textures.Right;
            MainPlayer.Move(new Vector2f(1, 0));
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
                if (Keyboard.IsKeyPressed(Keyboard.Key.P))
                    MainCam.Rotation += 10;
        }
        else MainCam.Rotation = 0;
    }

    private static void Resize(float by)
    {
        float newScale = Scale + by;
        _levelObjects.ForEach((obj) =>
        {
            obj.Pos = new Vector2f(
                obj.Pos.X / Scale * newScale,
                obj.Pos.Y / Scale * newScale
            );
            obj.Size = new Vector2f(
                obj.Size.X / Scale * newScale,
                obj.Size.Y / Scale * newScale
            );
        });
        Tmap.Scale = new Vector2f(
            Tmap.Scale.X / Scale * newScale,
            Tmap.Scale.Y / Scale * newScale
        );

        MainCam.Size = MainCam.Size / Scale * newScale;
        MainCam.Center = MainPlayer.Center;
        Win.Size = new Vector2u(
            (uint)Math.Round(Win.Size.X / Scale * newScale),
            (uint)Math.Round(Win.Size.Y / Scale * newScale)
        );
        Scale = newScale;
    }
}