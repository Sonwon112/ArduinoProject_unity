using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextSelected : MonoBehaviour
{
    public GameObject portConnectManager;
    public int index;
    // Start is called before the first frame update
    void Start()
    {
        portConnectManager = GameObject.Find("StartCanvas").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.IsPointerOverGameObject() == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                portConnectManager.GetComponent<OpensSerialPort>().selectedIndex = index;
            }
        }
    }
}
