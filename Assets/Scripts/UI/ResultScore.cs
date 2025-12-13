using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    void Start()
    {
        _scoreText.text = ScoreManager.Instance.CheckedObjectCount.ToString();
    }
}