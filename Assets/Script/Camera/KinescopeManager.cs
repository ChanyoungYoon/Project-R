using UnityEngine;

[DefaultExecutionOrder(500)]
public class KinescopeManager : MonoBehaviour
{
    public float pixelsPerUnit = 16;
    public float pixelsPerPixel = 4;
    public int padding = 8;
    public int renderTextureScale = 1;

    private float _pixelsPerPixel=0;
    private Vector2 _screenRes;

    private float mainCameraSizeRatio;

    private VirtualTransformFollower virtualTransformFollower;
    private Camera mainCamera;
    private Camera kinescopeCamera;
    private GameObject monitor;
    private Vector3 monitorScale;
    private Vector3 monitorPosition;
    private RenderTexture renderTexture;
    private Texture2D monitorTexture;
    public Material renderTextureMaterial;

    void Awake(){
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        kinescopeCamera = gameObject.GetComponent<Camera>();
        monitor = gameObject.transform.Find("Monitor").gameObject;

        Global.pixelsPerPixel = pixelsPerPixel;
        Global.mainCamera = mainCamera;
        Global.kinescopeCamera = kinescopeCamera;
        Global.ScreenToWorldPosition = ScreenToWorldPosition;
        Global.WorldToScreenPosition = WorldToScreenPosition;
        
        pixelsPerUnit = Global.pixelsPerUnit;
    }

    void Update(){
        CheckForUpdate();

        Graphics.ConvertTexture(renderTexture,monitorTexture);
        renderTextureMaterial.mainTexture = monitorTexture;
        MonitorOffset();
    }

    void CheckForUpdate(){
        if( pixelsPerPixel!=_pixelsPerPixel || _screenRes.x != Screen.width || _screenRes.y != Screen.height ) {
            _pixelsPerPixel = pixelsPerPixel;
            _screenRes.x = Screen.width;
            _screenRes.y = Screen.height;
            UpdateCamera();
        }
    }

    [ContextMenu("Update Camera")]
    private void UpdateCamera(){
        _pixelsPerPixel = pixelsPerPixel;
        _screenRes.Set(Screen.width,Screen.height);

        kinescopeCamera.orthographicSize = ((Screen.height/pixelsPerPixel)/pixelsPerUnit)*0.5f;
        kinescopeCamera.aspect = (float)Screen.width/(float)Screen.height;

        mainCameraSizeRatio = 1/Mathf.Sqrt(2);
        mainCamera.orthographicSize = ((kinescopeCamera.orthographicSize)+(padding/pixelsPerUnit))*mainCameraSizeRatio;
        mainCamera.aspect = (Screen.width+(padding*pixelsPerPixel*2))/((Screen.height+(padding*pixelsPerPixel*2))*mainCameraSizeRatio);

        if(renderTexture!=null) {mainCamera.targetTexture = null; Destroy(renderTexture);}
        renderTexture = new RenderTexture((Mathf.CeilToInt(Screen.width/pixelsPerPixel)+padding*2)*renderTextureScale,(Mathf.CeilToInt(Screen.height/pixelsPerPixel)+padding*2)*renderTextureScale,24,RenderTextureFormat.ARGBHalf,RenderTextureReadWrite.Linear);
        renderTexture.filterMode = FilterMode.Point;
        renderTexture.Create();

        monitorTexture = new Texture2D(Mathf.CeilToInt(Screen.width/pixelsPerPixel)+padding*2,Mathf.CeilToInt(Screen.height/pixelsPerPixel)+padding*2);
        monitorTexture.filterMode = FilterMode.Point;
        Graphics.ConvertTexture(renderTexture,monitorTexture);
        renderTextureMaterial.mainTexture = renderTexture as Texture;

        monitor.GetComponent<MeshRenderer>().material = renderTextureMaterial;
        monitorScale.x = (((Screen.width+padding*pixelsPerPixel*2)/pixelsPerPixel)/pixelsPerUnit);
        monitorScale.y = (((Screen.height+padding*pixelsPerPixel*2)/pixelsPerPixel)/pixelsPerUnit);
        monitorScale.z = 1;
        monitor.transform.localScale = monitorScale;

        mainCamera.targetTexture = renderTexture;
        kinescopeCamera.targetDisplay = 0;
    }

    private void MonitorOffset(){
        if(virtualTransformFollower==null) virtualTransformFollower = mainCamera.gameObject.GetComponent<VirtualTransformFollower>();
        monitorPosition = Vector3.zero;
        monitorPosition.Set(virtualTransformFollower.offsetVector.x,virtualTransformFollower.offsetVector.y,1);
        monitor.transform.localPosition = monitorPosition;
    }

   Vector3 ScreenToWorldPosition(Vector3 screenPos){
        Vector3 worldPos = Vector3.zero;
        //screenPos to unit scale
        screenPos.x = ((screenPos.x/Screen.width)-0.5f)*kinescopeCamera.orthographicSize*2*kinescopeCamera.aspect;
        screenPos.y = ((screenPos.y/Screen.height)-0.5f)*kinescopeCamera.orthographicSize*2;
        //worldPos to Main Camera's center
        worldPos = mainCamera.transform.position;
        worldPos.z += worldPos.y;
        worldPos.y = 0;
        //add screenPos
        worldPos.x += screenPos.x;
        worldPos.z += screenPos.y;
        return worldPos;
    }

    Vector3 WorldToScreenPosition(Vector3 worldPos){
        Vector3 screenPos = Vector3.zero;
        //worldPos to y=0
        worldPos.y = worldPos.z+worldPos.y-mainCamera.transform.position.y;
        worldPos.z = 0;
        //worldPos to offset from Main Camera
        worldPos.x -= mainCamera.transform.position.x;
        worldPos.y -= mainCamera.transform.position.z;

        screenPos.x = ((worldPos.x/(kinescopeCamera.orthographicSize*2*kinescopeCamera.aspect))+0.5f)*Screen.width;
        screenPos.y = ((worldPos.y/(kinescopeCamera.orthographicSize*2))+0.5f)*Screen.height;
        return screenPos;
    }
}

