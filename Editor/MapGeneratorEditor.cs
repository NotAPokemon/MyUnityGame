using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGen))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen map = (MapGen)target;

        if (DrawDefaultInspector())
        {
            if (map.autoUpdate)
            {
                map.DrawMapInEditor();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            map.DrawMapInEditor();
        }
    }
}
