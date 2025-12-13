using UnityEngine;

public class BuildingDataKeeper : MonoBehaviour
{
    [SerializeField] private BuildingData _buildingData;
    public BuildingData BuildingData => _buildingData;
}