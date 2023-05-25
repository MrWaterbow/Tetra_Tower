namespace Server.BricksLogic
{
    public class BrickInput
    {
        private readonly BricksSpace _brickSpace;

        private readonly IBrickInputView _brickInputView;

        public BrickInput(BricksSpace brickSpace, IBrickInputView brickInputView)
        {
            _brickSpace = brickSpace;
            _brickInputView = brickInputView;
        }

        public void SetCallbacks()
        {
            _brickInputView.OnMove += _brickSpace.TryMoveBrick;
        }

        public void DisposeCallbacks()
        {
            _brickInputView.OnMove -= _brickSpace.TryMoveBrick;
        }
    }
}