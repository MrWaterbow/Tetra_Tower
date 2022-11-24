using System;
using UnityEngine;


namespace Sources.RotationLogic
{
    public abstract class RotationInput : MonoBehaviour, IRotationInput
    {
        public abstract event Action RotateUp;
        public abstract event Action RotateDown;
        public abstract event Action RotateRight;
        public abstract event Action RotateLeft;

        public abstract void Enable();
        public abstract void Disable();
    }

    public interface IRotationInput
    {
        public event Action RotateUp;
        public event Action RotateDown;
        public event Action RotateRight;
        public event Action RotateLeft;
    }
}
