
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class CameraArea : UdonSharpBehaviour
{
    private GameObject parent;
    private GameObject sub;
    private Camera subCamera;
    void Start()
    {
        parent = transform.parent.gameObject;
        sub = parent.transform.Find("Camera").gameObject;
        subCamera = sub.GetComponent<Camera>();
    }

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player != Networking.LocalPlayer) return;
        subCamera.enabled = !subCamera.enabled;
    }
    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if (player != Networking.LocalPlayer) return;
        subCamera.enabled = true;
    }
    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player != Networking.LocalPlayer) return;
        subCamera.enabled = !subCamera.enabled;
    }
}
