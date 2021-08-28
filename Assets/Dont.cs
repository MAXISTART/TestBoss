using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dont : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    public void btnOn() {
        StaticMono.static_goj.GetComponent<Test1>().bb = 1;
    }
}
