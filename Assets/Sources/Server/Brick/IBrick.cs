using UnityEngine;

namespace Sources.BricksLogic
{
    /// <summary>
    /// Паттерн Memento - требуется для того что бы доступ к данным блока был только для чтения, а не для изменения. Например, позволяет проверить данные блока при тесте.
    /// </summary>
    public interface IReadOnlyBrick
    {
        Vector3Int Position { get; }
        Vector3Int[] Pattern { get; }
    }

    /// <summary>
    /// Нужен для низкоуровневых манипуляций (движение, поворот) с блоком. Интерфейс IReadOnlyBrick реализует интерфейс IBrick потому что доступ к данным должен быть у каждого блока в не зависимости от типа (полиморфизма).
    /// </summary>
    public interface IBrick : IReadOnlyBrick
    {
        void Move(Vector3Int direction);
    }
}