using DG.Tweening;
using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class BrickTileView : MonoBehaviour, IReadOnlyBrickTileView
    {
        [SerializeField] private Color _fadeColorOffset;
        [SerializeField] private float _fadeTime;

        [Space]

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private MeshFilter _meshFilter;

        private Color _startColor;

        public Mesh Mesh => _meshFilter.sharedMesh;
        public Color Color => _meshRenderer.material.color;

        /// <summary>
        /// Выставляет цвет тайла
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            _meshRenderer.material.color = color;
            _startColor = color;
        }

        /// <summary>
        /// Активирует Rigidbody ( снимает флаг isKinematic )
        /// </summary>
        public void ActiveRigidbody()
        {
            _rigidbody.isKinematic = false;
        }

        /// <summary>
        /// Добавляет импульс к Rigidbody
        /// </summary>
        public void AddForceToRigidbody()
        {
            Vector3 sideImpulse = new(Random.Range(0f, 10f), 10, Random.Range(0f, 10f));
            _rigidbody.AddForce(sideImpulse, ForceMode.Impulse);
        }

        public Tweener FadeIn()
        {
            return _meshRenderer.material.DOColor(_startColor + _fadeColorOffset, _fadeTime);
        }

        public Tweener FadeOut()
        {
            return _meshRenderer.material.DOColor(_startColor, _fadeTime);
        }

        public void FastFadeOut()
        {
            _meshRenderer.material.color = _startColor;
        }
    }
}