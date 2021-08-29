using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoutAnimation : MonoBehaviour
{
    

    public void StartAnimation()
    {
        Camera.main.GetComponent<CameraShaker>().Shake(0.91f, 10, 2, 0.1f);
    }
    public void StopAnimation()
    {
        
    }
}
