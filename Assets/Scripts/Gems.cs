using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gems : MonoBehaviour
{
	//переменные дл€ сравнени€ "позиции" гемов
    public int x;
	public int y;

	private bool isSelected = false;

	private static Gems gemsSelected = null;

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
		//print("¬ыбран элемент:"+gemsSelected.name);
	}

	private void Deselect()
	{
		isSelected = false;
		gemsSelected = null;
		//print("Ќет выбранных элементов");
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
		//обмен "позици€ми" гемов и именами
		sel.x = sel2.x;
		sel.y = sel2.y;
		sel.name = "X:" + sel.x + "Y:" + sel.y;

		sel2.x = tempX;
		sel2.y = tempY;
		sel2.name = "X:" + sel2.x + "Y:" + sel2.y;
	}


	bool CheckNearby(Gems g)
	{
		Gems sel = gemsSelected;
		Gems sel2 = g;
		//если наход€тс€ слева
		if (sel.x - 1 == sel2.x && sel.y == sel2.y)
		{
			return true;
		}
		//если находитс€ справа
		if (sel.x + 1 == sel2.x && sel.y == sel2.y)
		{
			return true;
		}
		//если находитс€ выше
		if (sel.x == sel2.x && sel.y + 1 == sel2.y)
		{
			return true;
		}
		//если находитс€ ниже
		if (sel.x == sel2.x && sel.y - 1 == sel2.y)
		{
			return true;
		}
		return false;
	}
}
