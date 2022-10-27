using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _speed = 20.0f;
    [SerializeField] private float _downCamBorder = 1.0f;
    [SerializeField] private float _upCamBorder = 33.0f;

    private float _targetPos;

    private Vector2 _startPos;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _targetPos = transform.position.y;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) _startPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        else if (Input.GetMouseButton(0))
        {
            float pos = _camera.ScreenToWorldPoint(Input.mousePosition).y - _startPos.y;
            _targetPos = Mathf.Clamp(transform.position.y - pos, _downCamBorder, _upCamBorder);
        }
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, _targetPos, _speed * Time.deltaTime), transform.position.z);
    }
}
