using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatPanel : MonoBehaviour
{
    public List<Stat> stats;
    public GameObject statPrefab;

    private void Start()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            GameObject newStat = Instantiate(statPrefab, transform);
            newStat.SetActive(true);
            newStat.transform.GetChild(0).GetComponent<Image>().sprite = stats[i].icon;
            TextMeshProUGUI name = newStat.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI amount = newStat.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            name.SetText(stats[i].displayName);
            name.color = stats[i].color;
            amount.SetText(stats[i].amount.ToString());
            amount.color = stats[i].color;
            newStat.transform.localPosition = Vector3.zero + Vector3.down * 10 * i + Vector3.up * 44.7f;

        }
    }

    void updateStats()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            Transform temp = transform.GetChild(i);
            TextMeshProUGUI name = temp.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI amount = temp.GetChild(2).GetComponent<TextMeshProUGUI>();
            name.SetText(stats[i].displayName);
            amount.SetText(stats[i].amount.ToString());
            name.ForceMeshUpdate();
            amount.ForceMeshUpdate();
        }
    }

    public void Update()
    {
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
