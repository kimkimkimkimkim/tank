﻿using UnityEngine;

public class TankMovement : MonoBehaviour {
    public int m_PlayerNumber = 1;
    public float m_Speed = 0.2f;
    public float m_TurnSpeed = 180f;
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;
    public float m_PitchRange = 0.2f;

    private string m_MovementAxisName;
    private string m_TurnAxisName;
    private Rigidbody m_Rigidbody;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    private float m_OriginalPitch;
    private float m_HorizontalValue;
    private float m_VerticalValue;
    private FixedJoystick m_joystick;


    private void Awake() {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_joystick = GameObject.Find("JoystickCanvas/FixedJoystick").GetComponent<FixedJoystick>();
    }


    private void OnEnable() {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable() {
        m_Rigidbody.isKinematic = true;
    }


    private void Start() {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    private void Update() {
        // Store the player's input and make sure the audio for the engine is playing.
        m_HorizontalValue = m_joystick.Horizontal;
        m_VerticalValue = m_joystick.Vertical;


        EngineAudio();
    }


    private void EngineAudio() {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f) {
            if (m_MovementAudio.clip == m_EngineDriving) {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        } else {
            if (m_MovementAudio.clip == m_EngineIdling) {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate() {
        // Move and turn the tank.
        Move();
    }


    private void Move() {
        // Adjust the position of the tank based on the player's input.
        Vector3 vector = new Vector3(m_HorizontalValue, 0, m_VerticalValue);
        vector = Quaternion.Euler(0, 60, 0) * vector;
        vector = vector.normalized; //長さ1に正規化

        m_Rigidbody.MovePosition(m_Rigidbody.position + vector * m_Speed);

        if (m_HorizontalValue == 0 && m_VerticalValue == 0) return;
        //m_Rigidbody.transform.rotation = Quaternion.LookRotation(vector); //向きを変更する
    }
}
