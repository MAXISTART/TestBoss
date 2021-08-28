using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class TestRig : MonoBehaviour
{
    Rig rig; 

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rig>();
        StartCoroutine(StopRig());
    }


    IEnumerator StopRig() {
        yield return new WaitForSeconds(2.5f);
        while (rig.weight > 0.01f) {
            rig.weight *= 0.9f;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
