using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.RotationLogic
{
    public class ButtonsRotationInputView : RotationInput
    {
        public override event Action RotateUp;
        public override event Action RotateDown;
        public override event Action RotateRight;
        public override event Action RotateLeft;

        [SerializeField] private Button _rotateUpButton;
        [SerializeField] private Button _rotateDownButton;
        [SerializeField] private Button _rotateRightButton;
        [SerializeField] private Button _rotateLeftButton;

        public override void Enable()
        {
            _rotateUpButton.onClick.AddListener(InvokeRotateUp);
            _rotateDownButton.onClick.AddListener(InvokeRotateDown);
            _rotateRightButton.onClick.AddListener(InvokeRotateRight);
            _rotateLeftButton.onClick.AddListener(InvokeRotateLeft);
        }
        public override void Disable()
        {
            _rotateUpButton.onClick.RemoveListener(InvokeRotateUp);
            _rotateDownButton.onClick.RemoveListener(InvokeRotateDown);
            _rotateRightButton.onClick.RemoveListener(InvokeRotateRight);
            _rotateLeftButton.onClick.RemoveListener(InvokeRotateLeft);
        }
        private void InvokeRotateUp()
        {
            RotateUp?.Invoke();
        }
        private void InvokeRotateDown()
        {
            RotateDown?.Invoke();
        }
        private void InvokeRotateRight()
        {
            RotateRight?.Invoke();
        }
        private void InvokeRotateLeft()
        {
            RotateLeft?.Invoke();
        }
    }
}
