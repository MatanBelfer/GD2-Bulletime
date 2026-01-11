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
        explosion.gameObject.SetActive(true);
        foreach (var collided in Physics2D.OverlapCircleAll(transform.position, scanRadius))
        {
            if (collided.CompareTag(GameManager.PlayerTag))
            {
                Debug.Log($"{gameObject.name}: Damaged player for {damage} damage.");
                IDamageable damaged;
                if (collided.TryGetComponent<IDamageable>(out damaged))
                {
                    damaged.OnHit(damage);
                }
            }
        }
        Destroy(this.gameObject, 1f);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
