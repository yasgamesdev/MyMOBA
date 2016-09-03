using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HPBarScript : MonoBehaviour {
    ClientSideObject parent;
    float offset;
    Slider slider;

    public void Initialize(ClientSideObject parent, ushort maxhp, float offset, byte faction)
    {
        this.parent = parent;
        this.offset = offset;
        slider = GetComponent<Slider>();
        slider.maxValue = maxhp;

        if (faction == 0)
        {
            transform.FindChild("Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.blue;
        }
        else if (faction == 1)
        {
            transform.FindChild("Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.red;
        }
        else
        {
            transform.FindChild("Fill Area/Fill").gameObject.GetComponent<Image>().color = Color.yellow;
        }

        SetPositionAndValue();
    }

    void Update()
    {
        SetPositionAndValue();
    }

    void SetPositionAndValue()
    {
        slider.value = parent.GetCurrentHP();

        Vector3 worldPos = parent.GetPrefabTransform().position;
        worldPos.y += offset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        transform.position = screenPos;
    }

    public void SetMaxHP(ushort maxhp)
    {
        slider.maxValue = maxhp;
    }
}
