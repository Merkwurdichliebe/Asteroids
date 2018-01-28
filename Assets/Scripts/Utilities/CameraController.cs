using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public float volume;

	void Start () {
        AudioListener.volume = volume;
	}
}
