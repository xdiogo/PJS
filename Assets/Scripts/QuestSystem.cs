using UnityEngine;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    // The Text component to show the quest on the screen
    public Text questText;

    // Quest details
    private string currentQuest = "Interact with the Cube";
    private bool isQuestCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the quest HUD text
        UpdateQuestText();
    }

    // Method to update the quest text display
    void UpdateQuestText()
    {
        if (isQuestCompleted)
        {
            questText.text = "Quest Completed: " + currentQuest;
        }
        else
        {
            questText.text = "Current Task: " + currentQuest;
        }
    }

    // Method to mark the quest as complete
    public void CompleteQuest()
    {
        isQuestCompleted = true;
        UpdateQuestText();
    }

    // Method to simulate completing the quest when interacting
    public void InteractWithQuestItem()
    {
        if (!isQuestCompleted)
        {
            CompleteQuest();
        }
    }
}
