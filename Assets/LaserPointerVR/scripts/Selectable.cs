using UnityEngine;

public class Selectable : MonoBehaviour {
  protected PlayerLogic playerLogic;

  void Awake() {
    gameObject.tag = "Selectable";
    if (gameObject.GetComponent<Collider>() == null) {
      gameObject.AddComponent<BoxCollider>();
    }
    playerLogic = GameObject.Find("Player").GetComponent<PlayerLogic>();
  }
}
