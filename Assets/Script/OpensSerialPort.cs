using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;
//using UnityEditor.Experimental;
using UnityEditor;

public class OpensSerialPort : MonoBehaviour
{
    public GameObject car;
    public GameObject stateCanvas;
    public Text listContent;
    public int selectedIndex = -1;
    public Button connectBtn;


    string[] ports = SerialPort.GetPortNames();

    SerialPort myPort;

    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    private void init()
    {
        int yValue = 0;
        if(ports.Length != 0)
        {
            for(int i = 0; i <ports.Length; i++)
            {
                listContent.text = ports[i];
                listContent.GetComponent<TextSelected>().index = i;
                var index = Instantiate(listContent, new Vector3(0,yValue,0),Quaternion.identity);
                index.transform.SetParent(GameObject.Find("Content").transform);
                yValue -= 200;
            }
        }
        else
        {
            listContent.text = "연결된 포트가 없습니다";
            var index = Instantiate(listContent, new Vector3(0, yValue, 0), Quaternion.identity);
            index.transform.SetParent(GameObject.Find("Content").transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        connectBtn.onClick.AddListener(connectPort);
    }

    void connectPort()
    {
        if(selectedIndex != -1)
        {
            myPort = new SerialPort(ports[selectedIndex], 9600);
            car.GetComponent<CarMovement>().mPort = myPort;
            stateCanvas.SetActive(true);
            this.gameObject.SetActive(false);
        }
        else
        {
            //bool result = EditorUtility.DisplayDialog("", "포트를 선택하세요", "OK");

        }
    }
}
