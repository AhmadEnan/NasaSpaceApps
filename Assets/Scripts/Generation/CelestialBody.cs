using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public struct UpdateCelestialObjectsJob : IJob
{
    public NativeArray<Vector3> data;
    public NativeArray<Vector3> dataOPositions;
    public NativeArray<float> dataOMass;
    public Vector3 myPos;
    public float myMass;
    // public float[] oMass;
    public void Execute()
    {
        for (int i = 0; i < dataOPositions.Length; i++)
        {
            float sqrDst = (dataOPositions[i] - myPos).sqrMagnitude;
            if (sqrDst == 0f) return;

            Vector3 forceDir = (dataOPositions[i] - myPos).normalized;
            Vector3 force = forceDir * Universe.GravitationalConstant * myMass * dataOMass[i] / sqrDst;
            Vector3 acceleration = force / myMass;
            data[0] += acceleration;
        }
    }
}

public class CelestialObject
{
    #region Physics Variables
    internal float radius;
    internal float mass;
    internal float gravity;
    internal Vector3 m_InitialVel;
    internal Vector3 m_CurrentVel;
    public float selfRotVel;
    #endregion
    #region Rendering Variables
    internal GameObject m_parent;
    internal MeshFilter meshFilter;
    public Renderer r;
    #endregion
    #region Getters & Setters
    public int ID { get { return m_id; } }
    internal int m_id;
    public Transform transform { get { return m_parent.transform; } }
    #endregion

    public virtual void Die() { return; }
    internal Rigidbody rb;
}