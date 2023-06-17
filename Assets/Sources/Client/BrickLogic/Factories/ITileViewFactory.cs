using UnityEngine;

namespace Client.BrickLogic
{
    internal interface ITileViewFactory
    {
        BrickTileView Create(Vector3Int position);
    }
}