using Server.BrickLogic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Client.BrickLogic
{
    internal sealed class ButtonsBrickInput : MonoBehaviour
    {
        // Список кнопок для ввода игрока
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _forwardButton;
        [SerializeField] private Button _backButton;

        [SerializeField] private Button _rotateButton;
        [SerializeField] private Button _toGroundButton;

        private IBrickInputPresenter _presenter;

        [Inject]
        private void Constructor(IBrickInputPresenter presenter)
        {
            _presenter = presenter;
        }

        /// <summary>
        /// Подписывается на нажатие кнопок
        /// </summary>
        public void OnEnable()
        {
            _rightButton.onClick.AddListener(InvokeMoveRight);
            _leftButton.onClick.AddListener(InvokeMoveLeft);
            _forwardButton.onClick.AddListener(InvokeMoveForward);
            _backButton.onClick.AddListener(InvokeMoveBack);

            _rotateButton.onClick.AddListener(InvokeRotate);
            _toGroundButton.onClick.AddListener(InvokeToGround);
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

            _rotateButton.onClick.RemoveListener(InvokeRotate);
            _toGroundButton.onClick.RemoveListener(InvokeToGround);
        }

        // Методы для вызова ивентов при движении
        private void InvokeMoveRight()
        {
            _presenter.MoveTo(Vector3Int.right);
        }

        private void InvokeMoveLeft()
        {
            _presenter.MoveTo(Vector3Int.left);
        }

        private void InvokeMoveForward()
        {
            _presenter.MoveTo(Vector3Int.forward);
        }

        private void InvokeMoveBack()
        {
            _presenter.MoveTo(Vector3Int.back);
        }
        
        private void InvokeRotate()
        {
            _presenter.Rotate();
        }

        private void InvokeToGround()
        {
            _presenter.ToGround();
        }
    }
}