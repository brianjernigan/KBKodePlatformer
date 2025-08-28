using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    private void Awake()
    {
        var existing = FindObjectsByType<SystemRoot>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        
        DontDestroyOnLoad(gameObject);
    }
}
