using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TitleScreenData", menuName="Scripts/Menu/TitleScreenData", order = 1)]
public class TitleScreenData : ScriptableObject
{
    public string name;
    public string description;
    
    public int maxPlayers;
    public int maxDifficulty;

    public Texture2D gameBackground;
    public AudioClip gameMusic;
}
