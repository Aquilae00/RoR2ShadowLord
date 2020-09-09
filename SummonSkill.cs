using EntityStates;
using RoR2;
using UnityEngine;

namespace ShadowLord.MyEntityStates
{
    public class SummonSkill : BaseSkillState
    {
        public float baseDuration = 0.5f;
        private float duration;
        public GameObject effectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark");
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/critspark");
        public GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/tracerbanditshotgun");
        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / base.attackSpeedStat;
            Ray aimRay = base.GetAimRay();
            base.StartAimMode(aimRay, 2f, false);
            base.PlayAnimation("Gesture, Override", "FireShotgun", "FireShotgun.playbackRate", this.duration * 1.1f);
            if (base.isAuthority)
            {
                
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

        private MasterSummon masterSummon;
    }
}