using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollorManager : MonoBehaviour
{
    public static event Action СolorСhange;
    
    public static CollorManager Instance = null;
    public Image choice;

    public Color color;
    
    private void Awake()
    {
        InstanceSingletone();
    }
    void Start()
    {
        color = Color.white;
    }

    public void SetCollor(string col)
    {
        ColorUtility.TryParseHtmlString(col, out color);
        choice.color = color;
        if(СolorСhange != null) СolorСhange.Invoke();
    }
    
    
    private void InstanceSingletone()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
