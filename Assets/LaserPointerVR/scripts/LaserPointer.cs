using UnityEngine;
using antilunchbox;

public class CastRayResult {
  public Ray ray;
  public RaycastHit hit;

  public void Reset() {
    ray.direction = Vector3.zero;
    ray.origin = Vector3.zero;
    hit.distance = 0f;
  }
}


public class LaserPointer : MonoBehaviour {
  public string outlineSoundName = "click";
  public string selectSoundName = "menu-select";
  public GameObject highlighter;
  public GameObject outlined;
  public SimpleViveInput viveInput;
  public int systemLayer = 9;
  private int systemLayerMask;
  public float delaySeconds = 1f;
  private float delayStart = 0;

  // No need to create new instances of these every update.
  private ActionContext ctx;
  private GameObject hitGo;

  void Awake() {
    systemLayerMask = 1 << systemLayer;
    if (highlighter == null) {
      highlighter = GameObject.Find("highlighter");
    }
    Debug.Assert(highlighter != null);
    HideHighlighter();

    ctx = new ActionContext();
    ctx.player = GameObject.Find("Player");
    ctx.raycast = new CastRayResult();

    var viveInputHandler = GameObject.Find("ViveInputHandler");
    if (viveInputHandler != null) {
      viveInput = viveInputHandler.GetComponent<SimpleViveInput>();
    } else {
      Debug.LogWarning("Could not find ViveInput.", gameObject);
    }
  }

  bool CastRay() {
    ctx.raycast.Reset();
    ctx.raycast.ray = new Ray(transform.position, transform.forward);
    Debug.DrawRay(ctx.raycast.ray.origin, transform.forward * 50);
    return Physics.Raycast(ctx.raycast.ray, out ctx.raycast.hit, 1000, systemLayerMask);
  }

  bool GetViveTriggerPressed() {
    //if (viveInput != null) {
			//return viveInput.right.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
   // }
    return false;
  }

  bool GetButtonPressed() {
    return (
        Input.GetMouseButtonDown(0)
        || GetViveTriggerPressed()
        );
  }

  void ShowHighlighter(Vector3 point) {
    // highlighter.GetComponent<Renderer>().enabled = true;
    highlighter.transform.position = point;
    highlighter.SetActive(true);
  }

  void HideHighlighter() {
    // highlighter.GetComponent<Renderer>().enabled = false;
    highlighter.SetActive(false);
  }

  void SetOutlined(GameObject go) {
    if (System.Object.ReferenceEquals(outlined, go)) return;
    SoundManager.PlaySFX(outlineSoundName);
    ClearOutlined();
    outlined = go;
    outlined.GetComponent<Renderer>().enabled = true;
  }

  bool ClearOutlined() {
    if (outlined) {
      outlined.GetComponent<Renderer>().enabled = false;
      outlined = null;
      return true;
    }
    return false;
  }

  void Update () {
    // Guard against actions taken too quickly in succession.
    if (!delayStart.Equals(0) && Time.time - delayStart < delaySeconds) {
      return;
    }

    delayStart = 0;
    bool select = false;
    bool highlight = false;
    bool outline = false;
    hitGo = null;

    if (CastRay()) {
      select = GetButtonPressed();
      hitGo = ctx.raycast.hit.collider.gameObject;
      switch (hitGo.tag) {
        case "Surface":
          highlight = true;
          break;
        case "Selectable":
          outline = true;
          break;
      }
    }

    if (highlight) {   
      ShowHighlighter(ctx.raycast.hit.point);
    } else {
      HideHighlighter();
    }

    if (outline) {
      SetOutlined(hitGo);
    } else {
      ClearOutlined();
    }

    if (hitGo != null && select) {
      var actionable = (IActionable)hitGo.GetComponent(typeof(IActionable));
      print(string.Format("Selected obect! {0} -- {1}", hitGo.name, actionable));
      // SoundManager.PlaySFX(selectSoundName);
      if (actionable != null) {
        actionable.Action(ctx);
        delayStart = Time.time;
      }
    }
  }
}
