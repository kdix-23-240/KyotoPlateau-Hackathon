using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    void OnEnable()
    {
        if (ScoreManager.Instance != null)
        {
            float scoreRate = ScoreManager.Instance.ScoreRate;
            _scoreText.text = scoreRate.ToString("F1") + "%";
        }
        else
        {
            _scoreText.text = "Error";
            Debug.LogError("ScoreManager.Instance is null. Cannot display score.");
        }
    }
}