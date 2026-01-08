using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private const string TERRAIN_TAG = "Terrain";
    public event Action<Bullet> OnReachEvent;

    [HideInInspector] public Vector3 Direction;

    // Set For Prefab
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    private bool _isMoving;
    private string _ignoreTag;

    void Start()
    {
        _ignoreTag = gameObject.tag;
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
        if (other.CompareTag(_ignoreTag)) return;

        IDamageable hit;
        if (other.TryGetComponent<IDamageable>(out hit))
        {
            hit.OnHit(damage);
        }

        if (other.CompareTag(TERRAIN_TAG))
        {
            _isMoving = false;
            transform.parent = other.transform; //To allow the bullet to move along when it's stuck, can do some puzzles with this
        }
    }

    public void Rewind(Transform target)
    {
        _isMoving = false; //To reset Move() if still in the air.
        transform.parent = null;
        GetComponent<SpriteRenderer>().color = Color.gold;
        StartCoroutine(RewindTowards(target));
    }

    private IEnumerator RewindTowards(Transform target)
    {
        yield return null;
        _ignoreTag = TERRAIN_TAG;
        _isMoving = true;
        while (_isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * speed * 1.5f);
            transform.up = target.position - transform.position; //Set correct rotation
            if(Vector2.Distance(transform.position, target.position) <= 0.05f)
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
