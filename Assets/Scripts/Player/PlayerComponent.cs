using UnityEngine;

public class PlayerComponent : MonoBehaviour
{
    //private
    private Rigidbody2D bodyRigidbody;
    private Rigidbody2D L_handRigidbody;
    private Rigidbody2D R_handRigidbody;
    private Rigidbody2D L_footRigidbody;
    private Rigidbody2D R_footRigidbody;
    private bool isLeftFoot = false;
    private bool isLeftHand = false;
    private bool isRightFoot = false;
    private bool isRightHand = false;
    private bool isJumped = false;
    private PlayerData playerData;

    //public
    public GameObject body;
    public GameObject L_hand;
    public GameObject R_hand;
    public GameObject L_foot;
    public GameObject R_foot;
    public float freezeFactor = 1f;
    public float maxSpeed = 11f; 
    public long jumpCoolTimeMs = 500;

    //function
    void Start()
    {
        // Get Rigidbody components
        if (body != null) bodyRigidbody = body.GetComponent<Rigidbody2D>();
        if (L_hand != null) L_handRigidbody = L_hand.GetComponent<Rigidbody2D>();
        if (R_hand != null) R_handRigidbody = R_hand.GetComponent<Rigidbody2D>();
        if (L_foot != null) L_footRigidbody = L_foot.GetComponent<Rigidbody2D>();
        if (R_foot != null) R_footRigidbody = R_foot.GetComponent<Rigidbody2D>();

        SetPlayerData();
    }
    
    void Update()
    {
        if (L_handRigidbody != null && R_handRigidbody != null && bodyRigidbody != null)
        {
            HandleInput();
        }
    }
    
    private void HandleInput()
    {
        // A key - L_hand 왼쪽으로 addforce
        if (Input.GetKey(KeyCode.A))
        {
            L_handRigidbody.AddForce(Vector3.left * playerData.forceStrength, ForceMode2D.Impulse);
            // 최고속도 제한 (freezeFactor와 비례)
            float currentMaxSpeed = maxSpeed * freezeFactor;
            if (L_handRigidbody.velocity.magnitude > currentMaxSpeed)
            {
                L_handRigidbody.velocity = L_handRigidbody.velocity.normalized * currentMaxSpeed;
            }
        }
        
        // D key - R_hand 오른쪽으로 addforce
        if (Input.GetKey(KeyCode.D))
        {
            R_handRigidbody.AddForce(Vector3.right * playerData.forceStrength, ForceMode2D.Impulse);
            // 최고속도 제한 (freezeFactor와 비례)
            float currentMaxSpeed = maxSpeed * freezeFactor;
            if (R_handRigidbody.velocity.magnitude > currentMaxSpeed)
            {
                R_handRigidbody.velocity = R_handRigidbody.velocity.normalized * currentMaxSpeed;
            }
        }
        
        // Space key - body 위쪽으로 addforce
        if (Input.GetKeyDown(KeyCode.Space) && !isJumped)
        {
            bodyRigidbody.AddForce(Vector3.up * playerData.forceStrength * playerData.jumpStrength, ForceMode2D.Impulse);
            isJumped = true;

            Timer jumpTimer = new Timer();
            jumpTimer.SetTimer(ETimerType.GameTime, false, false, jumpCoolTimeMs, 1000, actionOnExpire: (t) =>
            {
                isJumped = false;
            });
            TimeManager.Instance.ResisterTimer(jumpTimer);
        }

        //왼손
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!isLeftHand)
                 L_handRigidbody.bodyType = RigidbodyType2D.Static;
            else
                L_handRigidbody.bodyType = RigidbodyType2D.Dynamic;

            turnState(ref isLeftHand);
        }

        //오른손
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!isRightHand)
                R_handRigidbody.bodyType = RigidbodyType2D.Static;
            else
                R_handRigidbody.bodyType = RigidbodyType2D.Dynamic;

            turnState(ref isRightHand);
        }

        //왼발
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isLeftFoot)
                L_footRigidbody.bodyType = RigidbodyType2D.Static;
            else
                L_footRigidbody.bodyType = RigidbodyType2D.Dynamic;

            turnState(ref isLeftFoot);
        }

        //오른발
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isRightFoot)
                R_footRigidbody.bodyType = RigidbodyType2D.Static;
            else
                R_footRigidbody.bodyType = RigidbodyType2D.Dynamic;

            turnState(ref isRightFoot);
        }
    }

    private void turnState(ref bool state)
    {
        state = !state;
    }

    private void SetPlayerData()
    {
        playerData = PlayerDataManager.Instance.GetData();
    }

    public void SetFreezeFactor(float value)
    {
        freezeFactor = Mathf.Clamp01(value);
    }
}