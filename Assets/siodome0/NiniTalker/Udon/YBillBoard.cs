
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class YBillBoard : UdonSharpBehaviour
{
    public Transform Self;

    void Update()
    {
        var lookPosition = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
        lookPosition.y = Self.position.y;
        transform.LookAt(lookPosition);
    }
}
