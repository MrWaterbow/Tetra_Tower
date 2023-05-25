using Server.BricksLogic;
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
        IBrick Create(Vector3Int position);
    }
}