using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems; // Required to detect if clicking on UI

public class MouseLock : MonoBehaviour
{
    public static MouseLock current;

    public CinemachineInputAxisController aimInputController;

    private void Awake()
    {
        current = this;
    }

    void Start()
    {
        LockCursor(); // Lock the cursor by default
    }

    // Lock the cursor and hide it
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (aimInputController != null)
            aimInputController.enabled = true;
    }

    // Unlock the cursor and make it visible
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (aimInputController != null)
            aimInputController.enabled = false;
    }
}

