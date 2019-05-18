using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OctreeIndex{
    BottomLeftFront = 0, //000, 
    BottomRightFront = 2, //010,
    BottomLeftBack = 3, //011, 
    BottomRightBack = 1, //001,
    TopLeftFront = 4, //100,
    TopRightFront = 6, // 110,
    TopLeftBack = 7, //111,
    TopRightBack = 5 //101
}

public class Octree<TType> 
{
    private OctreeNode<TType> node; // Root node
    private int depth; // How deep the three can go (resolution) (number of subdivisions of cube)

    public Octree(Vector3 position, float size, int depth)
    {
        node = new OctreeNode<TType>(position, size);
        node.Subdivide(depth);
    }

    public class OctreeNode<TType> 
    {
        Vector3 position;
        float size; // Cube, so all sizes will be identical and not require a Vector3
        OctreeNode<TType>[] subNodes;
        IList<TType> value;

        public IEnumerable<OctreeNode<TType>> Nodes 
        { 
            get { return subNodes; } 

        }

        public Vector3 Position 
        { 
            get {return Position;}
        }
        public float Size 
        { 
            get {return Size;}
        }

        public OctreeNode(Vector3 pos, float size) 
        {
            position = pos;
            this.size = size;
        }

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

    private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition)
    {
        int index = 0;

        index |= lookupPosition.y > nodePosition.y ? 4 : 0;
        index |= lookupPosition.x > nodePosition.x ? 2 : 0;
        index |= lookupPosition.z > nodePosition.z ? 1 : 0;

        return index;
    }

    public OctreeNode<TType> GetRoot()
    {
        return node;
    }
}

