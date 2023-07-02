using System;
using UnityEngine;

namespace Server.AsteroidLogic
{
    public interface IReadOnlyAsteroid
    {
        event Action<float> OnFlyTick;
        event Action<Asteroid> OnCrash;

        Vector3Int Target { get; }
    }
}