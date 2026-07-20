using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public XRNode inputSource;
    public Animator handAnim;

    public float gripValue;
    public float triggerValue;

    
    // Start is called before the first frame update
    enum HandGrab { none = 0, ball = 1, gun = 2 };
    void UpdateHandAnimation(InputDevice device)
    {       

        if (device.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
        {
            handAnim.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnim.SetFloat("Trigger", 0);
        }
        if (device.TryGetFeatureValue(CommonUsages.grip, out gripValue))
        {
            handAnim.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnim.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);

        UpdateHandAnimation(device);
    }

    

}
