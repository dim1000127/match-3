using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LoadPlayerPrefs : MonoBehaviour
{
    [SerializeField] private GameObject _contentList;

    private List<string[]> allNoteLoad = new List<string[]>();
    private List<string[]> allNoteLoadForSort = new List<string[]>();

    string[] max;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            if (PlayerPrefs.HasKey($"N{i}"))
            {
                string[] records = TextParse(PlayerPrefs.GetString($"N{i}"));
                allNoteLoad.Add(records);
            }
        }

        Debug.Log(allNoteLoad.Count);

        if (allNoteLoad.Count != 0)
        {
            allNoteLoadForSort.AddRange(allNoteLoad);
            for (int i = 0; i < allNoteLoad.Count; i++)
            {
                max = SearchMax();
                var row = _contentList.transform.GetChild(i);
                row.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = max[0];
                row.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = max[1];
            }
        }
    }

    private string[] SearchMax()
    {
        string[] maxRecords = null;
        int maxScore = 0;

        for (int i = 0; i < allNoteLoadForSort.Count; i++)
        {
            if (maxScore <= Convert.ToInt32(allNoteLoadForSort[i][1]))
            {
                maxScore = Convert.ToInt32(allNoteLoadForSort[i][1]);
                maxRecords = allNoteLoadForSort[i];
            }
        }

        if (allNoteLoadForSort.Count == 1) 
        {
            maxRecords = allNoteLoadForSort[0];
        }

        allNoteLoadForSort.Remove(maxRecords);

        return maxRecords;
    }

    private string[] TextParse(string record)
    {
        return record.Split(new char[] { '&' });
    }
}
