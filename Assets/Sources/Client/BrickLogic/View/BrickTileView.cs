using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class BrickTileView : MonoBehaviour, IReadOnlyBrickTileView
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        public Mesh Mesh => _meshFilter.sharedMesh;
        public Color Color => _meshRenderer.material.color;

        public void SetColor(Color color)
        {
            _meshRenderer.material.color = color;
        }

        public void ActiveRigidbody()
        {
            _rigidbody.isKinematic = false;
        }

        public void AddForceToRigidbody()
        {
            Vector3 sideImpulse = new(Random.Range(0f, 10f), 10, Random.Range(0f, 10f));
            _rigidbody.AddForce(sideImpulse, ForceMode.Impulse);
        }
    }
}