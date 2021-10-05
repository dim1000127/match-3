using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
    public int x;
	public int y;

	private bool isSelected = false;
	private static Gems gemsSelected = null;

	Board brGm = new Board();


	void OnMouseDown()
	{
		if (SceneLoad.gameIsPause) 
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
					brGm.CheckAllMatches();
					if (Board.deleteCheck)
					{
						brGm.SearchNullBlocks();
					}
                    else 
					{
						SwapBack(gameObject.GetComponent<Gems>());
					}
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
		isSelected = true;
		gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
		gemsSelected = gameObject.GetComponent<Gems>();
	}

	private void Deselect()
	{
		isSelected = false;
		gemsSelected.transform.localScale = new Vector3(1f, 1f, 1f);
		gemsSelected = null;
	}

	void Swap(Gems g) 
	{
		//первый кристалл
		Gems sel = gemsSelected;
		//второй кристалл
		Gems sel2 = g;

		Vector3 gemPos = sel.transform.position;
		int tempX = sel.x;
		int tempY = sel.y;

		sel.transform.position = sel2.transform.position;
		sel2.transform.position = gemPos;
		
		sel.x = sel2.x;
		sel.y = sel2.y;
		sel.name = "X:" + sel.x + "Y:" + sel.y;

		sel2.x = tempX;
		sel2.y = tempY;
		sel2.name = "X:" + sel2.x + "Y:" + sel2.y;

		Board.board[sel.x, sel.y] = sel;
		Board.board[sel2.x, sel2.y] = sel2;
	}

	void SwapBack(Gems g) 
	{
		Gems sel = gemsSelected;
		Gems sel2 = g;

		Vector3 gemPos = sel.transform.position;

		int tempX = sel.x;
		int tempY = sel.y;

		sel.transform.position = sel2.transform.position;
		sel2.transform.position = gemPos;

		sel.x = sel2.x;
		sel.y = sel2.y;
		sel.name = "X:" + sel.x + "Y:" + sel.y;

		sel2.x = tempX;
		sel2.y = tempY;
		sel2.name = "X:" + sel2.x + "Y:" + sel2.y;

		Board.board[sel.x, sel.y] = sel;
		Board.board[sel2.x, sel2.y] = sel2;
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
