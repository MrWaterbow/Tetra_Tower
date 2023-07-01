using System;
using UnityEngine;

namespace Server.AsteroidLogic
{
    public sealed class Asteroid
    {
        public event Action<Vector3Int, float> OnFlyTick;
        public event Action<Asteroid> OnCrash;

        public readonly Vector3Int[] DestroyArea;
        public readonly Vector3Int Target;
        private float _flyTimer;
        private float _flyTime;

        public Asteroid(Vector3Int[] destroyArea, Vector3Int target, float flyTimer)
        {
            DestroyArea = destroyArea;
            Target = target;
            _flyTimer = flyTimer;
            _flyTime = flyTimer;
        }

        public float FlyTimer => _flyTimer;

        public void ToTarget()
        {
            _flyTimer = 0f;

            FlyTick();
        }

        public void FlyTick()
        {
            _flyTimer -= Time.deltaTime;

            OnFlyTick?.Invoke(Target, _flyTimer / _flyTime);

            CheckCrash();
        }

        private void CheckCrash()
        {
            if(_flyTimer <= 0)
            {
                OnCrash?.Invoke(this);
            }
        }
    }
}