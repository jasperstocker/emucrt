using UnityEngine;

public static class LUTGenerator
{
    public enum Types
    {
        SingleVertical,
        SingleHorizontal,
        TripleVertical,
        TripleHorizontal,
    }

    public static Texture2D Generate(Vector2Int displayResolution, Types type)
    {
        switch (type)
        {
            default:
                return GenerateSingleVertical(displayResolution);
            case Types.TripleVertical:
                return GenerateTripleVertical(displayResolution);
            case Types.SingleHorizontal:
                return GenerateSingleHorizontal(displayResolution);
            case Types.SingleVertical:
                return GenerateSingleVertical(displayResolution);
        }
    }
    
    public static Texture2D GenerateSingleHorizontal(Vector2Int displayResolution)
    {
        Texture2D output = new Texture2D(displayResolution.x, displayResolution.y);
        output.filterMode = FilterMode.Point;
        output.wrapMode = TextureWrapMode.Clamp;

        Color[] col = new Color[displayResolution.x * displayResolution.y];
        
        for (int x = 0; x < displayResolution.x; x++)
        {
            int px = x % 4;
            for (int y = 0; y < displayResolution.y; y++)
            {
                Color pixel = Color.black;
                
                int py = y % 8;
                
                if(py == 0 && px == 0) pixel.r = 1;
                if(py == 0 && px == 1) pixel.r = 1;
                if(py == 0 && px == 2) pixel.r = 1;
                if(py == 0 && px == 3) pixel.r = 0.5f;

                //green
                if(py == 1 && px == 0) pixel.g = 1;
                if(py == 1 && px == 1) pixel.g = 1;
                if(py == 1 && px == 2) pixel.g = 1;
                if(py == 1 && px == 3) pixel.g = 0.5f;

                //blue
                if(py == 2 && px == 0) pixel.b = 1;
                if(py == 2 && px == 1) pixel.b = 1;
                if(py == 2 && px == 2) pixel.b = 1;
                if(py == 2 && px == 3) pixel.b = 0.5f;
                
                //second phosphor 
                //red
                if(py == 4 && px == 0) pixel.r = 1;
                if(py == 4 && px == 1) pixel.r = 1;
                if(py == 4 && px == 2) pixel.r = 0.5f;
                if(py == 4 && px == 3) pixel.r = 1;

                //green
                if(py == 5 && px == 0) pixel.g = 1;
                if(py == 5 && px == 1) pixel.g = 1;
                if(py == 5 && px == 2) pixel.g = 0.5f;
                if(py == 5 && px == 3) pixel.g = 1;

                //blue
                if(py == 6 && px == 0) pixel.b = 1;
                if(py == 6 && px == 1) pixel.b = 1;
                if(py == 6 && px == 2) pixel.b = 0.5f;
                if(py == 6 && px == 3) pixel.b = 1;
                
                int index = x + y * displayResolution.x;
                col[index] = pixel;
            }
        }
        
        output.SetPixels(col);
        output.Apply(false);
        return output;
    }
    
    public static Texture2D GenerateSingleVertical(Vector2Int displayResolution)
    {
        Texture2D output = new Texture2D(displayResolution.x, displayResolution.y);
        output.filterMode = FilterMode.Point;
        output.wrapMode = TextureWrapMode.Clamp;

        Color[] col = new Color[displayResolution.x * displayResolution.y];
        
        for (int x = 0; x < displayResolution.x; x++)
        {
            int px = x % 8;
            for (int y = 0; y < displayResolution.y; y++)
            {
                Color pixel = Color.black;
                
                int py = y % 4;
                
                if(px == 0 && py == 0) pixel.r = 1;
                if(px == 0 && py == 1) pixel.r = 1;
                if(px == 0 && py == 2) pixel.r = 1;
                if(px == 0 && py == 3) pixel.r = 0.5f;

                //green
                if(px == 1 && py == 0) pixel.g = 1;
                if(px == 1 && py == 1) pixel.g = 1;
                if(px == 1 && py == 2) pixel.g = 1;
                if(px == 0 && py == 3) pixel.g = 0.5f;

                //blue
                if(px == 2 && py == 0) pixel.b = 1;
                if(px == 2 && py == 1) pixel.b = 1;
                if(px == 2 && py == 2) pixel.b = 1;
                if(px == 0 && py == 3) pixel.b = 0.5f;
                
                //sceond phosphor 
                //red
                if(px == 4 && py == 0) pixel.r = 1;
                if(px == 4 && py == 1) pixel.r = 1;
                if(px == 4 && py == 2) pixel.r = 0.5f;
                if(px == 4 && py == 3) pixel.r = 1;

                //green
                if(px == 5 && py == 0) pixel.g = 1;
                if(px == 5 && py == 1) pixel.g = 1;
                if(px == 4 && py == 2) pixel.g = 0.5f;
                if(px == 5 && py == 4) pixel.g = 1;

                //blue
                if(px == 6 && py == 0) pixel.b = 1;
                if(px == 6 && py == 1) pixel.b = 1;
                if(px == 4 && py == 2) pixel.b = 0.5f;
                if(px == 6 && py == 4) pixel.b = 1;
                
                int index = x + y * displayResolution.x;
                col[index] = pixel;
            }
        }
        
        output.SetPixels(col);
        output.Apply(false);
        return output;
    }

