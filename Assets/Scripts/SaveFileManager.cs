using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFileManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //PlayerSave player = new PlayerSave("player", 1, Vector3.zero);
        //player.Save();

        string filename = Application.dataPath + "/Data/player" + ".xml";
        PlayerSave player = PlayerSave.Load(filename);
        Debug.LogFormat("Name: {0}", player.name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
