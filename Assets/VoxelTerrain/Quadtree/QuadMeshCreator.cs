using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuadMeshCreator : MonoBehaviour
{
    public bool generate = false;
    public QuadtreeComponent quadtree;
    public Material voxelMaterial;

    private GameObject previousMesh;
    private bool initialized;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (quadtree.Quadtree != null)
        {
            initialized = true;
            quadtree.Quadtree.QuadtreeUpdated += (obj, args) => {generate = true;};
        }
        // Generate mesh once
        if (generate)
        {
            var generatedMesh = GenerateMesh();
            if (previousMesh != null)
            {
                Destroy(previousMesh);
            }
            previousMesh = generatedMesh;
            generate = false;
        }
        
    }

    private GameObject GenerateMesh()
    {
        GameObject chunk = new GameObject();
        chunk.name = "Voxel Chunk";
        chunk.transform.parent = this.transform;
        chunk.transform.localPosition = Vector3.zero;

        var mesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();
        var normals = new List<Vector3>();

        foreach (var leaf in quadtree.Quadtree.GetLeafNodes().Where((node) => node.Data))
        {
            var upperLeft = new Vector3(leaf.Position.x - leaf.Size * 0.5f, leaf.Position.y + leaf.Size * 0.5f, 0);
            var initialIndex = vertices.Count;
            
            vertices.Add(upperLeft);
            vertices.Add(upperLeft + Vector3.right * leaf.Size);
            vertices.Add(upperLeft + Vector3.down * leaf.Size);
            vertices.Add(upperLeft + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

            uvs.Add(upperLeft);
            uvs.Add(upperLeft + Vector3.right * leaf.Size);
            uvs.Add(upperLeft + Vector3.down * leaf.Size);
            uvs.Add(upperLeft + Vector3.down * leaf.Size + Vector3.right * leaf.Size);

            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);

            triangles.Add(initialIndex);
            triangles.Add(initialIndex + 1);
            triangles.Add(initialIndex + 2);

            triangles.Add(initialIndex + 3);
            triangles.Add(initialIndex + 2);
            triangles.Add(initialIndex + 1);
        }

        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);

        var meshFilter = chunk.AddComponent<MeshFilter>();
        var meshRenderer = chunk.AddComponent<MeshRenderer>();
        meshRenderer.material = voxelMaterial;

        meshFilter.mesh = mesh;
        return chunk;
    }
}
