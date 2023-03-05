using SFML.Graphics;
using SFML.Window;
using Dengin.GameObjects;

namespace Dengin;

public static class Game
{
    private static readonly RenderWindow Win = new RenderWindow(new VideoMode(480, 480), "Good Morning", Styles.Close);

    private static List<GameObject> _objects = new List<GameObject>()
    {
        new Player(Win)
    };

    public static void Main()
    {
        Win.Closed += (_, _) => Win.Close();
        
        while (Win.IsOpen) Loop();
    }

    private static void Loop()
    {
        Win.DispatchEvents();
        Win.Clear(Color.Cyan);
        
        _objects.ForEach((obj) => obj.Update());
        
        Win.Display();
    }
}