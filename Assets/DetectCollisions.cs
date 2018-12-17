using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisions : MonoBehaviour {

    void OnCollisionEnter (Collision col)
    {
        // if(col.gameObject.name == "prop_powerCube")
        // {
		Destroy(col.gameObject);
        // }
    }
}