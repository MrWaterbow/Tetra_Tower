using Server.AsteroidLogic;
using Server.BrickLogic;
using UnityEngine;

namespace Client.AsteroidLogic
{
    internal sealed class AsteroidView : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private IReadOnlyAsteroid _asteroid;
        private PlacingSurface _placingSurface;

        private Vector3 _start;
        private Vector3Int _target;

        public void Initialize(Vector3Int target)
        {
            _start = _transform.position;
            _target = target;
        }

        public void SetCallbacks(IReadOnlyAsteroid asteroid, PlacingSurface placingSurface)
        {
            _asteroid = asteroid;
            _placingSurface = placingSurface;

            _asteroid.OnFlyTick += UpdatePosition;
            _asteroid.OnCrash += Destroy;
        }

        public void RemoveCallbacks()
        {
            _asteroid.OnFlyTick -= UpdatePosition;
            _asteroid.OnCrash -= Destroy;
        }

        private void UpdatePosition(float CompletedPathPercent)
        {
            _transform.position = Vector3.Lerp(_placingSurface.GetWorldPosition(_target), _start, CompletedPathPercent);
        }

        private void Destroy(Asteroid asteroid)
        {
            RemoveCallbacks();

            Destroy(gameObject);
        }
    }
}