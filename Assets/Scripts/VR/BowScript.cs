using UnityEngine;

public class BowScript : MonoBehaviour {
    private SteamVR_TrackedObject trackedObj;

    public bool bowInHand = false;

    float check;
    public float ayy;

    private GameObject bowNookPosition;
    public GameObject handArrowNookPosition;

    private Animator animator;
    private int framesUnchanged;

    private GameObject arrow;

    public ControllerGrabObject rightController;

    public bool arrowVisible = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        arrow = GameObject.FindWithTag("playerarrow");
        bowNookPosition = GameObject.FindGameObjectWithTag("nook");

    }

    // Use this for initialization
    void Start () {
        arrow.SetActive(false);
        animator.enabled = false;

        // If there is an error here remember to re add the right controller into the BowScript in the scene
        rightController.bs = this;

        gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update () {

        if (handArrowNookPosition != null && bowNookPosition != null)
        {
            if (Vector3.Distance(handArrowNookPosition.transform.position, bowNookPosition.transform.position) > 162.7f)
            {
                handArrowNookPosition.SetActive(false);
                arrow.SetActive(true);
                rightController.bowIsNooked = true;
            }

            if (arrow.activeInHierarchy)
            {
                //if (Controller.GetHairTriggerDown())
                //{
                //    Debug.Log("ay");
                //}
            }

            if (ayy != check)
            {
                check = ayy;
                animator.enabled = true;
                animator.Play(0, 0, ayy);
                framesUnchanged = 0;
                if (ayy > 1)
                {
                    ayy = 0;
                }
            }
            else
            {
                framesUnchanged++;
                if (framesUnchanged > 2)
                {
                    animator.enabled = false;
                }
            }
        }
    }

    private void LateUpdate()
    {
    }

    public void activateBow()
    {
        gameObject.SetActive(true);
    }
}
