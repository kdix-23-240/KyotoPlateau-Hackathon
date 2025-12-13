using UnityEngine;

public class GoResultButton : MonoBehaviour
{
    [SerializeField] private GameObject _currentModal;
    [SerializeField] private GameObject _resultModal;
    public void OnClick()
    {
        GoResult();
    }

    private void GoResult()
    {
        _currentModal.SetActive(false);
        _resultModal.SetActive(true);
    }
}