using UnityEngine;

namespace Server.BrickLogic
{
    public interface IBrickInputPresenter
    {
        void MoveTo(Vector3Int direction);
        void ToGround();
    }
}