using TMPro;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public GameObject statPrefab;

    public StatPanel statPanel;

    GameObject main;

    bool made = false;
    bool uiOpen;

    float timeSinceActive = 1.5f;

    void Start()
    {
        main = transform.GetChild(0).gameObject;   
    }


    void make()
    {
        made = true;
        for (int i = 0; i < statPanel.stats.Count; i++)
        {
            GameObject newStat = Instantiate(statPrefab, main.transform);
            newStat.SetActive(true);
            newStat.transform.localPosition = Vector3.zero + Vector3.down * 10f * i;
        }
        
    }


    void updateScreen()
    {
        for (int i = 1; i < main.transform.childCount; i++)
        {
            main.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().SetText(statPanel.stats[i - 1].displayName);
            main.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(statPanel.stats[i - 1].color.r, statPanel.stats[i - 1].color.g, statPanel.stats[i - 1].color.b);
            main.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().SetText((Mathf.Round(Player.player.getHiddenStat(statPanel.stats[i-1].name) * 10)/10).ToString());
            main.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().color = new Color(statPanel.stats[i - 1].color.r, statPanel.stats[i - 1].color.g, statPanel.stats[i - 1].color.b);
            main.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().SetText("+" +(Mathf.Round(Player.player.getBaseStat(statPanel.stats[i - 1].name) * 10) / 10).ToString());
            main.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>().color = new Color(statPanel.stats[i - 1].color.r, statPanel.stats[i - 1].color.g, statPanel.stats[i - 1].color.b);
            Player.player.setHiddenStat(statPanel.stats[i - 1].name, Player.player.getBaseStat(statPanel.stats[i - 1].name));
        }
    }

    public void open()
    {
        int level = Player.player.level;
        updateScreen();
        Player.player.health = Player.player.MaxHealth;
        Player.player.mana = Player.player.MaxMana;
        if (!uiOpen)
        {
            uiOpen = true;
            timeSinceActive = 0;
            main.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!made)
        {
            make();
        }
        timeSinceActive += Time.deltaTime;
        if (timeSinceActive >= 5)
        {
            main.SetActive(false);
            uiOpen = false;
        }
    }
}
