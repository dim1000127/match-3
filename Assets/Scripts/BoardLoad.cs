using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLoad : MonoBehaviour
{
    [SerializeField] private List<GameObject> blocks = new List<GameObject>();

    //����������� �����
    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    Board br = new Board();

    void Start()
    {
        br.SizeBoard(xSize, ySize);
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
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
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
                tempBlock.name = "X:" + x + "Y:" + y;
                Gems gem = tempBlock.gameObject.GetComponent<Gems>();
                gem.x = x;
                gem.y = y;
                //���������� ���� � ������� ������������
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
