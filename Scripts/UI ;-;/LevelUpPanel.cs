using TMPro;
using UnityEngine;

public class LevelUpPanel : MonoBehaviour
{
    public GameObject statPrefab;

    public StatPanel statPanel;

    GameObject main;

    bool made = false;
    bool uiOpen;

    Player player;

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
            newStat.transform.localPosition = Vector3.zero + Vector3.down * 7.5f * i;
        }
        player = Player.player;
    }




    void updateScreen()
    {
        for (int i = 1; i < main.transform.childCount; i++)
        {

            TextMeshProUGUI name = main.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI current = main.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI add = main.transform.GetChild(i).GetChild(2).GetComponent<TextMeshProUGUI>();

            StatPanel.Stat stat = statPanel.stats[i - 1];

            name.SetText(stat.displayName);
            name.color = Calculator.cloneColor(stat.color); ;

            current.SetText(Calculator.Round(player.getHiddenStat(stat.name), 1) + "");
            current.color = Calculator.cloneColor(stat.color); ;

            add.SetText("+" +Calculator.Round(player.getBaseStat(stat.name), 1));
            add.color = Calculator.cloneColor(stat.color); ;
            player.setHiddenStat(stat.name, player.getBaseStat(stat.name));
        }
    }

    public void open()
    {
        updateScreen();
        player.health = player.MaxHealth;
        player.mana = player.MaxMana;
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
        if (timeSinceActive >= 3)
        {
            main.SetActive(false);
            uiOpen = false;
        }
    }
}
