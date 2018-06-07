using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the saving of the data into the save file type
/// </summary>
public class SaveFileManager : MonoBehaviour {

    static int saveIndex = 0;

    private void Awake()
    {
        saveIndex = 0;
    }

    #region Helper Methods

    // Save data this way give in true as second param to make the file save appendingly
    public static void SaveGame()
    {
        SaveFile save = new SaveFile();
        string saveName = "savefile" + saveIndex.ToString();
        save.Save(saveName);
        saveIndex++;
        //Debug.LogFormat("Save - Pose.x: {0} | Pos.Y: {1} | Pos.Z: {2}", save.playerPos.x, save.playerPos.y, save.playerPos.z);
    }

    // TODO give in an int parameter to choose which savefile to load. Savefiles are named savefile1, savefile2, ... This way, you can choose the right savefile in the load menu
    // Load data this way
    public static void LoadGame()
    {
        string filename = "savefile" + saveIndex.ToString();
        SaveFile save = SaveFile.Load(filename);
        if(save != null)
        {
            GameObject.FindGameObjectWithTag("Player").transform.position = save.playerPos;
        }
        //Debug.LogFormat("Load - Pos.x: {0} | Pos.y: {1} | Pos.z: {2}", save.playerPos.x, save.playerPos.y, save.playerPos.z);
    }

    #endregion

}
