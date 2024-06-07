using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChessPieceScript : MonoBehaviour
{
    public GameObject Controller;
    public GameObject HighlightTile;

    private int xBoard = -1;
    private int yBoard = -1;

    private bool isMoved = false;

    private bool isCastled = false;

    private string player;

    public Sprite BlackBishop, BlackKing, BlackKnight, BlackPawn, BlackQueen, BlackRook;
    public Sprite WhiteBishop, WhiteKing, WhiteKnight, WhitePawn, WhiteQueen, WhiteRook;

    public void Activate()
    {
        Controller = GameObject.FindGameObjectWithTag("GameController");

        applyCoords();

        switch (this.name)
        {
            case "WhiteBishop":
                this.GetComponent<SpriteRenderer>().sprite = WhiteBishop;
                player = "white";
                break;
            case "WhiteKing":
                this.GetComponent<SpriteRenderer>().sprite = WhiteKing;
                player = "white";
                break;
            case "WhiteKnight":
                this.GetComponent<SpriteRenderer>().sprite = WhiteKnight;
                player = "white";
                break;
            case "WhitePawn":
                this.GetComponent<SpriteRenderer>().sprite = WhitePawn;
                player = "white";
                break;
            case "WhiteQueen":
                this.GetComponent<SpriteRenderer>().sprite = WhiteQueen;
                player = "white";
                break;
            case "WhiteRook":
                this.GetComponent<SpriteRenderer>().sprite = WhiteRook;
                player = "white";
                break;

            case "BlackBishop":
                this.GetComponent<SpriteRenderer>().sprite = BlackBishop;
                player = "black";
                break;
            case "BlackKing":
                this.GetComponent<SpriteRenderer>().sprite = BlackKing;
                player = "black"; ;
                break;
            case "BlackKnight":
                this.GetComponent<SpriteRenderer>().sprite = BlackKnight;
                player = "black";
                break;
            case "BlackPawn":
                this.GetComponent<SpriteRenderer>().sprite = BlackPawn;
                player = "black";
                break;
            case "BlackQueen":
                this.GetComponent<SpriteRenderer>().sprite = BlackQueen;
                player = "black";
                break;
            case "BlackRook":
                this.GetComponent<SpriteRenderer>().sprite = BlackRook;
                player = "black";
                break;
        }
    }

    public void applyCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 0.66f;
        y *= 0.66f;

        x -= 2.3f;
        y -= 2.3f;

        this.transform.position = new Vector3(x, y, -1);
    }

    public void OnMouseUp()
    {
        if(!Controller.GetComponent<GameLoop>().isGameOver() && Controller.GetComponent<GameLoop>().getCurrentPlayer() == player)
        {
            DestroyHighlightedTiles();

            InitiateHighlightedTiles();
        }
    }

    public void DestroyHighlightedTiles()
    {
        GameObject[] HighlightedTiles = GameObject.FindGameObjectsWithTag("Highlighter");

        for (int i = 0; i < HighlightedTiles.Length; i++)
        {
            Destroy(HighlightedTiles[i]);
        }
    }

    public void InitiateHighlightedTiles()
    {
        switch (this.name)
        {
            case "WhiteQueen":
            case "BlackQueen":
                LineHighlighter(1, 0);
                LineHighlighter(-1, 0);
                LineHighlighter(0, 1);
                LineHighlighter(0, -1);
                LineHighlighter(1, 1);
                LineHighlighter(-1, -1);
                LineHighlighter(-1, 1);
                LineHighlighter(1, -1);
                break;
            case "WhiteKnight":
            case "BlackKnight":
                LHighlighter();
                break;
            case "WhiteBishop":
            case "BlackBishop":
                LineHighlighter(1, 1);
                LineHighlighter(-1, 1);
                LineHighlighter(1, -1);
                LineHighlighter(-1, -1);
                break;
            case "WhiteKing":
            case "BlackKing":
                KingHighlighter();
                break;
            case "WhiteRook":
            case "BlackRook":
                LineHighlighter(1, 0);
                LineHighlighter(-1, 0);
                LineHighlighter(0, 1);
                LineHighlighter(0, -1);
                break;
            case "WhitePawn":
                PawnHighlighter(xBoard, yBoard, 1);
                break;
            case "BlackPawn":
                PawnHighlighter(xBoard, yBoard, -1);
                break;
        }
    }

    public void LineHighlighter(int xMove, int yMove)
    {
        GameLoop gameLoop = Controller.GetComponent<GameLoop>();

        int x = xBoard + xMove;
        int y = yBoard + yMove;

        while (gameLoop.PositionOnBoard(x, y) && gameLoop.getPosition(x, y) == null)
        {
            HighlightRegular(x, y);
            x += xMove;
            y += yMove;
        }
        if (gameLoop.PositionOnBoard(x, y) && gameLoop.getPosition(x, y).GetComponent<ChessPieceScript>().player != player)
        {
            HighlightAttack(x, y);
        }
    }

    public void LHighlighter()
    {
        PointHighlighter(xBoard + 1, yBoard + 2);
        PointHighlighter(xBoard + 1, yBoard - 2);
        PointHighlighter(xBoard - 1, yBoard + 2);
        PointHighlighter(xBoard - 1, yBoard - 2);
        PointHighlighter(xBoard + 2, yBoard + 1);
        PointHighlighter(xBoard + 2, yBoard - 1);
        PointHighlighter(xBoard - 2, yBoard + 1);
        PointHighlighter(xBoard - 2, yBoard - 1);
    }

    public void KingHighlighter()
    {
        PointHighlighter(xBoard + 1, yBoard);
        PointHighlighter(xBoard - 1, yBoard);
        PointHighlighter(xBoard, yBoard + 1);
        PointHighlighter(xBoard, yBoard - 1);
        PointHighlighter(xBoard + 1, yBoard + 1);
        PointHighlighter(xBoard + 1, yBoard - 1);
        PointHighlighter(xBoard - 1, yBoard - 1);
        PointHighlighter(xBoard - 1, yBoard + 1);
        Castling();
    }

    public void Castling()
    {
        if (!isMoved)
        {
            GameLoop gameLoop = Controller.GetComponent<GameLoop>();

            GameObject ChessPiece = gameLoop.getPosition(xBoard + 3, yBoard);

            if (ChessPiece != null)
            {
                if (!ChessPiece.GetComponent<ChessPieceScript>().getIsMoved())
                {
                    if (gameLoop.getPosition(xBoard + 1, yBoard) == null && gameLoop.getPosition(xBoard + 2, yBoard) == null)
                    {
                        Highlight(xBoard + 2, yBoard, false);
                    }
                }
            }

            ChessPiece = gameLoop.getPosition(xBoard - 4, yBoard);

            if (ChessPiece != null)
            {
                if (!ChessPiece.GetComponent<ChessPieceScript>().getIsMoved())
                {
                    if (gameLoop.getPosition(xBoard - 1, yBoard) == null && gameLoop.getPosition(xBoard - 2, yBoard) == null)
                    {
                        Highlight(xBoard - 2, yBoard, false);
                    }
                }
            }
        }
    }

    public void PointHighlighter(int x, int y)
    {
        GameLoop gameLoop = Controller.GetComponent<GameLoop>();

        if (gameLoop.PositionOnBoard(x, y))
        {
            GameObject ChessPiece = gameLoop.getPosition(x, y);

            if (ChessPiece == null)
            {
                HighlightRegular(x, y);
            }
            else if (ChessPiece.GetComponent<ChessPieceScript>().player != player)
            {
                HighlightAttack(x, y);
            }
        }
    }

    public void PawnHighlighter(int x, int y, int delta)
    {
        GameLoop gameLoop = Controller.GetComponent<GameLoop>();

        y += delta;

        if (gameLoop.PositionOnBoard(x, y))
        {
            if (gameLoop.getPosition(x, y) == null)
            {
                HighlightRegular(x, y);

                y += delta;

                if(gameLoop.getPosition(x, y) == null && !isMoved)
                {
                    HighlightRegular(x, y);

                    isMoved = true;
                }

                y -= delta;
            }

            if (gameLoop.PositionOnBoard(x + 1, y) && gameLoop.getPosition(x + 1, y) != null && gameLoop.getPosition(x + 1, y).GetComponent<ChessPieceScript>().player != player)
            {
                HighlightAttack(x + 1, y);
            }
            if (gameLoop.PositionOnBoard(x - 1, y) && gameLoop.getPosition(x - 1, y) != null && gameLoop.getPosition(x - 1, y).GetComponent<ChessPieceScript>().player != player)
            {
                HighlightAttack(x - 1, y);
            }
        }
    }

    public void Highlight(int matrixX, int matrixY, bool isAttack)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x -= 2.3f;
        y -= 2.3f;

        float z = -2.0f;
        if (isAttack)
        {
            z = -3.0f;
        }
        GameObject Highlight = Instantiate(HighlightTile, new Vector3(x, y, z), Quaternion.identity);

        HighlightTileScript script = Highlight.GetComponent<HighlightTileScript>();
        script.isAttack = isAttack;
        script.setSelectedTile(gameObject);
        script.setCoords(matrixX, matrixY);
    }

    public void HighlightRegular(int x, int y)
    {
        Highlight(x, y, false);
    }

    public void HighlightAttack(int x, int y)
    {
        Highlight(x, y, true);
    }

    public int getXBoard() { return xBoard; }

    public int getYBoard() { return yBoard; }

    public bool getIsMoved() { return isMoved; }

    public bool getIsCastled() { return isCastled; }

    public void setXBoard(int xBoard) {  this.xBoard = xBoard; }

    public void setYBoard(int yBoard) { this.yBoard = yBoard; }

    public void setIsMoved(bool isMoved) { this.isMoved = isMoved; }

    public void setIsCastled(bool isCastled) { this.isCastled = isCastled; }
}
