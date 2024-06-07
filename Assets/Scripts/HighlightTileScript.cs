using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HighlightTileScript : MonoBehaviour
{
    public GameObject Controller;

    GameObject selectedPiece = null;

    int matrixX;
    int matrixY;

    public bool isAttack = false;

    public void Start()
    {
        if (isAttack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        Controller = GameObject.FindGameObjectWithTag("GameController");

        if (isAttack)
        {
            Attack(Controller, selectedPiece);
        }
        else
        {
            if (selectedPiece.name == "WhiteKing" || selectedPiece.name == "BlackKing")
            {
                if (!selectedPiece.GetComponent<ChessPieceScript>().getIsCastled())
                {
                    Castle(Controller);
                }
            }

            Move(Controller, matrixX, matrixY, selectedPiece);
        }
    }

    public void Move(GameObject contr, int x, int y, GameObject piece)
    {
        contr.GetComponent<GameLoop>().setPositionEmpty(selectedPiece.GetComponent<ChessPieceScript>().getXBoard(), selectedPiece.GetComponent<ChessPieceScript>().getYBoard());

        piece.GetComponent<ChessPieceScript>().setXBoard(x);
        piece.GetComponent<ChessPieceScript>().setYBoard(y);
        piece.GetComponent<ChessPieceScript>().setIsMoved(true);
        piece.GetComponent<ChessPieceScript>().applyCoords();

        contr.GetComponent<GameLoop>().setPosition(piece);

        contr.GetComponent<GameLoop>().NextTurn();

        piece.GetComponent<ChessPieceScript>().DestroyHighlightedTiles();
    }

    public void Attack(GameObject contr, GameObject piece)
    {
        GameObject ChessPiece = Controller.GetComponent<GameLoop>().getPosition(matrixX, matrixY);

        if (ChessPiece.name == "WhiteKing")
        {
            Controller.GetComponent<GameLoop>().Winner("Black");
        }
        if (ChessPiece.name == "BlackKing")
        {
            Controller.GetComponent<GameLoop>().Winner("White");
        }

        Destroy(ChessPiece);

        Move(contr, matrixX, matrixY, piece);
    }

    public void Castle(GameObject contr)
    {
        GameLoop gameLoop = Controller.GetComponent<GameLoop>();

        GameObject castle;
        int x;

        if (gameLoop.getPosition(matrixX + 1, matrixY) != null)
        {
            castle = gameLoop.getPosition(matrixX + 1, matrixY);
            x = matrixX - 1;
        }
        else
        {
            castle = gameLoop.getPosition(matrixX - 2, matrixY);
            x = matrixX + 1;
        }

        selectedPiece.GetComponent<ChessPieceScript>().setIsCastled(true);

        Move(Controller, x, matrixY, castle);

        contr.GetComponent<GameLoop>().NextTurn();
    }

    public void setCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void setSelectedTile(GameObject gameObject)
    {
        selectedPiece = gameObject;
    }

    public GameObject getSelectedTile()
    {
        return selectedPiece;
    }
}
