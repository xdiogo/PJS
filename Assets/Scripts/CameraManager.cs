using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager current;

    private void Awake()
    {
        current = this;
    }


    public void FocusOnCamera(CinemachineCamera camera)
    {
        camera.Priority = 20;
    }

    public void UnfocusCamera(CinemachineCamera camera)
    {
        camera.Priority = -1;

    }
}
