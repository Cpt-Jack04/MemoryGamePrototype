using UnityEngine;

public class MemoryGameCardBehavior : MonoBehaviour
{
    // Component references.
    private SpriteRenderer spriteRend;

    // Event for when this card is selected, deselected, and flipped.
    public delegate void FlipCardDelegate();
    public static event FlipCardDelegate SelectEvent;
    public static event FlipCardDelegate DselectEvent;
    public static event FlipCardDelegate FlipEvent;

    // Food values
    [HideInInspector]
    public string food = "";

    // Color values.
    [HideInInspector]
    public Color defaultValue = Color.white;
    private Color selectedValue = Color.white;

    /// <summary> Is this card currently selected? </summary>
    public bool isSelected
    {
        get
        {
            return isSelected;
        }
        private set
        {
            if (value)
                spriteRend.color = selectedValue;
            else
                spriteRend.color = defaultValue;
        }
    }
    /// <summary> Has this card be matched? </summary>
    public bool isMatched
    {
        get
        {
            return isMatched;
        }
        private set
        {
            gameObject.SetActive(!value);
            isSelected = false;
        }
    }

    void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();

        defaultValue = spriteRend.color;
        selectedValue = new Color(spriteRend.color.r, spriteRend.color.g, spriteRend.color.b, spriteRend.color.a / 2f);
    }

    /// <summary> Called when the first card is select. Call in MemoryGameManager.cs </summary>
    public void SelectCard()
    {
        // Calls the select event.
        if (SelectEvent != null)
            SelectEvent.Invoke();

        isSelected = true;
    }

    /// <summary> Called when the second card is selected and is not a match. Call in MemoryGameManager.cs </summary>
    public void DeselectCard()
    {
        // Calls the deselect event.
        if (DselectEvent != null)
            DselectEvent.Invoke();

        isSelected = false;
    }

    /// <summary> Called when the second card is selected and is a match. Call in MemoryGameManager.cs </summary>
    public void FlipCard()
    {
        // Calls the flip event.
        if (FlipEvent != null)
            FlipEvent.Invoke();

        isMatched = true;
    }

    /// <summary> Checks to see if the given behaviour matches the info of this one. </summary>
    /// <param name="cardBehavior"> The card info to be checked. </param>
    /// <returns> Does the given cardBehaviour match this one? </returns>
    public bool Matches(MemoryGameCardBehavior cardBehavior)
    {
        return food.Equals(cardBehavior.food);
    }

    /// <summary> Resets the isSelected and isMatched values </summary>
    public void Reset()
    {
        isMatched = false;
    }
}