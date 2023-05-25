using Server.BricksLogic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Input
{

    internal sealed class ButtonsBrickInput : MonoBehaviour, IBrickInputView
    {
        /// <summary>
        /// ����� ���������� ��� ��������� ����� �� ������ ��� ��������
        /// </summary>
        public event Action<Vector3Int> OnMove;
        /// <summary>
        /// ����� ���������� ��� ����������� �����
        /// </summary>
        public event Action OnLower;

        // ������ ������ ��� ����� ������
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _forwardButton;
        [SerializeField] private Button _backButton;

        /// <summary>
        /// ������������� �� ������� ������
        /// </summary>
        public void SetCallbacks()
        {
            _rightButton.onClick.AddListener(InvokeMoveRight);
            _leftButton.onClick.AddListener(InvokeMoveLeft);
            _forwardButton.onClick.AddListener(InvokeMoveForward);
            _backButton.onClick.AddListener(InvokeMoveBack);
        }

        /// <summary>
        /// ������������ �� ������� ������
        /// </summary>
        public void DisposeCallbacks()
        {
            _rightButton.onClick.RemoveListener(InvokeMoveRight);
            _leftButton.onClick.RemoveListener(InvokeMoveLeft);
            _forwardButton.onClick.RemoveListener(InvokeMoveForward);
            _backButton.onClick.RemoveListener(InvokeMoveBack);
        }

        // ������ ��� ������ ������� ��� ��������
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