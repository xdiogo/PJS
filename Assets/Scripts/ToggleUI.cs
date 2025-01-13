using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject page1;

    private bool isVisible = false;

    void Start()
    {
        // Initially hide all UI elements
        SetUIVisibility(false);
    }

    void Update()
    {
        // Check if the B key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Toggle visibility
            isVisible = !isVisible;
            SetUIVisibility(isVisible);
        }
    }

    private void SetUIVisibility(bool visibility)
    {
        if (visibility)
            MouseLock.current.UnlockCursor();
        else
            MouseLock.current.LockCursor();


        page1.SetActive(visibility);
    }
}