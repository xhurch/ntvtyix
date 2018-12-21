using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIKeys : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		

		if (Input.GetKeyDown(KeyCode.Alpha1)) {

			SceneManager.LoadScene ("cathedral");
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)) {

			SceneManager.LoadScene ("cathedral_snow");
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)) {

			SceneManager.LoadScene ("demo_2");
		}
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Screen.fullScreen = false;
			Cursor.visible = true;
		}


	}
}
