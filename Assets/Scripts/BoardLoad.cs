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
        Board.SizeBoard(xSize, ySize);
        //���������� ������� �������� �������(����)
        Vector2 offset = blocks[4].GetComponent<MeshRenderer>().bounds.size;
        //print(offset.x);
        //print(offset.y);
        //��� ���������� ����� ������ ������������ 0.15
        CreateBoard(offset.x + (float)0.15, offset.y + (float)0.15);
    }

    void Update()
    {
        if (Board.startRespawn)
        {
            Board.startRespawn = false;
            Board.deleteCheck = false;
            Respawn();
        }
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

                int randNum = Random.Range(0, checkBlocks.Count); //����� ���������� ���� �� ����� �����
                tempBlock = checkBlocks[randNum];
                tempBlock.name = "X:" + x + "Y:" + y;
                Gems gem = tempBlock.gameObject.GetComponent<Gems>();
                gem.x = x;
                gem.y = y;
                //���������� ���� � ������� ������������
                Board.board[x, y] = Instantiate(gem, new Vector3(startX + (xSizeGem * x), startY + (ySizeGem * y), 20), gem.transform.rotation, transform);
                gemsLeft[y] = tempBlock;
                gemsBottom = tempBlock;
            }
        }
    }

    public void Respawn()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        GameObject tempBlockRes;

        Vector2 offset2 = GameObject.Find("5 Side Diamond(size)").GetComponent<MeshRenderer>().bounds.size;
        for (int x = 0; x < Board.board.GetLength(0); x++)
        {
            for (int y = 0; y < Board.board.GetLength(1); y++)
            {
                int randNum = Random.Range(0, blocks.Count);
                tempBlockRes = blocks[randNum];
                //Debug.Log("�����");
                if (Board.board[x, y] == null)
                {
                    tempBlockRes.name = "X:" + x + "Y:" + y;

                    Gems gemRes = tempBlockRes.gameObject.GetComponent<Gems>();
                    gemRes.x = x;
                    gemRes.y = y;

                    Board.board[x, y] = Instantiate(gemRes, new Vector3(startX + ((offset2.x + (float)0.15) * x), startY + ((offset2.y + (float)0.15) * y), 20), gemRes.transform.rotation, transform);
                }
            }
        }
        br.CheckAllMatches();
        if (Board.deleteCheck)
        {
            br.SearchNullBlocks();
        }
    }
}
