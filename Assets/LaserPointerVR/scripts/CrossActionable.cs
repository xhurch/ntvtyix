using UnityEngine;

public class CrossActionable : MonoBehaviour, IActionable {
  private PlayerLogic playerLogic;

  void Awake() {
    playerLogic = GameObject.Find("Player").GetComponent<PlayerLogic>();
  }

  public void Action(ActionContext ctx) {
    print("Cross action handler!");
  }
}
