namespace Client.BrickLogic
{
    internal interface IBricksRuntimeData
    {
        IReadOnlyBrickView CurrentBrickView { get; }
    }
}