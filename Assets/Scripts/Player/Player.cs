using NetTopologySuite.Index.Bintree;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSetting _playerSetting;
    [SerializeField] private GameObject _camera;
    private float _playerSpeed;
    private float _cameraSpeedVertical;
    private float _cameraSpeedHorizontal;
    private float _rayDistance;

    void Awake()
    {
        _playerSpeed = _playerSetting.MoveSpeed;
        _cameraSpeedVertical = _playerSetting.CameraSpeedVertical;
        _cameraSpeedHorizontal = _playerSetting.CameraSpeedHorizontal;
        _rayDistance = _playerSetting.RayDistance;
    }

    void Update()
    {
        Move();
        CameraMove();

    #if UNITY_EDITOR
            OnDrawGizmos();
    #endif
    }

    private void Move()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * _playerSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * _playerSpeed * Time.deltaTime);
        } 
        if(Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _playerSpeed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _playerSpeed * Time.deltaTime); ;
        }
    }

    private void CameraMove()
    {
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseX = Input.GetAxis("Mouse X");

        if (Input.GetMouseButton(0))  
        {
            // マウスの垂直移動でカメラの上下移動
            _camera.transform.Rotate(-mouseY * _cameraSpeedVertical * Time.deltaTime, 0f, 0f);
            // マウスの水平移動でプレイヤーの左右回転
            transform.Rotate(0f, mouseX * _cameraSpeedHorizontal * Time.deltaTime, 0f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // カメラの正面に目線を合わせるためにRayを描画
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * _rayDistance, Color.red);
    }
#endif
}