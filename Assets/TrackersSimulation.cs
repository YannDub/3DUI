﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackersSimulation : MonoBehaviour {
	public GameObject leftTracker;
	public GameObject rightTracker;

	private Plane objectPlane;
	private GameObject selectedObject = null;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			rightTracker.transform.Rotate (new Vector3 (0, 0, 1));
		} else if (Input.GetKey (KeyCode.RightArrow)) {
			rightTracker.transform.Rotate (new Vector3 (0, 0, -1));
		} else if (Input.GetKey (KeyCode.UpArrow)) {
			rightTracker.transform.Rotate (new Vector3 (1, 0, 0));
		} else if (Input.GetKey (KeyCode.DownArrow)) {
			rightTracker.transform.Rotate (new Vector3 (-1, 0, 0));
		}

		if (Input.GetMouseButton (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			float rayDistance;
			if (selectedObject != null) {
				Vector3 mousePos = Input.mousePosition;
				mousePos.y = Screen.height - mousePos.y;
				if (objectPlane.Raycast (ray, out rayDistance)) {
					selectedObject.transform.position = ray.GetPoint (rayDistance);
				}
			} else if (Physics.Raycast (ray, out hit) && hit.collider.gameObject == rightTracker) {
				selectedObject = hit.collider.gameObject;
				objectPlane = new Plane (Camera.main.transform.forward, hit.collider.transform.position);
			} else {
				selectedObject = null;
			}
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			leftTracker.transform.rotation = new Quaternion ();
			rightTracker.transform.rotation = new Quaternion ();
		}

		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			leftTracker.transform.Rotate (new Vector3 (-2, 0, 0));
		} else if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			leftTracker.transform.Rotate (new Vector3 (2, 0, 0));
		}
	}
}
