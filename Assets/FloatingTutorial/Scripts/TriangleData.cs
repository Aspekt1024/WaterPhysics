using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoatTutorial
{
    public struct TriangleData
    {
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 p3;

        public Vector3 Center;
        public float DistanceToSurface;
        public Vector3 Normal;
        public float Area;
        public Vector3 Velocity;
        public Vector3 VelocityDir;
        public float CosTheta;

        public TriangleData(Vector3 p1, Vector3 p2, Vector3 p3, Rigidbody boatRb, float timeSinceStart)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;

            Center = (p1 + p2 + p3) / 3f;
            DistanceToSurface = Mathf.Abs(WaterController.current.DistanceToWater(Center, timeSinceStart));

            Normal = Vector3.Cross(p2 - p1, p3 - p1).normalized;

            Area = BoatPhysicsMath.GetTriangleArea(p1, p2, p3);
            Velocity = BoatPhysicsMath.GetTriangleVelocity(boatRb, Center);
            VelocityDir = Velocity.normalized;
            CosTheta = Vector3.Dot(VelocityDir, Normal);
        }
    }
}

