using System;
using System.Collections;
using System.Collections.Generic;
using Menu;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    // this is used by the game manager to set the selected settings accordingly
    public static GameSettings settings = new GameSettings();

    private List<TitleScreenData> Titles = new List<TitleScreenData>();
    private int selectedTitle = 0;
    private int selectedOption = 0; //up down for selected title, 0 is top.
    private bool titleSelected;

    [SerializeField] TitleScreenManager TitleScreenDisplay;
    
    private void Right()
    {
        selectedTitle = (selectedTitle == Titles.Count) ? 0 : selectedTitle + 1;
        selectedOption = 0;
    }

    private void Left ()
    {
        selectedTitle = (selectedTitle == 0) ? Titles.Count - 1 : selectedTitle - 1;
        selectedOption = 0;
    }
    
    private void Update()
    {
        //if (Input.GetAxis("Horizontal"))
        //{
        //
        //}
        
    }


    private void SetTitleDisplay()
    {
        TitleScreenDisplay.Setup(Titles[selectedTitle]);
    }
    
    
    void StartTitle()
    {

    }
    
}
