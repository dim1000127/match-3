using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Gems : MonoBehaviour
{
    public int x;
	public int y;

	private bool isSelected = false;
	private bool isSwap = false;
	private bool isSwapEnd = false;
	private bool isSwapBack = false;
	private static Gems gemsSelected = null;
	private static Gems gemsSelectedSecond = null;
	private Gems gemsSelectedForBack = null;

	const float speed = 15f;
	//private float tempTime = 1f;

	BoardLoad brGm = new BoardLoad();
	
	Gems sel;
	Gems sel2;
	Gems selBack;
	Gems sel2Back;

	Vector3 gemPosFirst;
	Vector3 gemPosSecond;
	Vector3 gemPosFirstBack;
	Vector3 gemPosSecondBack;

    private void Update() 
	{
		if (isSwap) 
		{
			float step = speed * Time.deltaTime;
			sel.transform.position = Vector3.MoveTowards(sel.transform.position, gemPosSecond, step);
			sel2.transform.position = Vector3.MoveTowards(sel2.transform.position, gemPosFirst, step);
			if (sel.transform.position == gemPosSecond && sel2.transform.position == gemPosFirst) 
			{
				isSwap = false;
				isSwapEnd = true;
				brGm.CheckAllMatches();
				if (!BoardLoad.deleteCheck)
				{
					SwapBack();
				}
			}
		}

		/*if (!BoardLoad.deleteCheck)
		{
			SwapBack(gameObject.GetComponent<Gems>());
		}*/

		if (isSwapBack && isSwapEnd) 
		{
			float step = speed * Time.deltaTime;
			selBack.transform.position = Vector3.MoveTowards(selBack.transform.position, gemPosSecondBack, step);
			sel2Back.transform.position = Vector3.MoveTowards(sel2Back.transform.position, gemPosFirstBack, step);
			if (selBack.transform.position == gemPosSecondBack && sel2Back.transform.position == gemPosFirstBack)
			{
				isSwapBack = false;
				isSwapEnd = false;
			}
		}

		/*if (isSwap)
		{
			tempTime -= Time.deltaTime * 2f;
			if (tempTime <= 0)
			{
				//deleteComplete = false;
				brGm.CheckAllMatches();
			}
		}*/
	}

	void OnMouseDown()
	{
		if (SceneLoad.gameIsPause) 
		{
			return;		
		}

		if (BoardLoad.animationActive) 
		{
			return;
		}

		if (isSelected)
		{
			Deselect();
		}
		else
		{
			if (gemsSelected == null)
			{
				Select();
			}
			else
			{
				if (CheckNearby(gameObject.GetComponent<Gems>()))
				{
					Swap(gameObject.GetComponent<Gems>());
					/*if (!BoardLoad.deleteCheck)
					{
						SwapBack(gameObject.GetComponent<Gems>());
					}*/
					gemsSelected.Deselect();
				}
				else 
				{
					gemsSelected.Deselect();
				}
			}
		}
	}

	private void Select()
	{
		var animContr = gameObject.GetComponent<AnimationController>();
		isSelected = true;
		//gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		animContr.ScaleLess(gameObject);
		//animationController.ScaleLess(gameObject);
		gemsSelected = gameObject.GetComponent<Gems>();
	}

	private void Deselect()
	{
		var animContr = gameObject.GetComponent<AnimationController>();
		isSelected = false;
		if(!BoardLoad.deleteCheck)
			animContr.ScaleMore(gameObject);
		//animationController.ScaleMore(gameObject);
		//gemsSelected.transform.localScale = new Vector3(1f, 1f, 1f);
		gemsSelected = null;
	}

	void Swap(Gems g) 
	{
		gemsSelectedSecond = g;
		gemsSelectedForBack = gemsSelected;
		//первый кристалл
		sel = gemsSelected;
		//второй кристалл
		sel2 = g;

		gemPosFirst = sel.transform.position;
		gemPosSecond = sel2.transform.position;
		int tempX = sel.x;
		int tempY = sel.y;

		//tempTime = 1f;
		isSwap = true;

		sel.x = sel2.x;
		sel.y = sel2.y;
		sel.name = "X:" + sel.x + "Y:" + sel.y;

		sel2.x = tempX;
		sel2.y = tempY;
		sel2.name = "X:" + sel2.x + "Y:" + sel2.y;

		BoardLoad.board[sel.x, sel.y] = sel;
		BoardLoad.board[sel2.x, sel2.y] = sel2;
	}

	void SwapBack() 
	{
		selBack = gemsSelectedForBack;
		sel2Back = gemsSelectedSecond;

		gemPosFirstBack = selBack.transform.position;
		gemPosSecondBack = sel2Back.transform.position;

		int tempX = sel.x;
		int tempY = sel.y;

		isSwapBack = true;

		selBack.x = sel2Back.x;
		selBack.y = sel2Back.y;
		selBack.name = "X:" + selBack.x + "Y:" + selBack.y;

		sel2Back.x = tempX;
		sel2Back.y = tempY;
		sel2Back.name = "X:" + sel2Back.x + "Y:" + sel2Back.y;

		BoardLoad.board[selBack.x, selBack.y] = selBack;
		BoardLoad.board[sel2Back.x, sel2Back.y] = sel2Back;
	}

	bool CheckNearby(Gems g)
	{
		Gems sel = gemsSelected;
		Gems sel2 = g;
		//если находятся слева
		if (sel.x - 1 == sel2.x && sel.y == sel2.y)
		{
			return true;
		}
		//если находится справа
		if (sel.x + 1 == sel2.x && sel.y == sel2.y)
		{
			return true;
		}
		//если находится выше
		if (sel.x == sel2.x && sel.y + 1 == sel2.y)
		{
			return true;
		}
		//если находится ниже
		if (sel.x == sel2.x && sel.y - 1 == sel2.y)
		{
			return true;
		}
		return false;
	}
}
