using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadPlayerPrefs : MonoBehaviour
{
    [SerializeField] private GameObject _contentList;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey($"N{i}"))
            {
                string[] record = TextParse(PlayerPrefs.GetString($"N{i}"));
                var row = _contentList.transform.GetChild(i);
                row.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = record[0];
                row.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = record[1];
            }
        }
    }

    string[] TextParse(string record)
    {
        return record.Split(new char[] { '&' });
    }
}
