using UnityEngine;
using UnityEngine.UI;

public class DiaryController : MonoBehaviour
{
    public GameObject[] pages; // Array of page objects
    public float pageSpeed = 1.0f; // Speed at which pages turn
    private int currentPage = 0; // Index of the current page
    private Quaternion targetRotation; // The target rotation of the page
    private bool isTurning = false; // Is a page currently turning?

    public Button nextPageButton;
    public Button previousPageButton;

    void Start()
    {
        // Initialize buttons and set their initial states
        nextPageButton.onClick.AddListener(TurnNextPage);
        previousPageButton.onClick.AddListener(TurnPreviousPage);
        UpdateButtons(); // Set initial button states
    }

    void Update()
    {
        // Smooth page turning using Slerp
        if (isTurning)
        {
            // Gradually rotate the current page to the target rotation
            pages[currentPage].transform.rotation = Quaternion.Slerp(
                pages[currentPage].transform.rotation,
                targetRotation,
                Time.deltaTime * pageSpeed
            );

            // Check if the page has reached the target rotation
            if (Quaternion.Angle(pages[currentPage].transform.rotation, targetRotation) < 1.0f) // Tolerance increased for smoother interaction
            {
                // Snap to the exact target rotation
                pages[currentPage].transform.rotation = targetRotation;
                isTurning = false; // Indicate the page is no longer turning

                // Update button states after the page has turned
                UpdateButtons();
            }
        }
    }

    // Called when the "Next" button is pressed
    void TurnNextPage()
    {
        Debug.Log("Next Page Button Pressed");
        if (currentPage < pages.Length - 1 && !isTurning)
        {
            currentPage++;
            targetRotation = Quaternion.Euler(0, 180, 0); // Target rotation for a turned page
            isTurning = true; // Set turning state
        }
    }

    // Called when the "Previous" button is pressed
    void TurnPreviousPage()
    {
        Debug.Log("Previous Page Button Pressed");
        if (currentPage > 0 && !isTurning)
        {
            currentPage--;
            targetRotation = Quaternion.Euler(0, 0, 0); // Target rotation for an unturned page
            isTurning = true; // Set turning state
        }
    }

    // Enable/Disable buttons based on the current page
    void UpdateButtons()
    {
        Debug.Log("Current Page: " + currentPage);
        nextPageButton.interactable = currentPage < pages.Length - 1;
        previousPageButton.interactable = currentPage > 0;
    }
}
