using UnityEngine;
using System.Collections;

public class Parent : MonoBehaviour {
    public ClientSideObject parent { get; private set; }

    public void SetParent(ClientSideObject parent)
    {
        this.parent = parent;
    }
}
