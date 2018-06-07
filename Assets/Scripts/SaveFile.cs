using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// The save file data type which stores all the information to save
/// </summary>
[XmlRoot("SaveFile")]
public class SaveFile : BaseSaveLoad<SaveFile> {

    [XmlElement("name")]
    public string name;
    [XmlElement("level")]
    public int level;
    [XmlElement("position")]
    public Vector3 playerPos;

    // Otherwise you could make this a generics class and then inherit from this class to save certain objects
    public SaveFile()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        name = "Player";
        level = 1;
        playerPos = player.transform.position;
    }
}
