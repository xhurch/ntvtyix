using UnityEngine;
using System.Collections;

using UnityEngine;
using Valve.VR;
using System.Collections;
using System.Collections.Generic;

public class SimpleViveInput : MonoBehaviour {
    
	public SteamVR_Controller.Device left;
	public SteamVR_Controller.Device right;

	public void Start() {
		DiscoverControllers();
	}

	bool DiscoverControllers() {
		left = SteamVR_Controller.Input(
			SteamVR_Controller.GetDeviceIndex(
				SteamVR_Controller.DeviceRelation.FarthestLeft));
		right = SteamVR_Controller.Input(
			SteamVR_Controller.GetDeviceIndex(
				SteamVR_Controller.DeviceRelation.FarthestRight));
		Debug.LogFormat("Found controllers! {0} - {1}", left.index, right.index);
		return true;
	}
    
}
