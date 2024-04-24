# Unity를 활용한 자동차 시뮬레이션 게임

Created: April 23, 2024 2:54 PM
github: https://github.com/Sonwon112/ArduinoProject_unity.git

### 개요

- 명칭 : 자동차 시뮬레이션 by 표식이( IoT 팀 프로젝트 명칭)
- 기간 : 2023.04 ~ 2023.06
- 제작 동기 : IoT 강의 중 팀 프로젝트를 진행하였을 때 원래 기능은 해당 기어의 기어 단수 정보만을 제공하였던 프로젝트였는데 실제 자동차에 적용하여 테스트하기 어려웠기에 시뮬레이션 
프로그램을 제작하여 테스트해보자하여 개발하게 되었습니다.

----
### 설계 및 고려 사항

1. IoT ( 아두이노 )와 어떻게 연결할 것이며 어떻게 데이터를 주고 받을 것인가?
    
    → 아두이노와는 Serial 통신 즉 USB 케이블로 연결할 것이며 데이터의 경우 숫자와 문자를 
    ;(세미콜론)이라는 구분자로 구분하여 데이터를 주고 받을 것입니다.
    
2. 어디까지 실제와 유사하게 구현 할 것인가?
    
    →핸들의 회전 차체 회전, 실제 자동차가 도로를 달리는 느낌을 나게 하고 싶다, 또한 수동 차량을 대상으로 표식이라는 장치를 만들었기에 기어 단 수에 따라 속도의 제한을 두어야 합니다.
    
----
### 진행과정

1. 클래스 구조 설계 및 알고리즘 작성
    
    
    |     클래스 명 |                                                             역할 |
    | --- | --- |
    | CarState | Scene에 존재하는 자동차에 대한 정보를 담을 객체 |
    | CarMovement | CarState에 맞춰 Scene에 배치한 자동차의 움직임을 조절하는 코드 |
    | OpenSerialPort | 프로그램 실행 시 컴퓨터의 Serial 포트를 연결하고 연결된 포트를 읽어와 시작화면에서 연결된 포트 리스트를 표시하는 코드 |
    | TextSelected | 포트 리스트에서 몇 번째에 위치한 Item에 선택 되었는지 알려주기 위해 필요한 컴포넌트 |
    
