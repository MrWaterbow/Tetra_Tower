using System.Collections.Generic;
using UnityEngine;

namespace Server.BricksLogic
{
    public sealed class BricksSpace
    {
        /// <summary>
        /// ��� �����, ������� ���������� � ������������
        /// </summary>
        private readonly List<IBrick> _bricks;
        /// <summary>
        /// �������������� ������� ����
        /// </summary>
        private readonly IBrick _controllableBrick;

        /// <summary>
        /// ����������� �� ������� �������� �����
        /// </summary>
        private readonly PlacingSurface _surface;

        /// <summary>
        ///
        /// </summary>
        /// <param name="surfaceSize">������ �����������</param>
        /// <param name="worldPositionOffset">�������� ������������ ������� ���������</param>
        /// <param name="controllableBrick">�������������� ����</param>
        public BricksSpace(Vector2Int surfaceSize, Vector3 worldPositionOffset, IBrick controllableBrick)
        {
            _surface = new(surfaceSize, worldPositionOffset);

            _bricks = new List<IBrick>() { controllableBrick };
            _controllableBrick = controllableBrick;
        }

        public BricksSpace(PlacingSurface surface, IBrick controllableBrick)
        {
            _surface = surface;

            _bricks = new List<IBrick>() { controllableBrick };
            _controllableBrick = controllableBrick;
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

            Debug.Log("Try move to " + direction.ToString());

            if (_surface.PatternInSurfaceLimits(_controllableBrick.Pattern, featurePosition))
            {
                _controllableBrick.Move(direction);

                Debug.Log("Move completed " + _controllableBrick.Position);
            }
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
        /// �������� �������������� ����
        /// </summary>
        public void LowerBrick()
        {
            _controllableBrick.Move(Vector3Int.down);
        }
    }
}