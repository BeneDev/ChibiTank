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
public class SaveFile : BaseSaveLoad<SaveFile> {

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
}
