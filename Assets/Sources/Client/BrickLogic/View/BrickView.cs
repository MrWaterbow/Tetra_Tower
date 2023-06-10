using DG.Tweening;
using Server.BricksLogic;
using UnityEngine;

namespace Client.BricksLogic
{
    public sealed class BrickView : MonoBehaviour
    {
        /// <summary>
        /// Transform ( ��������� Unity )
        /// </summary>
        [SerializeField] private Transform _transform;
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

        public void DisposeCallbacks()
        {
            _presenter.OnPositionChanged -= ChangePosition;
        }

        /// <summary>
        /// �������� ������� ����� ( ���������� DOTween )
        /// </summary>
        /// <param name="newPosition"></param>
        public void ChangePosition(Vector3 newPosition)
        {
            _transform.DOMove(newPosition, _changePositionSmoothTime);
        }
    }
}