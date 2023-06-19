using UnityEngine;

namespace Client.GhostLogic
{
    internal interface IGhostTileViewFactory
    {
        GhostTileView Create(Vector3Int position);
    }
}