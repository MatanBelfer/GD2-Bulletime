using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Tags

    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";
    
    #endregion


    #region Singleton Structure

    public static GameManager Instance;


    private void Awake()
    {
        Instance = this;
    }
    
    #endregion
}
