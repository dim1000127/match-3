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
    [SerializeField] private GameObject _GemForSize;

    public static Gems[,] board;

    private Gems game;
    private Gems game2;

    //лист для хранения блоков попавших под условие (3 и более в ряду)
    List<Gems> mathesAllBlocks = new List<Gems>();

    List<Gems> slideBlocks = new List<Gems>();

    private bool startRespawn = false;
    private bool startSlide = false;
    private bool slideBlocksDownEnd = false;
    public static bool deleteCheck = false;
    public static bool deleteComplete = false;
    public static bool animationActive = false;
    public static int score = 0;

    private const float timeDestroy = 0.8f;
    private const float speed = 45f;
    private const float positionZ = 20f;
    private const string nameTagBoard = "Board";

    private float tempTimeDelete = timeDestroy;
    private float tempTimeRespawn = timeDestroy;
    private float startYForSlide;
    private int yStartNullGem;
    private float ySizeGem;

    void Start()
    {
        SizeBoard(xSize, ySize);

        Vector2 offset = _GemForSize.GetComponent<MeshRenderer>().bounds.size;

        CreateBoard(offset.x + (float)0.15, offset.y + (float)0.15);
    }

    void Update()
    { 

        if (deleteComplete) 
        {
            animationActive = true;
            tempTimeDelete -= Time.deltaTime * 1.5f;
            if (tempTimeDelete <= 0) 
            {
                deleteComplete = false;
                animationActive = false;
                SearchNullBlocks();
            }
        }

        if (startRespawn)
        {
            animationActive = true;
            tempTimeRespawn -= Time.deltaTime * 1.5f;
            if (tempTimeRespawn <= 0)
            {
                animationActive = false;
                startRespawn = false;
                deleteCheck = false;
                _textScore.text = score.ToString();
                Respawn();
            }
        }
    }

    void FixedUpdate()
    {
        if (startSlide)
        {
            animationActive = true;
            for (int k = 0; k < slideBlocks.Count; k++)
            {
                float step = speed * Time.deltaTime;

                Vector3 positionEnd = slideBlocks[k].transform.position;
                positionEnd.y = startYForSlide + (ySizeGem * (yStartNullGem + k));

                slideBlocks[k].transform.position = Vector3.MoveTowards(slideBlocks[k].transform.position, positionEnd, step);
                if (slideBlocks[slideBlocks.Count - 1].transform.position == positionEnd)
                {
                    startSlide = false;
                    animationActive = false;
                    tempTimeRespawn = timeDestroy;
                    slideBlocks.Clear();
                    SearchNullBlocks();
                }
            }
        }
    }

    public static void SizeBoard(int x, int y)
    {
        board = new Gems[x, y];
    }

    private void CreateBoard(float xSizeGem, float ySizeGem)
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        float positionGemX;
        float positionGemY;

        var gemsLeft = new GameObject[ySize];
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

                positionGemX = startX + (xSizeGem * x);
                positionGemY = startY + (ySizeGem * y);

                board[x, y] = Instantiate(gem, new Vector3(positionGemX, positionGemY, positionZ), gem.transform.rotation, transform);

                gemsLeft[y] = tempBlock;
                gemsBottom = tempBlock;
            }
        }
    }

    public void CheckAllMatches()
    {
        tempTimeDelete = timeDestroy;
        //нахождение рядов по вертикали
        for (int x = 0; x < board.GetLength(0); x++)
        {
            var mathes = new List<Gems>();
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
            var mathes2 = new List<Gems>();
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
        int xFirstNull = 0;
        int yFirstNull = 0;

        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (board[x, y] == null)
                {
                    if (SearchNotNullBlocksUp(x, y))
                    {
                        xFirstNull = x;
                        yFirstNull = y;
                        x = board.GetLength(0) + 1;
                        SlideBlocksDown(xFirstNull, yFirstNull);
                        break;
                    }
                }
            }
        }
        startRespawn = true;
    }

    private bool SearchNotNullBlocksUp(int x, int yStart) 
    {
        for (int y = yStart; y < board.GetLength(1); y++)
        {
            if (board[x, y] != null)
            {
                return true;
            }
        }
        return false;
    }

    private void SlideBlocksDown(int x, int yStart)
    {
        yStartNullGem = yStart;
        startYForSlide = GameObject.FindGameObjectWithTag(nameTagBoard).transform.position.y;

        Vector2 offset = _GemForSize.GetComponent<MeshRenderer>().bounds.size;

        ySizeGem = offset.y + (float)0.15;

        for (int y = yStart; y < board.GetLength(1); y++)
        {
            if (board[x, y] != null)
            {
                slideBlocks.Add(board[x, y]);
                board[x, y] = null;
            }
        }

        startSlide = true;

        for (int k = 0; k < slideBlocks.Count; k++)
        {
            slideBlocks[k].name = "X:" + x + "Y:" + (yStart + k);
            slideBlocks[k].y = yStart + k;

            board[x, yStart + k] = slideBlocks[k];
        }
    }

    public void Respawn()
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

        float positionGemXRespawn;
        float positionGemYRespawn;

        GameObject tempBlockRes;

        Vector2 offset2 = _GemForSize.GetComponent<MeshRenderer>().bounds.size;

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

                    positionGemXRespawn = startX + ((offset2.x + (float)0.15) * x);
                    positionGemYRespawn = startY + ((offset2.y + (float)0.15) * y);

                    board[x, y] = Instantiate(gemRes, new Vector3(positionGemXRespawn, positionGemYRespawn, positionZ), gemRes.transform.rotation, transform);
                }
            }
        }
        tempTimeRespawn = timeDestroy;
        CheckAllMatches();
    }
}
