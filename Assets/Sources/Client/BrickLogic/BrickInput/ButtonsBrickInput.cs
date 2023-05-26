using Server.BricksLogic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Input
{

    internal sealed class ButtonsBrickInput : MonoBehaviour
    {
        public BrickInputPresenter Presenter;

        // Список кнопок для ввода игрока
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _forwardButton;
        [SerializeField] private Button _backButton;

        /// <summary>
        /// Подписывается на нажатие кнопок
        /// </summary>
        public void OnEnable()
        {
            _rightButton.onClick.AddListener(InvokeMoveRight);
            _leftButton.onClick.AddListener(InvokeMoveLeft);
            _forwardButton.onClick.AddListener(InvokeMoveForward);
            _backButton.onClick.AddListener(InvokeMoveBack);
        }

        /// <summary>
        /// Отписывается от нажатия блоков
        /// </summary>
        public void OnDisable()
        {
            _rightButton.onClick.RemoveListener(InvokeMoveRight);
            _leftButton.onClick.RemoveListener(InvokeMoveLeft);
            _forwardButton.onClick.RemoveListener(InvokeMoveForward);
            _backButton.onClick.RemoveListener(InvokeMoveBack);
        }

        // Методы для вызова ивентов при движении
        private void InvokeMoveRight()
        {
            Presenter.MoveTo(Vector3Int.right);
        }

        private void InvokeMoveLeft()
        {
            Presenter.MoveTo(Vector3Int.left);
        }

        private void InvokeMoveForward()
        {
            Presenter.MoveTo(Vector3Int.forward);
        }

        private void InvokeMoveBack()
        {
            Presenter.MoveTo(Vector3Int.back);
        }
    }
}