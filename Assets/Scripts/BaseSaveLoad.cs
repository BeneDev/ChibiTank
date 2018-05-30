using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class BaseSaveLoad<T> where T : class
{

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

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        // Stream is only opened in this following codeblock
        using (StreamWriter stream = new StreamWriter(name, false, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            serializer.Serialize(stream, this);
        }
    }

    public static T Load(string filename)
    {
        string name = Application.dataPath + "/Data/" + filename + ".xml";

        XmlSerializer serializer = new XmlSerializer(typeof(T));

        using (StreamReader stream = new StreamReader(filename, System.Text.Encoding.GetEncoding("UTF-8")))
        {
            T data = serializer.Deserialize(stream) as T;
            return data;
        }
    }

    #endregion
}
