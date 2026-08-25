using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public XRNode inputSource;
    public Animator handAnim;



    public float gripValue;
    public float triggerValue;
    public bool clickAxis;
    public float timeReset = 2.5f;
    float timer;

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

        device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out clickAxis);        
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);

        UpdateHandAnimation(device);

        if (clickAxis)
        {
            timer += Time.deltaTime;
            if (timer >= timeReset)
            {
                SceneManager.LoadSceneAsync(0);
                //timer = 0;
            }
        }

    }

}
