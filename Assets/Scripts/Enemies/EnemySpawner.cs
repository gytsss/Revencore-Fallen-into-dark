using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    #region EXPOSED_FIELDS

    [SerializeField] private GameObject _meleeEnemyPrefab;
    [SerializeField] private GameObject _distanceEnemyPrefab;

    [SerializeField] private float _meleeInterval = 3.5f;
    [SerializeField] private float _distanceInterval = 10f;

    [SerializeField] private EnemiesManager _enemiesManager;
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private Transform[] _room1SpawnPositions;
    [SerializeField] private Transform[] _room2SpawnPositions;
    [SerializeField] private Transform[] _room3SpawnPositions;
    [SerializeField] private Transform[] _houseSpawnPositions;

    #endregion

    #region PRIVATE_FIELDS

    private int _maxMeleeEnemies;
    private int _maxDistanceEnemies;
    private int _count = 0;
    private int _currentRoom = 0;
    private bool pause = false;

    #endregion

    #region UNITY_CALLS

    private void Awake()
    {
        RoomManager.OnPause += EnablePause;
        RoomManager.OnUnPause += DisablePause;
    }
    

    private void Start()
    {
        _maxMeleeEnemies = _enemiesManager._room1NecessaryKills;
        _maxDistanceEnemies = _enemiesManager._room3NecessaryKills;
    }

    private void Update()
    {
        int newRoom = _roomManager.GetCurrentRoom();
        if (newRoom != _currentRoom)
        {
            _currentRoom = newRoom;
            _count = 0;

            if (_currentRoom == 1)
            {
                StartCoroutine(
                    SpawnEnemy(_meleeInterval, _meleeEnemyPrefab, _maxMeleeEnemies, _room1SpawnPositions));
            }
            else if (_currentRoom == 3)
            {
                StartCoroutine(SpawnEnemy(_distanceInterval, _distanceEnemyPrefab, _maxDistanceEnemies,
                    _room2SpawnPositions));
            }
        }
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy, int maxEnemies, Transform[] spawners)
    {
        while (_count < maxEnemies)
        {
            yield return new WaitForSeconds(interval);

            if (!pause)
            {
                Transform randomSpawnPos = spawners[Random.Range(0, spawners.Length)];

                Instantiate(enemy, randomSpawnPos.position, Quaternion.identity);
                _count++;
            }
        }
    }

    private void OnDestroy()
    {
        RoomManager.OnPause -= EnablePause;
        RoomManager.OnUnPause -= DisablePause;
    }

    #endregion

    #region PRIVATE_METHODS

    private void EnablePause()
    {
        pause = true;
    }
    
    private void DisablePause()
    {
        pause = false;
    }

    #endregion
}