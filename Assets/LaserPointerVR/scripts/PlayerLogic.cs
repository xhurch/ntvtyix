using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PlayerLogic : MonoBehaviour {
  public Transform roomAnchors;
  public Dictionary<int, Transform> roomAnchorTransforms;
  private bool roomTeleportStarted = false;
  public string soundConnectionName;

  void Awake() {
    roomAnchorTransforms = new Dictionary<int, Transform>();
    if (roomAnchors != null) {
      var childTransforms = roomAnchors.GetComponentsInChildren<Transform>();
      for (int i=1; i<childTransforms.Length; i++) {
        var child = childTransforms[i];
        Match m = Regex.Match(child.name, @"\d+");
        print("Processing room anchor transform: " + m.Value);
        roomAnchorTransforms.Add(int.Parse(m.Value), child);
      }
    } else {
      Debug.LogWarning("No Room Anchors found.", gameObject);
    }
  }

  public static void Blink() {
    //SteamVR_Fade.Start(Color.black, 0);
   // SteamVR_Fade.Start(Color.clear, 1);
  }

  public bool RoomTeleport(string name) {
    print("Teleporting player to selected room: " + name);
    Match m = Regex.Match(name, @"\d+");
    var i = int.Parse(m.Value);
    if (roomAnchorTransforms.ContainsKey(i)) {
      var target = roomAnchorTransforms[i];
      print("Teleporting to target transform: " + target.name);
      if (!roomTeleportStarted) {
        SoundManager.PlayConnection(soundConnectionName);
        SoundManager.PlaySFX("xhurch-bell");
        roomTeleportStarted = true;
      } else {
        SoundManager.PlaySFX("teleport" + UnityEngine.Random.Range(1, 5));
      }
      Blink();
      transform.position = target.position;
      transform.rotation = target.rotation;
      return true;
    }

    return false;
  }

  public bool FloorTeleport(Vector3 position) {
    print("Teleporting player to new floor position: " + position);
    SoundManager.PlaySFX("menu-select");
    Blink();
    transform.position = position;
    return true;
  }
}
