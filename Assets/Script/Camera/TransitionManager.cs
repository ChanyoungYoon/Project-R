using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType{
    DownToUp,
    UpToDown,
    LeftToRight,
    RightToLeft,
}

public class TransitionManager : MonoBehaviour
{
    private GameObject black;

    void Awake() {
        black = transform.Find("Black").gameObject;
        black.SetActive(false);
        Global.Transition = Transition;
    }

    private void Transition(TransitionType type,float duration){
        StartCoroutine(TransitionCoroutine(type,duration));
    }

    private IEnumerator TransitionCoroutine(TransitionType type,float duration){
        black.SetActive(true);
        float t = 0;
        float t2 = t;
        int dirX = (type==TransitionType.LeftToRight)?1:(type==TransitionType.RightToLeft)?-1:0;
        int dirY = (type==TransitionType.DownToUp)?1:(type==TransitionType.UpToDown)?-1:0;
        float cameraHeight = transform.parent.GetComponent<Camera>().orthographicSize*2 + 0.1f;
        float cameraWidth = transform.parent.GetComponent<Camera>().aspect*cameraHeight + 0.1f;
        Vector3 position = Vector3.zero;
        Vector3 scale = Vector3.zero;
        bool reverse = false;
        while(t>0 || !reverse){
            t2=t*t*t;
            position.Set(Mathf.Lerp(-cameraWidth*0.5f*dirX,0,t2),Mathf.Lerp(-cameraHeight*0.5f*dirY,0,t2),black.transform.localPosition.z);
            scale.Set(Mathf.Lerp(cameraWidth*(1-Mathf.Abs(dirX)),cameraWidth,t2),Mathf.Lerp(cameraHeight*(1-Mathf.Abs(dirY)),cameraHeight,t2),1);
            black.transform.localPosition = position;
            black.transform.localScale = scale;
            yield return null;
            if(t>1) {
                yield return new WaitForSecondsRealtime(0.15f);
                reverse = true;
                dirX *= -1;
                dirY *= -1;
            }
            t+=(Time.deltaTime/duration)*(reverse?-1:1);
        }
    }
}
