using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour {

    enum Screen {MainMenu, GuessPassword, Win};

    int mGameLevel;
    int mEasyProgress, mMediumProgress, mHardProgress;
    Screen mCurrentScreen;
    string mCurrAnagram;

    List<List<string>> mAnagrams = new List<List<string>>();

    List<string> easyAnagrams = new List<string>();
    List<string> mediumAnagrams = new List<string>();
    List<string> hardAnagrams = new List<string>();

    // Use this for initialization
    void Start () {
        mCurrentScreen = Screen.MainMenu;

        easyAnagrams.Add("rent");
        easyAnagrams.Add("book");
        easyAnagrams.Add("scan");
        easyAnagrams.Add("quiet");
        easyAnagrams.Add("shelf");
        
        mediumAnagrams.Add("storage");
        mediumAnagrams.Add("index");
        mediumAnagrams.Add("author");
        mediumAnagrams.Add("article");
        mediumAnagrams.Add("barcode");

        hardAnagrams.Add("reference");
        hardAnagrams.Add("journal");
        hardAnagrams.Add("publisher");
        hardAnagrams.Add("catalogue");
        hardAnagrams.Add("newspaper");

        mAnagrams.Add(easyAnagrams);
        mAnagrams.Add(mediumAnagrams);
        mAnagrams.Add(hardAnagrams);

        mEasyProgress = 0;
        mMediumProgress = 0;
        mHardProgress = 0;

        ShowMainMenu(true);
	}

    void ShowMainMenu(bool clearScr) {

        if(clearScr)
        {
            Terminal.ClearScreen();
            Terminal.WriteLine("Solve the Anagram!!");
        }
        Terminal.WriteLine("> What level do you want to play?");
        Terminal.WriteLine(">   Press 1 for EASY level");
        Terminal.WriteLine(">   Press 2 for MEDIUM level");
        Terminal.WriteLine(">   Press 3 for HARD level");
    }

    void OnUserInput(string message) {

        switch(mCurrentScreen)
        {
            case Screen.MainMenu:
                ProcessMainMenu(message);
                break;
            case Screen.GuessPassword:
                ProcessPassword(message);
                break;
            case Screen.Win:
                // TODO
                break;
        }
    }

    void ProcessPassword(string message)
    {
        if (mCurrAnagram.Contains(message))
        {
            Terminal.WriteLine("Your guess was correct!!");
            Terminal.WriteLine("Try another! or type menu to go back");
            Terminal.WriteLine("");

            IncreaseLevel();
            GenerateAnagram();
        }
        else if(message == "menu")
        {
            mCurrentScreen = Screen.MainMenu;
            ShowMainMenu(true);
        }
        else
        {
            Terminal.WriteLine("Your guess was incorrect.");
            Terminal.WriteLine("Please try again");
            GenerateAnagram();
        }

    }
    
    void ProcessMainMenu(string input)
    {
        if (input == "1")
        {
            mGameLevel = 1;
            StartGame();
        }
        else if (input == "2")
        {
            mGameLevel = 2;
            StartGame();
        }
        else if (input == "3")
        {
            mGameLevel = 3;
            StartGame();
        }
        else
        {
            Terminal.WriteLine("The level you entered was invalid!");
            ShowMainMenu(false);
        }
    }

    void IncreaseLevel()
    {
        if (mGameLevel == 1)
        {
            mEasyProgress = (++mEasyProgress) % 5;
        }
        else if (mGameLevel == 2)
        {
            mMediumProgress = (++mMediumProgress) % 5;
        }
        else if (mGameLevel == 3)
        {
            mHardProgress = (++mHardProgress) % 5;
        }
    }

    void GenerateAnagram()
    {
        string currPass = "";
        if (mGameLevel == 1)
        {
            currPass = easyAnagrams[mEasyProgress];
        }
        else if (mGameLevel == 2)
        {
            currPass = mediumAnagrams[mMediumProgress];
        }
        else if (mGameLevel == 3)
        {
            currPass = hardAnagrams[mHardProgress];
        }

        mCurrAnagram = currPass;

        string anagram = StringExtension.Anagram(currPass);

        Terminal.WriteLine("Try to find the word : " + anagram);
    }

    void StartGame()
    {
        mCurrentScreen = Screen.GuessPassword;
        GenerateAnagram();
        //Terminal.WriteLine("You have selected Level " + mGameLevel);
    }

    // Update is called once per frame
    void Update () {
		
	}
}