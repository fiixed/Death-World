﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public float m_StartingHealth = 100f;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public GameObject m_ExplosionPrefab;
    public PhotonView pv;


    private GvrAudioSource m_ExplosionAudio;
    private ParticleSystem m_ExplosionParticles;
    private float m_CurrentHealth;
    private bool m_Dead = false;
    private string killer = "Null";


    private void Awake() {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<GvrAudioSource>();

        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable() {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }


    public void TakeDamage(float amount) {
        // Adjust the player's current health, update the UI based on the new health and check whether or not the player is dead.
        m_CurrentHealth -= amount;

        SetHealthUI();

        if (m_CurrentHealth <= 0f && !m_Dead) {
            //OnDeath();
        }
    }


    private void SetHealthUI() {
        // Adjust the value and colour of the slider

        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


   

    [PunRPC]
    public void ApplyDamage(int dmg, string kilr) {
        if (kilr != gameObject.name) {
            m_CurrentHealth -= dmg;
           
            killer = kilr;

            if (pv.isMine && m_CurrentHealth <= 0) {
                pv.RPC("Die", PhotonTargets.AllBuffered, null);
            }
        }
    }

    //private void OnDeath() {
    //    // Play the effects for the death of the player and deactivate it.
    //    m_Dead = true;

    //    m_ExplosionParticles.transform.position = transform.position;
    //    m_ExplosionParticles.gameObject.SetActive(true);

    //    m_ExplosionParticles.Play();

    //    m_ExplosionAudio.Play();

    //    gameObject.SetActive(false);
    //}

    [PunRPC]
    public void Die() {
        if (!m_Dead) {
            m_Dead = true;
            m_ExplosionParticles.transform.position = transform.position;
            m_ExplosionParticles.gameObject.SetActive(true);

            m_ExplosionParticles.Play();

            m_ExplosionAudio.Play();

            Destroy(gameObject);

            if (pv.isMine) {
                //PhotonNetwork.Instantiate(ragdoll.name, transform.position, transform.rotation, 0);
                GameObject.Find("_Room").GetComponent<roomManager>().isSpawned = false;
                GameObject.Find("_Network").GetComponent<PhotonView>().RPC("getKillFeed", PhotonTargets.All, PhotonNetwork.player.NickName, killer);
                if (GameObject.Find(killer) != null) {
                    GameObject.Find(killer).GetComponent<PhotonView>().RPC("gotKill", PhotonTargets.AllBuffered, PhotonNetwork.playerName);
                }
            }
        }
    }
}
