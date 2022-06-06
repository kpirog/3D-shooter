using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] float _spawnDelay = 12;
    [SerializeField] Zombie[] _zombiePrefabs;

    float _nextSpawnTime;
    int  _spawnCount;

    void Update()
    {
        if (ReadyToSpawn())
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        float delay = _spawnDelay - _spawnCount;
        delay = Mathf.Max(1, delay);

        _nextSpawnTime = Time.time + delay;
        _spawnCount++;

        int randomIndex = UnityEngine.Random.Range(0, _zombiePrefabs.Length);
        var zombiePrefab = _zombiePrefabs[randomIndex];

        var zombie = Instantiate(zombiePrefab, new Vector3(transform.position.x, 0.156f, transform.position.z - 0.48f), Quaternion.Euler(transform.position.x, -90f, transform.position.z));
        GetComponent<Animator>().SetBool("Open", true);
        yield return new WaitForSeconds(1f);
        zombie.StartWalking();
        yield return new WaitForSeconds(3f);
        GetComponent<Animator>().SetBool("Open", false);
    }

    bool ReadyToSpawn() => Time.time >= _nextSpawnTime;
}
