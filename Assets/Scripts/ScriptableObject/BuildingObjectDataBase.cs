using UnityEngine;

[CreateAssetMenu(fileName = "BuildingObjectDatabase", menuName = "ScriptableObjects/BuildingObjectDatabase", order = 1)]
public class BuildingObjectDatabase : ScriptableObject
{
    public BuildingData[] OtherBuildings;
}