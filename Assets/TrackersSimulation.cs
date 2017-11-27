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

    private int leftDevice;

    private float ceiling;

    // Use this for initialization
    void Start()
    {
        tracker = this.gameObject;
        myLine = new GameObject();

        leftDevice = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.FarthestLeft);

        lr = myLine.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));

        lr.endWidth = lr.startWidth = 0.02f;

        float pitchMax = 75;
        ceiling = pitchMax * 0.5f / 90;
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        myLine.transform.position = start;
        lr.startColor = lr.endColor = color;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }



    //Find the line of intersection between two planes.
    //The inputs are two game objects which represent the planes.
    //The outputs are a point on the line and a vector which indicates it's direction.
    /*void planePlaneIntersection(out Vector3 linePoint, out Vector3 lineVec, Vector3 normal1, Vector3 pt1, Vector3 normal2, Vector3 pt2)
    {

        linePoint = Vector3.zero;
        lineVec = Vector3.zero;

        //Get the normals of the planes.
        Vector3 plane1Normal = normal1;
        Vector3 plane2Normal = normal2;

        //We can get the direction of the line of intersection of the two planes by calculating the
        //cross product of the normals of the two planes. Note that this is just a direction and the line
        //is not fixed in space yet.
        lineVec = Vector3.Cross(plane1Normal, plane2Normal);

        //Next is to calculate a point on the line to fix it's position. This is done by finding a vector from
        //the plane2 location, moving parallel to it's plane, and intersecting plane1. To prevent rounding
        //errors, this vector also has to be perpendicular to lineDirection. To get this vector, calculate
        //the cross product of the normal of plane2 and the lineDirection.     
        Vector3 ldir = Vector3.Cross(plane2Normal, lineVec);

        float numerator = Vector3.Dot(plane1Normal, ldir);

        //Prevent divide by zero.
        if (Mathf.Abs(numerator) > 0.000001f)
        {

            Vector3 plane1ToPlane2 = plane1. - plane2.transform.position;
            float t = Vector3.Dot(plane1Normal, plane1ToPlane2) / numerator;
            linePoint = plane2.transform.position + t * ldir;
        }
    }*/

    float GetYawPitch(Vector3 dir)
    {
        /*Vector3 direction = GetDirection();


        Vector3 dronePos = dir;

        float pitch = Mathf.Asin(direction.y);
        float pitchb = Mathf.Asin(dronePos.y);

        float yaw = Mathf.Asin(direction.x / Mathf.Cos(pitch)); //Beware cos(pitch)==0, catch this exception!
        float yawb = Mathf.Asin(dronePos.x / Mathf.Cos(pitchb));

        float trueYaw = yawb - yaw;

        Debug.Log("yaw " + yaw + " - true " + trueYaw);
        
        return trueYaw;*/
        
        Vector2 direction = new Vector2(GetDirection().x, GetDirection().z);
        Vector2 droneDir = new Vector2(dir.x, dir.z);

        Debug.Log("angle " + Vector2.Angle(direction, droneDir));
        
        return (Vector2.Dot(direction, droneDir) < 0 ?  -1 : 1) * Vector2.Angle(direction, droneDir) * Mathf.Deg2Rad;
    }


    float previousPower = 0;
    float power = 0;

    bool lockPitch = false;
    bool lockYaw = false;

    float lastPitch;
    float lastYaw;


    float cumulatedPitch = 0;
    float lastBtnPressure = 0;
    float pitchSum;
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


        DrawLine(tracker.transform.position, r.GetPoint(100), Color.red);


        float trueYaw = GetYawPitch(drone.transform.forward);

        if (lockYaw) trueYaw = 0;
        
        if (lastYaw * trueYaw < 0) lockYaw = true;
        

        if (leftDevice != -1)
        {
            float buttonPression = SteamVR_Controller.Input(leftDevice).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x;

            float deltaPressure = buttonPression - lastBtnPressure;

            Vector3 intersect = Vector3.Cross(drone.transform.right, Vector3.up);

            float resetPitchAngle = Vector3.Angle(intersect, drone.transform.forward);

            //Debug.Log("button pression " + buttonPression);

            drone.Drive(buttonPression, (buttonPression > 0.1) ? 1 : 0, trueYaw, 0);
           
            lastBtnPressure = buttonPression;
        }


        lockPitch = lockYaw = false;

        lastYaw = trueYaw;
     
    }

    public Vector3 GetDirection()
    {
        return Vector3.Normalize(tracker.transform.rotation * new Vector3(0, 0, 1));
    }
}
