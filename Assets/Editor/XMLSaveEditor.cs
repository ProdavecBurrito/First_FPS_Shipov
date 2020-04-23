using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TracingWayPoints))]
public class XMLSaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TracingWayPoints wPath = (TracingWayPoints)target;

        SaveObjectSing saveObject = new SaveObjectSing();

        if (GUILayout.Button("Save"))
        {
            SaveObjectSing.SaveObj();
        }

        if (GUILayout.Button("Load"))
        {
            SaveObjectSing.LoadObj();
        }
    }
}
