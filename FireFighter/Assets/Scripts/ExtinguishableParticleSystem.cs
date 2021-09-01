using System;
using UnityEngine;
using UnityEngine.UI;


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

        /*
        private float timer = 0.0f;
        [SerializeField] private float intensifyFactor = 1.4f;
        [SerializeField] float intensifyBackTime = 1.0f;
        */

        private GameManager manager;
        
        private void Start()
        {
            manager = GameManager.instance; //caching GM
            manager.AddFlames();

            checkbox.SetActive(false);
            m_Systems = GetComponentsInChildren<ParticleSystem>();
            audioS = GetComponent<AudioSource>();
        }

        /*
        private void Update()
        {
            timer += Time.deltaTime / 60;

            if (timer >= intensifyBackTime)
            {
                IntensifyBack();
            }
        }

        private void IntensifyBack()
        { 
            foreach (var system in m_Systems)
            {
                if (multiplier < 1.1f)
                {
                    multiplier *= intensifyFactor;
                    audioS.volume *= intensifyFactor;
                    light.intensity *= intensifyFactor;

                    ParticleSystem.MainModule mainModule = system.main;

                    mainModule.startSizeMultiplier *= intensifyFactor;
                    mainModule.startSpeedMultiplier *= intensifyFactor;

                    system.Play();
                }
            }
        }
        */

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
                    manager.AddScore(); //adding score
                }
                else
                {
                    ParticleSystem.MainModule mainModule = system.main;

                    mainModule.startSizeMultiplier *= reduceFactor;
                    mainModule.startSpeedMultiplier *= reduceFactor;

                    //timer = 0.0f; //set timer back to 0

                    system.Play();
                }
            }
        }
    }
}
