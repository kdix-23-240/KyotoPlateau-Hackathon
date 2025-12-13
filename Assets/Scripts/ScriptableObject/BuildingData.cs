using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/BuildingData", order = 1)]

public class BuildingData : ScriptableObject
{
    public string PrefectureName = "京都府";
    public GameObject BuildingObject;
    public Sprite BuildingImage;
    public string BuildingName;
    public string Description;
}