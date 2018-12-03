using UnityEngine;

public class RoomTeleportActionable : Selectable, IActionable {
  public void Action(ActionContext ctx) {
    print("Room teleport action handler!");
    playerLogic.RoomTeleport(gameObject.name);
  }
}
