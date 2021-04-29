using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]   
public class Bullet : NetworkBehaviour
{
    Rigidbody m_rigidbody;

    Collider m_collider;

    public int m_speed = 100;

    List<ParticleSystem> m_allParticles;

    public float m_lifetime = 5f;

    public ParticleSystem m_explositionFX;

    public List<string> m_bounceTags;

    public List<string> m_collisionTags;

    public int m_bounces= 2;

    public float m_damage = 1f;
    void Start()
    {
        m_allParticles = GetComponentsInChildren<ParticleSystem>().ToList();
        m_collider = GetComponent<Collider>();
        m_rigidbody = GetComponent<Rigidbody>();
        StartCoroutine("SelfDestruct");
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(m_lifetime);
        Explode();
    }

    private void Explode()
    {
        m_collider.enabled = false;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.Sleep();


        foreach (ParticleSystem ps in m_allParticles)
        {
            ps.Stop();
        }
        if (m_explositionFX != null)
        {
            m_explositionFX.transform.parent = null;
            m_explositionFX.Play();
        }

        if (isServer)
        {
            Destroy(gameObject);
            foreach (MeshRenderer m in GetComponentsInChildren<MeshRenderer>())
            {
                m.enabled = false;
            }
        }
     
    }

    void Update()
    {
        
    }
    void OnCollisionExit(Collision collision)
    {
        if (m_rigidbody.velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(m_rigidbody.velocity);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        CheckCollision(collision);
        if (m_bounceTags.Contains(collision.gameObject.tag))
        {
            if (m_bounces <= 0)
            {
                Explode();
            }
            m_bounces --;
        }
    }

    void CheckCollision(Collision collision)
    {
        if (m_collisionTags.Contains(collision.collider.tag))
        {
            Explode();
            PlayerHealth playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();

            if (playerHealth !=null)
            {
                playerHealth.Damage(m_damage);
            }
        }
    }
}
