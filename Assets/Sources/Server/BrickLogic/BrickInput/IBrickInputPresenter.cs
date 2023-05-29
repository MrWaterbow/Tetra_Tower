using UnityEngine;

namespace Server.BricksLogic
{
    public interface IBrickInputPresenter
    {
        void MoveTo(Vector3Int direction);
        void ToGround();
    }
}