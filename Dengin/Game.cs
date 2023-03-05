using SFML.Graphics;
using SFML.Window;
using Dengin.GameObjects;
using SFML.System;

namespace Dengin;

public static class Game
{
    private static readonly RenderWindow Win = new RenderWindow(new VideoMode(480, 480), "Good Morning", Styles.Close);
    private static Player _mainPlayer = new Player(Win);
    private static List<GameObject> _levelObjects = new List<GameObject>()
    {
        _mainPlayer
    };

    public static void Main()
    {
        Win.SetFramerateLimit(60);
        
        Win.Closed += (_, _) => Win.Close();
        
        while (Win.IsOpen) Loop();
    }

    private static void Loop()
    {
        Win.DispatchEvents();
        Win.Clear(Color.Cyan);
        
        Control();
        _levelObjects.ForEach((obj) => obj.Update());
        
        Win.Display();
    }

    private static void Control()
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
        {
            _mainPlayer.Move(new Vector2f(0 ,-_mainPlayer.MoveSpeed));
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.S))
        {
            _mainPlayer.Move(new Vector2f(0 ,_mainPlayer.MoveSpeed));
        }
        
        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
        {
            _mainPlayer.Move(new Vector2f(-_mainPlayer.MoveSpeed, 0));
        }
        else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
        {
            _mainPlayer.Move(new Vector2f(_mainPlayer.MoveSpeed, 0));
        }
    }
}