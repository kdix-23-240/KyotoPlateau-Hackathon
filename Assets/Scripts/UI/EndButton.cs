using UnityEngine;

public class EndButton : MonoBehaviour
{
    [SerializeField] private GameObject _resutlModal;
    public void OnClick()
    {
        End();
    }

    private void End()
    {
        _resutlModal.SetActive(true);
    }
}