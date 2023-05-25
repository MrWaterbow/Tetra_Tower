using DG.Tweening;
using UnityEngine;

namespace Server.BricksLogic
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