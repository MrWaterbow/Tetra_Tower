using Sources.Factories;
using UnityEngine;

namespace Sources.BlockLogic
{
    public class BlockVisualization : MonoBehaviour, IBlockVisualization
    {
        [SerializeField] private float _transparency;
        [SerializeField] private VisualizationType _visualizationType;

        [Space]

        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        private Color _color;

        public GameObject GameObject => _gameObject;
        public MeshFilter MeshFilter => _meshFilter;
        public MeshRenderer MeshRenderer => _meshRenderer;

        private void OnValidate()
        {
            _transparency = Mathf.Clamp01(_transparency);
        }

        public void Show(Mesh mesh, Color color)
        {
            _color = color;

            _meshFilter.mesh = mesh;
            _meshRenderer.material.color = color;

            SetVisualizationEffect();

            _gameObject.SetActive(true);
        }
        public void Rotate(int degree)
        {
            _transform.rotation = Quaternion.Euler(0, degree, 0);
        }

        public void Hide()
        {
            _gameObject.SetActive(false);
        }

        public void SetPosition(Vector3 position)
        {
            _transform.position = position;
        }

        public void SetVisualization(VisualizationType type)
        {
            _visualizationType = type;

            SetVisualizationEffect();
        }

        private void SetVisualizationEffect()
        {
            switch(_visualizationType)
            {
                case VisualizationType.Full:
                    _meshRenderer.material.color = _color;
                    break;
                case VisualizationType.Transparency:
                    MeshRenderer.material.color = new Color(_color.r, _color.g, _color.b, _transparency);
                    break;
            }
        }

        public void SetRenderQueue(int value)
        {
            _meshRenderer.material.renderQueue = value;
        }
    }
}