using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    private Quaternion initRot;
    public Quadcopter copter;
	// Use this for initialization
	void Start () {
        initRot = transform.rotation;
        //transform.SetParent(copter.transform, false);

    }
	
	// Update is called once per frame
	void Update() {
        //Camera.main.transform.rotation = initRot;

        Camera.main.transform.position = copter.transform.position - 5 * copter.transform.forward + 2 * copter.transform.up;

        //Camera.main.transform.LookAt(copter.transform);
    }
}
