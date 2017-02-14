using UnityEngine;

public class Bone : MonoBehaviour
{
    private Vector2 startPosition;
    private Quaternion startRotation;
    private Vector3 offset;

    public LayerMask whatIsMeat;
    //private Transform pointA;
    //private Transform pointB;
    public bool isInside= true;
    private bool callOnce = false;

    //Components
    [HideInInspector]public Rigidbody2D rb;
    [HideInInspector]public Collider2D boneCollider;
    private SteakController steak;

    //Properties
    public Vector2 StartPosition { get { return startPosition; } set {} }
    public Quaternion StartRotation { get { return startRotation; } set{} }
    public bool CallOnce { get { return callOnce;} set { callOnce = value; } }

    public Vector2 pointA;
    public Vector2 pointB;

    void Start()
    {
        steak = GetComponentInParent<SteakController>();
        rb = GetComponent<Rigidbody2D>();
        boneCollider = GetComponent<Collider2D>();

        startPosition = transform.position;
        startRotation = transform.rotation;


    }

    void FixedUpdate()
    {
        pointA = this.gameObject.transform.GetChild(0).transform.position;
        pointB = this.gameObject.transform.GetChild(1).transform.position;

        //isInside = Physics2D.OverlapArea(pointA, pointB, whatIsMeat);
        Debug.DrawLine(pointA, pointB, Color.red);

        if (!isInside && !callOnce)
        {
            callOnce = true;
            steak.RegisterBone(this);
            rb.isKinematic = false;
            boneCollider.enabled = false;
            rb.AddTorque(180, ForceMode2D.Force);
        }

        else if(transform.position.y <= -20)
        {
            gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        offset = this.transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Meat" || other.gameObject.tag == "FirstSteak")
        {
            Debug.Log("Trigger exit");
            isInside = false;
        }
    }
}
