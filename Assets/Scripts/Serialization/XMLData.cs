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
        element.SetAttribute("Value", player._Name);
        mainNode.AppendChild(element);

        element = xmlDoc.CreateElement("Health");
        element.SetAttribute("Value", player._Health.ToString());
        mainNode.AppendChild(element);

        element = xmlDoc.CreateElement("Dead");
        element.SetAttribute("Value", player._Dead.ToString());
        mainNode.AppendChild(element);

        xmlDoc.Save(savingPath);

    }

    public PlayerData Load()
    {
        var res = new PlayerData();

        if (!File.Exists(savingPath))
        {
            Debug.Log("File not exist");
            return res;
        }

        using (XmlTextReader reader = new XmlTextReader(savingPath))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement("Name"))
                {
                    res._Name = reader.GetAttribute("value");
                }

                if (reader.IsStartElement("Health"))
                {
                    res._Health = Convert.ToInt32(reader.GetAttribute("value"));
                }

                if (reader.IsStartElement("Dead"))
                {
                    res._Dead = Convert.ToBoolean(reader.GetAttribute("value"));
                }
            }

        }
        return res;
    }
}
