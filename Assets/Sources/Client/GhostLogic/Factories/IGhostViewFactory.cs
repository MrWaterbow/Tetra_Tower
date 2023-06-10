using UnityEngine;

namespace Client.GhostLogic
{
    internal interface IGhostViewFactory
    {
        GhostView Create();
    }
}