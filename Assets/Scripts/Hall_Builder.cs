using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Hall_Builder : MonoBehaviour
{
    [SerializeField] private GameObject _column;
    private float _angle = 0f;
    private GameObject _hall;
    private GameObject _nonameColumn;

    //Строит хреновину в центре
    //Кому не нравятся комментарии можете поставить 5 и выключить видео, эта красота не для вас

    void Awake()
    {
        _hall = GameObject.Find("Hall");
        for (double i = 0; i < 2*Math.PI; i += Math.PI/8)
        {
            _nonameColumn = Instantiate(_column, new Vector3(-10 * (float)Math.Cos(i), 0, -10 * (float)Math.Sin(i)), Quaternion.identity);
            _nonameColumn.transform.rotation = Quaternion.Euler(0, -90-(float)(i *180/(float)Math.PI), 0);
            _nonameColumn.transform.parent = _hall.transform;
        }
    }
}
