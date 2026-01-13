using System;
using UnityEngine;

public class HintArea : MonoBehaviour
{
    [SerializeField] private string hintText;

    [SerializeField] private bool isOneTimeUse = true;
    
    public event Action<string> OnPlayerEnter;
    public event Action OnPlayerExit;
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.PlayerTag)) {
            OnPlayerEnter?.Invoke(hintText);
        }    
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GameManager.PlayerTag)) {
            OnPlayerExit?.Invoke();
            if (isOneTimeUse) {
                Destroy(this.gameObject);
            }
        }
    }
}
