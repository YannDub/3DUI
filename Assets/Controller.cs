using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	private int rightDevice;
	private int leftDevice;

	public PlayerMovement movement;

	// Use this for initialization
	void Start () {
		//Get the right device
		rightDevice = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestRight);

		//Check if the device is valid
		if(rightDevice == -1){
            Debug.Log("no right device");
            return ;}

		leftDevice = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestLeft);

		//Check if the device is valid
		if(leftDevice == -1){
            Debug.Log("no left device");
            return ;}

	}
	
	// Update is called once per frame
	void Update () {

		movement.leftSpeedMultiplier = SteamVR_Controller.Input (rightDevice).GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;
		movement.rightSpeedMultiplier = SteamVR_Controller.Input (leftDevice).GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;

		if (SteamVR_Controller.Input (leftDevice).GetAxis (Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad).x != 0) {
			movement.startLerping (this.transform.forward, this.transform.position);
		}
       
		//Get the x,y position on the pad
		//Vector2 touch = SteamVR_Controller.Input(rightDevice).GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);


		//Check if the player press on the pad button
		if(SteamVR_Controller.Input(leftDevice).GetPressDown(SteamVR_Controller.ButtonMask.Touchpad)){
            movement.startLerping(this.transform.forward, this.transform.position);
        }

	}
}
