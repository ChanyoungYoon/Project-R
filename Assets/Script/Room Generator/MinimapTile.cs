using UnityEngine;

public class MinimapTile : MonoBehaviour
{
    [SerializeField]
    private GameObject upLink;
    [SerializeField]
    private GameObject downLink;
    [SerializeField]
    private GameObject rightLink;
    [SerializeField]
    private GameObject leftLink;
    
    public void LinkUp(bool connected){
        upLink.SetActive(connected);
    }
    public void LinkDown(bool connected){
        downLink.SetActive(connected);
    }
    public void LinkRight(bool connected){
        rightLink.SetActive(connected);
    }
    public void LinkLeft(bool connected){
        leftLink.SetActive(connected);
    }

    #region Dev Tools
    [ContextMenu("Assign Link By Name")]
    private void AssignLink(){
        upLink = transform.Find("link_U").gameObject;
        downLink = transform.Find("link_D").gameObject;
        rightLink = transform.Find("link_R").gameObject;
        leftLink = transform.Find("link_L").gameObject;
    }
    #endregion
}
