using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;
using System.IO;

[Serializable]
public struct UserVector3
{
    public float X;
    public float Y;
    public float Z;
     
    public UserVector3(float _x, float _y, float _z)
    {
        X = _x;
        Y = _y;
        Z = _z;
    }

    public static implicit operator UserVector3(Vector3 val)
    {
        return new UserVector3(val.x, val.y, val.z);
    }

    public static implicit operator Vector3(UserVector3 val)
    {
        return new Vector3(val.X, val.Y, val.Z);
    }
}

[Serializable]
public struct UserQuartenion
{
    public float X;
    public float Y;
    public float Z;
    public float W;

    public UserQuartenion(float _x, float _y, float _z, float _w)
    {
        X = _x;
        Y = _y;
        Z = _z;
        W = _w;
    }

    public static implicit operator UserQuartenion(Quaternion val)
    {
        return new UserQuartenion(val.x, val.y, val.z, val.w);
    }

    public static implicit operator Quaternion(UserQuartenion val)
    {
        return new Quaternion(val.X, val.Y, val.Z, val.W);
    }
}

[Serializable]
public struct UserObject
{
    public string name;
    public UserVector3 position;
    public UserVector3 scale;
    public UserQuartenion rotation;
}

public class SaveObject
{
    static XmlSerializer formater;
    static string savingPath = Path.Combine(Application.dataPath, "XMLSave.xml");

    static SaveObject()
    {
        formater = new XmlSerializer(typeof(UserObject[]));
    }

    [MenuItem("User Instruments / Save, Load Scene / Save Scene")]
    static void SaveScene()
    {
        Scene tempScene = SceneManager.GetActiveScene();
        List<GameObject> mainObj = new List<GameObject>();
        tempScene.GetRootGameObjects(mainObj);
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

        using(FileStream fileStream = new FileStream(savingPath, FileMode.Create))
        {
            formater.Serialize(fileStream, lvlObj.ToArray());
        }
    }

    [MenuItem("User Instruments / Save, Load Scene / Load Scene")]
    static void LoadScene()
    {
        UserObject[] res;
        using (FileStream fileStream = new FileStream(savingPath, FileMode.Open))
        {
            res = (UserObject[]) formater.Deserialize(fileStream);
        }

        foreach (var item in res)
        {
            var pref = Resources.Load<GameObject>("Preffabs/LvlObjects/" + item.name);
            if (pref)
            {
                GameObject temp = MonoBehaviour.Instantiate(pref, item.position, item.rotation);
                temp.transform.localScale = item.scale;
                temp.name = item.name;
            }
        }
    }
}
public class XMLSave : MonoBehaviour
{

}
