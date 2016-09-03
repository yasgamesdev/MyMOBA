using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatusScript : MonoBehaviour {
    Hero parent = null;
    Slider hpSlider, mpSlider;
    Text hpText, mpText, goldText;
    bool shopOpen;
    GameObject shop;
    List<GameObject> icons = new List<GameObject>();

    void Start()
    {
        shop = GameObject.Find("Shop");
        shopOpen = false;
        shop.SetActive(shopOpen);
    }

    public void Initialize(Hero parent)
    {
        this.parent = parent;
        hpSlider = transform.FindChild("HP").GetComponent<Slider>();
        hpSlider.maxValue = parent.GetMaxHP();
        mpSlider = transform.FindChild("MP").GetComponent<Slider>();
        mpSlider.maxValue = parent.GetMaxMP();
        hpText = transform.FindChild("HPText").GetComponent<Text>();
        mpText = transform.FindChild("MPText").GetComponent<Text>();
        goldText = transform.FindChild("Items/GoldText").GetComponent<Text>();

        if (parent.GetFaction() == 0)
        {
            transform.FindChild("HP/Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.blue;
        }
        else
        {
            transform.FindChild("HP/Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.red;
        }

        SetValue();
    }

    void Update () {
        if (parent != null)
        {
            SetValue();
        }

        if(Input.GetMouseButtonDown(2))
        {
            SetItemIcons(null);
        }
    }

    void SetValue()
    {
        hpSlider.value = parent.GetCurrentHP();
        mpSlider.value = parent.GetCurrentMP();

        hpText.text = hpSlider.value.ToString() + "/" + hpSlider.maxValue.ToString();
        mpText.text = mpSlider.value.ToString() + "/" + mpSlider.maxValue.ToString();

        goldText.text = parent.GetGold().ToString() + "G";
    }

    public void SetMaxHP(ushort maxhp)
    {
        hpSlider.maxValue = maxhp;
    }

    public void ShopButtonClick()
    {
        shopOpen = !shopOpen;
        shop.SetActive(shopOpen);
    }

    public void BuySwordButtonClick()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendBuyItem(1);
    }

    public void BuyEpicSwordButtonClick()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendBuyItem(2);
    }

    public void BuyHeartButtonClick()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendBuyItem(3);
    }

    public void BuyEpicHeartButtonClick()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendBuyItem(4);
    }

    public void SetItemIcons(Items items)
    {
        foreach(GameObject obj in icons)
        {
            Destroy(obj);
        }

        float originX = -80;
        float originY = 8;

        for(int i=0; i<items.items.Length; i++)
        {
            if(items.items[i].itemID != 0)
            {
                GameObject icon = null;
                if (items.items[i].itemID == 1)
                {
                    icon = GameObject.Instantiate(Resources.Load("Prefabs/Icons/SwordIcon"), GameObject.Find("Items").transform) as GameObject;
                }
                else if(items.items[i].itemID == 2)
                {
                    icon = GameObject.Instantiate(Resources.Load("Prefabs/Icons/EpicSwordIcon"), GameObject.Find("Items").transform) as GameObject;
                }
                else if (items.items[i].itemID == 3)
                {
                    icon = GameObject.Instantiate(Resources.Load("Prefabs/Icons/HeartIcon"), GameObject.Find("Items").transform) as GameObject;
                }
                else
                {
                    icon = GameObject.Instantiate(Resources.Load("Prefabs/Icons/EpicHeartIcon"), GameObject.Find("Items").transform) as GameObject;
                }
                icon.transform.localPosition = new Vector3(originX + i * 32, originY, 0);
                icons.Add(icon);
            }
        }
    }

    public void SellSlot0()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendSellItem(0);
    }

    public void SellSlot1()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendSellItem(1);
    }

    public void SellSlot2()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendSellItem(2);
    }

    public void SellSlot3()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendSellItem(3);
    }

    public void SellSlot4()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendSellItem(4);
    }

    public void SellSlot5()
    {
        GameObject.Find("Network").GetComponent<NetworkScript>().SendSellItem(5);
    }
}
