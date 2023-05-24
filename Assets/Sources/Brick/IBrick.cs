using UnityEngine;

namespace Sources.BricksLogic
{
    //Паттерн Memento - требуется для того что бы доступ к данным блока был только для чтения, а не для изменения. Например, позволяет проверить данные блока при тесте.
    public interface IReadOnlyBrick
    {
        Vector3Int Position { get; }
        Vector3Int[] Pattern { get; }
    }

    // Нужен для низкоуровневых манипуляций (движение, поворот) с блоком.
    // Интерфесй IReadOnlyBrick реализует интерфейс IBrick потому что доступ к данным должен быть у каждого блока в не зависимости от типа (полиморфизма).
    public interface IBrick : IReadOnlyBrick
    {
        void Move(Vector3Int direction);
    }
}