using Server.BrickLogic;
using UnityEngine;

namespace Server.Factories
{
    /// <summary>
    /// ��������� ������� ��� �������� ������.
    /// </summary>
    public interface IBrickFactory
    {
        /// <summary>
        /// ����� �������� �����.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        Brick Create(Vector3Int position);
    }
}