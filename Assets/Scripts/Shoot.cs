using UnityEngine;
using System.Collections.Generic;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int magazineSize;
    private int _currentAmmo;

    private Stack<Bullet> shotBullets = new();

    private void Awake()
    {
        _currentAmmo = magazineSize;
    }

    private void Start()
    {
        UpdateCanvas();
    }

    public void RequestShoot(Vector2 targetPos)
    {
        if(_currentAmmo == 0)
        {
            Debug.LogWarning("Magazine empty, no bullets to shoot!");
            return;
        }
        ShootBullet(targetPos);
    }

    public void RequestRewind()
    {
        if (shotBullets.Count == 0)
        {
            Debug.LogWarning("Magazine full, no bullets to rewind!");
            return;
        }
        ReverseBullet();
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();

        bullet.TargetTag = GameManager.EnemyTag;
        bullet.IgnoreTag = GameManager.PlayerTag;
        shotBullets.Push(bullet);
        return bullet;
    }

    private void ShootBullet(Vector2 targetPos)
    {
        var bulletShot = CreateBullet();
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(targetPos.x, targetPos.y, Mathf.Abs(Camera.main.transform.position.z)));

        bulletShot.transform.up = mouseWorldPosition - transform.position; //Set correct rotation

        _currentAmmo--;
        UpdateCanvas();
    }

    private void ReverseBullet()
    {
        var lastBullet = shotBullets.Pop();
        lastBullet.Rewind(transform);
        lastBullet.OnReachEvent += GatherBullet;
    }

    private void GatherBullet(Bullet gathered)
    {
        _currentAmmo++;
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        UiManager.Instance.UpdateAmmo(_currentAmmo, magazineSize);
    }
}
