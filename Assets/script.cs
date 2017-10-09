using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour {

    private SteamVR_TrackedController device;
	// Use this for initialization
	void Start () {
        device = GetComponent<SteamVR_TrackedController>();
        device.TriggerClicked += new ClickedEventHandler(Trigger);
	}

    public void Trigger(object sender, ClickedEventArgs args)
    {
        Debug.Log("trigger clicked");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
