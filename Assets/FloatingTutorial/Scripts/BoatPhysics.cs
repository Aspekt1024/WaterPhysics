using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoatTutorial
{
    public class BoatPhysics : MonoBehaviour
    {
        public GameObject UnderwaterObj;
        public GameObject AbovewaterObj;
        public Rigidbody BoatRigidBody;
        public MeshFilter BoatMesh;
        public MeshFilter FoamSkirtObj;
        
        public Vector3 CenterOfMass;
        public float DragCoefficient = 0f;
        [HideInInspector]

        public FoamGenerator FoamGenerator;

        private ModifyBoatMesh modifyBoatMesh;
        private float waterDensity = BoatPhysicsMath.RHO_WATER;
        private float airDensiry = BoatPhysicsMath.RHO_AIR;

        // Meshes for debugging
        private Mesh underwaterMesh;
        private Mesh abovewaterMesh;

        private void Start()
        {
            FoamGenerator = new FoamGenerator(BoatMesh, FoamSkirtObj);
            modifyBoatMesh = new ModifyBoatMesh(this, BoatRigidBody.transform, BoatMesh.mesh, UnderwaterObj, AbovewaterObj, BoatRigidBody);
            underwaterMesh = UnderwaterObj.GetComponent<MeshFilter>().mesh;
            abovewaterMesh = AbovewaterObj.GetComponent<MeshFilter>().mesh;

            BoatRigidBody.centerOfMass = CenterOfMass;  // experimental?
        }

        private void Update()
        {
            FoamGenerator.ClearVertices();

            modifyBoatMesh.GenerateUnderwaterMesh();
            modifyBoatMesh.DisplayMesh(underwaterMesh, "Underwater Mesh", modifyBoatMesh.UnderwaterTriangleData);

            FoamGenerator.UpdateFoam();
            // TODO Can display abovewater mesh here.
        }

        private void FixedUpdate()
        {
            if (modifyBoatMesh.UnderwaterTriangleData.Count > 0)
            {
                AddUnderwaterForces();
            }
            if (modifyBoatMesh.AbovewaterTriangleData.Count > 0)
            {
                AddAbovewaterForces();
            }
        }

        private void AddUnderwaterForces()
        {
            float waterResistanceCoefficient = BoatPhysicsMath.ResistanceCoefficient(
                waterDensity,
                BoatRigidBody.velocity.magnitude,
                modifyBoatMesh.CalculateUnderwaterLength());

            List<SlammingForceData> slammingForceData = modifyBoatMesh.SlammingForceData;
            CalculateSlammingVelocities(slammingForceData);
            float boatArea = modifyBoatMesh.BoatArea;
            float boatMass = BoatRigidBody.mass;    // to be replaced?

            List<int> originalTriangleIndex = modifyBoatMesh.OriginalTriangleIndex;

            List<TriangleData> underwaterTriangleData = modifyBoatMesh.UnderwaterTriangleData;
            
            for (int i = 0; i < underwaterTriangleData.Count; i++)
            {



                TriangleData triangleData = underwaterTriangleData[i];


                // TODO return all of these at once rather than looping through each point individually
                Vector3 forceToAdd = Vector3.zero;
                forceToAdd += BoatPhysicsMath.BuoyancyForce(triangleData);
                forceToAdd += BoatPhysicsMath.ViscousWaterResistanceForce(waterDensity, triangleData, waterResistanceCoefficient);
                forceToAdd += BoatPhysicsMath.PressureDragForce(triangleData);
                
                SlammingForceData slammingData = slammingForceData[originalTriangleIndex[i]];
                forceToAdd += BoatPhysicsMath.SlammingForce(slammingData, triangleData, boatArea, boatMass);
                

                BoatRigidBody.AddForceAtPosition(forceToAdd, triangleData.Center);

                //Debug.DrawRay(triangleData.center, triangleData.normal * 1f, Color.white);
                //Debug.DrawRay(triangleData.center, buoyancyForce.normalized * -1f, Color.blue);
            }
        }

        private void AddAbovewaterForces()
        {
            List<TriangleData> abovewaterTriangleData = modifyBoatMesh.AbovewaterTriangleData;

            for (int i = 0; i < abovewaterTriangleData.Count; i++)
            {
                TriangleData triangleData = abovewaterTriangleData[i];

                Vector3 forceToAdd = BoatPhysicsMath.AirResistanceForce(triangleData, DragCoefficient);
                BoatRigidBody.AddForceAtPosition(forceToAdd, triangleData.Center);
            }
        }

        private void CalculateSlammingVelocities(List<SlammingForceData> slammingForceData)
        {
            for (int i = 0; i < slammingForceData.Count; i++)
            {
                slammingForceData[i].PreviousVelocity = slammingForceData[i].Velocity;
                Vector3 center = transform.TransformPoint(slammingForceData[i].TriangleCenter);
                slammingForceData[i].Velocity = BoatPhysicsMath.GetTriangleVelocity(BoatRigidBody, center);
            }
        }

    }
}
