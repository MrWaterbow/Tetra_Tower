using DG.Tweening;
using Server.BrickLogic;
using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class BrickView : MonoBehaviour, IReadOnlyBrickView
    {
        /// <summary>
        /// Transform ( ��������� Unity )
        /// </summary>
        [SerializeField] private Transform _transform;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private MeshRenderer _meshRenderer;

        [Space]

        [SerializeField] private Color[] _colors;
        /// <summary>
        /// ��������� �������� �������
        /// </summary>
        [SerializeField] private float _changePositionSmoothTime;

        private IBrickViewPresenter _presenter;

        public void SetCallbacks(IBrickViewPresenter presenter)
        {
            presenter.OnPositionChanged += ChangePosition;

            _presenter = presenter;
        }

        public void SetCallbacks()
        {
            _presenter.OnPositionChanged += ChangePosition;
        }

        public void DisposeCallbacks()
        {
            _presenter.OnPositionChanged -= ChangePosition;
        }

        /// <summary>
        /// �������� ������� ����� ( ���������� DOTween )
        /// </summary>
        /// <param name="newPosition"></param>
        private void ChangePosition(Vector3 newPosition)
        {
            _transform.DOMove(newPosition, _changePositionSmoothTime);
        }

        private void Awake()
        {
            _meshRenderer.material.color = GetRandomColor();
        }

        private Color GetRandomColor()
        {
            return _colors[Random.Range(0, _colors.Length)];
        }

        public Mesh GetMesh()
        {
            return _meshFilter.mesh;
        }

        public Color GetColor()
        {
            return _meshRenderer.material.color;
        }
    }
}