using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isStuck = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float speed)
    {
        rb.linearVelocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isStuck) return;

        isStuck = true;
        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; 
        
        transform.SetParent(collision.transform);
        
        GetComponent<Collider2D>().enabled = false;
    }
}
