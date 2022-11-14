using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Visual settings", menuName = "ScriptableObjects/Visual Settings", order = 1)]
public class GameVisualSettings : ScriptableObject
{
    public List<Color> PlayerColors;
    public List<string> PlayerNames;


    public Color ColorOfPlayer(int playerIndex)
    {
        return PlayerColors[playerIndex % PlayerColors.Count];
    }

    public string NameOfPlayer(int playerIndex)
    {
        return PlayerNames[playerIndex % PlayerNames.Count];
    }
}
