using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using System;

public class PatternManager : MonoBehaviour
{
    public GameManager _gameManager { get; private set; }

    private ObjectPool<Missle> _pool;
    private List<Missle> _currentMissles = new List<Missle>();
    private IEnumerator[] _pattern;
    [SerializeField] private Transform[][] _patternSpawnPoint;
    [SerializeField] private GameObject[] _misslePrefabs;
    [SerializeField] private float _spawnTime;
    private int _currentPattern;

    public void Init(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    private void Awake()
    {
        _currentPattern = 0;
        _pattern = new IEnumerator[6];
        SetTransform();
        InitiatePattern();
        _pattern[_currentPattern] = Pattern();
        StartCoroutine(_pattern[_currentPattern]);
    }

    private void InitiatePattern()
    {
        _pool = new ObjectPool<Missle>(StartPattern, OnGetMissle, OnReleaseMissle, OnDestroyMissle, maxSize: 20);
    }
    private Missle StartPattern()
    {
        int randomPosition = UnityEngine.Random.Range(0, _patternSpawnPoint[_currentPattern].Length);
        Missle missle = Instantiate(_misslePrefabs[_currentPattern], _patternSpawnPoint[_currentPattern][randomPosition].position, _patternSpawnPoint[_currentPattern][randomPosition].rotation).GetComponent<Missle>();
        missle.SetPool(_pool);
        return missle;
    }
    private void OnGetMissle(Missle missle)
    {
        int randomPosition = UnityEngine.Random.Range(0, _patternSpawnPoint[_currentPattern].Length);
        missle.gameObject.transform.position = _patternSpawnPoint[_currentPattern][randomPosition].position;
        missle.gameObject.transform.rotation = _patternSpawnPoint[_currentPattern][randomPosition].rotation;
        missle.gameObject.SetActive(true);
        _currentMissles.Add(missle);
    }

    private void OnReleaseMissle(Missle missle)
    {
        missle.gameObject.SetActive(false);
        _currentMissles.Remove(missle);
    }
    private void OnDestroyMissle(Missle missle)
    {
        Destroy(missle.gameObject);
    }

    IEnumerator Pattern()
    {
        WaitForSeconds spawnTimer = new WaitForSeconds(_spawnTime);
        while (true)
        {
            Missle missle = _pool.Get();
            yield return spawnTimer;
        }
    }

    public void ChangePattern()
    {
        StopCoroutine(_pattern[_currentPattern]);
        _pool.Clear();
        foreach (Missle missle in _currentMissles)
        {
            Destroy(missle.gameObject);
        }
        _currentMissles.Clear();
        ++_currentPattern;
        _pattern[_currentPattern] = Pattern();
        InitiatePattern();
        StartCoroutine(_pattern[_currentPattern]);
    }

    private void SetTransform()
    {
        _patternSpawnPoint = new Transform[gameObject.transform.childCount - 1][];
        int childId = 0;
        for (int i = 0; i < gameObject.transform.childCount - 1; ++i)
        {
            Transform Pattern = transform.GetChild(childId);
            _patternSpawnPoint[childId] = new Transform[Pattern.childCount];
            for (int j = 0; j < Pattern.childCount; ++j)
            {
                _patternSpawnPoint[childId][j] = Pattern.GetChild(j);
            }
            ++childId;
        }
    }
}
