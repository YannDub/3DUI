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
	public float rotationSpeed = 5;

	private float lerpFactor = 0;
	private bool isLerping = false;
	private Quaternion from;
	private Quaternion to;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float leftSpeed = maxSpeed * leftSpeedMultiplier;
		float rightSpeed = maxSpeed * rightSpeedMultiplier;

		this.transform.Translate(rightDirection*maxSpeed*rightSpeedMultiplier);
		this.transform.Translate(leftDirection*maxSpeed*leftSpeedMultiplier);

		if (isLerping) {
			lerpFactor += Time.deltaTime;
			transform.rotation = Quaternion.Lerp (from, to, lerpFactor * rotationSpeed);
			isLerping = lerpFactor < 1;
		}
	}

	public void startLerping(Vector3 to, Vector3 newPosition) {
        if (isLerping) return;
		this.from = this.transform.rotation;
		this.to = Quaternion.FromToRotation (this.transform.forward, to);
		isLerping = true;
		lerpFactor = 0;
	}

}
