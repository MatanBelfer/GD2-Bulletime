using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamageable
{
    private const float SCAN_INTERVAL = 1f;
    [SerializeField] protected int health = 5;
    [SerializeField] protected int damage;
    [SerializeField] protected float moveSpeed = 10;
    [SerializeField] protected float scanRadius = 5;
    [SerializeField] protected float attackRangeRadius = 0.5f;
    protected Transform targetToChase;
    protected Coroutine state;
    protected Animator anim;
    private SpriteRenderer _sr;



    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>(); 
    }

    void Start()
    {
        ChangeState(Scan());
    }

    protected void ChangeState(IEnumerator newState)
    {
        if(state != null) StopCoroutine(state);
        state = StartCoroutine(newState);
        // Debug.Log($"{gameObject.name}: Changed state to {newState}");
    }

    protected virtual IEnumerator Scan()
    {
        targetToChase = null;
        while(targetToChase == null)
        {
            var collided = Physics2D.OverlapCircleAll(transform.position, scanRadius);
            foreach(var collider in collided)
            {
                if(collider.CompareTag(GameManager.PlayerTag))
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
        anim.SetTrigger("isChasing");
        while(Vector2.Distance(transform.position, targetToChase.position) < scanRadius)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetToChase.position, moveSpeed * Time.deltaTime);
            
            _sr.flipX = CheckFlip();

            if(Vector2.Distance(transform.position, targetToChase.position) <= attackRangeRadius)
            {
                ChangeState(Attack());
            }
            yield return null;
        }
        anim.SetTrigger("isScanning");
        yield return new WaitForSeconds(SCAN_INTERVAL);
        ChangeState(Scan());
    }

    private bool CheckFlip()
    {
        return (transform.position - targetToChase.position).x < 0;
    }
    protected abstract IEnumerator Attack();

    public void OnHit(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            anim.SetTrigger("onDeath");
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
     Gizmos.DrawWireSphere(transform.position, scanRadius);
        Gizmos.DrawWireSphere(transform.position, attackRangeRadius);
    }
}
