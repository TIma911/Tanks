using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerSetup : NetworkBehaviour
{
    public Color m_playerColor;
    public string m_basename = "PLAYER";
    public int m_playerNum = 1;
    public Text m_playerNameNext;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (m_playerNameNext != null)
        {
            m_playerNameNext.enabled = false;
           
        }
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
    
    foreach (MeshRenderer r in meshes)
        {
            r.material.color = m_playerColor;
        }
        if (m_playerNameNext != null)
        {
            m_playerNameNext.enabled = true;
            m_playerNameNext.text = m_basename + m_playerNum.ToString();
        }
    }


}

