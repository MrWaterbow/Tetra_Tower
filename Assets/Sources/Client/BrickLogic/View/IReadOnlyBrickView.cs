using System.Collections.Generic;
using UnityEngine;

namespace Client.BrickLogic
{
    internal interface IReadOnlyBrickView
    {
        IReadOnlyList<IReadOnlyBrickTileView> Tiles { get; }
        Mesh Mesh { get; }
        Color GeneralColor { get; }
    }
}