using Sources.Factories;
using UnityEngine;

namespace Sources.BlockLogic
{
    public class BlockVisualization : MonoBehaviour, IBlockVisualization
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Transform _transform;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        private VisualizationType _visualizationType;

        public GameObject GameObject => _gameObject;
        public MeshFilter MeshFilter => _meshFilter;
        public MeshRenderer MeshRenderer => _meshRenderer;

        public void Show(Mesh mesh, Color color)
        {
            _meshFilter.mesh = mesh;
            _meshRenderer.sharedMaterial.color = color;

            SetVisualizationEffect();
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
        }

        private void SetVisualizationEffect()
        {
            print("Set effect...");
        }
    }
}