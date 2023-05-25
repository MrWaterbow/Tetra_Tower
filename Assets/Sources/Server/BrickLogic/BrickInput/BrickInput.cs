namespace Server.BricksLogic
{
    public class BrickInput
    {
        /// <summary>
        /// Пространство блоков
        /// </summary>
        private readonly BricksSpace _brickSpace;

        /// <summary>
        /// Реализация получения ввода от игрока
        /// </summary>
        private readonly IBrickInputView _brickInputView;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brickSpace">Пространство блоков</param>
        /// <param name="brickInputView">Реализация получения ввода от игрока</param>
        public BrickInput(BricksSpace brickSpace, IBrickInputView brickInputView)
        {
            _brickSpace = brickSpace;
            _brickInputView = brickInputView;
        }

        /// <summary>
        /// Подписывается на ивенты от ввода игрока
        /// </summary>
        public void SetCallbacks()
        {
            _brickInputView.OnMove += _brickSpace.TryMoveBrick;
        }

        /// <summary>
        /// Отписывается от ивентов для ввода игрока
        /// </summary>
        public void DisposeCallbacks()
        {
            _brickInputView.OnMove -= _brickSpace.TryMoveBrick;
        }
    }
}