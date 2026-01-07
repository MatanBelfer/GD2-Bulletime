using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public event Action<Bullet> OnReachEvent;

    // Set Through Spawner
    [HideInInspector] public string TargetTag;
    [HideInInspector] public string IgnoreTag;
    [HideInInspector] public Vector3 Direction;

    // Set For Prefab
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    private bool _isMoving;


    void Start()
    {
        StartCoroutine(nameof(Move));
    }

    
    private IEnumerator Move()
    {
        _isMoving = true;
        while (_isMoving)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hit;
        if (other.TryGetComponent<IDamageable>(out hit))
        {
            hit.OnHit();
        }

        if (other.CompareTag("Terrain"))
        {
            _isMoving = false;
        }
    }

    public void Rewind(Transform target)
    {
        _isMoving = false; //To reset Move() if still in the air.
        StartCoroutine(RewindTowards(target));
    }

    private IEnumerator RewindTowards(Transform target)
    {
        yield return null;
        _isMoving = true;
        while (_isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed * 1.5f);
            transform.up = target.position - transform.position; //Set correct rotation
            if (Vector2.Distance(transform.position, target.position) <= 0.05f)
            {
                OnReachEvent?.Invoke(this);
                DestroyBullet();
            }
            yield return null;
        }
    }

    private void DestroyBullet()
    {
        StopAllCoroutines();
        Destroy(this.gameObject);
    }
}
