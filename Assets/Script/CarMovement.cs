using System.IO.Ports;
using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    
    SerialPort mPort = new SerialPort("COM9",9600);
    string mdata = null;
    private string currGear;
    private int handleRotation;
    public float speed = 1f;
    public float rotateAngle = 10f;
    float maxAngle = 45f;

    public GameObject[] wheel;
    public GameObject[] rotatePoint;

    // Start is called before the first frame update
    void Start()
    {
        mPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        // Debug.Log(rotateAngle);
        this.transform.Translate(Vector3.forward*vertical*speed*Time.smoothDeltaTime);
        if(horizontal < 0){
            this.transform.Rotate(rotatePoint[0].transform.up,rotateAngle*vertical*horizontal*rotateAngle*Time.smoothDeltaTime);
        }else if(horizontal > 0){
            this.transform.Rotate(rotatePoint[1].transform.up,rotateAngle*vertical*horizontal*rotateAngle*Time.smoothDeltaTime);
        }
        
        try{
            if(mPort.IsOpen){
                mdata = mPort.ReadLine();
                Debug.Log(mdata);
                mPort.ReadTimeout = 30;
            }
        }catch(Exception e){
            Debug.Log(e);
        }

    }
}
