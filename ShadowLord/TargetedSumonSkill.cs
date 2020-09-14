using EntityStates;
using RoR2;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
namespace ShadowLord.MyEntityStates
{
    public class TargetedSumonSkill : BaseSkillState
    {
        public float baseDuration = 0.5f;
        private float duration;
        private GameObject ownerPrefab;
        private HuntressTracker huntressTracker;
        private MasterSummon masterSummon;

        private HurtBox initialOrbTarget;

        public override void OnEnter()
        {
            base.OnEnter();
            if (this.huntressTracker && base.isAuthority)
            {
                this.initialOrbTarget = this.huntressTracker.GetTrackingTarget();
            }

            if (base.isAuthority)
            {
                this.huntressTracker = base.GetComponent<HuntressTracker>();
                this.ownerPrefab = base.gameObject;
                CharacterBody targetBody = this.huntressTracker.GetTrackingTarget().healthComponent.body;
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
        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
        }
        public override void OnDeserialize(NetworkReader reader)
        {
            this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }

    }
}