    public static Texture2D GenerateTripleVertical(Vector2Int displayResolution)
    {
        Texture2D output = new Texture2D(displayResolution.x, displayResolution.y);
        output.filterMode = FilterMode.Point;
        output.wrapMode = TextureWrapMode.Clamp;

        Color[] col = new Color[displayResolution.x * displayResolution.y];

        for (int x = 0; x < displayResolution.x; x++)
        {
            int px = x % 14;
            for (int y = 0; y < displayResolution.y; y++)
            {
                Color pixel = Color.black;

                int py = y % 6;

                if (px == 0 && py == 0) pixel.r = 1;
                if (px == 0 && py == 1) pixel.r = 1;
                if (px == 0 && py == 2) pixel.r = 1;
                if (px == 0 && py == 3) pixel.r = 1;
                if (px == 0 && py == 4) pixel.r = 1;

                if (px == 1 && py == 0) pixel.r = 1;
                if (px == 1 && py == 1) pixel.r = 1;
                if (px == 1 && py == 2) pixel.r = 1;
                if (px == 1 && py == 3) pixel.r = 1;
                if (px == 1 && py == 4) pixel.r = 1;

                //green
                if (px == 2 && py == 0) pixel.g = 1;
                if (px == 2 && py == 1) pixel.g = 1;
                if (px == 2 && py == 2) pixel.g = 1;
                if (px == 2 && py == 3) pixel.g = 1;
                if (px == 2 && py == 4) pixel.g = 1;

                if (px == 3 && py == 0) pixel.g = 1;
                if (px == 3 && py == 1) pixel.g = 1;
                if (px == 3 && py == 2) pixel.g = 1;
                if (px == 3 && py == 3) pixel.g = 1;
                if (px == 3 && py == 4) pixel.g = 1;

                //blue
                if (px == 4 && py == 0) pixel.b = 1;
                if (px == 4 && py == 1) pixel.b = 1;
                if (px == 4 && py == 2) pixel.b = 1;
                if (px == 4 && py == 3) pixel.b = 1;
                if (px == 4 && py == 4) pixel.b = 1;

                if (px == 5 && py == 0) pixel.b = 1;
                if (px == 5 && py == 1) pixel.b = 1;
                if (px == 5 && py == 2) pixel.b = 1;
                if (px == 5 && py == 3) pixel.b = 1;
                if (px == 5 && py == 4) pixel.b = 1;

                //sceond phosphor 
                //red
                if (px == 7 && py == 0) pixel.r = 1;
                if (px == 7 && py == 1) pixel.r = 1;
                if (px == 7 && py == 5) pixel.r = 1;
                if (px == 7 && py == 3) pixel.r = 1;
                if (px == 7 && py == 4) pixel.r = 1;

                if (px == 8 && py == 0) pixel.r = 1;
                if (px == 8 && py == 1) pixel.r = 1;
                if (px == 8 && py == 5) pixel.r = 1;
                if (px == 8 && py == 3) pixel.r = 1;
                if (px == 8 && py == 4) pixel.r = 1;

                //green
                if (px == 9 && py == 0) pixel.g = 1;
                if (px == 9 && py == 1) pixel.g = 1;
                if (px == 9 && py == 5) pixel.g = 1;
                if (px == 9 && py == 3) pixel.g = 1;
                if (px == 9 && py == 4) pixel.g = 1;

                if (px == 10 && py == 0) pixel.g = 1;
                if (px == 10 && py == 1) pixel.g = 1;
                if (px == 10 && py == 5) pixel.g = 1;
                if (px == 10 && py == 3) pixel.g = 1;
                if (px == 10 && py == 4) pixel.g = 1;

                //blue
                if (px == 11 && py == 0) pixel.b = 1;
                if (px == 11 && py == 1) pixel.b = 1;
                if (px == 11 && py == 5) pixel.b = 1;
                if (px == 11 && py == 3) pixel.b = 1;
                if (px == 11 && py == 4) pixel.b = 1;

                if (px == 12 && py == 0) pixel.b = 1;
                if (px == 12 && py == 1) pixel.b = 1;
                if (px == 12 && py == 5) pixel.b = 1;
                if (px == 12 && py == 3) pixel.b = 1;
                if (px == 12 && py == 4) pixel.b = 1;

                int index = x + y * displayResolution.x;
                col[index] = pixel;
            }
        }

        output.SetPixels(col);
        output.Apply(false);
        return output;
    }
}
