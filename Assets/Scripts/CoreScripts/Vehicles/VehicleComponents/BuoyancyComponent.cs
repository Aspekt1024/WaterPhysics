using UnityEngine;
using System.Collections;

public class BuoyancyComponent : MonoBehaviour {
    public float BounceDampening;
    public bool ShowBuoyancyEdges;
    
    private Rigidbody body;
    private Mesh submarineMesh;
    private WaterControl waterControl;

    private float sprayTimer;
    private const float sprayInterval = 0.01f;
    
    private enum State
    {
        None, Enabled, Disabled
    }
    private State state;

    private void Start()
    {
        body = GetComponentInParent<Rigidbody>();
        submarineMesh = GetComponent<MeshFilter>().mesh;
        waterControl = FindObjectOfType<WaterControl>();
    }

    private void FixedUpdate()
    {
        switch(state)
        {
            case State.None:
                break;
            case State.Disabled:
                break;
            case State.Enabled:
                ApplyBuoyancy();
                break;
        }

    }

    public void SetEnabled()
    {
        state = State.Enabled;
    }

    public void SetDisabled()
    {
        state = State.Disabled;
    }

    private void ApplyBuoyancy()
    {
        Vector3[] vertices = new Vector3[3];
        for (int i = 0; i < submarineMesh.triangles.Length; i += 3)
        {
            vertices[0] = transform.TransformPoint(submarineMesh.vertices[submarineMesh.triangles[i]]);
            vertices[1] = transform.TransformPoint(submarineMesh.vertices[submarineMesh.triangles[i + 1]]);
            vertices[2] = transform.TransformPoint(submarineMesh.vertices[submarineMesh.triangles[i + 2]]);

            Vector3 triangleCenter = GetCenterPoint(vertices);
            float triangleArea = GetTriangleArea(vertices);
            float waterHeightAtCenter = waterControl.GetWaveYPos(triangleCenter);
            CalculateForceAtPosition(triangleCenter, triangleArea, waterHeightAtCenter);

            if (ShowBuoyancyEdges)
            {
                CheckIntersectingVertices(vertices, triangleCenter, waterHeightAtCenter);
            }
        }
    }

    private void CheckIntersectingVertices(Vector3[] vertices, Vector3 center, float waterHeight)
    {
        float highestY = vertices[0].y;
        float lowestY = vertices[0].y;

        for (int i = 1; i < 3; i++)
        {
            if (vertices[i].y < lowestY)
            {
                lowestY = vertices[i].y;
            }
            if (vertices[i].y > highestY)
            {
                highestY = vertices[i].y;
            }
        }

        // Show edges intersecting the water because... it looks cool
        Vector3 point = center;
        point.y = waterHeight;
        if (highestY > point.y && lowestY < point.y)
        {
            Debug.DrawLine(point, point + Vector3.up, Color.cyan);
        }
    }

    private Vector3 GetCenterPoint(Vector3[] vertices)
    {
        return (vertices[0] + vertices[1] + vertices[2]) / 3f;
    }

    private float GetTriangleArea(Vector3[] vertices)
    {
        float a = Vector3.Distance(vertices[0], vertices[1]);
        float c = Vector3.Distance(vertices[2], vertices[0]);
        return (a * c * Mathf.Sin(Vector3.Angle(vertices[1] - vertices[0], vertices[2] - vertices[0]) * Mathf.Deg2Rad)) / 2f;
    }
    
    private void CalculateForceAtPosition(Vector3 position, float area, float waterHeight)
    {
        float forceFactor = 1f - (position.y - waterHeight);

        if (forceFactor > 0f)
        {
            Vector3 uplift = -Physics.gravity * (forceFactor - body.velocity.y * BounceDampening) / body.mass;
            uplift.x = 0f;
            uplift.z = 0f;
            body.AddForceAtPosition(uplift * area, position);
        }
    }
}
