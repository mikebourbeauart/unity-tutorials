using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageToVoxelGenerator : MonoBehaviour
{

    public Texture2D image;
    public QuadtreeComponent quadtree;
    public float threshold = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Generate()
    {
        // Pow function will tell us numb of rows across of the quadtree
        int cells = (int)Mathf.Pow(2, quadtree.depth); 
        for (int x = 0; x < cells; ++ x)
        {
            for (int y = 0; y < cells; ++y)
            {
                // Get quadtree pos in world space
                Vector2 position = quadtree.transform.position;

                // Locate something in quadtree
                position.x += (x - cells / 2) / (float)cells * quadtree.size; 
                position.y += (y - cells / 2) / (float)cells * quadtree.size; 

                var pixel = image.GetPixelBilinear(x / (float)cells, y / (float)cells);
                
                if (pixel.r > threshold)
                {
                    //Fill cells
                    quadtree.Quadtree.Insert(position, true);

                }
            }
        }
    }
}
