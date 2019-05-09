using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MemoryGameManager : MonoBehaviour
{
    private GameObject[] memoryCards = new GameObject[0];                           // Reference to the GameObject that holds the memory game.
    [SerializeField] private List<MemoryGameCardData> memoryCardData = null;        // Reference to the data that holds the memory game.
    
    private MemoryGameCardBehavior selectedCard = null;     // Used to reference a selected card.

    private Vector3 interactPos = Vector3.zero;     // Variable for determining the point the users wishes to interact with.

    void Awake()
    {
        // Finds the memory cards on screen.
        memoryCards = new GameObject[transform.childCount * transform.GetChild(0).childCount];

        // Stores them in an array.
        for (int index1 = 0; index1 < transform.childCount; index1++)
        {
            Transform parentTransform = transform.GetChild(index1);     // Used to access the cards within rows.
            for (int index2 = 0; index2 < parentTransform.childCount; index2++)
            {
                memoryCards[(index1 * parentTransform.childCount) + index2] = parentTransform.GetChild(index2).gameObject;
            }
        }

        // Assigns data to the cards.
        AssignCardValues();
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))    // Checks for user input.
        {
            if (Input.touchCount > 0)   // Touch Input
            {
                // Gets the position of the touch.
                Touch touch = Input.GetTouch(0);
                interactPos = Camera.main.ScreenToWorldPoint(touch.position);
                interactPos.z = 0f;
            }
            else if (Input.GetMouseButtonDown(0))   // Mouse Input
            {
                // Gets the position of the mouse click.
                interactPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                interactPos.z = 0f;
            }
            CheckCards();
        }
        else if (Input.GetKeyDown(KeyCode.R))   // Resets game.
            ResetGame();
    }

    /// <summary> Assigns the data to the cards. </summary>
    private void AssignCardValues()
    {
        List<MemoryGameCardData> memoryCardDataCopy = memoryCardData;   // Used for storing values only during this method.
        for (int index = 0; index < memoryCards.Length; index++)
        {
            // Creates an array to choose a data type.
            MemoryGameCardData[] memoryCardDataArray = new MemoryGameCardData[memoryCardDataCopy.Count];   // Reference to the data while assigning data.
            memoryCardDataCopy.CopyTo(memoryCardDataArray);

            // Picks a random data type.
            int randomInt = Random.Range(0, memoryCardDataArray.Length);
            MemoryGameCardData randomData = memoryCardDataArray[randomInt];
            // Currently throws a null refererence expection after first assigning values.
            // MemoryCardData is made null after the first values are assigned. Not sure why.

            // Assigns data.
            memoryCards[index].GetComponent<SpriteRenderer>().color = randomData.color;
            memoryCards[index].GetComponent<MemoryGameCardBehavior>().food = randomData.food;
            memoryCards[index].transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = randomData.food;

            // Increases count of that data and removes it if it has reached the limit.
            randomData.count++;
            if (randomData.count == 2)
                memoryCardDataCopy.Remove(randomData);
        }
    }
    
    /// <summary> Checks the interaction point against each card's position for an overlap. </summary>
    private void CheckCards()
    {
        bool cardFound = false;     // Has the card been found?
        foreach (GameObject card in memoryCards)
        {
            if (!cardFound)
            {
                Vector2 cardSize = card.GetComponent<SpriteRenderer>().size;
                // Don't know x needed to be divided 4 instead of 2. Unity thinks it twice as long as it is. Try changing this back to 2 when changing the sprite.
                if (interactPos.x > (card.transform.position.x - (cardSize.x / 4f)) && interactPos.x < (card.transform.position.x + (cardSize.x / 4f)))
                {
                    if (interactPos.y > (card.transform.position.y - (cardSize.y / 2f)) && interactPos.y < (card.transform.position.y + (cardSize.y / 2f)))
                    {
                        CardSelected(card);
                        cardFound = true;
                    }
                }
            }
        }
    }

    /// <summary> Handles the processes of selecting a card. </summary>
    /// <param name="card"> The card to be selected. </param>
    private void CardSelected(GameObject card)
    {
        if (selectedCard != null)
        {
            if (selectedCard.gameObject.Equals(card))
            {
                card.GetComponent<MemoryGameCardBehavior>().DeselectCard();
                selectedCard = null;
                return;
            }
            else if (selectedCard.Matches(card.GetComponent<MemoryGameCardBehavior>()))
            {
                card.GetComponent<MemoryGameCardBehavior>().FlipCard();
                selectedCard.FlipCard();
                selectedCard = null;
            }
        }
        else
        {
            selectedCard = card.GetComponent<MemoryGameCardBehavior>();
            card.GetComponent<MemoryGameCardBehavior>().SelectCard();
        }
    }

    /// <summary> Enables all the cards and sets new values for the cards. </summary>
    /// Currently doesn't work because memoryCardData is made null after the first values are assigned. Not sure why.
    private void ResetGame()
    {
        for (int index = 0; index < memoryCards.Length; index++)
        {
            memoryCards[index].GetComponent<MemoryGameCardBehavior>().Reset();
        }
        AssignCardValues();
    }
}