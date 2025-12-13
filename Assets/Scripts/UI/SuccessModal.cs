using UnityEngine;
using UnityEngine.UI;

public class SuccessModal : MonoBehaviour
{
    [SerializeField] private Image _buildingImage;
    [SerializeField] private Text _prefectureName;
    [SerializeField] private Text _buildingName;
    [SerializeField] private Text _description;

    public void Show(Sprite buildingImage, string prefectureName, string buildingName, string description)
    {
        _buildingImage.sprite = buildingImage;
        _prefectureName.text = prefectureName;
        _buildingName.text = buildingName;
        _description.text = description;

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}