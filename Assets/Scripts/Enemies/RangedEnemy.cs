using System.Collections;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float attackInterval = 10f;
    private Shoot _shoot;

    protected override void Awake()
    {
        base.Awake();
        _shoot = GetComponent<Shoot>();
    }

    protected override IEnumerator Attack()
    {
        while(Vector2.Distance(transform.position, targetToChase.position) <= attackRangeRadius)
        {
            _shoot.RequestShoot(targetToChase.position);
            //Debug.Log("shoot");
            yield return new WaitForSeconds(attackInterval);
        }
        ChangeState(Chase());
    }
}
