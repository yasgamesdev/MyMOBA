using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPMPBarScript : MonoBehaviour {
    ClientSideObject parent;
    float offset;
    Slider hpSlider, mpSlider;

    public void Initialize(ClientSideObject parent, ushort maxhp, ushort maxmp, byte level, float offset, byte faction)
    {
        this.parent = parent;
        this.offset = offset;
        hpSlider = transform.FindChild("HP").GetComponent<Slider>();
        hpSlider.maxValue = maxhp;
        mpSlider = transform.FindChild("MP").GetComponent<Slider>();
        mpSlider.maxValue = maxmp;

        if (faction == 0)
        {
            transform.FindChild("HP/Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.blue;
        }
        else if (faction == 1)
        {
            transform.FindChild("HP/Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            transform.FindChild("HP/Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.yellow;
        }

        SetPositionAndValue();

        transform.FindChild("Text").gameObject.GetComponent<Text>().text = level.ToString();
    }

    void Update()
    {
        SetPositionAndValue();
    }

    void SetPositionAndValue()
    {
        hpSlider.value = parent.GetCurrentHP();
        mpSlider.value = ((Hero)parent).GetCurrentMP();

        Vector3 worldPos = parent.GetPrefabTransform().position;
        worldPos.y += offset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        transform.position = screenPos;
    }

    public void SetMaxHP(ushort maxhp)
    {
        hpSlider.maxValue = maxhp;
    }

    public void SetLevel(byte level)
    {
        transform.FindChild("Text").gameObject.GetComponent<Text>().text = level.ToString();
    }
}
