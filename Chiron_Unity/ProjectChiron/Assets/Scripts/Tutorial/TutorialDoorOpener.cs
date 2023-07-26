using System;
using System.Collections;
using UnityEngine;


namespace Tutorial
{
    public class TutorialDoorOpener : MonoBehaviour
    {

        [SerializeField] private GameObject dashDoor; 
        [SerializeField] private GameObject hackDoor; 
        [SerializeField] private GameObject slowFieldDoor; 
        [SerializeField] private GameObject weaponBoostDoor; 
        [SerializeField] private GameObject secondLifeDoor;

        [SerializeField] private Renderer dashDoorRenderer; 
        [SerializeField] private Renderer hackDoorRenderer; 
        [SerializeField] private Renderer slowFieldDoorRenderer; 
        [SerializeField] private Renderer weaponBoostDoorRenderer; 
        [SerializeField] private Renderer secondLifeDoorRenderer;
        private static readonly int Dissolve = Shader.PropertyToID("Dissolve");

        private void Awake()
        {
            ResetLevel();
        }

        public void OpenDashDoor()
        {
            if (dashDoor == null) return;
            StartCoroutine(DissolveCoroutine(dashDoorRenderer, dashDoor));
        }
    
        public void OpenHackDoor()
        {
            if (hackDoor == null) return;
            StartCoroutine(DissolveCoroutine(hackDoorRenderer, hackDoor));
        }
    
        public void OpenSlowFieldDoor()
        {
            if (slowFieldDoor == null) return;
            StartCoroutine(DissolveCoroutine(slowFieldDoorRenderer, slowFieldDoor));
        }
    
        public void OpenWeaponBoostDoor()
        {
            if (weaponBoostDoor == null) return;
            StartCoroutine(DissolveCoroutine(weaponBoostDoorRenderer, weaponBoostDoor));
        }

        public void ResetLevel()
        {
            dashDoor.SetActive(true);
            dashDoorRenderer.material.SetFloat("Dissolve", 0);
            hackDoor.SetActive(true);
            hackDoorRenderer.material.SetFloat("Dissolve", 0);
            slowFieldDoor.SetActive(true);
            slowFieldDoorRenderer.material.SetFloat("Dissolve", 0);
            weaponBoostDoor.SetActive(true);
            weaponBoostDoorRenderer.material.SetFloat("Dissolve", 0);
        }
        
        private IEnumerator DissolveCoroutine(Renderer aRenderer, GameObject aGameObject)
        {
            float value = 0;
            while (value < 0.65f)
            {
                value += 0.2f * Time.deltaTime; 
                aRenderer.material.SetFloat("Dissolve", value);
                yield return null;
            }
            aGameObject.SetActive(false);
        }
    }
}
