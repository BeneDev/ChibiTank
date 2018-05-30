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
        Debug.LogFormat("Save - Pose.x: {0} | Pos.Y: {1} | Pos.Z: {2}", save.playerPos.x, save.playerPos.y, save.playerPos.z);
    }

    // Load data this way
    public static void LoadGame()
    {
        string filename = "savefile";
        SaveFile save = SaveFile.Load(filename);
        GameObject.FindGameObjectWithTag("Player").transform.position = save.playerPos;
        Debug.LogFormat("Load - Pos.x: {0} | Pos.y: {1} | Pos.z: {2}", save.playerPos.x, save.playerPos.y, save.playerPos.z);
    }
}
