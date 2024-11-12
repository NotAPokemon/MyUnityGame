using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class StatPanel : MonoBehaviour
{
    public List<Stat> stats;
    public GameObject statPrefab;
    public static StatPanel self;
    bool made = false;

    private void make()
    {
        made = true;
        for (int i = 0; i < stats.Count; i++)
        {
            GameObject newStat = Instantiate(statPrefab, transform);
            newStat.SetActive(true);
            newStat.transform.localPosition = Vector3.zero + Vector3.down * 10 * i + Vector3.up * 44.7f;

        }
    }

    private void Awake()
    {
        self = this;
    }

    public void updateStats()
    {
        if (!made)
        {
            make();
        }
        for (int i = 0; i < stats.Count; i++)
        {
            Transform temp = transform.GetChild(i);
            temp.GetChild(0).GetComponent<Image>().sprite = stats[i].icon;
            TextMeshProUGUI name = temp.GetChild(1).GetComponent<TextMeshProUGUI>();
            name.color = new Color(stats[i].color.r, stats[i].color.g, stats[i].color.b);
            name.SetText(stats[i].displayName + ":  " + Calculator.Round(stats[i].amount,2));
        }
    }

    public void Update()
    {
        Player.player.HandleBouns();
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].amount = Player.player.getStat(stats[i].name);
        }
        updateStats();
    }


    [Serializable]
    public class Stat
    {
        public string name;
        public string displayName;
        public Color color;
        public float amount;
        public Sprite icon;
    }
}
