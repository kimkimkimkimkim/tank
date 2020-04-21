using System;
using UnityEngine;
using UnityEngine.AI;

public enum MovementType {
    Player,
    CPU1
}


[Serializable]
public class TankManager {
    public Color m_PlayerColor;
    public Transform m_SpawnPoint;
    [HideInInspector] public int m_PlayerNumber;
    [HideInInspector] public string m_ColoredPlayerText;
    [HideInInspector] public GameObject m_Instance;
    [HideInInspector] public GameObject playerTank; //プレイヤーのタンクオブジェクト
    [HideInInspector] public int m_Wins;


    private TankMovement m_Movement;
    private TankShooting m_Shooting;
    private NavMeshAgent m_NavMeshAgent;
    private GameObject m_CanvasGameObject;



    public void Setup() {
        m_Movement = m_Instance.GetComponent<TankMovement>();
        m_Shooting = m_Instance.GetComponent<TankShooting>();
        m_NavMeshAgent = m_Instance.GetComponent<NavMeshAgent>();
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        if (m_PlayerNumber == 1) {
            //Playerだったら
            m_Movement.movementType = MovementType.Player;
            m_Shooting.movementType = MovementType.Player;
            m_Instance.GetComponent<Cpu1Controller>().enabled = false;
        } else {
            m_Movement.movementType = MovementType.CPU1;
            m_Shooting.movementType = MovementType.CPU1;
            m_Instance.GetComponent<Cpu1Controller>().TargetObject = playerTank;
            m_Shooting.playerTank = playerTank;
        }

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].material.color = m_PlayerColor;
        }
    }


    public void DisableControl() {
        m_Movement.enabled = false;
        m_Shooting.enabled = false;
        m_NavMeshAgent.isStopped = true;

        m_CanvasGameObject.SetActive(false);
    }


    public void EnableControl() {
        m_Movement.enabled = true;
        m_Shooting.enabled = true;
        m_NavMeshAgent.isStopped = false;

        m_CanvasGameObject.SetActive(true);
    }


    public void Reset() {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
