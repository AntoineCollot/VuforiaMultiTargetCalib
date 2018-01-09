using UnityEngine;
using System.Collections;

public class LookAtCam : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(2 * transform.position - Camera.main.transform.position);
    }
}
