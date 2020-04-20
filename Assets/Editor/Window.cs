using System.Collections;
using UnityEngine;
using UnityEditor;

public class Window : EditorWindow
{
    string name = "MedKit";
    [SerializeField] GameObject prefab;
    [SerializeField] int objCount;
    [SerializeField] float radius;

    [MenuItem("User Instruments / Prefab Creation/ Med Kit Creation")]
    static void ShowWindow()
    {
        GetWindow(typeof(Window), false, "Med Kit generator");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        prefab = EditorGUILayout.ObjectField("Med Kit", prefab, typeof(GameObject), true) as GameObject;
        objCount = EditorGUILayout.IntSlider("Count", objCount, 2, 100);
        radius = EditorGUILayout.Slider("Radius", radius, 4, 20);

        if (GUILayout.Button("Create"))
        {
            if (prefab)
            {
                GameObject main = new GameObject("MainObj");
                for (int i = 0; i < objCount; i++) 
                {
                    float angle = i * Mathf.PI * 2 / objCount;
                    Vector3 pos = (new Vector3(Mathf.Sin(angle), 0 ,Mathf.Cos(angle)) * radius);
                    GameObject temp = Instantiate(prefab, pos, Quaternion.identity);
                    temp.transform.parent = main.transform;
                    temp.name = "("+ name +" "+ i +")";
                }
            }
        }
    }
}
