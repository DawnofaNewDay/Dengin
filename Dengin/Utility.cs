using SFML.System;

namespace Dengin;

public static class Utility
{
    public static float Lerp(float firstFloat, float secondFloat, float by)
    {
        return firstFloat * (1 - by) + secondFloat * by;
    }
    
    public static Vector2i ToTilePos(Vector2f pos)
    {
        return new Vector2i((int)Math.Floor(pos.X / Game.TileSizePx), (int)Math.Floor(pos.Y / Game.TileSizePx));
    }

    public static int PullTile(Vector2f pos)
    {
        if (pos.X < 0 
            || pos.Y < 0 
            || pos.X > (Game.MapDimensions.X - 1) * Game.TileSizePx 
            || pos.Y > (Game.MapDimensions.Y - 1) * Game.TileSizePx)
            return 0;
        
        return Game.Map[ToTilePos(pos).Y, ToTilePos(pos).X];
    }
    
    public static int[] ToOneDimArray(int[,] twoDimArray)
    {
        int[] oneDimArray = new int[twoDimArray.Length];
        int i = 0;
        foreach (int element in twoDimArray)
        {
            oneDimArray[i] = element;
            i++;
        }
        return oneDimArray;
    }
    
    public static bool IsWalkable(int Tile)
    {
        return !Game.TmapCollision.Contains(Tile);
    }

    public static (int[,], Vector2u, Vector2i) SequenceToMap(string sequence)
    {
        string[] sequenceArray = sequence.Split('-');
        int[,] map = new int[int.Parse(sequenceArray[1]), int.Parse(sequenceArray[0])];

        string[] tileSequence = sequenceArray[2].Split(".");

        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                Console.WriteLine(tileSequence[y * map.GetLength(1) + x].ToString());
                map[y, x] = int.Parse(tileSequence[y * map.GetLength(1) + x].ToString());
            }
        }

        return (
            map, 
            new Vector2u((uint)map.GetLength(1), (uint)map.GetLength(0)), 
            new Vector2i(int.Parse(sequenceArray[3]), int.Parse(sequenceArray[4]))
        );
    }

    public static int[] LoadCollision(string filename)
    {
        return File.ReadAllText($"Data/Images/Tilemaps/{filename}.collision").Split('.').Select(int.Parse).ToArray();
    }
}