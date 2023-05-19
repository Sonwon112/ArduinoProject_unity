using System.IO.Ports;
using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    // COM9로 연결되는 아두이노 보드 포스 설정    
    SerialPort mPort = new SerialPort("COM9",9600);
    string mdata = null;
    CarState carState = new CarState();

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
        if(carState.getIsStartUp){
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Debug.Log(rotateAngle);
            this.transform.Translate(Vector3.forward*vertical*speed*Time.smoothDeltaTime);
            if(horizontal < 0){
                this.transform.Rotate(rotatePoint[0].transform.up,rotateAngle*vertical*horizontal*rotateAngle*Time.smoothDeltaTime);
            }else if(horizontal > 0){
                this.transform.Rotate(rotatePoint[1].transform.up,rotateAngle*vertical*horizontal*rotateAngle*Time.smoothDeltaTime);
            }
        }
        



        
        try{
            if(mPort.IsOpen){
                mdata = mPort.ReadLine();
                // Debug.Log(mdata);
                string[] splitData = mdata.split(",");
                carState.setGear(splitData[0]);
                carState.setHandleRotation(int.Parse(splitData[1]));
                carState.setIsAcceleration(int.Parse(splitData[2]));
                carState.setIsBreak(int.Parse(splitData[3]));
                carState.setIsStartUp(int.Parse(splitData[4]));
                mPort.ReadTimeout = 30;
            }
        }catch(Exception e){
            Debug.Log(e);
        }

    }
}

class CarState{
    private string currGear = "C";
    private int handleRotation = "0";
    private boolean isAcceleration = false;
    private boolean isBreak = false;
    private boolean isStartUp = false;

    public void setGear(string gear){
        currGear = gear;
    }
    public void setHandleRotation(int handleRotation){
        this.handleRotation = handleRotation;
    }
    public void setIsAcceleration(boolean isAcceleration){
        this.isAcceleration = isAcceleration;
    }
    public void setIsBreak(boolean isBreak){
        this.isBreak = isBreak;
    }
    public void setIsStartUp(boolean isStartUp){
        this.isStartUp = isStartUp;
    }

    public string getGear(){
        retrun currGear;
    }
    public int getHandleRotation(){
        retrun handleRotation;
    }
    public boolean getIsAcceleration(){
        retrun isAcceleration;
    }
    public boolean getIsBreak(){
        retrun isBreak;
    }
    public boolean getIsStartUp(){
        retrun isStartUp;
    }

}
