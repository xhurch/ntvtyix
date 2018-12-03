using UnityEngine;

public class RoomTriggerAction : MonoBehaviour, IActionable {
  public AudioClip audioClip;
  private GameObject player;
  private GameObject anchor;

  void Start() {
    player = GameObject.Find("Player");
  }

  public void Action(ActionContext ctx) {
    /*
    iTween.Stab(gameObject, iTween.Hash(
          "audioclip", audioClip,
          "volume", 1));
    */
	/*
    iTween.ColorTo(gameObject, iTween.Hash(
          "color", Color.black,
          "time", 1,
          // "looptype", "pingPong",
          "easetype", "linear",
          "oncomplete", "Transition"
          ));
	*/

    var num = gameObject.name[gameObject.name.Length-1];
    print("Selected room number: " + num);
    anchor = GameObject.Find("cam-anchor-" + num);
    print(string.Format("Got anchor: {0}", anchor.name));
	Transition();
  }

  public void Transition() {
    gameObject.GetComponent<Renderer>().material.color = Color.green;
    print("Transitioning!");
    var newpos = anchor.transform.position;
    newpos.y = 0;
    player.transform.position = newpos;
    player.transform.rotation = anchor.transform.rotation;
  }
}
