using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generator))]
public class MGEditor : Editor
{
    MapDisplay mapDisplay;
    bool autoUpdate = true;
    public override void OnInspectorGUI()
    {
        if (mapDisplay == null)
            mapDisplay = GameObject.FindGameObjectWithTag("MapDisplay").GetComponent<MapDisplay>();

        Generator gen = (Generator)target;

        if (DrawDefaultInspector())
        {
            if (autoUpdate)
            {
                float[,] map = gen.GetNoiseMap();
                gen.GenerateStars(map);
                mapDisplay.DrawMap(map);
            }
        }

        GUILayout.Space(2f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Simulation Timestep");
        Universe.SimulationTimeStep = EditorGUILayout.Slider(Universe.SimulationTimeStep, -1f, 2f);
        GUILayout.EndHorizontal();
        GUILayout.Space(20f);
        
        if (GUILayout.Button("Generate"))
        {
            float[,] map = gen.GetNoiseMap();
            gen.GenerateStars(map);
            mapDisplay.DrawMap(map);
        }

        if (GUILayout.Button("Clear")) 
        {
            gen.CleanUp();
        }

        autoUpdate = GUILayout.Toggle(autoUpdate, "Auto Update");
    }
   
}
