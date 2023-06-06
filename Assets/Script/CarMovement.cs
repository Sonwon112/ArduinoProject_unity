using System.IO.Ports;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour
{
    // COM9로 연결되는 아두이노 보드 포스 설정    
    SerialPort mPort = new SerialPort("COM5",9600);
    string mdata = null;
    CarState carState = new CarState();

    public float speed = 1f;
    private float minSpeed = 0f;
    private float maxSpeed = 0f;
    private int movedDirection = 1;

    public float rotationAngle = 0f;


    public GameObject[] wheels;
    public GameObject rotatePoint;
    public GameObject handle;

    public Text HandleAngle;
    public Text speedTxt;
    public Text gearTxt;

    public bool testMode = false;
    public float handleRotation = 0f;
    public bool audioStart = false;

    AudioSource carStartAudio;
    //public float handleTest = 0f;



    // Start is called before the first frame update
    void Start()
    {
        mPort.Open();
        carState.setPrevtime(Time.time);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Replay")==1)
        {
            SceneManager.LoadScene(0);
        }
        if (!testMode)
        {
            speedTxt.text = "" + (int)speed;
            gearTxt.text = carState.getGear();
            HandleAngle.text = "" + carState.getHandleRotation();
            if (carState.getIsStartUp())
            {
                switch (carState.getGear())
                {
                    case "N":
                        setCarSpeed(0f, 8f, 1);
                        break;
                    case "1":
                        setCarSpeed(0f, 8f, 1);
                        break;
                    case "2":
                        setCarSpeed(0f, 16f, 1);
                        break;
                    case "3":
                        setCarSpeed(0f, 22f, 1);
                        break;
                    case "4":
                        setCarSpeed(0f, 28f, 1);
                        break;
                    case "5":
                        setCarSpeed(0f, 30f, 1);
                        break;
                    case "R":
                        setCarSpeed(0f, 8f, -1);
                        break;
                }


                if (carState.getIsAcceleration())
                {
                    if (speed <= maxSpeed) speed += 0.01f;
                }
                if (carState.getIsBreak())
                {
                    if (speed >= minSpeed) speed -= 0.01f;
                }
                this.transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime * movedDirection);

                rotationAngle = carState.getHandleRotation();

                handle.gameObject.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(handle.transform.localEulerAngles.x, handleRotation, 0.1f), 0, 0);
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].gameObject.transform.localEulerAngles = new Vector3(0f, CarState.remap(rotationAngle, -900, 900, -45, 45), 0f);
                }
                int isMove = speed > 0f ? 1 : 0;
                this.transform.Rotate(rotatePoint.transform.up * rotationAngle / 15 * isMove * Time.smoothDeltaTime);

            }

            try
            {
                if (mPort.IsOpen)
                {
                    mdata = mPort.ReadLine();
                    //Debug.Log(mdata);
                    // 0 : 기어 1 : 핸들 회전값 2 : 엑셀을 밟았는지 3 : 브레이크를 밟았는지 4 : 시동이 걸렸는지
                    string[] splitData = mdata.Split(",");
                    carState.setGear(splitData[0]);
                    carState.setHandleRotation(int.Parse(splitData[1]) * -1);
                    carState.setIsAcceleration(splitData[4].Equals("0") ? true : false);
                    carState.setIsBreak(splitData[3].Equals("0") ? true : false);
                    carState.setIsStartUp(splitData[2].Equals("0") ? true : false);
                    mPort.ReadTimeout = 30;
                }
            }
            catch (Exception e)
            {
                //Debug.Log(e);
            }
        }
        else
        {
            if (audioStart)
            {
                carStartAudio = this.GetComponent<AudioSource>();
                carStartAudio.Play();
            }
                
            handle.gameObject.transform.localEulerAngles = new Vector3(Mathf.LerpAngle(handle.transform.localEulerAngles.x, handleRotation, 0.1f), 0, 0);
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
    private string currGear = "N";
    private float handleRotation = 0;
    private bool isAcceleration = false;
    private bool isBreak = false;
    private bool isStartUp = false;
    private float prevTime = 0;

    public void setGear(string gear){
        currGear = gear;
    }
    public void setHandleRotation(int handleRotation){
        this.handleRotation = remap(handleRotation,-30f,30f,-900f,900f);
    }
    public void setIsAcceleration(bool isAcceleration){
        this.isAcceleration = isAcceleration;
    }
    public void setIsBreak(bool isBreak){
        this.isBreak = isBreak;
    }
    public void setIsStartUp(bool isStartUp){
        float currTime = Time.time;

        if (currTime - prevTime > 0.5)
        {
            if (isStartUp)
            {
                this.isStartUp = !this.isStartUp;
                prevTime = currTime;
            }
        }
    }
    public void setPrevtime(float prevTime)
    {
        this.prevTime = prevTime;
    }

    public string getGear(){
        return currGear;
    }
    public float getHandleRotation(){
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
    public static float remap(float val, float in1, float in2, float out1, float out2)  //리맵하는 함수
    {
        return out1 + (val - in1) * (out2 - out1) / (in2 - in1);
    }

}
