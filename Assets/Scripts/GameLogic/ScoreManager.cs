using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CheckedObjectCount { get; private set; } = 0;
    public int TotalOthersBuildingCount { get; set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void IncrementCount()
    {
        CheckedObjectCount++;
        Debug.Log("Checked objects count: " + CheckedObjectCount);
    }

    public void ResetCount()
    {
        CheckedObjectCount = 0;
        Debug.Log("Count has been reset.");
    }
}
