using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/// <summary>
/// The base functions to load and save the game
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseSaveLoad<T> where T : class
{

    #region XML Conversion

    // Overload for save, which creates the filename itself
    public void Save()
    {
        string filename = "savedata";
        Save(filename);
    }

    // Open a datastream to write into an xml file the player relevant data
    public void Save(string filename)
    {
        if (!Directory.Exists(Application.dataPath + "/Data/"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Data/");
        }

        string name = Application.dataPath + "/Data/" + filename + ".xml";

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        // Stream is only opened in this following codeblock
        using (StreamWriter stream = new StreamWriter(name, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    // Open a datastream to get input from the savestate file
    public static T Load(string filename)
    {
        string name = Application.dataPath + "/Data/" + filename + ".xml";

        if (System.IO.File.Exists(name))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (StreamReader stream = new StreamReader(name, System.Text.Encoding.GetEncoding("UTF-8")))
            {
                T data = serializer.Deserialize(stream) as T;
                return data;
            }
        }
        else
        {
            Debug.LogFormat("The Savefile {0} could not be loaded.", name);
            return null;
        }
    }

    #endregion
}
