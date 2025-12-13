using UnityEngine;

public class ModalManager : MonoBehaviour
{
    [SerializeField] private GameObject _successModal;
    [SerializeField] private GameObject _gameOverModal;

    public static ModalManager Instance { get; private set; }

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

        var gameOverModalComponent = _gameOverModal.GetComponent<SuccessModal>();
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
