using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLoad : MonoBehaviour
{
    public List<GameObject> blocks = new List<GameObject>();

    //размерность доски
    public int xSize;
    public int ySize;

    void Start()
    {
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
                Instantiate(tempBlock, new Vector3(startX + (xSizeGem * x), startY + (ySizeGem * y), 20), tempBlock.transform.rotation, transform);

                gemsLeft[y] = tempBlock;
                gemsBottom = tempBlock;
            }
        }
    }
}
