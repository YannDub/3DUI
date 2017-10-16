using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackersSimulation : MonoBehaviour {
	private GameObject tracker;

	private Plane objectPlane;
	private GameObject selectedObject = null;
    private GameObject myLine;

    public PlayerMovement movement;

    private LineRenderer lr;

	public string side;

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
		tracker = this.gameObject;
        myLine = new GameObject();
        
		lr = myLine.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        lr.endWidth = lr.startWidth = 0.02f;
    }

	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f) {
        myLine.transform.position = start;
        lr.startColor = lr.endColor = color;
        lr.SetPosition(0, start);
		lr.SetPosition(1, end);
	}

	private float lerpFactor = 0;
	private bool isLerping = false;
	private Quaternion from;
	private Quaternion to;
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftArrow)) {
			tracker.transform.Rotate (new Vector3 (0, -1, 0));
		} 
		if (Input.GetKey (KeyCode.RightArrow)) {
			tracker.transform.Rotate (new Vector3 (0, 1, 0));
		} 
		if (Input.GetKey (KeyCode.UpArrow)) {
			tracker.transform.Rotate (new Vector3 (-1, 0, 0));
		} 
		if (Input.GetKey (KeyCode.DownArrow)) {
			tracker.transform.Rotate (new Vector3 (1, 0, 0));
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
			} else if (Physics.Raycast (ray, out hit) && hit.collider.gameObject == tracker) {
				selectedObject = hit.collider.gameObject;
				objectPlane = new Plane (Camera.main.transform.forward, hit.collider.transform.position);
			} else {
				selectedObject = null;
			}
		}

		if (Input.GetKeyDown (KeyCode.R)) {
			tracker.transform.rotation = new Quaternion ();
		}


		Ray r = new Ray (tracker.transform.position, tracker.transform.rotation * new Vector3 (0, 0, 1));
        //Debug.Log(leftTracker.transform.rotation.eulerAngles.x);
		if (Input.GetKey (KeyCode.E)) {
			//Camera.main.transform.TransformDirection (tracker.transform.rotation * new Vector3 (0, 0, 1));
			//Camera.main.transform.Rotate(tracker.transform.rotation * new Vector3 (0, 0, 1), Vector3.Angle(new Vector3(0, 0, 1), tracker.transform.rotation * new Vector3 (0, 0, 1)));
			/*Vector3 point = r.GetPoint(10);
			Camera.main.transform.LookAt (point);*/

			this.transform.position = tracker.transform.position;
			from = Camera.main.transform.rotation;
			to = Quaternion.FromToRotation (Camera.main.transform.forward, tracker.transform.transform.transform.transform.forward);
			isLerping = true;
			lerpFactor = 0;

			//Camera.main.transform.rotation *= q;
		}

		if (Input.GetKeyUp (KeyCode.E)) {
			isLerping = false;
		}

		if (isLerping) {
			lerpFactor += Time.deltaTime;
			//Camera.main.transform.rotation = Quaternion.Lerp (from, to, lerpFactor * rotationSpeed);
			Quaternion q = Quaternion.Lerp (from, to, lerpFactor * rotationSpeed);
			movement.rotate (q);
			isLerping = lerpFactor < 1;
		}
		
		DrawLine (tracker.transform.position, r.GetPoint (100), Color.red);

		if (side == "left") {
			movement.leftDirection = GetDirection();
		} else if (side == "right") {
			movement.rightDirection = GetDirection();
		}

    }

    public Vector3 GetDirection() {
        return Vector3.Normalize(tracker.transform.rotation * new Vector3(0, 0, 1));
    }
}
