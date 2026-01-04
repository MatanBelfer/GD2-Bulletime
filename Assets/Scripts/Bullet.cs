using System;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{

    public event UnityAction<GameObject,int> OnHit;
    
    // Set For Prefab
    [SerializeField] private int damage;
    [SerializeField] private float speed;
    [SerializeField] private float timeToDie;
    private float _timeAlive;
    
    // Set Through Spawner
    [HideInInspector] public string targetTag;
    [HideInInspector] public string ignoreTag;
    [HideInInspector] public Vector3 direction;
    

    void Start()
    {
        _timeAlive = 0f;
    }
    
    
    private void Update()
    {
        transform.position += direction * (speed * Time.deltaTime);
        
        _timeAlive += Time.deltaTime;
        if (_timeAlive > timeToDie) {
            Destroy(gameObject);
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (targetTag != "" && other.CompareTag(targetTag)) {
            OnHit?.Invoke(other.gameObject,damage);
        }
        
        if ((ignoreTag != "" && !other.CompareTag(ignoreTag)) || ignoreTag == "")
            Destroy(gameObject);
    }
}
