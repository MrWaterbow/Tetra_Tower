using UnityEngine;

namespace Client.AsteroidLogic
{
    internal interface IAsteroidViewFactory
    {
        AsteroidView Create(Vector3Int target);
    }
}