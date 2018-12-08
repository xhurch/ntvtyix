using UnityEngine;
using System.Collections;

public class FloorActionable : MonoBehaviour, IActionable {
  private PlayerLogic playerLogic;

  void Awake() {
    gameObject.tag = "Surface";
    if (gameObject.GetComponent<Collider>() == null) {
      gameObject.AddComponent<BoxCollider>();
    }
    playerLogic = GameObject.Find("Player").GetComponent<PlayerLogic>();
  }

  public void Action(ActionContext ctx) {
    print("Floor action handler!");
		Vector3 p = ctx.raycast.hit.point;
		p.y -= 0.5f;
    playerLogic.FloorTeleport(p);
  }
}
