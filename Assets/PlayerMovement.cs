using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public Vector3 rightDirection;
	public Vector3 leftDirection;
	public float rightSpeedMultiplier;
	public float leftSpeedMultiplier;

	public float threshold = 0.10f;
	private float maxSpeed = 0.1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(rightDirection*maxSpeed*rightSpeedMultiplier);
		this.transform.Translate(leftDirection*maxSpeed*leftSpeedMultiplier);
	}

	public void rotate(Quaternion q) {
		Debug.Log ("rotate " + q);
		transform.rotation = q;
	}
}
