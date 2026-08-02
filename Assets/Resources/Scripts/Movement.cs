using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Movement : MonoBehaviour
{
    public float speed = 1;
    private XROrigin rig;
    public float gravity = -9.81f;
    public float centerOffset = 0.01f;
    public LayerMask groundLayer;
    public float groundDiv = 1.0f;

    public float additionalHeight = 0.2f;
    public float fallingSpeed;
    public XRNode inputSource;
    private Vector2 inputAxis;
    private CharacterController character;

    public MeshRenderer sphereEvents;
    public bool Warning = false;
    public float sinv;

    [Tooltip("Giro con los controles")]
    public bool enableTurn;
    public XRNode rightInputSource;
    Vector2 r_inputAxis;
    public float m_turnAmount = 45.0f;
    /// <summary>
    /// hl
    /// </summary>
    public bool turnSnap = false;
    float timeToTurn = 0;
    public float cdToTurn = 0.2f;

    public UnityEvent quemadura;

    private void Awake()
    {
        StartCoroutine(TransitionLvlOut());

    }
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XROrigin>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis);

        InputDevice r_device = InputDevices.GetDeviceAtXRNode(rightInputSource);
        r_device.TryGetFeatureValue(CommonUsages.primary2DAxis, out r_inputAxis);

        if (enableTurn) SnapTurn();

    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        Quaternion headYaw = Quaternion.Euler(0, rig.Camera.transform.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(inputAxis.x, 0, inputAxis.y);
        if (character.enabled) character.Move(Time.deltaTime * speed * direction);

        bool isGrounded = CheckIfGrounded();
        if (isGrounded)        
            fallingSpeed = 0;
        else      
            fallingSpeed += gravity * Time.deltaTime;

        if (character.enabled) character.Move(fallingSpeed * Time.deltaTime * Vector3.up);
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.CameraInOriginSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.Camera.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }
    bool CheckIfGrounded()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLenght = character.center.y + centerOffset;
        bool hasHit = Physics.SphereCast(rayStart, character.radius / groundDiv, Vector3.down, out RaycastHit hitInfo, rayLenght, groundLayer);
        return hasHit;
    }

    void SnapTurn()
    {
        if (!turnSnap)
        {
            transform.eulerAngles += new Vector3(0, r_inputAxis.x * m_turnAmount * Time.deltaTime, 0);
            return;
        }

        if (timeToTurn < cdToTurn) timeToTurn += Time.deltaTime;

        if (Mathf.Abs(r_inputAxis.x) > 0.85f && timeToTurn >= cdToTurn)
        {
            timeToTurn = 0;
            float turn = r_inputAxis.x > 0 ? 1 : -1;
            //transform.eulerAngles += new Vector3(0, m_turnAmount * turn, 0);
            transform.RotateAround(rig.Camera.transform.position, Vector3.up, m_turnAmount * turn);
        }
    }

    public void llamarTransitionIn()
    {
        StartCoroutine(TransitionLvlIn());
    }
    public void llamarTransitionOut()
    {
        StartCoroutine(TransitionLvlOut());
    }

    IEnumerator TransitionLvlIn()
    {
        sphereEvents.gameObject.SetActive(true);
        //in
        Material Materialcolor = sphereEvents.material;
        Materialcolor.color = new Color(0, 0, 0, 0);
        //black
        for (float i = 0; i <= 1; i += 0.2f)
        {            
            Color mat = Materialcolor.color;
            mat.a = i;
            Materialcolor.color = mat;
            yield return new WaitForSeconds(0.1f);
        }
        Materialcolor.color = Color.black;
    }

    IEnumerator TransitionLvlOut()
    {
        sphereEvents.gameObject.SetActive(true);
        Material Materialcolor = sphereEvents.material;
        Materialcolor.color = Color.black;
        yield return new WaitForSeconds(0.2f);
        //out
        for (float i = 1; i >= 0; i -= 0.2f)
        {
            Color mat = Materialcolor.color;
            mat.a = i;
            Materialcolor.color = mat;
            yield return new WaitForSeconds(0.1f);
        }
        Color mf = Materialcolor.color;
        mf.a = 0;
        Materialcolor.color = mf;
        sphereEvents.gameObject.SetActive(false);
    }
    public void Gravedad(float valor)
    {
        gravity = valor;
        
    }
    public void Velocidad(float velocidad)
    {
        speed = velocidad;
    }

    public float VelocidadCaida()
    {
        return fallingSpeed;
    }

}
