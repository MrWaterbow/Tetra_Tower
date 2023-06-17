using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class BrickTileView : MonoBehaviour, IReadOnlyBrickTileView
    {
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        public Mesh Mesh => _meshFilter.sharedMesh;
        public Color Color => _meshRenderer.material.color;

        public void SetColor(Color color)
        {
            _meshRenderer.material.color = color;
        }
    }
}