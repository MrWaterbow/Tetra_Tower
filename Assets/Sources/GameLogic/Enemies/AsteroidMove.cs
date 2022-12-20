using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    [SerializeField] private GameObject _asteroidPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _force;


    private void Start()
    {
        //Invoke("SpawnAsteroid", 3);
    }
    private void SpawnAsteroid()
    {
        GameObject asteroid = Instantiate(_asteroidPrefab, _spawnPoint.position, Quaternion.identity);
        asteroid.GetComponent<Rigidbody>().AddRelativeForce(Vector3.right * _force, ForceMode.Force);
    }
}
