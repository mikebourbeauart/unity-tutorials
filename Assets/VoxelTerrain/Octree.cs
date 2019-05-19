using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Not used, but will help visualize where in the cube is the index
// Bottom = 0, Top = 1 | Left = 0, Right = 1 | Front = 0, Back = 1
public enum OctreeIndex{
    BottomLeftFront = 0, //000, 
    BottomLeftBack = 1, //001, 
    BottomRightFront = 2, //010,
    BottomRightBack = 3, //011,
    TopLeftFront = 4, //100,
    TopLeftBack = 5, //101,
    TopRightFront = 6, // 110,
    TopRightBack = 7 //111
}

// Octree structure
public class Octree<TType> 
{
    private OctreeNode<TType> node; // Root node
    private int depth; // How deep the three can go (resolution or number of subdivisions of cube)

    public Octree(Vector3 position, float size, int depth)
    {
        node = new OctreeNode<TType>(position, size);
        node.Subdivide(depth);
    }

    // Node
    public class OctreeNode<TType> // Inside the Octree class so it will only be used within an octree
    {
        Vector3 position; // Position of the node itself
        float size; // Cube, so all sizes will be identical and not require a Vector3
        OctreeNode<TType>[] subNodes;
        IList<TType> value; // IList so we can add and remove things

        public IEnumerable<OctreeNode<TType>> Nodes 
        { 
            get { return subNodes; } 
        }

        public Vector3 Position 
        { 
            get {return position;}
        }
        public float Size 
        { 
            get {return size;}
        }

        public OctreeNode(Vector3 pos, float size) 
        {
            position = pos;
            this.size = size;
        }

        // Subdivides the node
        public void Subdivide(int depth = 0)
        {
            subNodes = new OctreeNode<TType>[8];
            for (int i = 0; i < subNodes.Length; ++i)
            {
                Vector3 newPos = position;
                if ((i & 4) == 4)
                {
                    newPos.y += size * 0.25f;
                }
                else
                {
                    newPos.y -= size * 0.25f;
                }

                if ((i & 2) == 2)
                {
                    newPos.x += size * 0.25f;
                }
                else
                {
                    newPos.x -= size * 0.25f;
                }

                if ((i & 1) == 1)
                {
                    newPos.z += size * 0.25f;
                }
                else
                {
                    newPos.z -= size * 0.25f;
                }

                subNodes[i] = new OctreeNode<TType>(newPos, size * 0.5f);
                if (depth > 0)
                {
                    subNodes[i].Subdivide(depth-1);
                }
            }
        }

        public bool IsLeaf()
        {
            return subNodes == null;
        }
    }

    // Given a position, where is it within the cube?
    private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition)
    {
        int index = 0;

        index |= lookupPosition.x > nodePosition.x ? 2 : 0; // Left/Right
        index |= lookupPosition.y > nodePosition.y ? 4 : 0; // Top/Bottom
        index |= lookupPosition.z > nodePosition.z ? 1 : 0; // Front/Back

        return index;
    }

    public OctreeNode<TType> GetRoot()
    {
        return node;
    }
}

