using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player_Controller : MonoBehaviour
{
    private int _score;
    private bool _activeCamera;
    public bool canShoot;
    private Transform _mainCamera;
    public float sensitivityX = 3f;
    public float sensitivityY = 3f;
    private float _rotationX;
    private float _rotationY;
    private GameObject _canvas;

    //См. спавн врагов аваке
    void Awake()
    {
        _score = 0;
        _activeCamera = true;
        canShoot = false;
        _mainCamera = transform.GetChild(0);
        _canvas = GameObject.Find("Canvas");
        StartCoroutine(ThirtySecondsForGame());
    }
   
    // Вращение камеры и стрельба
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {

            transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
            RaycastHit hit;
            if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).TransformDirection(Vector3.forward), out hit, 1000, LayerMask.GetMask("enemyLayer"))
                && !Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).TransformDirection(Vector3.forward), 1000, LayerMask.GetMask("barriers")))
            {
                hit.transform.parent.parent.GetComponent<Enemy_Spawner>().EnemyKilled(); IncreaseScore();
                Destroy(hit.transform.gameObject);
            }
            StartCoroutine(WeaponCooldown());
        }
        if (_activeCamera)
        {
            _rotationX = Input.GetAxis("Mouse X");
            _rotationY = Input.GetAxis("Mouse Y");
            if (_rotationX != 0 || _rotationY != 0)
            {
                if (_rotationX != 0)
                    _mainCamera.transform.localRotation *= Quaternion.AngleAxis(_rotationX * sensitivityX, Vector3.up);

                if (_rotationY != 0)
                    _mainCamera.transform.localRotation *= Quaternion.AngleAxis(_rotationY * sensitivityY, Vector3.left);
                _mainCamera.transform.rotation = Quaternion.Euler(_mainCamera.transform.rotation.eulerAngles.x, _mainCamera.transform.rotation.eulerAngles.y, 0);
            }
        }
    }


    // Время игры
    private IEnumerator ThirtySecondsForGame()
    {
        StartCoroutine(CountDown(30f));
        yield return new WaitForSeconds(30f);
        GameObject.Find("Spawner").GetComponent<Enemy_Spawner>().ClearEnemies();
        StopCoroutine(GameObject.Find("Spawner").GetComponent<Enemy_Spawner>().twoSecondsKillTimerCoroutine);
        _activeCamera = false;
        canShoot = false;
        Transform end = _canvas.transform.GetChild(4);
        end.gameObject.SetActive(true);
        end.GetChild(2).gameObject.GetComponent<TMP_Text>().text = _score.ToString(); 
    }


    // Красивые тикалки
    public IEnumerator CountDown(float s)
    {
        float i = s;
        while (i > 0)
        {
            i--;
            yield return new WaitForSeconds(1f);
            _canvas.transform.GetChild(2).GetComponent<TMP_Text>().text = i.ToString();
            if (!canShoot)
                _canvas.transform.GetChild(3).GetComponent<TMP_Text>().text = (3 - s + i).ToString();
        }        
    }

    // Кд на выстрел глазами
    private IEnumerator WeaponCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

    // У вас 2 попытки чтобы отгадать эти функции
    public void DecreaseScore()
    {
        _score = _score > 0 ? _score - 1 : 0;
        _canvas.transform.GetChild(1).GetComponent <TMP_Text> ().text = _score.ToString();
    }
    public void IncreaseScore()
    {
        _score++;
        _canvas.transform.GetChild(1).GetComponent<TMP_Text>().text = _score.ToString();
    }
}
