using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadFromConfig : MonoBehaviour {

    string configFilePath = @"\Unity Projects\Find The Gold\Assets\Text Files\config.txt";
    float cardWidth, cardLength, cardHeight, gridWidth, gridLength, cardGap, boardMargin, boardWidth, boardLength, boardHeight, camSize;
    string startPos;
    string[] endPos;

    GameObject boardGO;
    public GameObject playTileGO, startTileGO, endTileGO;
    Camera mainCam;
   
    // Use this for initialization
    void Start () {

        //Load lines from config.txt into a string array
        string[] lines = File.ReadAllLines(configFilePath);

        //Get the "Board" GO to edit its dimensions based on config.txt values
        boardGO = GameObject.FindGameObjectWithTag("Board");
        mainCam = Camera.main;

        foreach (string line in lines)
        {
            if (!line.Equals(""))
            {
                string[] splitLine = line.Split('|');

                if (splitLine[0][0].Equals("/") && splitLine[0][1].Equals("/"))
                    return;//This is a comment

                if (splitLine[0].Equals("CARDSIZE"))
                {
                    cardWidth = float.Parse(splitLine[1]);
                    cardLength = float.Parse(splitLine[2]);
                    cardHeight = float.Parse(splitLine[3]);
                }

                else if (splitLine[0].Equals("BOARDGRID"))
                {
                    gridWidth = float.Parse(splitLine[1]);
                    gridLength = float.Parse(splitLine[2]);
                }

                else if (splitLine[0].Equals("CARDGAP"))
                {
                    cardGap = float.Parse(splitLine[1]);
                }

                else if (splitLine[0].Equals("BMARGIN"))
                {
                    boardMargin = float.Parse(splitLine[1]);
                }

                else if (splitLine[0].Equals("STARTPOS"))
                {
                    startPos = splitLine[1];
                }

                else if (splitLine[0].Equals("ENDCARDS"))
                {
                    endPos = splitLine[1].Split(' ');
                }
            }
        }

        //Calculate the board size based on the values from config.txt
        boardHeight = 0.2f;
        boardWidth = (gridWidth * cardLength) + (2 * boardMargin) + ((gridWidth - 1) * cardGap);
        boardLength = (gridLength * cardWidth) + (2 * boardMargin) + ((gridLength - 1) * cardGap);

        //Set the camera's orthographic size to display the full board
        //Value specified should be half the required size.
        camSize = Mathf.Ceil(boardLength / 2.0f) + 2.0f;

        boardGO.transform.localScale = new Vector3(boardWidth, boardHeight, boardLength);
        mainCam.orthographicSize = camSize;

        CreatePlayingTile();
        AddStartAndEndCards();
    }

    void CreatePlayingTile()
    {
        //Getting bottom-left of the board as origin for further calculations
        Vector2 originNew = new Vector2(-(boardWidth / 2), -(boardLength / 2));

        //Get the starting point for the first card holder
        Vector2 cardStart = new Vector2((originNew.x + (cardLength / 2) + boardMargin), (originNew.y + (cardWidth / 2) + boardMargin));

        //Start creating the card holders
        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridWidth; j++)
            {
                GameObject cardHolder = GameObject.Instantiate(playTileGO, new Vector3((cardStart.x + j * (cardLength + cardGap)), ((boardHeight / 2) + (cardHeight / 2)), (cardStart.y + i * (cardWidth + cardGap))), Quaternion.identity);

                cardHolder.transform.localScale = new Vector3(cardLength, cardHeight, cardWidth);
                cardHolder.name = i.ToString() + "-" + j.ToString();
            }
        }
    }

    void AddStartAndEndCards()
    {
        if ((float.Parse(startPos)-1) < gridWidth)
        {
            string startCardName = "0-" + (float.Parse(startPos)-1).ToString();

            GameObject tileToPlaceOn = GameObject.Find(startCardName);

            GameObject startTile = GameObject.Instantiate(startTileGO, tileToPlaceOn.transform.position, Quaternion.identity);
            startTile.transform.localScale = new Vector3(cardLength, cardHeight, cardWidth);
            Destroy(tileToPlaceOn);
            startTile.name = startCardName;
        }
        else
            Debug.Log("Start tile position is outside the placeable grid.");

        if (endPos.Length != 0)
        {
            for (int n = 0; n < endPos.Length; n++)
            {
                if ((float.Parse(endPos[n]) - 1) < gridWidth)
                {
                    string endCardName = (gridLength-1).ToString() + "-" + (float.Parse(endPos[n]) - 1).ToString();

                    GameObject tileToPlaceOn = GameObject.Find(endCardName);

                    GameObject endTile = GameObject.Instantiate(endTileGO, tileToPlaceOn.transform.position, Quaternion.identity);
                    endTile.transform.localScale = new Vector3(cardLength, cardHeight, cardWidth);
                    Destroy(tileToPlaceOn);
                    endTile.name = endCardName;
                }
                else
                    Debug.Log("End tile position is outside the placeable grid.");
            }
        }
    }
}
