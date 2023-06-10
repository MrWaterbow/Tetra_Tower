using DG.Tweening;
using Server.BrickLogic;
using UnityEngine;

namespace Client.BrickLogic
{
    internal sealed class BrickView : MonoBehaviour, IReadOnlyBrickView
    {
        /// <summary>
        /// Transform ( компонент Unity )
        /// </summary>
        [SerializeField] private Transform _transform;
        [SerializeField] private MeshFilter _meshFilter;

        [Space]

        /// <summary>
        /// Плавность сменения позиции
        /// </summary>
        [SerializeField] private float _changePositionSmoothTime;

        private IBrickViewPresenter _presenter;

        public MeshFilter MeshFilter => _meshFilter;

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
        /// Изменяет позицию блока ( использует DOTween )
        /// </summary>
        /// <param name="newPosition"></param>
        private void ChangePosition(Vector3 newPosition)
        {
            _transform.DOMove(newPosition, _changePositionSmoothTime);
        }
    }
}