using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SlowDownAnimation : MonoBehaviour
{
    public Rig rig;

    public void StartAnimation()
    {
        StartCoroutine(SlowDown());
    }
    public void StopAnimation()
    {
        rig.weight = 1;
    }

    IEnumerator SlowDown()
    {
        while (rig.weight > 0.00000001f)
        {
            rig.weight *= 0.1f;
            yield return null;
        }
    }

}
