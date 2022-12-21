using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    [SerializeField] private Rigidbody _asteroidPrefab;
    [SerializeField] private Transform _spawnPoint;

    [SerializeField] private float _force;

    private void Start()
    {
        //Invoke("SpawnAsteroid", 3);
    }

    private void SpawnAsteroid()
    {
        Rigidbody asteroid = Instantiate(_asteroidPrefab, _spawnPoint.position, Quaternion.identity);
        asteroid.AddRelativeForce(Vector3.right * _force, ForceMode.Force);
    }
}
