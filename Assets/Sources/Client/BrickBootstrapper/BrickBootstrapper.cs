using Server.BricksLogic;
using Server.Factories;
using UnityEngine;

namespace Client.Bootstrapper
{
    internal class BrickBootstrapper : Bootstrapper
    {
        [SerializeField] private Vector2Int _surfaceSize;
        [SerializeField] private Vector3Int _startBrickPosition;

        [Space]

        [SerializeField] private GameObject _brickInput;
        [SerializeField] private Transform _worldPositionAnchor;

        private IBrickInputView _brickInputView;
        private Vector3 _worldPositionOffset;

        public override void Boot()
        {
            IBrickFactory brickFactory = new RandomPatternBrickFactory(BrickPatterns.AllPatterns);
            IBrick startBrick = brickFactory.Create(_startBrickPosition);

            PlacingSurface placingSurface = new(_surfaceSize, _worldPositionOffset);
            BricksSpace bricksSpace = new(placingSurface, startBrick);

            BrickInput brickInput = new(bricksSpace, _brickInputView);
            brickInput.SetCallbacks();

            Debug.Log(bricksSpace.ControllableBrick);
        }

        private void OnValidate()
        {
            if(_brickInput != null && _brickInput.TryGetComponent<IBrickInputView>(out var component))
            {
                _brickInputView = component;
            }
            else
            {
                _brickInput = null;
            }

            if(_worldPositionAnchor != null)
            {
                _worldPositionOffset = _worldPositionAnchor.position;
            }
            else
            {
                _worldPositionOffset = Vector3.zero;
            }
        }
    }
}