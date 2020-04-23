using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;
using System.IO;

[Serializable]
public struct UserVector3Sing
{
    public float X;
    public float Y;
    public float Z;

    public UserVector3Sing(float _x, float _y, float _z)
    {
        X = _x;
        Y = _y;
        Z = _z;
    }

    public static implicit operator UserVector3Sing(Vector3 val)
    {
        return new UserVector3Sing(val.x, val.y, val.z);
    }

    public static implicit operator Vector3(UserVector3Sing val)
    {
        return new Vector3(val.X, val.Y, val.Z);
    }
}

[Serializable]
public struct UserQuartenionSing
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public UserQuartenionSing(float _x, float _y, float _z, float _w)
    {
        X = _x;
        Y = _y;
        Z = _z;
        W = _w;
    }

    public static implicit operator UserQuartenionSing(Quaternion val)
    {
        return new UserQuartenionSing(val.x, val.y, val.z, val.w);
    }

    public static implicit operator Quaternion(UserQuartenionSing val)
    {
        return new Quaternion(val.X, val.Y, val.Z, val.W);
    }
}

[Serializable]
public struct UserObjectSing
{
    public string name;
    public UserVector3Sing position;
    public UserVector3Sing scale;
    public UserQuartenionSing rotation;
}

public class SaveObjectSing
{
    static XmlSerializer formater;
    static string savingPath = Path.Combine(Application.dataPath, "XMLSaveSing.xml");

    static SaveObjectSing()
    {
        formater = new XmlSerializer(typeof(UserObject[]));
    }

    [MenuItem("User Instruments / Save, Load Scene / Save Sing Object")]
    public static void SaveObj()
    {
        GameObject tempObj = GameObject.FindObjectOfType<TracingWayPoints>().gameObject;
        List<GameObject> mainObj = new List<GameObject>();
        foreach (Transform item in tempObj.transform)
        {
            mainObj.Add(item.gameObject);
        }
        List<UserObject> lvlObj = new List<UserObject>();

        foreach (var item in mainObj)
        {
            var temp = item.transform;

            lvlObj.Add
                (
                    new UserObject
                    {
                        name = temp.name,
                        position = temp.position,
                        scale = temp.localScale,
                        rotation = temp.rotation
                    }
                );
        }

        if (lvlObj.Count <= 0)
        {
            Debug.Log("В массиве нет обьектов");
        }
        if (String.IsNullOrEmpty(savingPath))
        {
            Debug.Log("Нет пути");
        }

        using (FileStream fileStream = new FileStream(savingPath, FileMode.Create))
        {
            formater.Serialize(fileStream, lvlObj.ToArray());
        }
    }

    [MenuItem("User Instruments / Save, Load Scene / Load Sing Object")]
    public static void LoadObj()
    {
        UserObject[] res;
        using (FileStream fileStream = new FileStream(savingPath, FileMode.Open))
        {
            res = (UserObject[])formater.Deserialize(fileStream);
        }

        var mainPref = new GameObject("WayPoints");
        mainPref.AddComponent<TracingWayPoints>();

        foreach (var item in res)
        {
            var pref = new GameObject(item.name);
            pref.transform.position = item.position;
            pref.transform.localScale = item.scale;
            pref.transform.rotation = item.rotation;
            pref.transform.parent = mainPref.transform;
        }
    }
}
