using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLoad : MonoBehaviour
{
    [SerializeField] private List<GameObject> blocks = new List<GameObject>();

    //размерность доски
    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    Board br = new Board();

    void Start()
    {
        br.SizeBoard(xSize, ySize);
        //вычисление размера игрового объекта(гема)
        Vector2 offset = blocks[4].GetComponent<MeshRenderer>().bounds.size;
        //print(offset.x);
        //print(offset.y);
        //для расстояния между гемами прибавляется 0.15
        CreateBoard(offset.x + (float)0.15, offset.y + (float)0.15);
    }

    void Update()
    {
        
    }

    void CreateBoard(float xSizeGem, float ySizeGem)
    {
        float startX = transform.position.x;     
        float startY = transform.position.y;

        //массив для хранения добавленных гемов слева
        GameObject[] gemsLeft = new GameObject[ySize];
        //массив для хранения добавленного гема внизу
        GameObject gemsBottom = null;
        GameObject tempBlock;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                //новый List(копия листа с гемами) для динмической работы с ним
                List<GameObject> checkBlocks = new List<GameObject>();
                checkBlocks.AddRange(blocks);
                //удаление из List элементов находящ. слева и внизу
                checkBlocks.Remove(gemsLeft[y]);
                checkBlocks.Remove(gemsBottom);
                //print(checkBlocks.Count);
                int randNum = Random.Range(0, checkBlocks.Count); //номер рандомного гема из листа гемов
                tempBlock = checkBlocks[randNum];
                tempBlock.name = "X:" + x + "Y:" + y;
                Gems gem = tempBlock.gameObject.GetComponent<Gems>();
                gem.x = x;
                gem.y = y;
                //добавление гема в игровое пространство
                Board.board[x, y] = Instantiate(tempBlock, new Vector3(startX + (xSizeGem * x), startY + (ySizeGem * y), 20), tempBlock.transform.rotation, transform);
                gemsLeft[y] = tempBlock;
                gemsBottom = tempBlock;
            }
        }

        /*for (int x = 0; x < br.board.GetLength(0); x++)
        {
            for (int y = 0; y < br.board.GetLength(1); y++)
            {
                Debug.Log(br.board[x, y].gameObject.name);
            }
        }*/
    }
}
