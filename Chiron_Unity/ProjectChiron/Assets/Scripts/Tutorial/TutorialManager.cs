using System;
using System.Net.Mail;
using Level_Generation;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using FixedUpdate = UnityEngine.PlayerLoop.FixedUpdate;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TutorialDoorOpener doorOpener;
        [SerializeField] private PlayerCharacterController pcc;

        //Enemies in HackSection
        [SerializeField] private SphereBehaviour sphereOne; 
        [SerializeField] private SphereBehaviour sphereTwo;

        [SerializeField] private DashConfig dashConfig;
        [SerializeField] private SlowFieldConfig slowFieldConfig;
        [SerializeField] private WeaponBoostConfig weaponBoostConfig;
        
        //Enemies in Last Room
        [SerializeField] private SphereBehaviour sphereThree; 
        [SerializeField] private SphereBehaviour sphereFour;

        [SerializeField] private GameObject tutorialDoor;
        [SerializeField] private GameObject tutorialUITextBox;
        [SerializeField] private GameObject tutorialUIButton;
        [SerializeField] private GameObject tutorialInstructionText;

        [SerializeField] private SphereBehaviour slowSphereEnemy; 
        [SerializeField] private SphereBehaviour slowSphereEnemyAttacked; 
        
        [SerializeField] private AiDirector aiDirector;

        private LevelDoor _levelDoor;

        private void Awake()
        {
            _levelDoor = new LevelDoor(tutorialDoor);
            dashConfig.tutCondition.Where(x => x!= false).Subscribe(x => doorOpener.OpenDashDoor());
            slowFieldConfig.tutCondition.Where(x => x!= false).Subscribe(x => doorOpener.OpenSlowFieldDoor());
            weaponBoostConfig.tutCondition.Where(x => x!= false).Subscribe(x => doorOpener.OpenWeaponBoostDoor());
            //pcc.TutEnableDash();
            pcc.ResetAfterTutorial();
        }

        private void Start()
        {
            //slowSphereEnemy.hackable.Subscribe(x => x = true);
            //aiDirector.HackEnemy(slowSphereEnemy.GameObject(), slowSphereEnemyAttacked.GameObject()); 
        }

        private void FixedUpdate()
        {
            if (!sphereOne.GetIsAlive() || !sphereTwo.GetIsAlive())
            {
                doorOpener.OpenHackDoor();
            }

            if (!sphereThree.GetIsAlive() && !sphereFour.GetIsAlive())
            {
                LevelGenerator.showVaporWave = true; 
                _levelDoor.OpenDoor();
                doorOpener.ResetLevel();
            }
        }

        public void CloseInstructionDialogue()
        {
            pcc.ShootLock = false;
            pcc.MoveLock = false;
            pcc.AimLock = false;
            tutorialUIButton.SetActive(false);
            tutorialUITextBox.SetActive(false);
            tutorialInstructionText.SetActive(true);
        }
        
    }
}
