using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DungeonEntrance))]
public class DungeonEntranceEditor : Editor
{

    public GameObject room;

    public override void OnInspectorGUI()
    {
        DungeonEntrance map = (DungeonEntrance)target;

        if (GUILayout.Button("Generate"))
        {
            map.generate(0.5f, 0.5f, room);
        }
    }
}

