using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using Unity.Mathematics;

public class AstroPhysics : MonoBehaviour
{
    [SerializeField] float MaxIntensity = 30f;
    List<CelestialObject> _SpawnedObjects;

    public struct JobData 
    {
        public Vector3 initialVel;
        public Vector3[] oPositions;
        public Vector3 myPos;
        public float[] oMass;
        public Vector3 _resultingVelocity;
    }

    public void Setup() 
    {
        _SpawnedObjects = Generator.spawnedObjects;
    }

    public void Update()
    {
        if (_SpawnedObjects == null) return;

        for (int i = 0; i < _SpawnedObjects.Count; i++)
        {
            StarObject star = (StarObject)_SpawnedObjects[i];

            star.transform.RotateAround(star.transform.position, Vector3.up, star.selfRotVel * Time.deltaTime);
            float s = Mathf.Clamp(Mathf.Sin(Time.realtimeSinceStartup * Universe.SimulationTimeStep), 0.319f, 1f);

            star.r.material.SetColor(Shader.PropertyToID("_BaseColor"), star.baseEmmissiveColor * Mathf.Lerp(star.emmissiveIntensity * 0.00000000000000001f, 30f, s));
            // Debug.Log("final intensity : " + Mathf.Lerp(star.emmissiveIntensity, MaxIntensity, s));
            float distFromCenter = Vector3.Distance(Generator.GetCenter, star.transform.position);
            star.transform.position = Vector3.MoveTowards(star.transform.position, Generator.GetCenter, -1 * distFromCenter * Generator.GetExpansionRate) * Universe.SimulationTimeStep;

        }
    }

}
