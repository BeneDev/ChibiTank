using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the saving of the data into the save file type
/// </summary>
public class SaveFileManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        // Save data this way give in true as second param to make the file save appendingly
        SaveFile save = new SaveFile("PlayerName", 1, Vector3.zero);
        save.Save("savefile");

        // Load data this way
        //string filename = "savefile.xml";
        //SaveFile player = SaveFile.Load(filename);
        //Debug.LogFormat("Name: {0}", player.name);
    }
}
