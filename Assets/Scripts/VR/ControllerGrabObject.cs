using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;
    // 1
    private GameObject collidingObject;
    // 2
    public GameObject objectInHand;

    public GameObject bow;
    public GameObject handArrow;

    public BowScript bs;

    private bool isTriggered;
    public bool bowIsNooked = false;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        bow = GameObject.FindGameObjectWithTag("playerbow");
        handArrow = GameObject.FindGameObjectWithTag("playerhandarrow");
    }

    private void Start()
    {

        if (handArrow != null)
        {
            handArrow.SetActive(false);
        } else
        {
            Debug.Log("THIS KEEPS HAPPENING BUT SHOULDNT!");
        }
    }

    private void SetCollidingObject(Collider col)
    {
        // 1
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }
        // 2
        collidingObject = col.gameObject;
    }

    // Update is called once per frame
    void Update () {

        if (isTriggered)
        {
            bs.ayy += 0.01f;
        }

        if (Controller.GetHairTriggerDown() && bowIsNooked && Controller.index == 2)
        {
            isTriggered = true;
        } else if (Controller.GetHairTriggerDown()) { 
            if (collidingObject) 
            {
                GrabObject();
            }
        }
        // 2
        if (Controller.GetHairTriggerUp())
        {
            isTriggered = false;
            if (objectInHand)
            {
                ReleaseObject();
            }
        }
    }

    // 1
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    // 2
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    // 3
    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void GrabObject()
    {
        // 1
        objectInHand = collidingObject;
        collidingObject = null;
        // 2
        if (objectInHand == GameObject.FindGameObjectWithTag("bowhandle") && gameObject == GameObject.FindGameObjectWithTag("leftcontroller"))
        {
            bow.SetActive(true);
            objectInHand.gameObject.SetActive(false);

            GameObject rightController = GameObject.FindGameObjectWithTag("rightcontroller");
            handArrow.transform.parent = rightController.transform;
            bow.GetComponent<BowScript>().handArrowNookPosition = handArrow;
            handArrow.SetActive(true);
        } else { 
        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
        }
    }

    // 3
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        // 1
        if (GetComponent<FixedJoint>())
        {
            // 2
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());
            // 3
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        // 4
        objectInHand = null;
    }
}
