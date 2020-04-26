using System.IO;
using UnityEngine;

public class JSONData : ISaveData
{
    string savingPath = Path.Combine(Application.dataPath, "JSONData.json");

    public void Save(PlayerData player)
    {
        string jsonFile = JsonUtility.ToJson(player);
        File.WriteAllText(savingPath, jsonFile);
    }

    public PlayerData Load()
    {
        var res = new PlayerData();

        if (!File.Exists(savingPath))
        {
            Debug.Log("File not exist");
            return res;
        }
        string temp = File.ReadAllText(savingPath);
        return JsonUtility.FromJson<PlayerData>(temp);
    }
}
