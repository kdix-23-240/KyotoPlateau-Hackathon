using UnityEngine;

public class ModalManager : MonoBehaviour
{
    [SerializeField] private GameObject _successModal;
    [SerializeField] private GameObject _gameOverModal;

    public static ModalManager Instance { get; private set; }

    private float _completeRate; // New field

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

    public void ShowSuccessModal(BuildingData data)
    {
        if (_successModal == null)
        {
            Debug.LogError("_successModal is not assigned in the Inspector on the ModalManager script.");
            return;
        }
        var successModalComponent = _successModal.GetComponent<SuccessModal>();
        if (successModalComponent == null)
        {
            Debug.LogError("The assigned _successModal GameObject does not have a SuccessModal component.", _successModal);
            return;
        }

        Debug.Log("All components found. Calling SuccessModal.Show().");
        successModalComponent.Show(
            data.BuildingImage,
            data.PrefectureName,
            data.BuildingName,
            data.Description
        );
        Debug.Log("Successfully called Show() on modal for: " + data.BuildingName);
    }

    public void ShowGameOverModal(BuildingData data)
    {
        if (_gameOverModal == null)
        {
            Debug.LogError("_gameOverModal is not assigned in the Inspector on the ModalManager script.");
            return;
        }

        // Calculate _completeRate
        int checkedCount = ScoreManager.Instance.CheckedObjectCount;
        int totalOthers = ScoreManager.Instance.TotalOthersBuildingCount;
        
        if (totalOthers > 0)
        {
            _completeRate = ((float)checkedCount / totalOthers) * 100f;
        }
        else
        {
            _completeRate = 0f; // No 'other' buildings to check, so 0% completion.
        }
        Debug.Log($"Game Over! Checked: {checkedCount} / {totalOthers} Other buildings. Complete Rate: {_completeRate:F2}%");

        var gameOverModalComponent = _gameOverModal.GetComponent<SuccessModal>(); // This might be wrong, but keep for now.
        if (gameOverModalComponent == null)
        {
            Debug.LogError("The assigned _gameOverModal GameObject does not have a SuccessModal component.", _gameOverModal);
            return;
        }

        gameOverModalComponent.Show(
            data.BuildingImage,
            data.PrefectureName,
            data.BuildingName,
            data.Description
        );
        Debug.Log("Game Over: Checked a Kyoto building (" + data.BuildingName + ").");
    }
}
