using UnityEngine;
using System.Collections;

public class CrossControl : MonoBehaviour {
  void Awake() {
    var childTransforms = GetComponentsInChildren<Transform>();
    for (int i=1; i<childTransforms.Length; i++) {
      var go = childTransforms[i].gameObject;
      print("Processing cross child: " + go.name);
      if (go.name.EndsWith("selector")) {
        go.AddComponent<CrossSectionActionable>();
      }
    }
  }
}
