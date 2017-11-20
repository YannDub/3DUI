using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackersSimulation : MonoBehaviour
{
    private GameObject tracker;

    private Plane objectPlane;
    private GameObject selectedObject = null;
    private GameObject myLine;

    public Quadcopter drone;

    //public PlayerMovement movement;

    private LineRenderer lr;

    public string side;

    // Use this for initialization
    void Start()
    {
        tracker = this.gameObject;
        myLine = new GameObject();

        lr = myLine.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        lr.endWidth = lr.startWidth = 0.02f;
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        myLine.transform.position = start;
        lr.startColor = lr.endColor = color;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }


    Vector2 GetYawPitch(Vector3 dir)
    {
        Vector3 direction = GetDirection();

        Vector3 dronePos = dir;

        float pitch = Mathf.Asin(direction.y);
        float pitchb = Mathf.Asin(dronePos.y);

        float yaw = Mathf.Asin(direction.x / Mathf.Cos(pitch)); //Beware cos(pitch)==0, catch this exception!
        float yawb = Mathf.Asin(dronePos.x / Mathf.Cos(pitchb));

        float truePitch = pitchb - pitch;
        float trueYaw = yawb - yaw;

        return new Vector2(trueYaw, truePitch);
    }




    bool lockPitch = false;
    bool lockYaw = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            tracker.transform.Rotate(new Vector3(0, -1, 0));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            tracker.transform.Rotate(new Vector3(0, 1, 0));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            tracker.transform.Rotate(new Vector3(-1, 0, 0));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            tracker.transform.Rotate(new Vector3(1, 0, 0));
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            float rayDistance;
            if (selectedObject != null)
            {
                Vector3 mousePos = Input.mousePosition;
                mousePos.y = Screen.height - mousePos.y;
                if (objectPlane.Raycast(ray, out rayDistance))
                {
                    selectedObject.transform.position = ray.GetPoint(rayDistance);
                }
            } else if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == tracker)
            {
                selectedObject = hit.collider.gameObject;
                objectPlane = new Plane(Camera.main.transform.forward, hit.collider.transform.position);
            } else
            {
                selectedObject = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            tracker.transform.rotation = new Quaternion();
        }

        Ray r = new Ray(tracker.transform.position, tracker.transform.rotation * new Vector3(0, 0, 1));

        /*if (Input.GetKeyDown (KeyCode.E)) {
			movement.startLerping(tracker.transform.forward, tracker.transform.position);
		}*/

        DrawLine(tracker.transform.position, r.GetPoint(100), Color.red);



        float power = Input.GetAxisRaw("Power");

        //Debug.Log(power + " " + truePitch + " " + trueYaw + " " + roll);

        // Flys the quadcopter using the inputs

        //if (Mathf.Abs(trueYaw) <= 0.1) lockYaw = true;
        //if (Mathf.Abs(truePitch) <= 0.1) lockPitch = true;

        float trueYaw = GetYawPitch(drone.transform.forward).x;
        float truePitch = GetYawPitch(drone.transform.up).y;

        if (lockYaw) trueYaw = 0;
        if (lockPitch) truePitch = 0;

        if (Mathf.Abs(truePitch) <= 0.1 && Mathf.Abs(trueYaw) <= 0.1)
        {
            lockYaw = lockPitch = true;
            Debug.Log("Fin rotation");
            drone.Drive(0, 0, 0, 0);
        }

        drone.Drive(0.1f, truePitch, trueYaw, 0);

        Debug.Log(drone.transform.up + " " + tracker.transform.forward);


        /*if (side == "left") {
			movement.leftDirection = GetDirection();
		} else if (side == "right") {
			movement.rightDirection = GetDirection();
		}*/

    }

    public Vector3 GetDirection()
    {
        return Vector3.Normalize(tracker.transform.rotation * new Vector3(0, 0, 1));
    }
}
