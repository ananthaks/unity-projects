using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunshipController : MonoBehaviour {

    private AudioSource m_audioSource;

    [SerializeField] Animator m_animator;
    [SerializeField] GameObject MainWeaponBolt;
    [SerializeField] Transform shotspawn;

    // Editable Fields
    [SerializeField] int TotalLife;
    [SerializeField] int TotalBattery;
    [SerializeField] int RechargePeriod;

    private int m_currentBattery;
    private int m_currentLife;
    private float m_currentTime;
    private float m_timePerLevel;

    // Use this for initialization
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_currentBattery = TotalBattery;
        m_currentLife = TotalLife;

        m_currentTime = Time.time;
        m_timePerLevel = (float)TotalBattery / (float)RechargePeriod;
    }


    // Update is called once per frame
    void Update()
    {
        if(m_currentBattery < TotalBattery && Time.time > m_currentTime)
        {
            m_currentBattery++;
            m_currentTime = Time.time + m_timePerLevel;
            UpdateUIBars();

            if(m_currentBattery == TotalBattery)
            {
                OpenCloseAnimate(true);
            }
        }
    }

    void FixedUpdate()
    {

    }

    private void UpdateUIBars()
    {

    }

    private void OpenCloseAnimate(bool isOpen)
    {

    }

    private void FireAnimate()
    {


    }
    public void OnShotHit()
    {
        m_currentLife--;
        if(m_currentLife == 0)
        {
            // Destroy
            Destroy(gameObject);
        }
        else
        {
            UpdateUIBars();
        }
    }

    public void FireWeapons()
    {
        if(m_currentBattery == TotalBattery)
        {
            Instantiate(MainWeaponBolt, shotspawn.position, shotspawn.rotation);
            m_currentBattery = 0;

            if (m_audioSource != null)
            {
                m_audioSource.Play();
            }

            FireAnimate();
            OpenCloseAnimate(false);
        }
    }
}
