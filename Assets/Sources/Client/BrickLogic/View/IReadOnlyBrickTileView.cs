using UnityEngine;

namespace Client.BrickLogic
{
    internal interface IReadOnlyBrickTileView
    {
        Mesh Mesh { get; }
        Color Color { get; }
    }
}