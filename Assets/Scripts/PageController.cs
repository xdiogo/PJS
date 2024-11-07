using UnityEngine;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    public Image frontPage;
    public Image backPage;

    private void Start()
    {
        // Set the same sprite for both pages
        backPage.sprite = frontPage.sprite;
    }

    // Call this method to flip the page
    public void FlipPage()
    {
        // Logic for flipping the page can go here
        // This could involve animations or swapping visibility, etc.
        Debug.Log("Page flipped!");
    }
}
