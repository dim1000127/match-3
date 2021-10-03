using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board:MonoBehaviour
{
    public static GameObject[,] board;
    public GameObject game;
    public GameObject game2;
    //лист для хранения блоков попавших под условие (3 и более в ряду)
    List<GameObject> mathesAllBlocks = new List<GameObject>();
    public void SizeBoard(int x, int y) {
        board = new GameObject[x, y];
    }

    public void CheckAllMatches()
    {
        //нахождение рядов по вертикали
        for (int x = 0; x < board.GetLength(0); x++)
        {
            List<GameObject> mathes = new List<GameObject>();
            game = board[x, 0];
            for (int y = 0; y < board.GetLength(1)-1; y++)
            {
                if (board[x, y].tag == board[x, y + 1].tag)
                {
                    mathes.Add(board[x, y+1]);
                    if (mathes.Count == 2) 
                    {
                        mathes.Add(game);
                    }
                }
                else 
                {
                    if (mathes.Count >= 3)
                    {
                        mathesAllBlocks.AddRange(mathes);
                        mathes.Clear();
                    }
                    else 
                    {
                        mathes.Clear();
                    }
                    game = board[x, y + 1];
                }
            }
        }
        //нахождение рядов по горизонтали
        for (int y = 0; y < board.GetLength(1); y++)
        {
            List<GameObject> mathes2 = new List<GameObject>();
            game2 = board[0, y];
            for (int x = 0; x < board.GetLength(0) - 1; x++)
            {
                if (board[x, y].tag == board[x + 1, y].tag)
                {
                    mathes2.Add(board[x+1, y]);
                    if (mathes2.Count == 2)
                    {
                        mathes2.Add(game2);
                    }
                }
                else
                {
                    if (mathes2.Count >= 3)
                    {
                        mathesAllBlocks.AddRange(mathes2);
                        mathes2.Clear();
                    }
                    else
                    {
                        mathes2.Clear();
                    }
                    game2 = board[x + 1, y];
                }
            }
        }
        DeleteAllMatches();
    }

    public void DeleteAllMatches() 
    {
        for (int i = 0; i < mathesAllBlocks.Count; i++) 
        {
            Destroy(mathesAllBlocks[i]);
        }
        mathesAllBlocks.Clear();
    }
}
