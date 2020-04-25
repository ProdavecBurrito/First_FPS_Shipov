using System;
using System.IO;
using UnityEngine;
using System.Xml;
public class XMLData : ISaveData
{

    string savingPath = Path.Combine(Application.dataPath, "XMLData.xml");
    public void Save(PlayerData player)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode mainNode = xmlDoc.CreateElement("Player");
        xmlDoc.AppendChild(mainNode);

        XmlElement element = xmlDoc.CreateElement("Name");
    }

    public PlayerData Load()
    {
        var res = new PlayerData();
        return res;
    }
}
