using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

	public LadderTask scriptLadder;
	public Method1 scriptMethod1;
	public Method2 scriptMethod2;

	private int method = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			method = (method + 1) % 3;
		}

		switch(method) {
		case 0:
			scriptLadder.enabled = true;
			scriptMethod1.enabled = false;
			scriptMethod2.enabled = false;
			break;
		case 1:
			scriptLadder.enabled = false;
			scriptMethod1.enabled = true;
			scriptMethod2.enabled = false;
			break;
		case 2:
			scriptLadder.enabled = false;
			scriptMethod1.enabled = false;
			scriptMethod2.enabled = true;
			break;
		default:
			scriptLadder.enabled = true;
			scriptMethod1.enabled = false;
			scriptMethod2.enabled = false;
			break;
		}
	}
}
