﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameControllerScript : MonoBehaviour
{
    GameObject player;

    GameObject tapText;
    GameObject toText;
    GameObject startText;
    GameObject bestTimeText;

    Camera mainCamera;

    GameObject highScoreText;
    GameObject currentScoreText;
    GameObject currentTimeText;

    public bool started = false;
    public bool firstClicked = false;
    public bool showHighScore = false;
    public bool showBestTime = false;

    float[] highScore = new float[7];
    float highScorePercentage = 0f;
    public float[] levelLength;
    
    float currentScore = 0f;

    int[] levelsFinished = new int[7];
    float[] bestLevelTimes = new float[7];
    

    void Start()
    {
        levelLength = new float[7];
        levelLength[1] = 341.5f; 
        levelLength[2] = 240.5f; 
        levelLength[3] = 391.15f; 
        levelLength[4] = 434.5f; 
        levelLength[5] = 434.5f; 
        levelLength[6] = 821.25f;

        player = GameObject.Find("Player");
        DontDestroyOnLoad(this);

        mainCamera = Camera.main;

        tapText = GameObject.Find("TapText");
        toText = GameObject.Find("ToText");
        startText = GameObject.Find("StartText");
        


        highScoreText = GameObject.Find("HighScoreText");
        currentScoreText = GameObject.Find("CurrentScoreText");
        currentTimeText = GameObject.Find("CurrentTimeText");
        bestTimeText = GameObject.Find("BestTimeText");

        highScore[1] = PlayerPrefs.GetFloat("highScore1", 0f);
        highScore[2] = PlayerPrefs.GetFloat("highScore2", 0f);
        highScore[3] = PlayerPrefs.GetFloat("highScore3", 0f);
        highScore[4] = PlayerPrefs.GetFloat("highScore4", 0f);
        highScore[5] = PlayerPrefs.GetFloat("highScore5", 0f);
        highScore[6] = PlayerPrefs.GetFloat("highScore6", 0f);

        levelsFinished[1] = PlayerPrefs.GetInt("level1FinishedPref", 0);
        bestLevelTimes[1] = PlayerPrefs.GetFloat("level1BestTime", 10000f);
        

        levelsFinished[2] = PlayerPrefs.GetInt("level2FinishedPref", 0);
        bestLevelTimes[2] = PlayerPrefs.GetFloat("level2BestTime", 10000f);
        

        levelsFinished[3] = PlayerPrefs.GetInt("level3FinishedPref", 0);
        bestLevelTimes[3] = PlayerPrefs.GetFloat("level3BestTime", 10000f);

        levelsFinished[4] = PlayerPrefs.GetInt("level4FinishedPref", 0);
        bestLevelTimes[4] = PlayerPrefs.GetFloat("level4BestTime", 10000f);

        levelsFinished[5] = PlayerPrefs.GetInt("level5FinishedPref", 0);
        bestLevelTimes[5] = PlayerPrefs.GetFloat("level5BestTime", 10000f);

        levelsFinished[6] = PlayerPrefs.GetInt("level6FinishedPref", 0);
        bestLevelTimes[6] = PlayerPrefs.GetFloat("level6BestTime", 10000f);



        if (highScore[SceneManager.GetActiveScene().buildIndex] > 0.01f)
        {
            highScorePercentage =  (highScore[SceneManager.GetActiveScene().buildIndex] / levelLength[SceneManager.GetActiveScene().buildIndex]) * 100f;
            if(highScorePercentage > 99.49f && levelsFinished[SceneManager.GetActiveScene().buildIndex] == 0)
            {
                highScorePercentage = 99f;
            }
            //highScoreText.GetComponent<TextMeshPro>().text = "High Score: " + highScore.ToString("0");
            highScoreText.GetComponent<TextMeshPro>().text = highScorePercentage.ToString("0") + "%";
            highScoreText.GetComponent<MeshRenderer>().enabled = true;
        }

        if(levelsFinished[SceneManager.GetActiveScene().buildIndex] == 1)
        {
            bestTimeText.GetComponent<TextMeshPro>().text = "Best Time: " + ConvertSecondsToMinutesAndSeconds(bestLevelTimes[SceneManager.GetActiveScene().buildIndex]);

            bestTimeText.GetComponent<MeshRenderer>().enabled = true;
        }

        
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    

    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (tapText == null)
        {
            tapText = GameObject.Find("TapText");
        }

        if (toText == null)
        {
            toText = GameObject.Find("ToText");
        }

        if (startText == null)
        {
            startText = GameObject.Find("StartText");
        }

        if (highScoreText == null)
        {
            highScoreText = GameObject.Find("HighScoreText");
            if (highScore[SceneManager.GetActiveScene().buildIndex] > 0.01f && showHighScore)
            {
                highScorePercentage = (highScore[SceneManager.GetActiveScene().buildIndex] / levelLength[SceneManager.GetActiveScene().buildIndex]) * 100f;
                if (highScorePercentage > 99.49f && levelsFinished[SceneManager.GetActiveScene().buildIndex] == 0)
                {
                    highScorePercentage = 99f;
                }
                highScoreText.GetComponent<TextMeshPro>().text = highScorePercentage.ToString("0") + "%";
                highScoreText.GetComponent<MeshRenderer>().enabled = true;
            }

        }

        if (bestTimeText == null)
        {
            bestTimeText = GameObject.Find("BestTimeText");
            if (showBestTime && (levelsFinished[SceneManager.GetActiveScene().buildIndex] == 1))
            {
                bestTimeText.GetComponent<TextMeshPro>().text = "Best Time: " + ConvertSecondsToMinutesAndSeconds(bestLevelTimes[SceneManager.GetActiveScene().buildIndex]);

                bestTimeText.GetComponent<MeshRenderer>().enabled = true;
            }

        }


        if (currentScoreText == null)
        {
            currentScoreText = GameObject.Find("CurrentScoreText");
        }

        if (currentTimeText == null)
        {
            currentTimeText = GameObject.Find("CurrentTimeText");
        }

        if (!firstClicked || !started)
        {
            CheckForFirstClick();
        }




        if (player != null)
        {
            Vector3 desiredPosition = new Vector3(player.transform.position.x, player.transform.position.y -13.42f, player.transform.position.z - 25.57f);
            Vector3 smoothedPosition = Vector3.Lerp(mainCamera.transform.position, desiredPosition, 0.10f);
            mainCamera.transform.position = smoothedPosition;

        }
        
    }

    public void CheckForFirstClick()
    {
        if (!started && !firstClicked)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                tapText.GetComponent<MeshRenderer>().enabled = false;
                toText.GetComponent<MeshRenderer>().enabled = false;
                startText.GetComponent<MeshRenderer>().enabled = false;
                highScoreText.GetComponent<MeshRenderer>().enabled = false;
                bestTimeText.GetComponent<MeshRenderer>().enabled = false;
                showHighScore = false;
                showBestTime = false;
                started = true;
                firstClicked = true;
                player.GetComponent<PlayerControllerScript>().makePlayerActive();
            }
        }
        else if(started && !firstClicked)
        {
            if(player != null)
            {
                player.GetComponent<MeshRenderer>().enabled = true;
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                started = true;
                firstClicked = true;
                showHighScore = false;
                showBestTime = false;
                highScoreText.GetComponent<MeshRenderer>().enabled = false;
                bestTimeText.GetComponent<MeshRenderer>().enabled = false;
                player.GetComponent<PlayerControllerScript>().makePlayerActive();
            }
        }
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void LoadNextLevel()
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex;

        if(buildIndex < 6)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        

    }

    public void ProcessNewScore(float newScore)
    {
        if(newScore > highScore[SceneManager.GetActiveScene().buildIndex])
        {
            highScore[SceneManager.GetActiveScene().buildIndex] = newScore;
            highScorePercentage = (highScore[SceneManager.GetActiveScene().buildIndex] / levelLength[SceneManager.GetActiveScene().buildIndex]) * 100f;
            PlayerPrefs.SetFloat("highScore"+ SceneManager.GetActiveScene().buildIndex.ToString(), newScore);
        }

        if(levelsFinished[SceneManager.GetActiveScene().buildIndex] == 1)
        {
            showBestTime = true;
        }
        showHighScore = true;
    }

    public void SetBestTimeOnLevel(float time)
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        levelsFinished[level] = 1;
        PlayerPrefs.SetInt("level" + level.ToString() + "FinishedPref", 1);
        if(time < bestLevelTimes[level])
        {
            bestLevelTimes[level] = time;
            PlayerPrefs.SetFloat("level"+ level.ToString() +"BestTime", time);
        }
        showBestTime = true;
        
    }

    public float CurrentLevelBestTime()
    {
        return bestLevelTimes[SceneManager.GetActiveScene().buildIndex];
    }

    private string ConvertSecondsToMinutesAndSeconds(float originalSeconds)
    {
        string returnString = "";
        float minutes = originalSeconds / 60f;
        if(minutes < 1f)
        {
            returnString += "00:";
        }
        else if(minutes < 10f)
        {
            returnString += "0" + Mathf.FloorToInt(minutes) + ":";
        }
        else
        {
            
            returnString += Mathf.FloorToInt(minutes) + ":";
        }

        originalSeconds = originalSeconds - (60f * Mathf.FloorToInt(minutes));

        if (originalSeconds < 1f)
        {
            returnString += "00" + RoundToTwoDecimalPoints(originalSeconds);
        }
        else if (originalSeconds < 10f)
        {
            returnString += "0" + RoundToTwoDecimalPoints(originalSeconds);
        }
        else
        {
            returnString += RoundToTwoDecimalPoints(originalSeconds);
        }
        

        return returnString;
    }

    private string RoundToTwoDecimalPoints(float toRound)
    {
        string rounded = "";

        rounded = toRound.ToString("0.00");
        return rounded;
    }
}
