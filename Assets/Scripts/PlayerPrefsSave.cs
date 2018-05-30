using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsSave : MonoBehaviour {

    // Player Prefs are really good for resolution, audio Settings, usw.
    // This creates an integer in the playerprefs and writes in the value or gets the value. It gets saved in the Player Prefs
    int PersistentNumber
    {
        get
        {
            return PlayerPrefs.GetInt("PersistentNumber");
        }
        set
        {
            PlayerPrefs.SetInt("PersistentNumber", value);
        }
    }

	// Update is called once per frame
	void Update () {
        // Count up the player pref number
		if(Input.GetKeyDown(KeyCode.Return))
        {
            PersistentNumber++;
        }
	}

    // Show the player pref count on the viewport
    private void OnGUI()
    {
        GUILayout.Label("Persistent Number:" + PersistentNumber);
    }

}
