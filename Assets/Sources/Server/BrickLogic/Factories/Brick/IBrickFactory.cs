using Server.BricksLogic;
using UnityEngine;

namespace Server.Factories
{
    public interface IBrickFactory
    {
        Brick Create(Vector3Int position);
    }
}