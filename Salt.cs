using UnityEngine;

public class Salt : MonoBehaviour
{
    private Vector2 startPosition;
    private Rigidbody2D rb;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (transform.position.y <= -3)
            ResetPosition();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        ResetPosition();
    }

    public void RealeaseSalt()
    {
        rb.isKinematic = false;
    }

    void ResetPosition()
    {
        transform.position = startPosition;
        rb.isKinematic = true;
    }
}
