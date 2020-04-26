using System;
using System.IO;
using UnityEngine;

public class StreamData : ISaveData
{
    string savingPath = Path.Combine(Application.dataPath, "StreamData.txt");

    public void Save(PlayerData player)
    {
        using (StreamWriter writer = new StreamWriter(savingPath))
        {
            writer.WriteLine(player._Name);
            writer.WriteLine(player._Health);
            writer.WriteLine(player._Dead);
        }
    }

    public PlayerData Load()
    {
        var res = new PlayerData();

        if (!File.Exists(savingPath))
        {
            Debug.Log("File not exist");
            return res;
        }
        using (StreamReader reader = new StreamReader(savingPath))
        {
            res._Name = reader.ReadLine();
            res._Health = Convert.ToInt32(reader.ReadLine());
            res._Dead= Convert.ToBoolean(reader.ReadLine());
        }
        return res;
    }
}
