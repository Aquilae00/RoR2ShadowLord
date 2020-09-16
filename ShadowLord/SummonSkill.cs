using EntityStates;
using RoR2;
using UnityEngine;
using System;
using System.Linq;

namespace ShadowLord.MyEntityStates
{
    public class SummonSkill : BaseSkillState
    {
        public float baseDuration = 0.5f;
        private float duration;
        public GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark");
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
        public GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbanditshotgun");
        private GameObject targetPrefab = Resources.Load<GameObject>("prefabs/characterbodies/LemurianBody");
        private GameObject ownerPrefab;
        private TargetTracker targetTracker;
        private MasterSummon masterSummon;

        public override void OnEnter()
        {
            base.OnEnter();
            if (base.isAuthority)
            {
                //this.targetTracker = base.GetComponent<TargetTracker>();
                this.ownerPrefab = base.gameObject;
                CharacterBody targetBody = this.targetPrefab.GetComponent<CharacterBody>();
                CharacterBody ownerBody = this.ownerPrefab.GetComponent<CharacterBody>();
                GameObject bodyPrefab = BodyCatalog.FindBodyPrefab(targetBody);
                CharacterMaster characterMaster = MasterCatalog.allAiMasters.FirstOrDefault((CharacterMaster master) => master.bodyPrefab == bodyPrefab);
                this.masterSummon = new MasterSummon();
                masterSummon.masterPrefab = characterMaster.gameObject;
                masterSummon.position = ownerBody.footPosition;
                CharacterDirection component = ownerBody.GetComponent<CharacterDirection>();
                masterSummon.rotation = (component ? Quaternion.Euler(0f, component.yaw, 0f) : ownerBody.transform.rotation);
                masterSummon.summonerBodyObject = (ownerBody ? ownerBody.gameObject : null);

                CharacterMaster characterMaster2 = masterSummon.Perform();

                GameObject trackingTargetAsGO = this.targetTracker.GetTrackingTargetAsGO();
                RoR2.Console.Log log = new RoR2.Console.Log();
                if (trackingTargetAsGO != null)
                {
                    log.message = "REEE";
                    RoR2.Console.logs.Add(log);
                } else
                {
                    log.message = "YEET";
                    RoR2.Console.logs.Add(log);
                }
            
            }
           

        }
        public override void OnExit()
        {
            base.OnExit();
        }
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }


    }
}