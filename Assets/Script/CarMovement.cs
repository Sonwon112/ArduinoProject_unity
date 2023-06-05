using System.IO.Ports;
using System;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    // COM9로 연결되는 아두이노 보드 포스 설정    
    SerialPort mPort = new SerialPort("COM5",9600);
    string mdata = null;
    CarState carState = new CarState();

    private float speed = 1f;
    private float minSpeed = 0f;
    private float maxSpeed = 0f;
    private int movedDirection = 1;

    public float rotationAngle = 0f;


    public GameObject[] wheels;
    public GameObject[] rotatePoint;
    public GameObject handle;



    // Start is called before the first frame update
    void Start()
    {
        mPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if(carState.getIsStartUp()){
            switch (carState.getGear())
            {
                case "N":
                    setCarSpeed(0f, 20f, 1);
                    break;
                case "1":
                    setCarSpeed(0f, 20f, 1);
                    break;
                case "2":
                    setCarSpeed(20f, 40f, 1);
                    break;
                case "3":
                    setCarSpeed(40f, 60f, 1);
                    break;
                case "4":
                    setCarSpeed(60f, 80f, 1);
                    break;
                case "5":
                    setCarSpeed(80f, 100f, 1);
                    break;
                case "R":
                    setCarSpeed(0f, 20f, -1);
                    break;
            }


            if (carState.getIsAcceleration())
            {
                if (speed <= maxSpeed) speed += 0.1f;                
            }else if (carState.getIsBreak())
            {
                if (speed >= minSpeed) speed -= 0.1f;
            }
            this.transform.Translate(this.transform.forward * speed * Time.smoothDeltaTime*movedDirection);

            rotationAngle = carState.getHandleRotation();
            handle.gameObject.transform.eulerAngles = new Vector3(rotationAngle, 0f, 0f);
            for(int i = 0; i  < wheels.Length; i++)
            {
                wheels[i].gameObject.transform.eulerAngles = new Vector3(0f, rotationAngle/16, 0f);
            }
            int isMove = speed > 0f ? 1 : 0;
            if (rotationAngle < 0f)
            { 
                this.transform.Rotate(rotatePoint[0].transform.up * rotationAngle / 16 * isMove* Time.smoothDeltaTime);
            }
            else
            {
                this.transform.Rotate(rotatePoint[1].transform.up * rotationAngle / 16 * isMove * Time.smoothDeltaTime);
            }

        }
         
        try{
            if(mPort.IsOpen){
                mdata = mPort.ReadLine();
                Debug.Log(mdata);
                // 0 : 기어 1 : 핸들 회전값 2 : 엑셀을 밟았는지 3 : 브레이크를 밟았는지 4 : 시동이 걸렸는지
                string[] splitData = mdata.Split(",");
                carState.setGear(splitData[0]);
                carState.setHandleRotation(int.Parse(splitData[1])*-1);
                carState.setIsAcceleration(splitData[2].Equals("0") ? false : true);
                carState.setIsBreak(splitData[3].Equals("0") ? false : true);
                carState.setIsStartUp(splitData[4].Equals("0") ?  false : true);
                mPort.ReadTimeout = 30;
            }
        }catch(Exception e){
            Debug.Log(e);
        }

    }

    void setCarSpeed(float minSpeed, float maxSpeed, int movedDirection)
    {
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;
        this.movedDirection = movedDirection;
    }

    private void OnApplicationQuit()
    {
        mPort.Close();
    }
}

class CarState{
    private string currGear = "C";
    private int handleRotation = 0;
    private bool isAcceleration = false;
    private bool isBreak = false;
    private bool isStartUp = false;

    public void setGear(string gear){
        currGear = gear;
    }
    public void setHandleRotation(int handleRotation){
        this.handleRotation = handleRotation;
    }
    public void setIsAcceleration(bool isAcceleration){
        this.isAcceleration = isAcceleration;
    }
    public void setIsBreak(bool isBreak){
        this.isBreak = isBreak;
    }
    public void setIsStartUp(bool isStartUp){
        this.isStartUp = isStartUp;
    }

    public string getGear(){
        return currGear;
    }
    public int getHandleRotation(){
        return handleRotation;
    }
    public bool getIsAcceleration(){
        return isAcceleration;
    }
    public bool getIsBreak(){
        return isBreak;
    }
    public bool getIsStartUp(){
        return isStartUp;
    }

}
