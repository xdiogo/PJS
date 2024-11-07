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

    void Update()
    {
        // Unlock the cursor when the user presses Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }
       
    }

    // Lock the cursor and hide it
    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        aimInputController.enabled = true;  
    }

    // Unlock the cursor and make it visible
    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        aimInputController.enabled = false;  
    }

    // Check if the pointer is over a UI element
    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }
}
