namespace Server.BricksLogic
{
    public class BrickInput
    {
        /// <summary>
        /// ������������ ������
        /// </summary>
        private readonly BricksSpace _brickSpace;

        /// <summary>
        /// ���������� ��������� ����� �� ������
        /// </summary>
        private readonly IBrickInputView _brickInputView;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">������������ ������</param>
        /// <param name="brickInputView">���������� ��������� ����� �� ������</param>
        public BrickInput(BricksSpace brickSpace, IBrickInputView brickInputView)
        {
            _brickSpace = brickSpace;
            _brickInputView = brickInputView;
        }

        /// <summary>
        /// ������������� �� ������ �� ����� ������
        /// </summary>
        public void SetCallbacks()
        {
            _brickInputView.OnMove += _brickSpace.TryMoveBrick;
        }

        /// <summary>
        /// ������������ �� ������� ��� ����� ������
        /// </summary>
        public void DisposeCallbacks()
        {
            _brickInputView.OnMove -= _brickSpace.TryMoveBrick;
        }
    }
}