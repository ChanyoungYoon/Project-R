using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField]
    int poolSize = 5;

    void Awake() {
        InitializePool(poolSize);
        Global.fxManager = this;
    }

    private void InitializePool(int size){
        GameObject instance;
        transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < size-1; i++)
        {
            instance = Instantiate(transform.GetChild(0).gameObject);
            instance.transform.SetParent(transform);
            instance.SetActive(false);
        }
    }

    private IEnumerator PlayCoroutine(GameObject effect, string name, Vector3 position, float rotation, Vector3 scale, bool flipX){
        Animator animator = effect.transform.GetChild(0).gameObject.GetComponent<Animator>();

        effect.SetActive(true);
        effect.transform.SetPositionAndRotation(position,Quaternion.Euler(0,0,rotation));
        effect.transform.localScale = scale;
        animator.gameObject.GetComponent<SpriteRenderer>().flipX = flipX;
        
        animator.Play(name,-1);
        
        yield return new WaitForSecondsRealtime(animator.GetCurrentAnimatorStateInfo(0).length);

        effect.SetActive(false);
    }

    public void Play(string name, Vector3 position) => Play(name,position,0,Vector3.one,false);
    public void Play(string name, Vector3 position, bool flipX) => Play(name,position,0,Vector3.one,flipX);
    public void Play(string name, Vector3 position,float rotation,Vector3 scale,bool flipX){
        for (int i = 0; i < transform.childCount; i++)
        {
            if(!transform.GetChild(i).gameObject.activeInHierarchy){
                StartCoroutine(PlayCoroutine(transform.GetChild(i).gameObject,name,position,rotation,scale,flipX));
                return;
            }
        }
        Debug.Log("no more effect objects");
    }
    
}
