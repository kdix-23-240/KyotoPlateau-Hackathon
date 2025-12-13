using UnityEngine;

public class BuildingModalDisappearButton : MonoBehaviour
{
    [SerializeField] private GameObject _buildingModal;
    public void OnClick()
    {
        Disappear();
    }

    private void Disappear()
    {
        _buildingModal.GetComponent<SuccessModal>().Hide();
    }
}