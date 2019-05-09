using UnityEngine;
[System.Serializable]

public class MemoryGameCardData
{
    /// <summary> Name of the card. </summary>
    public string food;

    /// <summary> Color of the card. </summary>
    public Color color;

    /// <summary> Number of times this data point is used when assigning. </summary>
    [HideInInspector]
    public int count;
}