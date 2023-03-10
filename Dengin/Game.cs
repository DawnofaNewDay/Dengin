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
    
    public static Player MainPlayer;
    public static View MainCam;
    public static List<GameObject> _levelObjects = new();

    public static string Sequence = File.ReadAllText("Data/Maps/Map1");
    public static int[,] Map;
    public static Vector2u MapDimensions;
    public static Vector2i SpawnPos;
    public static Tilemap Tmap;
    public static int[] TmapCollision;

    public static void Main()
    {
        (Map, MapDimensions, SpawnPos) = Utility.SequenceToMap(Sequence);
        Tmap = new Tilemap(Utility.ToOneDimArray(Map), MapDimensions.X, MapDimensions.Y, new Vector2u(TileSize, TileSize));
        TmapCollision = Utility.LoadCollision("Default");

        MainPlayer = new Player();
        MainCam = new(MainPlayer.Center, (Vector2f)Win.Size);
        _levelObjects.Add(MainPlayer);

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