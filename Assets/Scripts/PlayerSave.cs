using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// The save file data type
/// </summary>
[XmlRoot("SaveFile")]

public class PlayerSave {

    [XmlElement("name")]
    public string name;
    [XmlElement("age")]
    public int level;
    [XmlElement("position")]
    public Vector3 position;

    public PlayerSave()
    {
        name = "";
        level = 1;
        position = Vector3.zero;
    }

    public PlayerSave(string name, int level, Vector3 pos)
    {
        this.name = name;
        this.level = level;
        this.position = pos;
    }

    #region XML Conversion

    // Overload for save, which creates the filename itself
    public void Save()
    {
        string filename = Application.dataPath + "/Data/" + name + ".xml";
        Save(filename);
    }

    public void Save(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerSave));

        // Stream is only opened in this following codeblock
        using (StreamWriter stream = new StreamWriter(filename, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static PlayerSave Load(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(PlayerSave));

        using (StreamReader stream = new StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            PlayerSave data = serializer.Deserialize(stream) as PlayerSave;
            return data;
        }
    }

    #endregion

}
