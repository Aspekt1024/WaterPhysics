using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoatTutorial
{
    public static class BoatPhysicsMath
    {
        // Densities
        public const float RHO_WATER = 1000f;
        public const float RHO_OCEAN_WATER = 1027f;
        public const float RHO_AIR = 1.225f;

        public const float DragCoefficientFlatPlatePerpendicularToFlow = 1.28f;

        public static Vector3 BuoyancyForce(TriangleData triangleData)
        {
            float density = RHO_WATER;
            Vector3 volume = triangleData.DistanceToSurface * triangleData.Area * triangleData.Normal;
            Vector3 buoyancyForce = density * Physics.gravity.y * volume;

            // The horizontal component of the hydrostatic forces cancel out;
            buoyancyForce.x = 0f;
            buoyancyForce.z = 0f;

            buoyancyForce = CheckForceIsValid(buoyancyForce, "Buoyancy");

            return buoyancyForce;
        }

        public static Vector3 ViscousWaterResistanceForce(float density, TriangleData triangleData, float resistanceCoeff)
        {
            Vector3 B = triangleData.Normal;
            Vector3 A = triangleData.Velocity;
            Vector3 velocityTangent = Vector3.Cross(B, (Vector3.Cross(A, B) / B.magnitude)) / B.magnitude;
            Vector3 tangentalDirection = velocityTangent.normalized * -1f;
            Vector3 tangentVelocity = triangleData.Velocity.magnitude * tangentalDirection;
            Vector3 viscousWaterResistanceForce = 0.5f * density * tangentVelocity.magnitude * tangentVelocity * triangleData.Area * resistanceCoeff;

            viscousWaterResistanceForce = CheckForceIsValid(viscousWaterResistanceForce, "Viscous Water Resistance");
            return viscousWaterResistanceForce;
        }

        public static Vector3 GetTriangleVelocity(Rigidbody rigidBody, Vector3 triangleCenter)
        {
            Vector3 pointBVelocity = rigidBody.velocity;
            Vector3 pointBAngularVelocty = rigidBody.angularVelocity;
            Vector3 dist = triangleCenter - rigidBody.worldCenterOfMass;
            Vector3 pointAVelocity = pointBVelocity + Vector3.Cross(pointBAngularVelocty, dist);
            return pointAVelocity;
        }

        public static float ResistanceCoefficient(float density, float speed, float length)
        {
            // Coefficient of frictional resistance
            float fluidViscocity = 0.000001f;
            float reynoldsNumber = (speed * length) / fluidViscocity;
            float resistanceCoefficient = 0.075f / Mathf.Pow((Mathf.Log10(reynoldsNumber) - 2f), 2f);
            return resistanceCoefficient;
        }

        public static Vector3 PressureDragForce(TriangleData triangleData)
        {
            float speed = 1f;
            Vector3 pressureDragForce = Vector3.zero;

            if (triangleData.CosTheta > 0f)
            {
                //TODO change the variables real-time - add the finished values later
                float C_PD1 = DebugPhysics.current.C_PD1;
                float C_PD2 = DebugPhysics.current.C_PD2;
                float f_P = DebugPhysics.current.f_P;

                pressureDragForce = -(C_PD1 * speed + C_PD2 * (speed * speed)) * triangleData.Area * Mathf.Pow(triangleData.CosTheta, f_P) * triangleData.Normal;
            }
            else
            {            
                //TODO change the variables real-time - add the finished values later
                float C_SD1 = DebugPhysics.current.C_SD1;
                float C_SD2 = DebugPhysics.current.C_SD2;
                float f_S = DebugPhysics.current.f_S;

                pressureDragForce = (C_SD1 * speed + C_SD2 * (speed * speed)) * triangleData.Area * Mathf.Pow(Mathf.Abs(triangleData.CosTheta), f_S) * triangleData.Normal;
            }

            pressureDragForce = CheckForceIsValid(pressureDragForce, "Pressure Drag");
            return pressureDragForce;
        }

        public static Vector3 SlammingForce(SlammingForceData slammingData, TriangleData triangleData, float area, float mass)
        {
            if (triangleData.CosTheta < 0f || slammingData.OriginalArea <= 0f)
            {
                return Vector3.zero;
            }
            
            Vector3 volumeOfWater = slammingData.SubmergedArea * slammingData.Velocity;
            Vector3 volumeOfWaterPrev = slammingData.PreviousSubmergedArea * slammingData.PreviousVelocity;

            Vector3 accelerationVector = (volumeOfWater - volumeOfWaterPrev) / (slammingData.OriginalArea * Time.fixedDeltaTime);
            float acceleration = accelerationVector.magnitude;

            Vector3 stoppingForce = mass * triangleData.Velocity * ((2f * triangleData.Area) / area);

            float slammingForceRampupPower = 2f;
            float maxAcceleration = acceleration;
            float slammingCheat = DebugPhysics.current.SlammingCheat;
            Vector3 slammingForce = -Mathf.Pow(Mathf.Clamp01(acceleration / maxAcceleration), slammingForceRampupPower) * triangleData.CosTheta * stoppingForce * slammingCheat;

            slammingForce = CheckForceIsValid(slammingForce, "Slamming");
            return slammingForce;
        }

        public static float ResidualResistanceForce()
        {
            // Not used, i guess...
            return 0;
        }

        public static Vector3 AirResistanceForce(TriangleData triangleData, float AirResistanceCoefficient)
        {
            // Only add air resistance if the normal is pointing in the same direction as the velocity
            if (triangleData.CosTheta < 0f)
            {
                return Vector3.zero;
            }

            Vector3 airResistanceForce = -0.5f * RHO_AIR * triangleData.Velocity.magnitude * triangleData.Velocity * triangleData.Area * AirResistanceCoefficient;
            airResistanceForce = CheckForceIsValid(airResistanceForce, "Air Resistance");
            return airResistanceForce;
        }

        public static float GetTriangleArea(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float a = Vector3.Distance(p1, p2);
            float c = Vector3.Distance(p3, p1);
            float area = (a * c * Mathf.Sin(Vector3.Angle(p2 - p1, p3 - p1) * Mathf.Deg2Rad)) / 2f;
            return area;
        }

        private static Vector3 CheckForceIsValid(Vector3 force, string forceName)
        {
            if (float.IsNaN(force.x + force.y + force.z))
            {
                Debug.Log(forceName + " force is NaN");
                return Vector3.zero;
            }
            else
            {
                return force;
            }
        }
    }
}

