using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadShakingAnimation : MonoBehaviour
{
    public Transform head;
    public float amp;
    public float damp;
    public float shakeFreq;
    public bool stoped = true;


    Vector3 targetPosition;


    public void StartAnimation() {
        StartCoroutine(HeadShaking());
    }
    public void StopAnimation()
    {
        stoped = true;
    }


    IEnumerator HeadShaking() {
        stoped = false;
        Vector3 origin = head.position;
        while (!stoped) {
            targetPosition = origin + Random.onUnitSphere * amp;
            yield return new WaitForSeconds(1/(shakeFreq + 0.00000001f));
        }
    }

    private void Update()
    {
        if (!stoped) {
            head.position = Vector3.Lerp(head.position, targetPosition, damp);
        }
    }
}
