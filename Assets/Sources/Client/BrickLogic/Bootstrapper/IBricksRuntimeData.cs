namespace Client.BrickLogic
{
    /// <summary>
    /// Интерфейс, который позволяет получить параметры визуального блока у клиента.
    /// </summary>
    internal interface IBricksRuntimeData
    {
        IReadOnlyBrickView CurrentBrickView { get; }
    }
}