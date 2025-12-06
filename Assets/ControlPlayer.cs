using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlPlayer : MonoBehaviour
{
    float speed, rotationAroundY;
    Animator anim;
    CharacterController controller;
    AnimatorStateInfo info;
    [SerializeField] public GameObject userMessage;
    [SerializeField] private GameObject shopUI;

    GameObject weapon;
    bool weaponIsActive = false;

    bool isTalking = false;
    bool itemToPickupNearBy = false;

    public bool shopIsDisplayed;

    GameObject objectToPickup;

    [Header("UI References")]
    [SerializeField] private GameObject pickupPanel;
    [SerializeField] private TextMeshProUGUI pickupText;

    [Header("Health Settings")]
    [Tooltip("Health value between 0 and 100.")] public int health = 50;


    public void IncreaseHealth(int amount) 
    { 
        health += amount; 
        if (health > 100) health = 100; print("Health:" + health); 
        GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health); 
    }




    public void displayShopUI()
    {
        shopUI.SetActive(true);
    }

    void Start()
    {
        weapon = GameObject.Find("playerWeapon").gameObject;
        weapon.SetActive(false);

        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        if (pickupPanel != null)
        {
            pickupPanel.SetActive(false);
            pickupText.text = "";
        }

        shopUI.SetActive(false);

        GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health);
    }

    public void DecreaseHealth(int amount) 
    { 
        health -= amount; 
        if (health <= 0) 
            health = 0; 
        GameObject.Find("healthBar").GetComponent<ManageBar>().SetValue(health); 
    }

    void Update()
    {
        if(!shopIsDisplayed)
        {
            if (isTalking) return;

            //if (Input.GetKeyDown(KeyCode.B)) 
            //GameObject.Find("shopSystem").GetComponent<ShopSystem>().Init();

            info = anim.GetCurrentAnimatorStateInfo(0);
            speed = Input.GetAxis("Vertical");
            rotationAroundY = Input.GetAxis("Horizontal");

            anim.SetFloat("speed", speed);
            speed *= 2.0f;

            transform.Rotate(0, rotationAroundY, 0);
            if (speed > 0)
                controller.Move(transform.forward * speed * Time.deltaTime * 2.0f);

            if (itemToPickupNearBy)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                    PickUpObject1();

                if (Input.GetKeyDown(KeyCode.N))
                {
                    pickupText.text = "";
                    pickupPanel.SetActive(false);
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                weaponIsActive = !weaponIsActive;

                if (weaponIsActive)
                    anim.SetTrigger("UseWeapon");
                else
                    anim.SetTrigger("PutWeaponBack");
            }


            if (info.IsName("UseWeapon"))
            {
                if (info.normalizedTime % 1.0f >= 0.5f)
                    weapon.SetActive(true);
            }
            else if (info.IsName("PutWeaponBack"))
            {
                if (info.normalizedTime % 1.0f >= 0.8f)
                    weapon.SetActive(false);
            }
            else
            {
                weapon.SetActive(weaponIsActive);
            }


            if (Input.GetButtonDown("Fire1")) 
            { 
                if (weaponIsActive) anim.SetTrigger("attackWithWeapon"); 
            
            }



        }

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.gameObject.name == "Diana" && !isTalking)
        {
            hit.collider.gameObject.GetComponent<DialogueSystem>().startDialogue();
            isTalking = true;
            anim.SetFloat("speed", 0);

            hit.collider.isTrigger = true;
            hit.collider.gameObject.GetComponent<BoxCollider>().size = new Vector3(2, 1, 2);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("itemToBeCollected"))
        {
            objectToPickup = other.gameObject;
            itemToPickupNearBy = true;

            PickUpObject1();
            PickUpObject2();
        }
        if (other.gameObject.name == "Shop")
        {
            shopIsDisplayed = true;

            anim.SetFloat("speed", 0); 
            displayShopUI(); 
            GameObject.Find("shopSystem").GetComponent<ShopSystem>().Init(); 
        }

    }

    private void OnTriggerExit(Collider other)
    {
        itemToPickupNearBy = false;

        if (pickupText != null)
            pickupText.text = "";

        if (pickupPanel != null)
            pickupPanel.SetActive(false);
    }


    void PickUpObject1()
    {
        var inventory = GetComponent<InventorySystem>();
        var itemComponent = objectToPickup.GetComponentInParent<ObjectToBeCollected>();
        if (itemComponent == null) return;

        var itemType = itemComponent.type;

        if (inventory.UpdateItem(itemType, 1))
        {
            Destroy(objectToPickup);
            itemToPickupNearBy = false;
            pickupText.text = "";
            pickupPanel.SetActive(false);

            userMessage.GetComponent<TextMeshProUGUI>().text = "";
            userMessage.SetActive(false);

            GameObject.Find("GameManager").GetComponent<QuestSystem>()
                .Notify(QuestSystem.possibleActions.acquire_a, itemComponent.item.name);
        }
        else
        {
            string message = "You cannot pick up this item, you have reached your limit.";
            pickupText.text = message;
            pickupPanel.SetActive(true);
        }
    }

    void PickUpObject2()
    {
        var itemComponent = objectToPickup.GetComponent<ObjectToBeCollected>();
        if (itemComponent == null) return;

        string article = itemComponent.item.article;
        string name = itemComponent.item.name;

        string message = $"You just found {article} {name}\nCollect? (Y/N)";
        pickupText.text = message;
        pickupPanel.SetActive(true);
    }

    public void EndTalking()
    {
        isTalking = false;
    }
}