2. 모델링 및 리소스 확보
    
    ![Untitled](https://github.com/Sonwon112/ArduinoProject_unity/assets/67799705/401e5bac-eef4-4f76-b2e3-1a3ba616a62f)
    
    ![%EC%8A%A4%ED%81%AC%EB%A6%B0%EC%83%B7_2024-04-23_202220](https://github.com/Sonwon112/ArduinoProject_unity/assets/67799705/9c7e620c-7678-497d-9a0b-08fb142e3b34)
    
    
    - 현재 수동차량이 가장많은 군용 차량을 대상으로 두고 프로젝트를 진행하여서 샘플 시뮬레이션 차량도 군용 차량과 가장 유사한 차량 모델을 선택하여 사용하였습니다.
    - 빠른 시일 내에 구현을 해야 했기에 도시 풍경보다는 긴 도로 모델을 선택하여 사용 하였습니다.
    - 시뮬레이션을 목적으로 구현하였기에 차가 달릴 때, 정지할 때, 시동을 걸 때 소리가 달라야 한다고 판단하였기에 AudioSource를 사용하였습니다.
    
    ※ 자세한 정보는
     👨‍💻”[깃허브 링크](https://github.com/Sonwon112/ArduinoProject_unity.git)”를 참조해 주세요. ※ 
    
    |         시점 |                               사운드 |
    | --- | --- |
    | 시동을 걸 때 | 시동을 거는 소리 car_start_engine.wav |
    | 차가 시동이 걸려있을 때,
    속도가 5이상을 유지하고 있을 때 | 시동이 걸려 있을 때 car_runningSound.wav volume = 1 |
    | 속도가 느려 졌을 때 | car_runningSound.wav를 재생하지만 volume = 0.5 |
   
4. 자동차 전진 회전 구현
    - 우선 핸들이 아직 구현이 되지 않았기 때문에 키보드의 w,a,s,d를 활용화여 전진과 후진, 그리고 회전을 구현 하였습니다.
    
    ```csharp
    	// 전진과 후진 구현
    	// speed 전진과 후진 상관없이 자동차의 이동 속도
    	// movedDirection 차의 이동 방향 
    	//  -> 실제 자동차의 경우 기어 변속을 통해 후진을 결정하기 때문에 
    	//     movedDirection이라는 변수를 따로 두어 구현
    	this.transform.Translate(Vector3.forward * speed * Time.smoothDeltaTime * movedDirection);
    ```
    
    - 회전의 경우 실제 차는 회전 축이 차 내부가 아닌 외부에 존재하였던 점이 회전을 구현하면서 어려웠습니다.
   <p align = "center">
      <img src="https://github.com/Sonwon112/ArduinoProject_unity/assets/67799705/c3875620-4ef8-447b-899e-da3db3781483" width=40%">
      <img src="https://github.com/Sonwon112/ArduinoProject_unity/assets/67799705/442159fe-2db2-4a77-bc88-d653ac99c595" width=40%">
   </p>

    현재 모델의 앞바퀴가 회전하여 차가 이동하는 것이 아니기 때문에 좌측 이미지 처럼 정확하게 구현하기 어려울 뿐더러, 아두이노로부터 데이터를 받아오는 속도에 맞춰 회전각을 조정해야 하기에 정확한 연산보다는 정보가 들어오는 속도에 맞춰 실시간으로 처리되는 것이 중요하다고 판단하여 정확한 회전축을 구하는 것이 아닌 차량의 후면에 두어 마치 실제 차량이 회전하는 듯한 느낌을 주게끔 구현 하였습니다.
    
    ```csharp
    // rotatePoint 위 사진에서 차량의 후반에 빈 GameObject를 추가하여 차체의 회전축으로 설정
    // 그리고 현재 차가 전진 혹은 후진을 하고 있는 경우에만 회전해야하므로
    // isMove라는 변수에 1 또는 0이란 값을 부여하여 움직이는 경우에만 회전을 실시하게 됩니다.
    this.transform.Rotate(rotatePoint.transform.up * rotationAngle / 15 * isMove * Time.smoothDeltaTime * movedDirection);
    ```
    
4. 아두이노와 연동하여 Serial 통신을 주고 받으며 차체 회전 및 전진 구현
    - 우선 Unity에서 IO 포트가 동작이 되게끔 하기 위하여 코드상 IO 포트도 열어야하지하지만 Unity Project Setting에서 .NET Framework로 설정 해줘야 한다.
        
        ![Untitled 3](https://github.com/Sonwon112/ArduinoProject_unity/assets/67799705/828e6fa8-569d-49ed-90c4-604e548d29ec)
        
    
    ```csharp
    // 포트가 열려있고 서로 연결이 되어 있다면 읽어오는 것을 대기하게 되고 읽게 되면 
    // ;(세미콜론)을 구분자로 하여 split하고 해당 데이터를 carState 객체에 저장하여
    // carMovement에서 정보들을 얻어와 속도와 핸들의 각도를 조정하게 됩니다.
    
    if (mPort.IsOpen)
    {
        mdata = mPort.ReadLine();
        //Debug.Log(mdata);
        // 0 : 기어 1 : 핸들 회전값 2 : 시동이 걸렸는지 
        //3 : 브레이크를 밟았는지 4 : 엑셀을 밟았는지
        string[] splitData = mdata.Split(",");
        carState.setGear(splitData[0]);
        carState.setHandleRotation(int.Parse(splitData[1]) * -1);
        carState.setIsAcceleration(splitData[4].Equals("0") ? true : false);
        carState.setIsBreak(splitData[3].Equals("0") ? true : false);
        carState.setIsStartUp(splitData[2].Equals("0") ? true : false);
        mPort.ReadTimeout = 30;
    }
    ```
    
5. 사용자가 포트를 선택할 수 있게 끔 구현
    - 프로젝트 Build 후 아두이노를 연결하였는데 아두이노의 연결 포트가 변경하는 일이 발생하였다. Build 후에도 Port 선택을 하게끔 수정하여 변동에도 대응 할 수 있게 수정하였습니다.
    
  ![Untitled 4](https://github.com/Sonwon112/ArduinoProject_unity/assets/67799705/63c9cbc0-cf19-4b05-b4ae-e00d66c23d93)
    

```csharp
// SerialPort의 이름들을 얻어와 배열에 저장
string[] ports = SerialPort.GetPortNames();

// scrollView에 텍스트 박스를 추가하여 선택할 수 있게끔 객체를 추가
for(int i = 0; i <ports.Length; i++)
{
    listContent.text = ports[i];
    listContent.GetComponent<TextSelected>().index = i;
    var index = Instantiate(listContent, new Vector3(0,yValue,0),Quaternion.identity);
    index.transform.SetParent(GameObject.Find("Content").transform);
    yValue -= 200;
}

// TextSelected.cs
// 해당 텍스트 박스 객체 선택시 ScrollView에 해당 정보를 전달
if(EventSystem.current.IsPointerOverGameObject() != false)
{
    if (Input.GetMouseButtonDown(0))
    {
        portConnectManager.GetComponent<OpensSerialPort>().selectedIndex = index;
        this.GetComponent<Text>().text = "o" + this.GetComponent<Text>().text;
    }
}

```
----
### 성과 및 결과 영상

[https://drive.google.com/file/d/16y2tNQkCqFh8Fsg2ux1-dBhWnt2mtqty/view?usp=sharing](https://drive.google.com/file/d/16y2tNQkCqFh8Fsg2ux1-dBhWnt2mtqty/view?usp=sharing)

----
### 진행하면서 어려웠던 점 및 이슈

- 진행 과정에서 설명했 듯 차량의 실제 조향 방식을 핸들의 실시간 처리를 위해서 어떻게 처리를 해야할까에 대해 고민을 꽤 오랜 시간 하였습니다. 시뮬레이션은 실제와 근사하게 표시해야겠다라는 강박을 가지고 있었는데 실시간으로 들어오는 데이터에 맞게끔 동작을 수행을 할려면 실시간을 맞추는게 맞다고 배우기도 하였고, 맞다고 판단이 되어서 정확한 연산은 포기하였지만, 그렇다면 어떻게 실제와 비슷하게 표현을 해야할까를 고민하였고 여러 위치를 축으로 두었을 가장 비슷한 위치를 찾아내는 과정이 어려웠습니다. 이를 통해 최적화를 위해서는 너무 현실감 있게 구현하는것은 포기하고 어느정도의 합의점을 가져야한다는 것을 깨달을 수 있었습니다
- 발생했던 이슈중 가장 기억에 남는 것은 port를 선택하게끔 하게 하는 것이 어려웠습니다. 이전에는 Unity 내에서만 작업을 하고 Port는 Unity 에디터에서만 수정해서 변경을 하였었는데 이번에는 Build과정을 거쳐서 최종적인 프로젝트를 내는 것이였기 때문에 Build 후에 포트가 변경되어서 포트를 수정하기 위해 코드에서 수정을 하는 것이 너무 비효율적이라고 판단되었고 이를 해결 하기위해 UI를 생성하여 사용자의 입장해서 연결된 Port를 선택할 수 있게끔 구현하여서 코드 수정 없이 포트 변경이 가능하게끔 수정하였습니다. 이를 통해 프로그램을 제작할 때는 **“개발자의 입장에서만 생각하는 것이 아닌” “사용자의 입장에서 어떻게 하면 편리할지, 이점은 불편하지 않을지, 사용자가 선택할수 있는영역이여야하는지”**를 설계단계에서 부터 UI로 어떻게 표현해야할지를 고려해야 겠다고 생각하였습니다.
