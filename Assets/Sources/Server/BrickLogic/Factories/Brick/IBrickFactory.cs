using Database.BricksLogic;
using UnityEngine;

namespace Server.Factories
{
    public interface IBrickFactory
    {
        /// <summary>
        /// ���������� ��� �������� �����
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Brick Create(Vector3Int position);
    }
}