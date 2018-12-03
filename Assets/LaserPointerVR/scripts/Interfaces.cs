using UnityEngine;
using System.Collections.Generic;

public class TimestampedTouchEvent {
  public float timestamp;
  public int frameCount;
}


public class TimestampedSelection {
  public float timestamp;
  public int frameCount;
  public GameObject selection;
  
  public TimestampedSelection(float ts, int fc, GameObject go) {
    timestamp = ts;
    frameCount = fc;
    selection = go;
  }
}


public class ActionContext {
  public Queue<TimestampedSelection> previousSelections;
  public CastRayResult raycast;
  public GameObject player;
  public TimestampedTouchEvent tsTouchEvent;
  
  public ActionContext() {
    previousSelections = new Queue<TimestampedSelection>(10);
  }
}


public interface IActionable {
  void Action(ActionContext ctx);
}
