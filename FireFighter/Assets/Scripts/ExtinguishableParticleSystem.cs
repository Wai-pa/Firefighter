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

        [SerializeField] private float audioVolume;
        [SerializeField] private float lightIntensity;
        [SerializeField] private float particleSystemSizeMultiplier;
        [SerializeField] private float particleSystemSpeedMultiplier;

        private GameManager manager;
        
        private void Start()
        {
            manager = GameManager.instance;
            manager.AddFlames();

            checkbox.SetActive(false);
            m_Systems = GetComponentsInChildren<ParticleSystem>();
            audioS = GetComponent<AudioSource>();

            audioVolume = audioS.volume;
            lightIntensity = light.intensity;

            foreach(var system in m_Systems)
            {
                ParticleSystem.MainModule mainModule = system.main;
                particleSystemSizeMultiplier = mainModule.startSizeMultiplier;
                particleSystemSpeedMultiplier = mainModule.startSpeedMultiplier;
            }

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
                    manager.extinguishedFlames.Add(gameObject);
                    manager.activeFlames.Remove(gameObject);
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

        public void RenableFlame()
        {
            
            foreach (var system in m_Systems)
            {
                var emission = system.emission;
                emission.enabled = true;

                ParticleSystem.MainModule mainModule = system.main;

                mainModule.startSizeMultiplier = particleSystemSizeMultiplier;
                mainModule.startSpeedMultiplier = particleSystemSpeedMultiplier;
            }
            

            checkbox.SetActive(false);
            light.enabled = true;
            audioS.enabled = true;
            stop = false;

            multiplier = 1;
            audioS.volume = audioVolume;
            light.intensity = lightIntensity;
        }
    }
}
