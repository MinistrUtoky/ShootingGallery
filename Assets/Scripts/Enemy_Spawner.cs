using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour
{
    [SerializeField] private int _howManyEnemies;
    [SerializeField] private GameObject _enemyPrefab;
    private GameObject _enemy;
    private GameObject _enemies;
    private List<string> _killingPopups;
    public Coroutine twoSecondsKillTimerCoroutine;
    private GameObject _player;
    private bool _gameEnded;
    private int _currentEnemiesCount=0;
    private GameObject _canvas;

    //�������������� ������ �� ����� (�������� �� ����� ���������� ����������, �� ��� ���� ������ ���������������, �������������� � �� ��� ����� ������� ��������� �����
    void Awake()
    {
        _gameEnded = false;
        _player = GameObject.Find("Player");
        _killingPopups = new List<string>{"Killing Spree", "Monster Kill", "Rampage"};
        _enemies = GameObject.Find("Enemies");
        _canvas = GameObject.Find("Canvas");
        SpawnEnemies();
        StartCoroutine(GameStartAwaiting());
    }

    //�������� ���
    private void SpawnEnemies()
    {
        for (int i = 0; i < _howManyEnemies; i++)
        {
            SpawnNewEnemy();
        }
        _currentEnemiesCount = _howManyEnemies;
    }
    
    // ������� �������� � ��������� �� ��������� �� ���� ������ � ������ ����/�����
    public void SpawnNewEnemy()
    {
        Vector3 v = new Vector3(UnityEngine.Random.Range(-45, 45), 2, UnityEngine.Random.Range(-45, 45));
        while (Physics.OverlapSphere(v, _enemyPrefab.GetComponent<CapsuleCollider>().radius * 5, LayerMask.GetMask("barriers")).Length != 0
            || Physics.OverlapSphere(v, _enemyPrefab.GetComponent<CapsuleCollider>().radius * 2, LayerMask.GetMask("enemyLayer")).Length != 0)
            v = new Vector3(UnityEngine.Random.Range(-45, 45), 2, UnityEngine.Random.Range(-45, 45));
        _enemy = Instantiate(_enemyPrefab, v, Quaternion.identity);
        _enemy.transform.parent = _enemies.transform;
    }
    
    // ������� ������ ��������
    private IEnumerator GameStartAwaiting()
    {
        yield return new WaitForSeconds(3f);
        _player.GetComponent<Player_Controller>().canShoot = true;
        _canvas.transform.GetChild(3).gameObject.SetActive(false);
        twoSecondsKillTimerCoroutine = StartCoroutine(TwoSecondsKillTimer());
    }

    // ������� ������ ������ 2 ������� � ��������� ������������� �����
    private IEnumerator TwoSecondsKillTimer()
    {
        while (_enemies.transform.childCount > 0)
        {
            yield return new WaitForSeconds(2f);
            if (_enemies.transform.childCount == 0) break;
            if (_enemies.transform.childCount == _howManyEnemies) _player.GetComponent<Player_Controller>().DecreaseScore();
            ClearEnemies();
            SpawnEnemies();
        }
    }

    //...
    public void EnemyKilled() => _currentEnemiesCount--;

    // ������ ���� �� ������� ��������� ���������
    public void ClearEnemies()
    {
        while (_currentEnemiesCount > 0)
        {
            Destroy(_enemies.transform.GetChild(_currentEnemiesCount-1).gameObject); 
            _currentEnemiesCount--;
        }
    }
}
