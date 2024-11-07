using UnityEngine;

public class ToggleUI : MonoBehaviour
{
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject nextPageButton;
    public GameObject previousPageButton;

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
        page2.SetActive(visibility);
        page3.SetActive(visibility);
        nextPageButton.SetActive(visibility);
        previousPageButton.SetActive(visibility);
    }
}