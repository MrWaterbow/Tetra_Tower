using UnityEngine;

namespace Server.BrickLogic
{
    /// <summary>
    /// Интерфейс презентера для реализации получения ввода от игрока.
    /// </summary>
    public interface IBrickInputPresenter
    {
        void MoveTo(Vector3Int direction);
        void ToGround();
    }
}