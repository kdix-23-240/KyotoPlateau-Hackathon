using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSetting", menuName = "ScriptableObjects/PlayerSetting", order = 1)]
public class PlayerSetting : ScriptableObject
{
    public int MoveSpeed;
    public int CameraSpeedVertical;
    public int CameraSpeedHorizontal;
    public int RayDistance;
}