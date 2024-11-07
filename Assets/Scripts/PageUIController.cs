using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PageUIController : MonoBehaviour
{
    public Image frontPage; // Front UI Image
    public Image backPage; // Back UI Image
    public float pageSpeed = 5f; // Speed of the page turning
    private bool isFlipped = false; // Track the page state

    void Start()
    {
        backPage.gameObject.SetActive(false); // Hide the back page initially
    }

    public void TurnPage()
    {
        if (isFlipped)
        {
            StartCoroutine(TurnToFront());
        }
        else
        {
            StartCoroutine(TurnToBack());
        }
        isFlipped = !isFlipped; // Toggle the state
    }

    private IEnumerator TurnToBack()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            // Optional: Add rotation logic here for a more dynamic flip
            elapsedTime += Time.deltaTime * pageSpeed;
            yield return null;
        }
        backPage.gameObject.SetActive(true); // Show the back side
        frontPage.gameObject.SetActive(false); // Hide the front side
    }

    private IEnumerator TurnToFront()
    {
        float elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            // Optional: Add rotation logic here for a more dynamic flip
            elapsedTime += Time.deltaTime * pageSpeed;
            yield return null;
        }
        frontPage.gameObject.SetActive(true); // Show the front side
        backPage.gameObject.SetActive(false); // Hide the back side
    }
}
