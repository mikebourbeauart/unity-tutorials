using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadMeshCreator : MonoBehaviour
{
    public bool generate = false;
    public QuadtreeComponent quadtree;

    // Need references to these in order to generate mesh
    private MeshFilter filter;
    private MeshRenderer renderer;
    private BoxCollider box;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Generate mesh once
        if (generate)
        {
            GenerateMesh();
            generate = false;
        }
        
    }

    private void GenerateMesh()
    {
        foreach (var leaf in quadtree.Quadtree.GetLeafNodes())
        {
            //if (leaf)
            var gobj = GameObject.CreatePrimitive(PrimitiveType.Quad);
            // Parent gameobject under quadtree transform
            gobj.transform.parent = quadtree.transform;
            // Center it at quadtree node transform
            gobj.transform.position = leaf.Position;
            gobj.transform.localScale = Vector3.one * leaf.Size;
        }
    }
}
