using System.Collections;
using UnityEngine;
//using UnityEngine.UI;

[DefaultExecutionOrder(550)]
//by 조한음 : 카메라는 항상 Virtual Camera Transform을 정중앙에 두려고함. 카메라를 움직이지 말고 Virtual Camera Transform을 움직일 것
[ExecuteInEditMode]
public class VirtualTransformFollower : MonoBehaviour
{
    public Transform virtualCameraTransform;
    //public Text text;
    
    private float pixelsPerUnit = 16;
    public float yHeight = 10;

    [HideInInspector]
    public Vector2 offsetVector = Vector2.zero;
    private Vector3 position = Vector3.zero;
    private Vector2 offset = Vector2.zero;

    void Awake() {
        if(virtualCameraTransform == null) print("Virtual Camera Transform not assigned");

        Global.virtualCameraTransform = virtualCameraTransform.gameObject;
        Global.ScreenShake = ScreenShake;

        pixelsPerUnit = Global.pixelsPerUnit;
    }
    void Update() {
        //set camera position to target virtual transform
        position = virtualCameraTransform.position;
        position.y = yHeight;
        position.z -= yHeight - virtualCameraTransform.position.y;
        position.x += offset.x;
        position.z += offset.y;
        //backup position before snapping
        offsetVector.Set(position.x,position.z);
        //snap camera position by pixels per unit
        position.x = (Mathf.Round(position.x*pixelsPerUnit)/pixelsPerUnit);
        position.y = (Mathf.Round(position.y*pixelsPerUnit)/pixelsPerUnit);
        position.z = (Mathf.Round(position.z*pixelsPerUnit)/pixelsPerUnit);
        //get offset vector
        offsetVector.Set(position.x-offsetVector.x,position.z-offsetVector.y);
        //assign position
        transform.position = position;

        //fps counter
        /*if(a>0){
            a--;
            b+=Time.deltaTime;
        }else{
            a=100;
            b /= a;
            text.text = (1/b).ToString();
        }*/
    }

    //small rumble = mag = 0.05, freq = 2, len = 0.5
    private void ScreenShake(float magnitude,float frequency,float length){
        StartCoroutine(ScreenShakeCoroutine(magnitude,frequency,length));
    }
   
    private IEnumerator ScreenShakeCoroutine(float magnitude,float frequency,float length){
        float scale = 0.05f;
        float scaleSquared = 1;
        float t=0;
        float random = Random.value;
        float random2 = Random.value;
        bool decay = false;
        Vector2 currentPos = Vector2.zero;

        while(scale>0){
            if(!decay) {
                scale+=Time.deltaTime*8;
                if(scale>1) decay = true;
            }
            else scale -= Time.deltaTime*(1/length);
            offset -= currentPos;
            scaleSquared = scale*scale;
            currentPos.x = (Mathf.Sin(t)+(Mathf.PerlinNoise(t,0)*0.5f))*scaleSquared*magnitude*(Mathf.Round(random)*2-1);
            currentPos.y = (Mathf.Sin(t*1.2f)+(Mathf.PerlinNoise(t,10)*0.5f))*scaleSquared*magnitude*(Mathf.Round(random2)*2-1);
            offset += currentPos;
            t+=Time.deltaTime*20*frequency;
            yield return null;
        }
        offset -= currentPos;
    }
}
