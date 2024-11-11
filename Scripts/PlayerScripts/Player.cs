using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseEntity
{

    public float speed = 1.0f;
    public float jumpHight = 1.0f;
    Vector3 move;
    float gravity = 9.81f * 2;
    private float yVelocity = 0f;
    public bool locked;
    public GameObject inventory;
    public GameObject Hand;
    public GameObject Manager;

    private float hiddenMaxHealth;
    private float hiddendamage;
    private float hiddenregen;
    private float hiddenmana;
    private float hiddenmanareg;
    private float hiddenspeed;


    public MouseMove mouse;

    public float regen;
    public float manaRegen;

    public List<BaseItem> items;
    public List<BaseItem> handItems;
    public List<BaseArmor> armors;
    public int Num = -1;
    public static Player player;
    float timeFromLastReg = 0f;







    protected override void Start()
    {
        base.Start();
        FindObjectOfType<MapGen>().noiseData.seed = UnityEngine.Random.Range(10, 100000);
        hiddenMaxHealth = MaxHealth;
        hiddendamage = damageAmount;
        lastHealth = health;
        hiddenregen = regen;
        hiddenmana = MaxMana;
        hiddenmanareg = manaRegen;
        hiddenspeed = speed;
        player = this;
        for (int i = 0; i < 4; i++)
        {
            armors.Add(makeNullItem());
        }
        for (int i = 0; i < 30 - getItemAmount(); i++)
        {
            items.Add(makeNullItem());
        }
    }

    public float getStat(string name)
    {
        switch (name)
        {
            case "MAX_HEALTH":
                return MaxHealth;
            case "DAMAGE":
                if (handItems.Count < 1)
                {
                    return damageAmount;
                }
                return damageAmount + handItems[0].damage;
            case "HEALTH_REGENERATION":
                return regen;
            case "DEFENSE":
                return defense;
            case "MAX_MANA":
                return MaxMana;
            case "MANA_REGENERATION":
                return manaRegen;
            case "SPEED":
                return speed;
            default: break;
        }
        return -1;
    }

    public static NullItem makeNullItem()
    {
        NullItem filler = Instantiate(Inventory.nullItem);
        filler.transform.SetParent(Inventory.fillers.transform);
        return filler;
    }

    public void swapItems(int a, int b)
    {
        if (a < 0 || b < 0)
        {
            Debug.LogError("Indices must be non-negative.");
            return;
        }

        int max = Mathf.Max(a, b);
        while (items.Count <= max)
        {
            NullItem filler = makeNullItem();
            items.Add(filler);
        }

        BaseItem temp = items[a];
        items[a] = items[b];
        items[b] = temp;
        if (a == Num)
        {
            setHeldItem(items[a], 0);
        }

        if (b == Num)
        {
            setHeldItem(items[b], 0);
        }
    }


    public void swarpArmor(int armorIndex, int itemIndex)
    {
        if (armorIndex == -1)
        {
            Debug.LogError("Unknown armor type");
            return;
        }
        BaseArmor temp = armors[armorIndex];
        temp.equiped = false;
        armors[armorIndex] =  (BaseArmor)items[itemIndex];
        items[itemIndex] = temp;
        armors[armorIndex].State = ItemState.Worn;
        items[itemIndex].State = ItemState.Stored;
        if (itemIndex == Num)
        {
            setHeldItem(items[itemIndex], 0);
        }

    }


    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal") * 10;
        float z = Input.GetAxis("Vertical") * 10;
        move = transform.right * x + transform.forward * z;
        HandleJump();
        move.y = yVelocity;
        Controller.Move(move * speed * (mana/MaxMana) * Time.deltaTime);
    }

    void HandleJump()
    {
        bool isGrounded = Physics.CheckSphere(GroundCheck.transform.position, groundDistance, ground);

        if (isGrounded && Input.GetKey(KeyCode.Space))
        {
            yVelocity = Mathf.Sqrt(jumpHight * 2.5f * gravity);
        }
        else if (!isGrounded)
        {
            yVelocity -= gravity * Time.deltaTime;
        }
    }


    protected override void handleDeath()
    {
        locked = true;
    }

    public int getItemAmount()
    {
        int count = 0;
        for (int i = 0; i < items.Count; i ++)
        {
            if (items[i] is not NullItem)
            {
                count++;
            }
        }
        return count;
    }

    public void storeItem(BaseItem item)
    {
        item.transform.SetParent(inventory.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.Euler(0, 0, 0);
        item.State = ItemState.Stored;
    }

    public void setHeldItem(BaseItem item, int mode)
    {
        try
        {
            if (mode == 0)
            {
                storeItem(handItems[0]);
            }
            handItems[0] = item;
        }
        catch
        {
            handItems.Add(item);
        }
        handItems[0].transform.SetParent(Hand.transform);
        handItems[0].transform.localPosition = Vector3.zero;
        handItems[0].State = ItemState.Hand;
        handItems[0].gameObject.SetActive(true);
    }

    public void addItem(BaseItem item)
    {
        bool added = false;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] is NullItem)
            {
                items[i] = item;
                if (i == Num)
                {
                    setHeldItem(item, 1);
                }
                added = true;
                break;
            }
        }
        if (!added)
        {
            items.Add(item);
        }
    }

    void HandlePickup()
    {
        BaseItem[] itemsOnGround = FindObjectOfType<EntityManager>().GetComponentsInChildren<BaseItem>();
        for (int i = 0; i < itemsOnGround.Length; i++)
        {
            if (Vector3.Distance(transform.position, itemsOnGround[i].transform.position) <= 2 && itemsOnGround[i] is not NullItem && getItemAmount() < 30)
            {
                storeItem(itemsOnGround[i]);
                if (Num == -1)
                {
                    items.Insert(0, itemsOnGround[i]);
                    setHeldItem(itemsOnGround[i], 1);
                    Num = 0;
                }
                else
                {
                    addItem(itemsOnGround[i]);
                }
            }

            
        }
    }

    public void dropItem()
    {
        handItems[0].transform.SetParent(Manager.transform);
        handItems[0].transform.position = transform.position + transform.forward * 3;
        handItems[0].State = ItemState.Ground;
        setHeldItem(makeNullItem(), 1);
        items[Num] = makeNullItem();
    }

    void HandleKeyInput()
    {

        for (int i = 0; i < Mathf.Min(9, items.Count); i++)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + (i + 1))) && i != Num)
            {
                setHeldItem(items[i], 0);
                Num = i;
            }
        }
        if (Input.GetKey(KeyCode.Q) && Num != -1 && handItems[0] is not NullItem)
        {
            dropItem();
        } else if (Input.GetKeyDown(KeyCode.X))
        {
            try
            {
                Destroy(FindObjectOfType<MagicReader>().transform.GetChild(0).gameObject);
            } catch { }
        }
    }

    public void HandleBouns()
    {
        float def = 0f;
        float hp = 0f;
        float dmg = 0f;
        float rg = 0f;
        float m = 0f;
        float mrg = 0f;
        float s = 0f;
        for (int i = 0;i < armors.Count;i++)
        {
            def += armors[i].armorBonus;
            hp += armors[i].healthBonus;
            dmg += armors[i].damageBonus;
            rg += armors[i].regenBonus;
            m += armors[i].manaBonus;
            mrg += armors[i].manaRegenBonus;
            s += armors[i].speedBonus;
        }
        MaxMana = m + hiddenmana;
        manaRegen = mrg + hiddenmanareg;
        MaxHealth = hp + hiddenMaxHealth;
        damageAmount = dmg + hiddendamage;
        regen = rg + hiddenregen;
        defense = def;
        speed = s + hiddenspeed;
    }

    void HandleRegen()
    {
        if (timeFromLastReg >= 2)
        {
            health += regen;
            mana += manaRegen;
            timeFromLastReg = 0f;
        }
        timeFromLastReg += Time.deltaTime;
        health = Mathf.Clamp(health, 0f, MaxHealth);
        mana = Mathf.Clamp(mana, 0f, MaxMana);
    }

    


    protected override void Update()
    {
        base.Update();
        HandleRegen();
        HandlePickup();
        HandleBouns();
        if (locked) { return; }
        HandleMovement();
        HandleKeyInput();
        
    }
}
