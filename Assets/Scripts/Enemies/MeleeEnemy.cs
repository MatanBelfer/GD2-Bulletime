using System.Collections;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    protected override IEnumerator Attack()
    {
        Debug.Log("I attacked!");
        yield return new WaitForSeconds(2f);
        ChangeState(Scan());
    }
}
