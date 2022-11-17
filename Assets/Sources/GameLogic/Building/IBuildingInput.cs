using System;
using UnityEngine;

namespace Sources.BuildingLogic
{
    public abstract class BuildingInput : MonoBehaviour, IBuildingInput
    {
        public abstract event Action MovingUp;
        public abstract event Action MovingDown;
        public abstract event Action MovingRight;
        public abstract event Action MovingLeft;
        public abstract event Action MovingGround;

        public abstract void Enable();
        public abstract void Disable();
    }

    public interface IBuildingInput
    {
        public event Action MovingUp;
        public event Action MovingDown;
        public event Action MovingRight;
        public event Action MovingLeft;
        public event Action MovingGround;
    }
}