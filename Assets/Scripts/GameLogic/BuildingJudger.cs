using UnityEngine;

public enum BuildingType
{
    Kyoto,
    Other
}

public static class BuildingJudger
{
    public static BuildingType Judge(BuildingData buildingData)
    {
        if (buildingData == null)
        {
            Debug.LogWarning("Attempted to judge null BuildingData.");
            return BuildingType.Other;
        }

        if (buildingData.PrefectureName == "京都府")
        {
            return BuildingType.Kyoto;
        }
        else
        {
            return BuildingType.Other;
        }
    }
}
