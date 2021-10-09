using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoardLoad : MonoBehaviour
{
    [SerializeField] private List<GameObject> blocks = new List<GameObject>();
    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    [SerializeField] private TextMeshProUGUI _textScore;

    public static Gems[,] board;

    private Gems game;
    private Gems game2;

    //лист для хранения блоков попавших под условие (3 и более в ряду)
    List<Gems> mathesAllBlocks = new List<Gems>();

    List<Gems> slideBlocks = new List<Gems>();

    public bool startRespawn = false;
    public bool startSlide = false;
    public static bool deleteCheck = false;
    public static bool deleteComplete = false;
    public static bool animationActive = false;
    public static int score = 0;

    const float timeDestroy = 0.5f;
    const float speed = 15f;
    private float tempTime = timeDestroy;
    private float startYForSlide;
    private int yStartNullGem;
    private int xStartNullGem;
    private float ySizeGem;


    //AnimationController animContr = new AnimationController();
    void Start()
    {
        SizeBoard(xSize, ySize);

        Vector2 offset = blocks[4].GetComponent<MeshRenderer>().bounds.size;

        CreateBoard(offset.x + (float)0.15, offset.y + (float)0.15);
    }

    private void Update()
    { 
        //Debug.Log(tempTime);

        if (deleteComplete) 
        {
            animationActive = true;
            tempTime -= Time.deltaTime * 1.5f;
            if (tempTime <= 0) 
            {
                deleteComplete = false;
                animationActive = false;
                SearchNullBlocks();
            }
        }

        /*if (startSlide) 
        {
            for (int k = 0; k < slideBlocks.Count; k++)
            {
                Debug.Log(k);
                //var position = slideBlocks[k].transform.position;
                float step = speed * Time.deltaTime;

                Vector3 positionEnd = slideBlocks[k].transform.position;
                positionEnd.y = startYForSlide + (ySizeGem * (yStartNullGem + k));

                slideBlocks[k].transform.position = Vector3.MoveTowards(slideBlocks[k].transform.position, positionEnd, step);
                if (slideBlocks[k].transform.position == positionEnd)
                {
                    //slideBlocks[k].gameObject.transform.position = position;
                    slideBlocks[k].name = "X:" + xStartNullGem + "Y:" + (yStartNullGem + k);
                    slideBlocks[k].y = yStartNullGem + k;

                    board[xStartNullGem, yStartNullGem + k] = slideBlocks[k];
                    if (k == slideBlocks.Count - 1)
                    {
                        //slideBlocks.Clear();
                        startSlide = false;
                    }
                }
            }
            //startSlide = false;
        }*/

        if (startRespawn)
        {
            startRespawn = false;
            deleteCheck = false;
            _textScore.text = score.ToString();
            Respawn();
        }
    }

    public static void SizeBoard(int x, int y)
    {
        board = new Gems[x, y];
    }

    void CreateBoard(float xSizeGem, float ySizeGem)
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        GameObject[] gemsLeft = new GameObject[ySize];
        GameObject gemsBottom = null;
        GameObject tempBlock;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                List<GameObject> checkBlocks = new List<GameObject>();

                checkBlocks.AddRange(blocks);
                checkBlocks.Remove(gemsLeft[y]);
                checkBlocks.Remove(gemsBottom);

                int randNum = Random.Range(0, checkBlocks.Count);
                tempBlock = checkBlocks[randNum];
                tempBlock.name = "X:" + x + "Y:" + y;

                Gems gem = tempBlock.gameObject.GetComponent<Gems>();
                gem.x = x;
                gem.y = y;

                board[x, y] = Instantiate(gem, new Vector3(startX + (xSizeGem * x), startY + (ySizeGem * y), 20), gem.transform.rotation, transform);

                gemsLeft[y] = tempBlock;
                gemsBottom = tempBlock;
            }
        }
    }

    public void CheckAllMatches()
    {
        tempTime = timeDestroy;
        //нахождение рядов по вертикали
        for (int x = 0; x < board.GetLength(0); x++)
        {
            List<Gems> mathes = new List<Gems>();
            game = board[x, 0];
            for (int y = 0; y < board.GetLength(1) - 1; y++)
            {
                if (board[x, y].gameObject.tag == board[x, y + 1].gameObject.tag)
                {
                    mathes.Add(board[x, y + 1]);
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
            for (int x = 0; x < board.GetLength(0) - 1; x++)
            {
                if (board[x, y].gameObject.tag == board[x + 1, y].gameObject.tag)
                {
                    mathes2.Add(board[x + 1, y]);
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

            var animContr = destroyObj.gameObject.GetComponent<AnimationController>();
            animContr.DeleteGems(destroyObj.gameObject);

            Destroy(destroyObj.gameObject, timeDestroy);
        }

        mathesAllBlocks.Clear();
        deleteComplete = true;
        SceneLoad.timeLeft = 10f;
    }

    public void SearchNullBlocks()
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (board[x, y] == null)
                {
                    //StartCoroutine(Waiter(x,y));
                    SlideBlocksDown(x, y);
                }
            }
        }
        startRespawn = true;
    }

    /*private IEnumerator Waiter(int x, int y) 
    {

        yield return new WaitForSeconds(0.5f);
        SlideBlocksDown(x, y);
    }*/

    private void SlideBlocksDown(int x, int yStart)
    {
        //slideBlocks.Clear();
        /*xStartNullGem = x;
        yStartNullGem = yStart;
        startYForSlide = GameObject.FindGameObjectWithTag("Board").transform.position.y;*/
        float startY = GameObject.FindGameObjectWithTag("Board").transform.position.y;

        Vector2 offset = GameObject.Find("5 Side Diamond(size)").GetComponent<MeshRenderer>().bounds.size;
        List<Gems> slideBlocks = new List<Gems>();
        float ySize = offset.y + (float)0.15;

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
        //slideBlocks.Clear();
    }

    public void Respawn()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        GameObject tempBlockRes;

        Vector2 offset2 = GameObject.Find("5 Side Diamond(size)").GetComponent<MeshRenderer>().bounds.size;

        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                int randNum = Random.Range(0, blocks.Count);
                tempBlockRes = blocks[randNum];

                if (board[x, y] == null)
                {
                    tempBlockRes.name = "X:" + x + "Y:" + y;

                    Gems gemRes = tempBlockRes.gameObject.GetComponent<Gems>();
                    gemRes.x = x;
                    gemRes.y = y;

                    board[x, y] = Instantiate(gemRes, new Vector3(startX + ((offset2.x + (float)0.15) * x), startY + ((offset2.y + (float)0.15) * y), 20), gemRes.transform.rotation, transform);
                }
            }
        }

        CheckAllMatches();

        /*if (deleteCheck)
        {
            SearchNullBlocks();
        }*/

        //SceneLoad.timeLeft = 10f;
    }
}
