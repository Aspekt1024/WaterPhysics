using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoatTutorial
{
    public class WaterController : MonoBehaviour
    {
        public static WaterController current;
        
        public float waveScale = 0.1f;
        public float speed = 1f;
        public float waveDistance = 1f;

        public MeshFilter WaterMesh;

        private void Start()
        {
            current = this;
        }

        public float GetWaveYPos(Vector3 position, float timeSinceStart)
        {
            return WaveTypes.SinXWave(position, speed, waveScale, waveDistance, 0f, 0f, timeSinceStart);
        }

        public float DistanceToWater(Vector3 position, float timeSinceStart)
        {
            float waterHeight = GetWaveYPos(position, timeSinceStart);
            float distanceToWater = position.y - waterHeight;
            return distanceToWater;
        }
    }
}
