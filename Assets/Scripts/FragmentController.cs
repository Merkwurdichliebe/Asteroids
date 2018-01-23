using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentController : MonoBehaviour {
    
    // Destroy ship fragments after 3 seconds

	void Start ()
    {
        Destroy(gameObject, 3.0f);
	}
}
