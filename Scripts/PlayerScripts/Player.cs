using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    void HandlePickup()
    {
        BaseItem[] itemsOnGround = FindObjectOfType<EntityManager>().GetComponentsInChildren<BaseItem>();
        for (int i = 0; i < itemsOnGround.Length; i++)
        {
            if (Vector3.Distance(transform.position, itemsOnGround[i].transform.position) <= 1)
            {
                if (items.Count < 30)
                {
                    itemsOnGround[i].State = ItemState.Stored;
                    items.Add(itemsOnGround[i]);
                    itemsOnGround[i].transform.SetParent(inventory.transform);
                    itemsOnGround[i].transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    void HandleKeyInput()
    {

        for (int i = 0; i < Mathf.Min(10, items.Count + 1); i++)
        {
            if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)))
            {
                if (i - 1 != Num)
                {
                    try
                    {
                        handItems[0].transform.SetParent(inventory.transform);
                        handItems[0].transform.localPosition = Vector3.zero;
                        handItems[0].State = ItemState.Stored;
                        handItems[0] = items[i - 1];
                    }
                    catch
                    {
                        handItems.Add(items[i - 1]);
                    }
                    items[i - 1].State = ItemState.Hand;
                    items[i - 1].transform.gameObject.SetActive(true);
                    items[i - 1].transform.SetParent(Hand.transform, false);
                    items[i - 1].transform.localPosition = Vector3.zero;
                    Num = i - 1;
                }
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
