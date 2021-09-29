using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLoad : MonoBehaviour
{
    public int[,] board;
    public GameObject[] blocks;
    //public GameObject tile;
    //размерность доски
    public int xSize; 
    public int ySize;

    void Start()
    {

        board = new int[xSize, ySize];
        //Vector2 offset = tile.GetComponent<SpriteRenderer>().bounds.size;
        CreateBoard();
    }

    void Update()
    {
        
    }

    void CreateBoard()
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                int randNum = Random.Range(0, blocks.Length); //получаем номер рандомного гема из массива гемов
                GameObject gem = (GameObject)Instantiate(blocks[randNum], new Vector3(x, y, 10), blocks[randNum].transform.rotation) as GameObject;

                board[x, y] = randNum;
                Gems b = gem.gameObject.AddComponent<Gems>();
                b.x = x;
                b.y = y;
                b.ID = randNum;
            }
        }
    }
}
