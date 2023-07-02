using UnityEngine;

namespace Server.AsteroidLogic
{
    public interface IAsteroidFactory
    {
        Asteroid Create(Vector3Int target);
    }
}