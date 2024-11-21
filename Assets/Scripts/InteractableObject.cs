using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Reference to the QuestSystem
    public QuestSystem questSystem;

    // Interaction distance
    public float interactionRange = 3f;  // How close the player needs to be to interact with the cube

    // Update is called once per frame
    void Update()
    {
        // Perform raycast from the camera (player's viewpoint)
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  // Get the ray from the camera to the mouse pointer

        if (Physics.Raycast(ray, out hit, interactionRange)) // Raycast with a max distance (interactionRange)
        {
            // Check if the raycast hit the cube and if the player presses E
            if (hit.collider.CompareTag("Interactable") && Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }
    }

    // Handle interaction (completing the quest)
    void Interact()
    {
        // If the QuestSystem is not null, complete the quest
        if (questSystem != null)
        {
            questSystem.InteractWithQuestItem(); // Complete the quest
        }
    }
}
