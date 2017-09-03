using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoatTutorial
{
    public class ModifyBoatMesh
    {
        public Vector3[] boatVerticesGlobal;
        public List<TriangleData> UnderwaterTriangleData = new List<TriangleData>();
        public List<TriangleData> AbovewaterTriangleData = new List<TriangleData>();
        public List<SlammingForceData> SlammingForceData = new List<SlammingForceData>();
        public List<int> OriginalTriangleIndex = new List<int>();
        public float BoatArea;

        private float timeSinceStart;   // double? we're not running this for too long, so shouldn't matter too much
        private float[] allDistancesToWater;

        private Transform boatTf;
        Vector3[] boatVertices;
        int[] boatTriangles;
        private Rigidbody boatRb;
        private Mesh underwaterMesh;
        private MeshCollider underwaterMeshCollider;
        private BoatPhysics boatPhysics;

        public ModifyBoatMesh(BoatPhysics boatPhysics, Transform boatTf, Mesh boatMesh, GameObject underwaterObj, GameObject aboveWaterObj, Rigidbody boatRb)
        {
            this.boatPhysics = boatPhysics;
            this.boatTf = boatTf;
            this.boatRb = boatRb;
            underwaterMesh = underwaterObj.GetComponent<MeshFilter>().mesh;
            underwaterMeshCollider = underwaterObj.GetComponent<MeshCollider>();
            
            boatVertices = boatMesh.vertices;
            boatTriangles = boatMesh.triangles;

            boatVerticesGlobal = new Vector3[boatVertices.Length];
            allDistancesToWater = new float[boatVertices.Length];

            for (int i = 0; i < (boatTriangles.Length / 3); i++)
            {
                SlammingForceData.Add(new SlammingForceData());
            }

            CalculateOriginalTrianglesArea();
        }

        public void GenerateUnderwaterMesh()
        {
            UnderwaterTriangleData.Clear();
            AbovewaterTriangleData.Clear();
            OriginalTriangleIndex.Clear();
            timeSinceStart = Time.time;     // Ensures we find the distance to water with the same time

            for (int i = 0; i < SlammingForceData.Count; i++)
            {
                SlammingForceData[i].PreviousSubmergedArea = SlammingForceData[i].SubmergedArea;
            }

            for (int i = 0; i < boatVertices.Length; i++)
            { 
                Vector3 globalPos = boatTf.TransformPoint(boatVertices[i]);
                boatVerticesGlobal[i] = globalPos;
                allDistancesToWater[i] = WaterController.current.DistanceToWater(globalPos, timeSinceStart);
            }

            AddTriangles();
        }

        private void AddTriangles()
        {
            List<VertexData> vertexData = new List<VertexData>();
            vertexData.Add(new VertexData());
            vertexData.Add(new VertexData());
            vertexData.Add(new VertexData());
            
            int i = 0;
            int triangleCounter = 0;
            while (i < boatTriangles.Length)
            {
                for (int j = 0; j < 3; j++)
                {
                    vertexData[j].distance = allDistancesToWater[boatTriangles[i]];
                    vertexData[j].index = j;
                    vertexData[j].globalVertexPos = boatVerticesGlobal[boatTriangles[i]];
                    i++;
                }
                if (vertexData[0].distance > 0f && vertexData[1].distance > 0f && vertexData[2].distance > 0f)
                {
                    // All vertices are above water
                    Vector3 p1 = vertexData[0].globalVertexPos;
                    Vector3 p2 = vertexData[1].globalVertexPos;
                    Vector3 p3 = vertexData[2].globalVertexPos;

                    AbovewaterTriangleData.Add(new TriangleData(p1, p2, p3, boatRb, timeSinceStart));
                    SlammingForceData[triangleCounter].SubmergedArea = 0f;
                    continue;
                }

                if (vertexData[0].distance < 0f && vertexData[1].distance < 0f && vertexData[2].distance < 0f)
                {
                    // All vertices are under water
                    Vector3 p1 = vertexData[0].globalVertexPos;
                    Vector3 p2 = vertexData[1].globalVertexPos;
                    Vector3 p3 = vertexData[2].globalVertexPos;
                    
                    UnderwaterTriangleData.Add(new TriangleData(p1, p2, p3, boatRb, timeSinceStart));

                    SlammingForceData[triangleCounter].SubmergedArea = SlammingForceData[triangleCounter].OriginalArea;
                    OriginalTriangleIndex.Add(triangleCounter);
                }
                else
                {
                    // 1 or 2 vertices are under water
                    vertexData.Sort((x, y) => x.distance.CompareTo(y.distance));
                    vertexData.Reverse();

                    if (vertexData[0].distance > 0f && vertexData[1].distance < 0f && vertexData[2].distance < 0f)
                    {
                        AddTrianglesOneAboveWater(vertexData, triangleCounter);
                    }
                    else if (vertexData[0].distance > 0f && vertexData[1].distance > 0f && vertexData[2].distance < 0f)
                    {
                        AddTrianglesTwoAboveWater(vertexData, triangleCounter);
                    }
                }
                triangleCounter++;
            }
        }

        private void AddTrianglesOneAboveWater(List<VertexData> vertexData, int triangleCounter)
        {
            // The guy writing this tutoiral started using horrible variable names.
            // I have no idea what he meant by them, but we're eventually cutting a square in half
            // to form two triangles.

            Vector3 H = vertexData[0].globalVertexPos;
            int midIndex = vertexData[0].index - 1;
            if (midIndex < 0)
            {
                midIndex = 2;
            }
            
            float heightH = vertexData[0].distance;
            float heightM = 0f;
            float heightL = 0f;

            Vector3 M = Vector3.zero;
            Vector3 L = Vector3.zero;

            if (vertexData[1].index == midIndex)
            {
                M = vertexData[1].globalVertexPos;
                L = vertexData[2].globalVertexPos;
                heightM = vertexData[1].distance;
                heightL = vertexData[2].distance;
            }
            else
            {
                M = vertexData[2].globalVertexPos;
                L = vertexData[1].globalVertexPos;
                heightM = vertexData[2].distance;
                heightL = vertexData[1].distance;
            }
            
            // Again, not my code, but useful variable names would help here..
            // Point I_M
            Vector3 MH = H - M;
            float t_M = -heightM / (heightH - heightM);
            Vector3 MI_M = t_M * MH;
            Vector3 I_M = MI_M + M;

            // Point I_L
            Vector3 LH = H - L;
            float t_L = -heightL / (heightH - heightL);
            Vector3 LI_L = t_L * LH;
            Vector3 I_L = LI_L + L;

            // Two triangles below water
            UnderwaterTriangleData.Add(new TriangleData(M, I_M, I_L, boatRb, timeSinceStart));
            UnderwaterTriangleData.Add(new TriangleData(M, I_L, L, boatRb, timeSinceStart));
            
            // One triangle above water
            AbovewaterTriangleData.Add(new TriangleData(I_M, H, I_L, boatRb, timeSinceStart));
            
            boatPhysics.FoamGenerator.AddIntersectionVertex(I_M);
            boatPhysics.FoamGenerator.AddIntersectionVertex(I_L);

            float totalArea = BoatPhysicsMath.GetTriangleArea(M, I_M, I_L) + BoatPhysicsMath.GetTriangleArea(M, I_L, L);

            SlammingForceData[triangleCounter].SubmergedArea = totalArea;

            // added twice as 2 submerged triangles need to connect to the same original triangle
            OriginalTriangleIndex.Add(triangleCounter);
            OriginalTriangleIndex.Add(triangleCounter);
        }

        private void AddTrianglesTwoAboveWater(List<VertexData> vertexData, int triangleCounter)
        {
            Vector3 L = vertexData[2].globalVertexPos;
            int highIndex = vertexData[2].index + 1;
            if (highIndex > 2)
            {
                highIndex = 0;
            }
            
            float heightL = vertexData[2].distance;
            float heightM = 0f;
            float heightH = 0f;

            Vector3 H = Vector3.zero;
            Vector3 M = Vector3.zero;

            if (vertexData[1].index == highIndex)
            {
                H = vertexData[1].globalVertexPos;
                M = vertexData[0].globalVertexPos;
                heightH = vertexData[1].distance;
                heightM = vertexData[0].distance;
            }
            else
            {
                H = vertexData[0].globalVertexPos;
                M = vertexData[1].globalVertexPos;
                heightH = vertexData[0].distance;
                heightM = vertexData[1].distance;
            }

            // Again, not my code, but useful variable names would help here..
            // Point J_M
            Vector3 LM = M - L;
            float t_M = -heightL / (heightM - heightL);
            Vector3 LJ_M = t_M * LM;
            Vector3 J_M = LJ_M + L;

            // Point J_H
            Vector3 LH = H - L;
            float t_H = -heightL / (heightH - heightL);
            Vector3 LJ_H = t_H * LH;
            Vector3 J_H = LJ_H + L;

            // One triangle below water
            UnderwaterTriangleData.Add(new TriangleData(L, J_H, J_M, boatRb, timeSinceStart));

            // Two triangles above water
            AbovewaterTriangleData.Add(new TriangleData(J_H, H, J_M, boatRb, timeSinceStart));
            AbovewaterTriangleData.Add(new TriangleData(J_M, H, M, boatRb, timeSinceStart));

            boatPhysics.FoamGenerator.AddIntersectionVertex(J_H);
            boatPhysics.FoamGenerator.AddIntersectionVertex(J_M);

            SlammingForceData[triangleCounter].SubmergedArea = BoatPhysicsMath.GetTriangleArea(L, J_H, J_M);
            OriginalTriangleIndex.Add(triangleCounter);
        }

        private class VertexData
        {
            public float distance;  // between water and this
            public int index;
            public Vector3 globalVertexPos;
        }

        public void DisplayMesh(Mesh mesh, string name, List<TriangleData> triangleData)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();
            
            for (int i = 0; i < triangleData.Count; i++)
            {
                Vector3 p1 = boatTf.InverseTransformPoint(triangleData[i].p1);
                Vector3 p2 = boatTf.InverseTransformPoint(triangleData[i].p2);
                Vector3 p3 = boatTf.InverseTransformPoint(triangleData[i].p3);

                vertices.Add(p1);
                triangles.Add(vertices.Count - 1);

                vertices.Add(p2);
                triangles.Add(vertices.Count - 1);

                vertices.Add(p3);
                triangles.Add(vertices.Count - 1);
            }
            
            mesh.Clear();
            mesh.name = name;
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateBounds();
        }

        public float CalculateUnderwaterLength()
        {
            return underwaterMesh.bounds.size.z;
        }

        private void CalculateOriginalTrianglesArea()
        {
            int i = 0;
            int triangleCounter = 0;

            while (i < boatTriangles.Length)
            {
                Vector3 p1 = boatVertices[boatTriangles[i]];
                i++;
                Vector3 p2 = boatVertices[boatTriangles[i]];
                i++;
                Vector3 p3 = boatVertices[boatTriangles[i]];
                i++;

                float triangleArea = BoatPhysicsMath.GetTriangleArea(p1, p2, p3);
                SlammingForceData[triangleCounter].OriginalArea = triangleArea;

                BoatArea += triangleArea;
                triangleCounter += 1;
            }
        }
    }

}
