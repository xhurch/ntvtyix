using UnityEngine;
using System;
using System.Collections;

public class LookSelector3 : MonoBehaviour {
  public GameObject highlighter;

  public GameObject outlined;
  public float distanceTimeMultiplier = 0.05f;
  public AudioClip outlineSound;
  public AudioClip selectSound;
  public Vector3 highlighterInitialScale;
  public TimestampedTouchEvent tsTouchEvent;

  // No need to create new instances of these every update.
  private ActionContext ctx;
  private CastRayResult raycast;
  private int navGridLayerMask = 1 << 19;
  private GameObject hitGo;

  void Start () {
    // OVRTouchpad.Create();
    // OVRTouchpad.TouchHandler += HandleTouchHandler;

    ctx = new ActionContext();
    ctx.player = GameObject.Find("Player");
    raycast = new CastRayResult();

    highlighter = GameObject.Find("highlighter");
    HideHighlighter();

    tsTouchEvent = new TimestampedTouchEvent();
  }

  void HandleTouchHandler(object sender, EventArgs args) {
    // tsTouchEvent.touchArgs = (OVRTouchpad.TouchArgs)args;
    tsTouchEvent.timestamp = Time.time;
    tsTouchEvent.frameCount = Time.frameCount;
  }

  void HandleKeys() {
    // OVRTouchpad.TouchEvent ev = OVRTouchpad.TouchEvent.SingleTap;
    var hor = Input.GetAxis("Horizontal");
    var vert = Input.GetAxis("Vertical");
    if (hor == 0 && vert == 0) {
      return;
    } else if (hor < 0) {
      // ev = OVRTouchpad.TouchEvent.Left;
    } else if (hor > 0) {
      // ev = OVRTouchpad.TouchEvent.Right;
    } else if (vert < 0) {
      // ev = OVRTouchpad.TouchEvent.Down;
    } else if (vert > 0) {
      // ev = OVRTouchpad.TouchEvent.Up;
    }

    print(string.Format("hor: {0} - vert: {1}", hor, vert));
	/*			
    if (ev != OVRTouchpad.TouchEvent.SingleTap) {
      print("adding event! " + ev.ToString());
      tsTouchEvent.touchArgs = new OVRTouchpad.TouchArgs();
      tsTouchEvent.touchArgs.TouchType = ev;
      tsTouchEvent.timestamp = Time.time;
      tsTouchEvent.frameCount = Time.frameCount;
    }
	*/
  }

  void ShowHighlighter(Vector3 point) {
    var distance = Vector3.Distance(transform.position, point);
    highlighter.transform.localScale = distance * highlighterInitialScale;
    highlighter.GetComponent<Renderer>().enabled = true;
    highlighter.transform.position = point;
  }

  void HideHighlighter() {
    highlighter.GetComponent<Renderer>().enabled = false;
  }

  bool CastRay() {
    raycast.Reset();
    // raycast.ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.45f, 0.0f));
    raycast.ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
    Debug.DrawRay(raycast.ray.origin, Camera.main.transform.forward * 100);
    // return Physics.Raycast(raycast.ray, out raycast.hit, 1000, navGridLayerMask);
    return Physics.Raycast(raycast.ray, out raycast.hit, 500);
  }

  void SetOutlined(GameObject go) {
    if (System.Object.ReferenceEquals(outlined, go)) return;
    ClearOutlined();
    outlined = go;
	/*
    var echoGo = outlined.GetComponentInChildren<EchoGameObject>();
    if (echoGo != null) {
      echoGo.Outline(true);
    }
	*/
    // iTween.Stab(go, outlineSound, 0);
  }

  bool ClearOutlined() {
    if (outlined) {
	  /*		
      var echoGo = outlined.GetComponentInChildren<EchoGameObject>();
      print(string.Format("echo go: {0} - {1}", outlined.name, echoGo != null));
      if (echoGo != null) {
        echoGo.Outline(false);
      }
	  */
      outlined = null;
      return true;
    }
    return false;
  }

  void Update () {
    bool highlight = false;
    bool outline = false;
    bool select = false;
    hitGo = null;

    if (CastRay()) {
      HandleKeys();
      hitGo = raycast.hit.collider.gameObject;
      ctx.raycast = raycast;

      switch (hitGo.tag) {
        case "Surface":
          highlight = true;
          break;

        case "Selectable":
          highlight = true;
          outline = true;
          // print(string.Format("frame counts: {0} - {1}", tsTouchEvent.frameCount, Time.frameCount));
          if (tsTouchEvent.frameCount == Time.frameCount - 1) {
            select = true;
            ctx.tsTouchEvent = tsTouchEvent;
          } else {
            ctx.tsTouchEvent = null;
          }
          break;
      }
    }

    if (highlight) {   
      ShowHighlighter(raycast.hit.point);
    } else {
      HideHighlighter();
    }

    if (outline) {
      SetOutlined(hitGo);
    } else {
      ClearOutlined();
    }

    if (select) {
      IActionable actionable = (IActionable)hitGo.GetComponent(typeof(IActionable));
      print(string.Format("Selected object! {0} -- {1}", hitGo.name, actionable));
      if (actionable != null) actionable.Action(ctx);
      ctx.previousSelections.Enqueue(new TimestampedSelection(Time.time, Time.frameCount, hitGo));
    }
  }
}
