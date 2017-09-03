using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoamGenerator {


    private List<Vector3> intersectionVertices = new List<Vector3>();

    private Mesh boatMesh;
    private Mesh foamMesh;

    public FoamGenerator(MeshFilter boatMesh, MeshFilter foamMesh)
    {
        this.boatMesh = boatMesh.mesh;
        this.foamMesh = foamMesh.mesh;
    }

    public void UpdateFoam()
    {
        foreach (Vector3 vertex in intersectionVertices)
        {
            Debug.DrawLine(vertex, vertex + Vector3.up, Color.green);
        }
    }

    public void AddIntersectionVertex(Vector3 vertex)
    {
        intersectionVertices.Add(vertex);
    }

    public void ClearVertices()
    {
        intersectionVertices = new List<Vector3>();
    }
}
