using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadtreePencil : MonoBehaviour
{
    public QuadtreeComponent quadtree;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var insertionPoint = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            quadtree.Quadtree.Insert(insertionPoint.origin, false);
        }
        else if (Input.GetMouseButton(1))
        {
            quadtree.Quadtree.Insert(insertionPoint.origin, true);
        }
    }
}
