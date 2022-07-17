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

	BoardLoad boardrLoad = new BoardLoad();
	
	private Gems sel;
	private Gems sel2;
	private Gems selBack;
	private Gems sel2Back;

	private Vector3 gemPosFirst;
	private Vector3 gemPosSecond;
	private Vector3 gemPosFirstBack;
	private Vector3 gemPosSecondBack;

    private void FixedUpdate() 
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
				boardrLoad.CheckAllMatches();
				if (!BoardLoad.deleteCheck)
				{
					SwapBack();
				}
			}
		}

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
	}

	private void OnMouseDown()
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

		animContr.ScaleLess(gameObject);
		gemsSelected = gameObject.GetComponent<Gems>();
	}

	private void Deselect()
	{
		var animContr = gameObject.GetComponent<AnimationController>();
		isSelected = false;
		if(!BoardLoad.deleteCheck)
			animContr.ScaleMore(gameObject);
		gemsSelected = null;
	}

	private void Swap(Gems gemSelected) 
	{
		gemsSelectedSecond = gemSelected;
		gemsSelectedForBack = gemsSelected;
		//������ ��������
		sel = gemsSelected;
		//������ ��������
		sel2 = gemSelected;

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

	private void SwapBack() 
	{
		selBack = gemsSelectedForBack;
		sel2Back = gemsSelectedSecond;

		gemPosFirstBack = selBack.transform.position;
		gemPosSecondBack = sel2Back.transform.position;

		int tempX = selBack.x;
		int tempY = selBack.y;

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

	private bool CheckNearby(Gems gemSelected)
	{
		Gems selCheck = gemsSelected;
		Gems sel2Check = gemSelected;
		//���� ��������� �����
		if (selCheck.x - 1 == sel2Check.x && selCheck.y == sel2Check.y)
		{
			return true;
		}
		//���� ��������� ������
		if (selCheck.x + 1 == sel2Check.x && selCheck.y == sel2Check.y)
		{
			return true;
		}
		//���� ��������� ����
		if (selCheck.x == sel2Check.x && selCheck.y + 1 == sel2Check.y)
		{
			return true;
		}
		//���� ��������� ����
		if (selCheck.x == sel2Check.x && selCheck.y - 1 == sel2Check.y)
		{
			return true;
		}
		return false;
	}
}
