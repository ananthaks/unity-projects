using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hacker : MonoBehaviour {

	// Use this for initialization
	void Start () {

        var greetings = "Anantha";
        ShowMainMenu(greetings);
	}

    void ShowMainMenu(string msg) {
        Terminal.ClearScreen();
        Terminal.WriteLine("> Hello "+msg);
        Terminal.WriteLine("> What would you like to hack into?");
        Terminal.WriteLine(">   Press 1 for EASY level");
        Terminal.WriteLine(">   Press 2 for MEDIUM level");
        Terminal.WriteLine(">   Press 3 for HARD level");
    }

    void OnUserInput(string message) {
        Terminal.WriteLine("You wrote ");
        Terminal.WriteLine(message);
        //print(message);
        print(message == "1");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
