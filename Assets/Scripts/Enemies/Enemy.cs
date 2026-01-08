using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    private const float SCAN_INTERVAL = 1f;
    [SerializeField] protected int health = 5;
    [SerializeField] protected float moveSpeed = 10;
    [SerializeField] protected float scanRadius = 5;
    [SerializeField] protected float attackRangeDistance = 0.5f;
    protected Coroutine state;

    private Transform targetToChase;

    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();    
    }

    void Start()
    {
        ChangeState(Scan());
    }

    protected void ChangeState(IEnumerator newState)
    {
        if(state != null) StopCoroutine(state);
        state = StartCoroutine(newState);
        Debug.Log($"{gameObject.name}: Changed state to {newState}");
    }

    protected virtual IEnumerator Scan()
    {
        targetToChase = null;
        while(targetToChase == null)
        {
            var collided = Physics2D.OverlapCircleAll(transform.position, scanRadius);
            Debug.Log($"{gameObject.name}: Scanned {collided.Length} objects.");
            foreach(var collider in collided)
            {
                if(collider.CompareTag("Player"))
                {
                    targetToChase = collider.transform;
                    ChangeState(Chase());
                }
            }

                //In case of many colliders checked, this may cause some spikes- So I'm randomizing the time to make it less noticeable.
            float randomInterval = Random.Range(SCAN_INTERVAL + .25f, SCAN_INTERVAL - .25f);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    protected virtual IEnumerator Chase()
    {
        while(Vector2.Distance(transform.position, targetToChase.position) < scanRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetToChase.position, moveSpeed * Time.deltaTime);
            if(Vector2.Distance(transform.position, targetToChase.position) <= attackRangeDistance)
            {
                ChangeState(Attack());
            }
            yield return null;
        }
        Debug.Log($"{gameObject.name}: Out of range: scanning again.");
        yield return new WaitForSeconds(SCAN_INTERVAL);
        ChangeState(Scan());
    }

    protected abstract IEnumerator Attack();

    public void OnHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            _anim.SetTrigger("onDeath");
        }
    }

    protected virtual void OnDrawGizmos()
    {
     Gizmos.DrawWireSphere(transform.position, scanRadius);
        Gizmos.DrawWireSphere(transform.position, attackRangeDistance / 2);
    }
}
