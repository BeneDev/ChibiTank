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
public class SaveFile {

    [XmlElement("name")]
    public string name;
    [XmlElement("age")]
    public int level;
    [XmlElement("position")]
    public Vector3 position;

    // TODO get all the fields, which can only have one value, like level, name and player position here and write them into the member fields
    // Otherwise you could make this a generics class and then inherit from this class to save certain objects
    public SaveFile()
    {
        name = "";
        level = 1;
        position = Vector3.zero;
    }

    public SaveFile(string name, int level, Vector3 pos)
    {
        this.name = name;
        this.level = level;
        this.position = pos;
    }

    #region XML Conversion

    // Overload for save, which creates the filename itself
    public void Save()
    {
        string filename = "savedata";
        Save(filename);
    }

    public void Save(string filename)
    {
        string name = Application.dataPath + "/Data/" + filename + ".xml";

        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));

        // Stream is only opened in this following codeblock
        using (StreamWriter stream = new StreamWriter(name, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static SaveFile Load(string filename)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(SaveFile));

        using (StreamReader stream = new StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            SaveFile data = serializer.Deserialize(stream) as SaveFile;
            return data;
        }
    }

    #endregion

}
