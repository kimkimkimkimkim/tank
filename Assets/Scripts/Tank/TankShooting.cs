using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TankShooting : MonoBehaviour {
    public int m_PlayerNumber = 1;              // Used to identify the different players.
    public Rigidbody m_Shell;                   // Prefab of the shell.
    public Transform m_FireTransform;           // A child of the tank where the shells are spawned.
    public Slider m_AimSlider;                  // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;            // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;                // Audio that plays when each shot is fired.
    public float m_MinLaunchForce = 15f;        // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce = 30f;        // The force given to the shell if the fire button is held for the max charge time.
    public float m_MaxChargeTime = 0.75f;       // How long the shell can charge for before it is fired at max force.
    public float m_LaunchForce = 20f;
    public float range = 10f;
    public MovementType movementType = MovementType.Player;
    [HideInInspector] public GameObject playerTank; //プレイヤーのタンクオブジェクト


    private string m_FireButton;                // The input axis that is used for launching shells.
    private float m_CurrentLaunchForce;         // The force that will be given to the shell when the fire button is released.
    private float m_ChargeSpeed;                // How fast the launch force increases, based on the max charge time.
    private bool m_Fired = false;                       // Whether or not the shell has been launched with this button press.
    private FireJoystick m_Joystick;
    private Rigidbody m_Rigidbody;
    private float distance;

    private void Awake() {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Joystick = GameObject.Find("JoystickCanvas/FireJoystick").GetComponent<FireJoystick>();
    }


    private void OnEnable() {
        // When the tank is turned on, reset the launch force and the UI
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start() {
        // The fire axis is based on the player number.
        m_FireButton = "Fire" + m_PlayerNumber;

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }


    private void Update() {
        if (movementType == MovementType.Player) {
            //Player
            if (m_Joystick.isFire) {
                m_Joystick.isFire = false;
                Fire();
            }
        } else {
            //CPU
            distance = (m_Rigidbody.position - playerTank.transform.position).magnitude;
            if (distance <= range) {
                TurnFire();
            }
        }
    }

    private void FixedUpdate() {
        if (movementType != MovementType.Player) return;
        Turn();
    }

    private void Turn() {
        Vector3 vector = m_Joystick.Direction;
        vector = new Vector3(vector.x, 0, vector.y);
        vector = Quaternion.Euler(0, 60, 0) * vector;

        if (vector == Vector3.zero) return;
        m_Rigidbody.transform.rotation = Quaternion.LookRotation(vector); //向きを変更する
    }


    private void Fire() {
        if (m_Fired) return;
        m_Fired = true;
        StartCoroutine(CanFire());
        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = m_LaunchForce * m_FireTransform.forward; ;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

    }

    IEnumerator CanFire() {
        yield return new WaitForSeconds(1f);
        m_Fired = false;
    }

    //ターゲットの方向を向いて攻撃
    private void TurnFire() {
        m_Rigidbody.transform.LookAt(playerTank.transform);
        Fire();
    }
}
