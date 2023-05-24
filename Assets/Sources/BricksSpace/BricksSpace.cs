using System.Collections.Generic;
using UnityEngine;

namespace Sources.BricksLogic
{
    public sealed class BricksSpace
    {
        //_bricks ��� �����
        // _controllableBrick �������������� ������� ����
        //_surface �����������, �� ������� ��������������� �����. ������ ����� �������� �� ��, ��������, ��������� ��������� �� ���� �� �����������

        private readonly List<IBrick> _bricks;
        private readonly IBrick _controllableBrick;

        private readonly PlacingSurface _surface;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surfaceSize">������ �����������</param>
        /// <param name="worldPositionOffset">�������� ������������ ������� ���������</param>
        /// <param name="controlledBrick">�������������� ����</param>
        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, IBrick controlledBrick)
        {
            _surface = new(surfaceSize, worldPositionOffset);

            _bricks = new List<IBrick>() { controlledBrick };
            _controllableBrick = controlledBrick;
        }
        /// <summary>
        /// ��� �������� ��������� �������� ������ �����
        /// </summary>
        public IReadOnlyBrick ControllableBrick => _controllableBrick;

        /// <summary>
        /// �����������, �� ������� ��������������� �����
        /// </summary>
        public PlacingSurface Surface => _surface;

        /// <summary>
        /// ����� �������� �������� - ������� ������� ���� � � ������ ������ ������� ��� � ��������� �����������
        /// </summary>
        /// <param name="direction">�����������</param>
        public void TryMoveBrick(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            if (_surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition))
            {
                _controllableBrick.Move(direction);
            }
        }

        /// <summary>
        /// ����� ���������� �������� �� ������� ���� � ��������� �����������
        /// </summary>
        /// <param name="direction">�����������</param>
        /// <returns></returns>
        public bool PossibleMoveBrickTo(Vector3Int direction)
        {
            Vector2Int featurePosition = ComputeFeaturePosition(direction);

            return _surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition);
        }


        /// <summary>
        /// ���������� ������� ������� ������������ ����������
        /// </summary>
        /// <param name="direction">�����������</param>
        /// <returns></returns>
        private Vector2Int ComputeFeaturePosition(Vector3Int direction)
        {
            return new(_controllableBrick.Position.x + direction.x, _controllableBrick.Position.z + direction.z);
        }

        /// <summary>
        /// �������� �������������� ����
        /// </summary>
        public void LowerControllableBrick()
        {
            _controllableBrick.Move(Vector3Int.down);
        }
    }
}