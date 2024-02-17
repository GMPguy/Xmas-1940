using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControll : MonoBehaviour {

    public int AudioType = 0; // 0 sound, 1 music
    GameScript GS;

    void Start() {
        GS = GameObject.Find("GameScript").GetComponent<GameScript>();
        Set();
    }

    public void Set() {
        if(this.GetComponent<AudioSource>())
            if(AudioType == 0) this.GetComponent<AudioSource>().volume = GS.Volumes[0] * GS.Volumes[2];
            else this.GetComponent<AudioSource>().volume = GS.Volumes[0] * GS.Volumes[1];
    }
}
