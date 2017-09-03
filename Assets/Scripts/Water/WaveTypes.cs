using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTypes {

    public static float SinXWave(
        Vector3 position,
        float speed,
        float scale,
        float waveDistance,
        float noiseStrength,
        float noiseWalk,
        float timesinceStart)
    {
        float x = position.x;
        float y = 0f;
        float z = position.z;

        float waveType = z;

        y += Mathf.Sin((timesinceStart * speed + waveType) / waveDistance) * scale;

        y += Mathf.PerlinNoise(x + noiseWalk, y + Mathf.Sin(timesinceStart * 0.1f)) * noiseStrength;

        return y;
    }
}
