using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLoad : MonoBehaviour
{
    public GameObject[,] board;
    public List<GameObject> blocks = new List<GameObject>();

    //����������� �����
    public int xSize;
    public int ySize;

    void Start()
    {
        board = new GameObject[xSize, ySize];
        //���������� ������� �������� �������(����)
        Vector2 offset = blocks[4].GetComponent<MeshRenderer>().bounds.size;
        //print(offset.x);
        //print(offset.y);
        //��� ���������� ����� ������ ������������ 0.15
        CreateBoard(offset.x + (float)0.15, offset.y + (float)0.15);
    }

    void Update()
    {

    }

    void CreateBoard(float xSizeGem, float ySizeGem)
    {
        float startX = transform.position.x;     
        float startY = transform.position.y;

        //������ ��� �������� ����������� ����� �����
        GameObject[] gemsLeft = new GameObject[ySize];
        //������ ��� �������� ������������ ���� �����
        GameObject gemsBottom = null;
        GameObject tempBlock;
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                //����� List(����� ����� � ������) ��� ����������� ������ � ���
                List<GameObject> checkBlocks = new List<GameObject>();
                checkBlocks.AddRange(blocks);
                //�������� �� List ��������� �������. ����� � �����
                checkBlocks.Remove(gemsLeft[y]);
                checkBlocks.Remove(gemsBottom);
                //print(checkBlocks.Count);
                int randNum = Random.Range(0, checkBlocks.Count); //����� ���������� ���� �� ����� �����
                tempBlock = checkBlocks[randNum];
                //���������� ���� � ������� ������������
                Instantiate(tempBlock, new Vector3(startX + (xSizeGem * x), startY + (ySizeGem * y), 20), tempBlock.transform.rotation, transform);

                board[x, y] = tempBlock;
                gemsLeft[y] = tempBlock;
                gemsBottom = tempBlock;
            }
        }
    }
}
