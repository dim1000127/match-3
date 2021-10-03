using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
	//���������� ��� ��������� "�������" �����
    public int x;
	public int y;

	private bool isSelected = false;

	private static Gems gemsSelected = null;

	Board brGm = new Board();

	void Start()
    {

    }

    void Update()
    {
        
    }

	void OnMouseDown()
	{
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
					Deselect();
				}
				else 
				{
					Deselect();
				}
			}
		}
	}

	private void Select()
	{
		isSelected = true;

		//Color oldColor = gameObject.GetComponent<Renderer>().material.color;
		//Color newColor = new Color(0.754717f, 0.652f, 0.5375578f, oldColor.a);
		//gameObject.GetComponent<Renderer>().material.color = newColor;
		//print(newColor);
		gemsSelected = gameObject.GetComponent<Gems>();
		//print("������ �������:"+gemsSelected.name);
	}

	private void Deselect()
	{
		isSelected = false;
		gemsSelected = null;
		//print("��� ��������� ���������");
	}

	void Swap(Gems g) 
	{
		//������ ��������
		Gems sel = gemsSelected;
		//������ ��������
		Gems sel2 = g;
		//Debug.Log("��� �� ��������� � �������" + Board.board[sel.x, sel.y].tag);
		//Debug.Log("��� �� ��������� � �������" + Board.board[sel2.x, sel2.y].tag);
		Vector3 gemPos = sel.transform.position;
		int tempX = sel.x;
		int tempY = sel.y;

		sel.transform.position = sel2.transform.position;
		sel2.transform.position = gemPos;
		//����� "���������" ����� � �������
		sel.x = sel2.x;
		sel.y = sel2.y;
		sel.name = "X:" + sel.x + "Y:" + sel.y;

		sel2.x = tempX;
		sel2.y = tempY;
		sel2.name = "X:" + sel2.x + "Y:" + sel2.y;
		Board.board[sel.x, sel.y] = sel.gameObject;
		//Debug.Log("��� ����� ��������� � �������" + Board.board[sel.x, sel.y].tag);
		Board.board[sel2.x, sel2.y] = sel2.gameObject;
		//Debug.Log("��� ����� ��������� � �������" + Board.board[sel2.x, sel2.y].tag);
	}


	bool CheckNearby(Gems g)
	{
		Gems sel = gemsSelected;
		Gems sel2 = g;
		//���� ��������� �����
		if (sel.x - 1 == sel2.x && sel.y == sel2.y)
		{
			return true;
		}
		//���� ��������� ������
		if (sel.x + 1 == sel2.x && sel.y == sel2.y)
		{
			return true;
		}
		//���� ��������� ����
		if (sel.x == sel2.x && sel.y + 1 == sel2.y)
		{
			return true;
		}
		//���� ��������� ����
		if (sel.x == sel2.x && sel.y - 1 == sel2.y)
		{
			return true;
		}
		return false;
	}
}
