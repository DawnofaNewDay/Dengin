using SFML.Graphics;
using SFML.Window;
using Dengin.GameObjects;
using SFML.System;

namespace Dengin;

public static class Game
{
    public static readonly uint TileSize = 16;
    public static readonly uint Scale = 6;
    public static readonly uint TileSizePx = TileSize * Scale;
    
    private static readonly RenderWindow Win = new RenderWindow(new VideoMode(8 * TileSizePx, 8 * TileSizePx), "Good Morning", Styles.Close);
    private static readonly Player MainPlayer = new Player(Win);
    private static List<GameObject> _levelObjects = new List<GameObject>()
    {
        MainPlayer
    };

    public static int[,] Map =
    {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 4, 4, 4, 4, 4, 4, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 3, 0 },
        { 3, 0, 0, 0, 0, 0, 0, 3, 0 },
        { 0, 0, 0, 3, 0, 3, 0, 3, 0 },
        { 1, 1, 1, 1, 1, 1, 1, 1, 0 },
    };
    public static Vector2u MapDimensions = new(9, 8);
    
    public static Tilemap Tmap = new Tilemap(Utility.ToOneDimArray(Map), MapDimensions.X, MapDimensions.Y, new Vector2u(TileSize, TileSize));

    public static void Main()
    {
        Win.SetFramerateLimit(60);
        
        Win.Closed += (_, _) => Win.Close(); 
        Tmap.Scale = new Vector2f(Scale, Scale);
        Console.WriteLine(Tmap.Width * TileSizePx);
        Console.WriteLine(Win.Size);
        
        while (Win.IsOpen) Loop();
    }

    private static void Loop()
    {
        Win.DispatchEvents();
        Win.Clear(Color.Cyan);
        
        Win.Draw(Tmap);
        PlayerControl();
        _levelObjects.ForEach((obj) => obj.Update());

        Win.Display();
    }

    private static void PlayerControl()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            if (MainPlayer.CurrentTexture != Textures.Left)
            {
                MainPlayer.CurrentTexture = Textures.Left;
                MainPlayer.CurrentSprite.Texture = MainPlayer.MyTextures[(int)Textures.Left];
            }
            MainPlayer.Move(new Vector2f(-MainPlayer.MoveSpeed, 0));
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            if (MainPlayer.CurrentTexture != Textures.Right)
            {
                MainPlayer.CurrentTexture = Textures.Right;
                MainPlayer.CurrentSprite.Texture = MainPlayer.MyTextures[(int)Textures.Right];
            }
            MainPlayer.Move(new Vector2f(MainPlayer.MoveSpeed, 0));
        }

        if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && MainPlayer.JumpState == JumpState.Grounded)
        {
            MainPlayer.Jump();
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