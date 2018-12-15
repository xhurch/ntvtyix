using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.localEulerAngles.z > 180.0 && transform.localEulerAngles.z < 220.0) {
			print("cross is upside down");
		}
	}
}
