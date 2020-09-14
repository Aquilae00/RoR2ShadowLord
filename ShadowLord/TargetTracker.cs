using System;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;

namespace ShadowLord
{
    public class TargetTracker: HuntressTracker
    {
        private TargetTracker()
        {
            this.maxTrackingDistance = 22f;
        }

		private void Awake()
		{
			GameObject gameObject = Resources.Load<GameObject>("Prefabs/HuntressTrackingIndicator");
			if (gameObject != null)
			{
				Reflection.SetFieldValue<Indicator>(this, "indicator", new Indicator(base.gameObject, gameObject));
			}
		}
		private void Start()
		{
			this.OrigStart();
		}

		private void OrigStart()
		{
			Reflection.SetFieldValue<CharacterBody>(this, "characterBody", base.GetComponent<CharacterBody>());
			Reflection.SetFieldValue<InputBankTest>(this, "inputBank", base.GetComponent<InputBankTest>());
			Reflection.SetFieldValue<TeamComponent>(this, "teamComponent", base.GetComponent<TeamComponent>());
		}

		public GameObject GetTrackingTargetAsGO()
		{
			if (base.GetTrackingTarget() != null)
			{
				return base.GetTrackingTarget().gameObject;
			}
			return null;
		}
	}
}