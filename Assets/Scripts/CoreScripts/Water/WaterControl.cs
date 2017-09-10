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

    private static WaterControl instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

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

    public static void IncreaseWaveIntensity()
    {
        instance.WaveScale *= 1.1f;
        instance.WaveDistance *= 1.03f;
    }

    public static void DecreaseWaveIntensity()
    {
        instance.WaveScale /= 1.1f;
        instance.WaveDistance /= 1.03f;
    }

    public static float GetWaveYPos(Vector3 position)
    {
        return WaveTypes.SinXWave(position, instance.Speed, instance.WaveScale, instance.WaveDistance, 0, 0, instance.timeSinceStart);
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
