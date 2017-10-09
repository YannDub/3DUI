using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour {

	public GameObject targetObject;

	void Start() {
		this.gameObject.AddComponent<LineRenderer> ();
		LineRenderer line = this.GetComponent<LineRenderer> ();
		line.material.color = Color.red;
		line.widthMultiplier = 0.2f;
	}

	void Update() {
		if (targetObject != null) {
			LineRenderer line = this.GetComponent<LineRenderer> ();
			line.SetPosition (0, this.transform.position);
			line.SetPosition (1, targetObject.transform.position);
		}
	}

	void OnDrawGizmos() {
		if(targetObject != null)
			Debug.DrawLine (this.transform.position, targetObject.transform.position);
	}
}
