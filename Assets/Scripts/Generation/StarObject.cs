using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarObject : CelestialObject
{
    readonly float tempreature;
    public float emmissiveIntensity;
    public Color baseEmmissiveColor;
    
    public StarObject(int id, Vector3 _position, float _radius, float _tempreature, float _height, float _surfaceGravity, Material m, Mesh _mesh)
    {
        m_id = id;

        radius = _radius;
        tempreature = _tempreature;
        selfRotVel = 1.997f;
        mass = _surfaceGravity * _radius * _radius / Universe.GravitationalConstant;
        gravity = (Universe.GravitationalConstant * mass) / radius;
        // fix luminosity
        emmissiveIntensity = Mathf.Pow(_radius, 2) * Mathf.Pow(_tempreature, 4);
        baseEmmissiveColor = Color.yellow;
        m_parent = new GameObject("star_" + id);
        m_parent.transform.position = _position;
        r = m_parent.AddComponent<MeshRenderer>();
        r.material = m;

        rb = m_parent.AddComponent<Rigidbody>();
        rb.mass = mass;
        rb.useGravity = false;

        m_CurrentVel = m_InitialVel;

        meshFilter = m_parent.AddComponent<MeshFilter>();
        meshFilter.mesh = _mesh;

#if UNITY_EDITOR
        meshFilter.sharedMesh.RecalculateBounds();
        meshFilter.sharedMesh.RecalculateNormals();
        meshFilter.sharedMesh.RecalculateTangents();
#else
            meshFilter.mesh.RecalculateBounds();
            meshFilter.mesh.RecalculateNormals();
            meshFilter.mesh.RecalculateNormals();
#endif


        m_parent.transform.localPosition = new Vector3(m_parent.transform.localPosition.x, _height, m_parent.transform.localPosition.z);
        transform.localScale = new Vector3(radius, radius, radius);
    }

    public override void Die()
    {
        Generator.SafeDestroy(m_parent);
    }
}