using DG.Tweening;
using UnityEngine;

namespace Server.BricksLogic
{
    public sealed class BrickView : MonoBehaviour
    {
        /// <summary>
        /// Transform ( компонент Unity )
        /// </summary>
        [SerializeField] private Transform _transform;
        /// <summary>
        /// Плавность сменения позиции
        /// </summary>
        [SerializeField] private float _changePositionSmoothTime;

        /// <summary>
        /// Изменяет позицию блока ( использует DOTween )
        /// </summary>
        /// <param name="newPosition"></param>
        public void ChangePosition(Vector3 newPosition)
        {
            _transform.DOMove(newPosition, _changePositionSmoothTime);
        }
    }
}