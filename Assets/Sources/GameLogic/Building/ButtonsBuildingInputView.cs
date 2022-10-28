using System;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.BuildingLogic
{
    public class ButtonsBuildingInputView : BuildingInput
    {
        public override event Action MovingUp;
        public override event Action MovingDown;
        public override event Action MovingRight;
        public override event Action MovingLeft;

        [SerializeField] private Button _upButton;
        [SerializeField] private Button _downButton;
        [SerializeField] private Button _rightButton;
        [SerializeField] private Button _leftButton;

        private void Enable()
        {
            _upButton.onClick.AddListener(InvokeMovingUp);
            _downButton.onClick.AddListener(InvokeMovingDown);
            _rightButton.onClick.AddListener(InvokeMovingRight);
            _leftButton.onClick.AddListener(InvokeMovingLeft);
        }

        private void Disable()
        {
            _upButton.onClick.RemoveListener(InvokeMovingUp);
            _downButton.onClick.RemoveListener(InvokeMovingDown);
            _rightButton.onClick.RemoveListener(InvokeMovingRight);
            _leftButton.onClick.RemoveListener(InvokeMovingLeft);
        }

        private void InvokeMovingUp()
        {
            MovingUp?.Invoke();
        }

        private void InvokeMovingDown()
        {
            MovingDown?.Invoke();
        }

        private void InvokeMovingRight()
        {
            MovingRight?.Invoke();
        }

        private void InvokeMovingLeft()
        {
            MovingLeft?.Invoke();
        }
    }
}