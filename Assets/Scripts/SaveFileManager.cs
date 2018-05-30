using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the saving of the data into the save file type
/// </summary>
public class SaveFileManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

        //SaveGame();

        //LoadGame();
    }

    // Save data this way give in true as second param to make the file save appendingly
    public static void SaveGame()
    {
        SaveFile save = new SaveFile();
        save.Save("savefile");
    }

    // Load data this way
    public static void LoadGame()
    {
        string filename = "savefile";
        SaveFile save = SaveFile.Load(filename);
        GameObject.FindGameObjectWithTag("Player").transform.position = save.playerPos;
    }
}
