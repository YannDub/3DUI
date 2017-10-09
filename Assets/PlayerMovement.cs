using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	public Vector3 direction;
	public float speedMultiplier;

	public float threshold = 0.10f;
	private float maxSpeed = 10f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Translate(direction*maxSpeed*speedMultiplier);
	}
}
