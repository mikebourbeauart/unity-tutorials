using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuadtreeIndex{
	TopLeft = 0,     //00, 
	TopRight = 1,    //01,
	BottomLeft = 2,  //10, 
	BottomRight = 3, //11,
}

// 
public class Quadtree<TType> 
{
	private QuadtreeNode<TType> node; // Root node
	private int depth; // How deep the three can go (resolution) (number of subdivisions of cube)


	public Quadtree(Vector2 position, float size, int depth)
	{
		node = new QuadtreeNode<TType>(position, size, depth);
		this.depth = depth;
		// node.Subdivide(depth);
	}

	public void Insert( Vector2 position, TType value)
	{
		var leafNode= node.Subdivide(position, value, depth);
		leafNode.Data = value;
	}

	public class QuadtreeNode<TType> // Inside the Octree class so it will only be used within an octree
	{
		Vector2 position; // Position of the node itself
		float size; // Cube, so all sizes will be identical and not require a Vector3
		QuadtreeNode<TType>[] subNodes;
		TType data; // IList so we can add and remove things
		int depth; // Will use to get leaf node

		public IEnumerable<QuadtreeNode<TType>> Nodes 
		{ 
			get { return subNodes; } 

		}

		public QuadtreeNode(Vector2 pos, float size, int depth, TType value = default(TType)) 
		{
			position = pos;
			this.size = size;
			this.depth = depth;
		}
		
		public Vector2 Position 
		{ 
			get { return position; }
		}

		public float Size 
		{ 
			get { return size; }
		}

		public TType Data
		{
			get { return data; }

			// Only extensions of this class can set it
			internal set { this.data = value; }
		}
		
		// Subdivides the node	
		public QuadtreeNode<TType> Subdivide(Vector2 targetPosition, TType value, int depth = 0)
		{
			
			if (depth == 0)
			{
				// Go all the way down to depth of zero
				return this;
			}

			var subdivIndex = GetIndexOfPosition(targetPosition, position);

			if (subNodes == null)
			{
				subNodes = new QuadtreeNode<TType>[4];

				for (int i = 0; i < subNodes.Length; ++i)
				{
					Vector2 newPos = position;
					if ((i & 2) == 2)
					{
						newPos.y -= size * 0.25f;
					}
					else
					{
						newPos.y += size * 0.25f;
					}

					if ((i & 1) == 1)
					{
						newPos.x += size * 0.25f;
					}
					else
					{
						newPos.x -= size * 0.25f;
					}

					subNodes[i] = new QuadtreeNode<TType>(newPos, size * 0.5f, depth - 1);

				}
			}


			// Return index that we're looking for
			return subNodes[subdivIndex].Subdivide(targetPosition, value, depth-1);

		}

		public bool IsLeaf()
		{
			return depth == 0;
		}

		public IEnumerable<QuadtreeNode<TType>> GetLeafNodes()
		{
			if (IsLeaf())
			{
				// Return current node
				yield return this;
			}
			// Otherwise recursively call each ndoe
			else
			{
				if (Nodes != null)
					{					
					foreach(var node in Nodes)
					{
						foreach (var leaf in node.GetLeafNodes())
						{
							yield return leaf;
						}
					}
				}
			}
		}
	}

	// Given a position, where is it within the square?
	private static int GetIndexOfPosition(Vector2 lookupPosition, Vector2 nodePosition)
	{
		int index = 0;

		index |= lookupPosition.y < nodePosition.y ? 2 : 0;
		index |= lookupPosition.x > nodePosition.x ? 1 : 0;

		return index;
	}

	public QuadtreeNode<TType> GetRoot()
	{
		return node;
	}

	public IEnumerable<QuadtreeNode<TType>> GetLeafNodes()
	{
		return node.GetLeafNodes();
	}
}

