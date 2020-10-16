using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //private singleton

    [Header("Set in Inspector")]
    public Text uitLevel; //reference to level text object
    public Text uitShots; //reference to shots text object
    public Text uitButton; //reference to button object

    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level; //current level
    public int levelMax; //max level
    public int shotsTaken;
    public GameObject castle; //current castle
    public GameMode mode = GameMode.idle; //set the current game mode
    public string showing = "Show Slingshot"; //Follow cam mode (text that shows on button)

    // Start is called before the first frame update
    void Start()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        //get rid of the old castle if it exists
        if (castle != null)
        {
            Destroy(castle);
        }
        //get rid of any old projectiles
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //instantiate new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
       
        //reset the camera
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //reset the goal
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }


    // Update is called once per frame
    void Update()
    {
        UpdateGUI();
        
        //check for level end
        if (mode == GameMode.playing && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f); 

        }
    }
    void NextLevel()
    {
        level++;
        if(level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;

        switch(showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;
            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void shotFired()
    {
        S.shotsTaken++;
    }
}
