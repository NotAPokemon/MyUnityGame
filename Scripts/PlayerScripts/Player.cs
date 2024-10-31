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

    public MouseMove mouse;

    public List<BaseItem> items;
    public List<BaseItem> handItems;
    public int Num = -1;




    protected override void Start()
    {
        base.Start();
        FindObjectOfType<MapGen>().noiseData.seed = UnityEngine.Random.Range(10,100000);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal") * 10;
        float z = Input.GetAxis("Vertical") * 10;
        move = transform.right * x + transform.forward * z;
        HandleJump();
        move.y = yVelocity;
        Controller.Move(move * speed * Time.deltaTime);
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

    void HandlePickup()
    {
        BaseItem[] itemsOnGround = FindObjectOfType<EntityManager>().GetComponentsInChildren<BaseItem>();
        for (int i = 0; i < itemsOnGround.Length; i++)
        {
            if (itemsOnGround[i] is not NullItem)
            {
                if (Vector3.Distance(transform.position, itemsOnGround[i].transform.position) <= 1)
                {
                    if (items.Count < 30)
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
                            bool added = false;
                            for (int j = 0; j < items.Count; j++)
                            {
                                if (items[j] is NullItem)
                                {
                                    items[j] = itemsOnGround[i];
                                    if (j == Num)
                                    {
                                        setHeldItem(itemsOnGround[i], 1);
                                    }
                                    added = true;
                                }
                            }
                            if (!added)
                            {
                                items.Add(itemsOnGround[i]);
                            }
                        }

                    }
                }
            }
            
        }
    }

    public void dropItem()
    {
        handItems[0].transform.SetParent(Manager.transform);
        handItems[0].transform.position = transform.position + transform.forward * 2;
        handItems[0].State = ItemState.Ground;
        handItems.RemoveAt(0);
        items.RemoveAt(Num);
        Num = -1;
    }

    void HandleKeyInput()
    {

        for (int i = 0; i < Mathf.Min(9, items.Count); i++)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + (i + 1))))
            {
                if (i != Num)
                {
                    setHeldItem(items[i], 0);
                    Num = i;
                }
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (Num != -1 && handItems[0] is not NullItem)
            {
                dropItem();
            }
        }
    }

    
    protected override void Update()
    {
        base.Update();
        if (locked) { return; }
        HandleMovement();
        HandlePickup();
        HandleKeyInput();
    }
}
