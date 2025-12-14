using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSetting _playerSetting;
    [SerializeField] private GameObject _camera;
    private float _playerSpeed;
    private float _cameraSpeedVertical;
    private float _cameraSpeedHorizontal;
    private float _rayDistance;
    private bool _isCaughtObject = false;
    private GameObject _caughtObject;

    // For outline effect
    private Shader _outlineShader;
    private Material _outlineMaterial;
    private Renderer _highlightedRenderer;
    private Material[] _originalMaterials;

    void Awake()
    {
        _playerSpeed = _playerSetting.MoveSpeed;
        _cameraSpeedVertical = _playerSetting.CameraSpeedVertical;
        _cameraSpeedHorizontal = _playerSetting.CameraSpeedHorizontal;
        _rayDistance = _playerSetting.RayDistance;
        Debug.Log("Player script initialized. Ray distance: " + _rayDistance);

        // Initialize for outline effect
        _outlineShader = Shader.Find("Custom/OutlineOnly");
        if (_outlineShader == null)
        {
            Debug.LogError("Outline shader 'Custom/OutlineOnly' not found. Make sure it's in the project and there are no compilation errors.", this);
        }
        _outlineMaterial = new Material(_outlineShader);
        _outlineMaterial.SetColor("_OutlineColor", Color.yellow);
    }

    void Update()
    {
        Move();
        CameraMove();
        LookForObjects();
        ProcessCheckInput();

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
    
    private void LookForObjects()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _rayDistance))
        {
            if (_caughtObject != hit.collider.gameObject)
            {
                Debug.Log("Looking at: " + hit.collider.gameObject.name);
                
                // A new object is being looked at. Remove outline from the previous one.
                RemoveOutline();

                // Add outline to the new one.
                _highlightedRenderer = hit.collider.GetComponent<Renderer>();
                if (_highlightedRenderer != null)
                {
                    _originalMaterials = _highlightedRenderer.materials;
                    Material[] newMaterials = new Material[_originalMaterials.Length + 1];
                    _originalMaterials.CopyTo(newMaterials, 0);
                    newMaterials[newMaterials.Length - 1] = _outlineMaterial;
                    _highlightedRenderer.materials = newMaterials;
                }
            }
            _isCaughtObject = true;
            _caughtObject = hit.collider.gameObject;
        }
        else
        {
            if (_caughtObject != null)
            {
                Debug.Log("No longer looking at any object.");
                // We are no longer looking at anything, so remove the outline.
                RemoveOutline();
            }
            _isCaughtObject = false;
            _caughtObject = null;
        }
    }

    private void RemoveOutline()
    {
        if (_highlightedRenderer != null)
        {
            _highlightedRenderer.materials = _originalMaterials;
            _highlightedRenderer = null;
            _originalMaterials = null;
        }
    }

    private void ProcessCheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _isCaughtObject)
        {
            if (_caughtObject == null)
            {
                Debug.LogError("_caughtObject is null, but _isCaughtObject is true. This should not happen.");
                return;
            }
            CheckObject(_caughtObject);
        }
    }

    private void CheckObject(GameObject obj)
    {
        if (TryGetBuildingData(obj, out BuildingData buildingData))
        {
            BuildingType type = BuildingJudger.Judge(buildingData);
            switch (type)
            {
                case BuildingType.Kyoto:
                    ModalManager.Instance.ShowGameOverModal(buildingData);
                    break;
                case BuildingType.Other:
                    ScoreManager.Instance.IncrementCount();
                    ModalManager.Instance.ShowSuccessModal(buildingData);
                    Destroy(obj);
                    RemoveOutline();
                    _caughtObject = null;
                    _isCaughtObject = false;
                    break;
            }
        }
    }

    private bool TryGetBuildingData(GameObject obj, out BuildingData buildingData)
    {
        buildingData = null;
        Debug.Log("Attempting to interact with: " + obj.name);

        var buildingDataKeeper = obj.GetComponent<BuildingDataKeeper>();
        if (buildingDataKeeper == null)
        {
            Debug.LogError("The object '" + obj.name + "' does not have a BuildingDataKeeper component.", obj);
            return false;
        }

        buildingData = buildingDataKeeper.BuildingData;
        if (buildingData == null)
        {
            Debug.LogError("BuildingData on '" + obj.name + "' is null.", obj);
            return false;
        }
        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // カメラの正面に目線を合わせるためにRayを描画
        Debug.DrawRay(_camera.transform.position, _camera.transform.forward * _rayDistance, Color.red);
    }
#endif
}