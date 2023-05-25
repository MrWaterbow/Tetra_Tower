using Server.BricksLogic;
using System;
using UnityEngine;

namespace Client.Input
{

    internal sealed class ButtonsBrickInput : MonoBehaviour, IBrickInputView
    {
        public event Action<Vector3Int> OnMove;
        public event Action OnLower;
    }
}