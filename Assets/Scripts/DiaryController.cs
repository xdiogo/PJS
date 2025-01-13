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
