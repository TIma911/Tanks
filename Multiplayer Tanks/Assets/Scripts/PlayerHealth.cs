using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHealth : NetworkBehaviour
{
    float m_currentHealth;
    public float m_maxHealth = 3;

    public GameObject m_deathPrefab;
    public bool m_isDead = false;

    public RectTransform m_healthBar;
    void Start()
    {
        m_currentHealth = m_maxHealth;

        //StartCoroutine("CountDown");
    }
   IEnumerator CountDown()
    {
        yield return new WaitForSeconds(1f);
        Damage(1f);
        UpdateHealthBar(m_currentHealth);

        yield return new WaitForSeconds(1f);
        Damage(1f);
        UpdateHealthBar(m_currentHealth);

        yield return new WaitForSeconds(1f);
        Damage(1f);
        UpdateHealthBar(m_currentHealth);
    }
    
    void Update()
    {
        
    }
    void UpdateHealthBar(float value)
    {
        if (m_healthBar !=null)
        {
            m_healthBar.sizeDelta = new Vector2(value / m_maxHealth * 150f, m_healthBar.sizeDelta.y);
        }
    }
    public void Damage (float damage)
    {
        m_currentHealth -= damage;
        UpdateHealthBar(m_currentHealth);

        if (m_currentHealth <= 0 && !m_isDead)
        {
            m_isDead = true;
            Die();
        }
    }
    void Die()
    {
        if (m_deathPrefab)
        {
            GameObject deathFX = Instantiate(m_deathPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity) as GameObject;
            GameObject.Destroy(deathFX, 3f);
        }
        SetActiveState(false);
        gameObject.SendMessage("Disable");
    }
    void SetActiveState(bool state)
    {
        foreach (Collider c in GetComponentsInChildren<Collider>())
        {
            c.enabled = state;
        }
        foreach (Canvas c in GetComponentsInChildren<Canvas>())
        {
            c.enabled = state;
        }
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.enabled = state;
        }

    }
}
