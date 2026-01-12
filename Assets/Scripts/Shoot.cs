using UnityEngine;
using System.Collections.Generic;

public class Shoot : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int magazineSize;
    
    [Header("Ammo Distribution")]
    [Range(0, 1)]
    [SerializeField] private float ammoInMagRatio = 1f; // 1 = all in mag, 0 = all outside
    
    private int _currentAmmo;
    private int _ammoInMag;
    private int _ammoOutsideMag;

    [HideInInspector] public bool doesHaveWeapon = false;

    [SerializeField] private Stack<Bullet> shotBullets = new();
    [SerializeField] private List<Bullet> placedBullets = new List<Bullet>();

    private void OnValidate()
    {
        #if UNITY_EDITOR
        if (!Application.isPlaying && bulletPrefab != null)
        {
            UnityEditor.EditorApplication.delayCall += UpdateAmmoDistribution;
        }
        #endif
    }

    private void UpdateAmmoDistribution()
    {
        #if UNITY_EDITOR
        _ammoInMag = Mathf.RoundToInt(magazineSize * ammoInMagRatio);
        _ammoOutsideMag = magazineSize - _ammoInMag;

        // Clean up null references
        placedBullets.RemoveAll(b => b == null);

        // Remove excess bullets if slider moved up (more in mag)
        while (placedBullets.Count > _ammoOutsideMag)
        {
            int lastIndex = placedBullets.Count - 1;
            if (placedBullets[lastIndex] != null)
            {
                UnityEditor.Undo.DestroyObjectImmediate(placedBullets[lastIndex].gameObject);
            }
            placedBullets.RemoveAt(lastIndex);
        }

        // Add new bullets if slider moved down (more outside mag)
        while (placedBullets.Count < _ammoOutsideMag)
        {
            int index = placedBullets.Count;
            Vector3 spawnPos = transform.position + new Vector3((index % 5) * 1.5f, (index / 5) * 1.5f, 0);
            
            Bullet bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity, transform);
            bullet.name = $"PlacedBullet_{index}";
            bullet.SetOwner(gameObject);   bullet.setIgnoreTag(gameObject.tag);
            bullet.GetComponent<SpriteRenderer>().color = Color.cyan;
            bullet.enabled = false; // Disable script in editor
            
            placedBullets.Add(bullet);
        }
        
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    private void Awake()
    {
        if (Application.isPlaying)
        {
            _ammoInMag = Mathf.RoundToInt(magazineSize * ammoInMagRatio);
            _currentAmmo = _ammoInMag;
            
            // Enable placed bullets and add to stack
            for (int i = placedBullets.Count - 1; i >= 0; i--)
            {
                if (placedBullets[i] != null)
                {
                    placedBullets[i].enabled = true;
                    shotBullets.Push(placedBullets[i]);
                }
            }
        }
    }

    private void Start()
    {
        UpdateCanvas();
    }

    public void RequestShoot(Vector2 targetPos)
    {
        if (!doesHaveWeapon)
            return;
        
        if (_currentAmmo == 0) {
            Debug.LogWarning("Magazine empty, no bullets to shoot!");
            return;
        }

        ShootBullet(targetPos);
    }

    public void RequestRewind()
    {
        if (!doesHaveWeapon)
            return;
        
        if (shotBullets.Count == 0)
        {
            Debug.LogWarning("No bullets to rewind!");
            return;
        }

        ReverseBullet();
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.SetOwner(this.gameObject);
        shotBullets.Push(bullet);
        return bullet;
    }

    private void ShootBullet(Vector2 targetPos)
    {
        var bulletShot = CreateBullet();
        Vector3 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(new Vector3(targetPos.x, targetPos.y,
                Mathf.Abs(Camera.main.transform.position.z)));

        bulletShot.transform.up = mouseWorldPosition - transform.position;

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
        gathered.OnReachEvent -= GatherBullet;
        _currentAmmo++;
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        if (gameObject.CompareTag(GameManager.PlayerTag))
            UiManager.Instance.UpdateAmmo(_currentAmmo, magazineSize);
    }
}