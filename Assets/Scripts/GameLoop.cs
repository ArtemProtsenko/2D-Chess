using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLoop : MonoBehaviour
{
    public GameObject ChessPiece;

    private GameObject[,] positions = new GameObject[8, 8];

    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private string currentPlayer = "white";
    private bool gameOver = false;

    void Start()
    {
        playerWhite = new GameObject[]
        {
            CreateChessPiece("WhitePawn", 0, 1), CreateChessPiece("WhitePawn", 1, 1), CreateChessPiece("WhitePawn", 2, 1), CreateChessPiece("WhitePawn", 3, 1),
            CreateChessPiece("WhitePawn", 4, 1), CreateChessPiece("WhitePawn", 5, 1), CreateChessPiece("WhitePawn", 6, 1), CreateChessPiece("WhitePawn", 7, 1),
            CreateChessPiece("WhiteRook", 0, 0), CreateChessPiece("WhiteRook", 7, 0), CreateChessPiece("WhiteKnight", 1, 0), CreateChessPiece("WhiteKnight", 6, 0),
            CreateChessPiece("WhiteBishop", 2, 0), CreateChessPiece("WhiteBishop", 5, 0), CreateChessPiece("WhiteQueen", 3, 0), CreateChessPiece("WhiteKing", 4, 0)
        };

        playerBlack = new GameObject[]
        {
            CreateChessPiece("BlackPawn", 0, 6), CreateChessPiece("BlackPawn", 1, 6), CreateChessPiece("BlackPawn", 2, 6), CreateChessPiece("BlackPawn", 3, 6),
            CreateChessPiece("BlackPawn", 4, 6), CreateChessPiece("BlackPawn", 5, 6), CreateChessPiece("BlackPawn", 6, 6), CreateChessPiece("BlackPawn", 7, 6),
            CreateChessPiece("BlackRook", 0, 7), CreateChessPiece("BlackRook", 7, 7), CreateChessPiece("BlackKnight", 1, 7), CreateChessPiece("BlackKnight", 6, 7),
            CreateChessPiece("BlackBishop", 2, 7), CreateChessPiece("BlackBishop", 5, 7), CreateChessPiece("BlackQueen", 3, 7), CreateChessPiece("BlackKing", 4, 7)
        };

        for (int i = 0; i < playerWhite.Length; i++)
        {
            setPosition(playerWhite[i]);
            setPosition(playerBlack[i]);
        }
    }

    public void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("Game");
        }
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Winner(string player)
    {
        gameOver = true;

        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = player + " won!\nCongratulations!";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;

    }

    public GameObject CreateChessPiece(string name, int x, int y)
    {
        GameObject gameObject = Instantiate(ChessPiece, new Vector3(0, 0, -1), Quaternion.identity);
        ChessPieceScript script = gameObject.GetComponent<ChessPieceScript>();

        script.name = name;
        script.setXBoard(x);
        script.setYBoard(y);
        script.Activate();

        return gameObject;
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }

        return true;
    }

    public void setPosition(GameObject gameObject)
    {
        ChessPieceScript script = gameObject.GetComponent<ChessPieceScript>();

        positions[script.getXBoard(), script.getYBoard()] = gameObject;
    }

    public void setPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject getPosition(int x, int y)
    {
        return positions[x, y];
    }

    public string getCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool isGameOver()
    {
        return gameOver;
    }
}
