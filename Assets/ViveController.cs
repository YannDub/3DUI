using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViveController : MonoBehaviour {

	private SteamVR_TrackedController device;

	// Use this for initialization
	void Start () {
		device = GetComponent<SteamVR_TrackedController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
