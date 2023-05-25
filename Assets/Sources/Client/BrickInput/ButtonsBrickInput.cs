using Server.BricksLogic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Input
{

    internal sealed class ButtonsBrickInput : MonoBehaviour, IBrickInputView
    {
        /// <summary>
        /// Ивент вызывается при получения ввода от игрока для движения
        /// </summary>
        public event Action<Vector3Int> OnMove;
        /// <summary>
        /// Ивент вызывается при приземлении блока
        /// </summary>
        public event Action OnLower;

        // Список кнопок для ввода игрока
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _forwardButton;
        [SerializeField] private Button _backButton;

        /// <summary>
        /// Подписывается на нажатие кнопок
        /// </summary>
        public void SetCallbacks()
        {
            _rightButton.onClick.AddListener(InvokeMoveRight);
            _leftButton.onClick.AddListener(InvokeMoveLeft);
            _forwardButton.onClick.AddListener(InvokeMoveForward);
            _backButton.onClick.AddListener(InvokeMoveBack);
        }

        /// <summary>
        /// Отписывается от нажатия блоков
        /// </summary>
        public void DisposeCallbacks()
        {
            _rightButton.onClick.RemoveListener(InvokeMoveRight);
            _leftButton.onClick.RemoveListener(InvokeMoveLeft);
            _forwardButton.onClick.RemoveListener(InvokeMoveForward);
            _backButton.onClick.RemoveListener(InvokeMoveBack);
        }

        // Методы для вызова ивентов при движении
        private void InvokeMoveRight()
        {
            OnMove?.Invoke(Vector3Int.right);
        }

        private void InvokeMoveLeft()
        {
            OnMove?.Invoke(Vector3Int.left);
        }

        private void InvokeMoveForward()
        {
            OnMove?.Invoke(Vector3Int.forward);
        }

        private void InvokeMoveBack()
        {
            OnMove?.Invoke(Vector3Int.back);
        }
    }
}