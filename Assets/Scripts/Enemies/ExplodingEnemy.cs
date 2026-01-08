using System.Collections;
using UnityEngine;

public class ExplodingEnemy : Enemy
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionRadius;

    protected override IEnumerator Attack()
    {
        yield return null;
        anim.SetTrigger("isAttacking");
        //ChangeState(Scan());
    }

    public void Explode()
    {
        Instantiate(explosion);
        foreach(var collided in Physics2D.OverlapCircleAll(transform.position, scanRadius))
        {
            if(collided.CompareTag("Player"))
            {
                collided.GetComponent<IDamageable>().OnHit(damage);
            }
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
