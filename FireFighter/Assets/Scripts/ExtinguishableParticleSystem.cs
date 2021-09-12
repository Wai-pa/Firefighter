using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


namespace UnityStandardAssets.Effects
{
    public class ExtinguishableParticleSystem : MonoBehaviour
    {
        [SerializeField] private float multiplier = 1;
        [SerializeField] private float reduceFactor = 0.8f;
        [SerializeField] private GameObject checkbox = null;
        [SerializeField] private Light light = null;
        private ParticleSystem[] m_Systems;
        private AudioSource audioS;
        bool stop = false;

        private GameManager manager;
        
        private void Start()
        {
            manager = GameManager.instance;

            checkbox.SetActive(false);
            m_Systems = GetComponentsInChildren<ParticleSystem>();
            audioS = GetComponent<AudioSource>();
        }

        private void Update()
        {
            
        }

        public void Extinguish()
        {
            if (stop)
            {
                return;
            }

            multiplier *= reduceFactor;
            audioS.volume *= reduceFactor;
            light.intensity *= reduceFactor;

            foreach (var system in m_Systems)
            {
                if (multiplier < 0.01f)
                {
                    var emission = system.emission;
                    emission.enabled = false;
                    checkbox.SetActive (true);
                    light.enabled = false;
                    audioS.enabled = false;
                    stop = true;
                    manager.AddScore();
                    manager.extinguishedFlames++;
                    break;
                }
                else
                {
                    ParticleSystem.MainModule mainModule = system.main;

                    mainModule.startSizeMultiplier *= reduceFactor;
                    mainModule.startSpeedMultiplier *= reduceFactor;

                    system.Play();
                }
            }
        }

        public void DestroyIteself()
        {
            Destroy(gameObject);
        }
    }
}
