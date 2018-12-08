using UnityEngine;

public class CrossSectionActionable : Selectable, IActionable {
  public void Action(ActionContext ctx) {
    print("Cross section action handler!");
    playerLogic.RoomTeleport(gameObject.name);
  }
}
