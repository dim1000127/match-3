using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public static Gems[,] board;
    public Gems game;
    public Gems game2;
    //лист для хранения блоков попавших под условие (3 и более в ряду)
    List<Gems> mathesAllBlocks = new List<Gems>();
    public static bool startRespawn = false;
    public static bool deleteCheck = false;
    public static int score = 0;


    public static void SizeBoard(int x, int y) {
        board = new Gems[x, y];
    }

    public void CheckAllMatches()
    {
        //нахождение рядов по вертикали
        for (int x = 0; x < board.GetLength(0); x++)
        {
            List<Gems> mathes = new List<Gems>();
            game = board[x, 0];
            for (int y = 0; y < board.GetLength(1)-1; y++)
            {
                if (board[x, y].gameObject.tag == board[x, y + 1].gameObject.tag)
                {
                    mathes.Add(board[x, y+1]);
                    if (mathes.Count == 2) 
                    {
                        mathes.Add(game);
                    }
                    if (mathes.Count >= 3 && y == board.GetLength(1) - 2)
                    {
                        mathesAllBlocks.AddRange(mathes);
                        deleteCheck = true;
                        mathes.Clear();
                    }
                }

                else 
                {
                    if (mathes.Count >= 3)
                    {
                        mathesAllBlocks.AddRange(mathes);
                        deleteCheck = true;
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
            List<Gems> mathes2 = new List<Gems>();
            game2 = board[0, y];
            for (int x = 0; x < board.GetLength(0)-1; x++)
            {
                if (board[x, y].gameObject.tag == board[x + 1, y].gameObject.tag)
                {
                    mathes2.Add(board[x+1, y]);
                    if (mathes2.Count == 2)
                    {
                        mathes2.Add(game2);
                    }
                    if (mathes2.Count >= 3 && x == board.GetLength(0) - 2)
                    {
                        mathesAllBlocks.AddRange(mathes2);
                        deleteCheck = true;
                        mathes2.Clear();
                    }
                }
                else
                {
                    if (mathes2.Count >= 3)
                    {
                        mathesAllBlocks.AddRange(mathes2);
                        deleteCheck = true;
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
        if (deleteCheck)
        {
            DeleteAllMatches();
        }
    }

    private void DeleteAllMatches() 
    {
        Gems destroyObj;

        for (int i = 0; i < mathesAllBlocks.Count; i++) 
        {
            score += 10;
            destroyObj = mathesAllBlocks[i];
            board[destroyObj.x, destroyObj.y] = null;

            Destroy(mathesAllBlocks[i].gameObject);
        }
        mathesAllBlocks.Clear();
    }

    public void SearchNullBlocks()
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (board[x, y] == null)
                {
                    SlideBlocksDown(x, y);
                }
            }
        }
        startRespawn = true;
    }

    private void SlideBlocksDown(int x, int yStart)
    {
        float startY = GameObject.FindGameObjectWithTag("Board").transform.position.y;

        Vector2 offset = GameObject.Find("5 Side Diamond(size)").GetComponent<MeshRenderer>().bounds.size;
        float ySize = offset.y + (float)0.15;

        List<Gems> slideBlocks = new List<Gems>();

        for (int y = yStart; y < board.GetLength(1); y++)
        {
            if (board[x, y] != null)
            {
                slideBlocks.Add(board[x, y]);
                board[x, y] = null;
            }
        }

        for (int k = 0; k < slideBlocks.Count; k++)
        {
            var position = slideBlocks[k].transform.position;
            position.y = startY + (ySize * (yStart + k));

            slideBlocks[k].gameObject.transform.position = position;
            slideBlocks[k].name = "X:" + x + "Y:" + (yStart + k);
            slideBlocks[k].y = yStart + k;

            board[x, yStart + k] = slideBlocks[k];
        }
    }
}
