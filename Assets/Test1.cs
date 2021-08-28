using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    public int bb = 0;

    private void Start()
    {
        StaticMono.static_goj = gameObject;
    }

    private void Update()
    {
        //StaticMono.static_goj.GetComponent<Test1>().bb = 1;
    }
}
