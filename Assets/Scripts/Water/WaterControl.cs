using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class WaterControl : MonoBehaviour {
    
    public float WaveScale = 0.1f;
    public float Speed = 1f;
    public float WaveDistance = 1f;

    private float timeSinceStart;
    private Mesh waterMesh;
    private bool updateWaterThreadComplete;

    private Vector3 oceanPos;
    private Vector3[] oceanVertices;

    private void Start()
    {
        waterMesh = GetComponent<MeshFilter>().mesh;
        oceanVertices = waterMesh.vertices;
        oceanPos = transform.position;

        ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateWaterThread));
        StartCoroutine(UpdateWater());
    }

    private void UpdateWaterThread(object state)
    {
        for (int i = 0; i < oceanVertices.Length; i++)
        {
            Vector3 vertexPosGlobal = oceanVertices[i] + oceanPos;
            oceanVertices[i].y = GetWaveYPos(vertexPosGlobal);
        }
        updateWaterThreadComplete = true;
    }

    private void Update()
    {
        timeSinceStart = Time.time;
    }

    public float GetWaveYPos(Vector3 position)
    {
        return WaveTypes.SinXWave(position, Speed, WaveScale, WaveDistance, 0, 0, timeSinceStart);
    }

    private IEnumerator UpdateWater()
    {
        while (true)
        {
            if (updateWaterThreadComplete)
            {
                waterMesh.vertices = oceanVertices;
                waterMesh.RecalculateNormals();

                updateWaterThreadComplete = false;
                ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateWaterThread));
            }
            yield return new WaitForSeconds(Time.deltaTime * 3f);
        }
    }
}
