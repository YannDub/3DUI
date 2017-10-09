using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateShapes : MonoBehaviour {

	private int nbShapes;

	// Use this for initialization
	void Start () {
		nbShapes = (int) Random.Range (5, 15);
		this.generateShape ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void generateShape() {
		for (int i = 0; i < nbShapes; i++) {
			GameObject o = Object.Instantiate(GameObject.Find ("Cube"), this.transform);
			
			Vector3 pos = new Vector3 (Random.Range(-50, 50), 5, Random.Range(-50, 50));
			o.transform.position = pos;
			Vector3 scale = new Vector3 (Random.Range (5, 9), Random.Range (5, 9), Random.Range (5, 9));
			o.transform.localScale = scale;
		}
	}
}
