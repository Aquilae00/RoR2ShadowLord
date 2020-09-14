using System;
using BepInEx;
using EntityStates;
using R2API;
using R2API.AssetPlus;
using R2API.Utils;
using RoR2;
using RoR2.Skills;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using Mono.Cecil.Cil;
using MonoMod.Cil;


namespace ShadowLord
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin(
        "com.MyName.IHerebyGrantPermissionToDeprecateMyModFromThunderstoreBecauseIHaveNotChangedTheName",
        "IHerebyGrantPermissionToDeprecateMyModFromThunderstoreBecauseIHaveNotChangedTheName",
        "1.0.0")]
    [R2APISubmoduleDependency(nameof(LoadoutAPI), nameof(SurvivorAPI), nameof(AssetPlus))]
    public class ExamplePlugin : BaseUnityPlugin
    {
        private GameObject characterPrefab;
        public void Awake()
        {
            //IL.RoR2.CharacterMaster.OnBodyDeath += (il) =>
            //{
            //    ILCursor c = new ILCursor(il);
            //    c.GotoNext(
            //        x => x.MatchLdarg(0),
            //        x => x.MatchLdfld<CharacterMaster>("destroyOnBodyDeath"),
            //        x => x.MatchBrfalse(out _),
            //        x => x.MatchLdarg(0),
            //        x => x.MatchCallOrCallvirt<Component>("get_gameObject"),
            //        x => x.MatchLdcR4(1),
            //        x => x.MatchCallOrCallvirt<UnityEngine.Object>("Destroy"));
            //    c.Index += 1;
            //    c.RemoveRange(7);
            //};
            //On.RoR2.HurtBoxGroup.OnDeathStart += (orig, self) =>
            //{
            //    Chat.AddMessage("yahoo");
            //};
            //On.RoR2.CharacterAI.BaseAI.OnBodyDeath += (orig, self, test) =>
            //{
            //    Chat.AddMessage("dead ai");
            //};
            //On.RoR2.CharacterAI.BaseAI.OnBodyLost += (orig, self, test) =>
            //{
            //    Chat.AddMessage("dead ai");
            //};
            //////On.RoR2.HurtBoxGroup.SetHurtboxesActive += (orig, self, test) =>
            //////{
            //////    Chat.AddMessage("yay");
            //////};
            ////On.RoR2.BullseyeSearch.CheckVisible += (orig, self, test) =>
            ////{
            ////    return false;
            ////};

            //On.RoR2.HurtBox.OnDisable += (orig, self) =>
            //{
            //    Chat.AddMessage("OnDisable");
            //};

            //IL.RoR2.CameraRigController.Update += (il) => {
            //    var c = new ILCursor(il);

            //    //We use GotoNext to locate the code we want to edit
            //    //Notice we can specify a block of instructions to match, rather than only a single instruction
            //    //This is preferable as it is less likely to break something else because of an update

            //    c.GotoNext(
            //        x => x.MatchLdloc(4),      // num14 *= 0.5f;
            //        x => x.MatchLdcR4(0.5f),   // 
            //        x => x.MatchMul(),         // 
            //        x => x.MatchStloc(4),      // 
            //        x => x.MatchLdloc(5),      // num15 *= 0.5f;
            //        x => x.MatchLdcR4(0.5f),   //
            //        x => x.MatchMul(),         //
            //        x => x.MatchStloc(5));     //

            //    //When we GotoNext, the cursor is before the first instruction we match.
            //    //So we remove the next 8 instructions
            //    c.RemoveRange(8);

            //};

            // myCharacter should either be
            // Resources.Load<GameObject>("prefabs/characterbodies/BanditBody");
            // or BodyCatalog.FindBodyIndex("BanditBody");
            this.characterPrefab = Resources.Load<GameObject>("prefabs/characterbodies/HuntressBody");

            // If you're confused about the language tokens, they're the proper way to add any strings used by the game.
            // We use AssetPlus API for that
            Languages.AddToken("MYBANDIT_DESCRIPTION", "The description of my survivor" + Environment.NewLine);

            var mySurvivorDef = new SurvivorDef
            {
                //We're finding the body prefab here,
                bodyPrefab = this.characterPrefab,
                //Description
                descriptionToken = "MYBANDIT_DESCRIPTION",
                //Display 
                displayPrefab = Resources.Load<GameObject>("prefabs/characterdisplays/CommandoDisplay"),
                //Color on select screen
                primaryColor = new Color(0.8039216f, 0.482352942f, 0.843137264f),
                //Unlockable name
                unlockableName = "",
            };
            SurvivorAPI.AddSurvivor(mySurvivorDef);

            // If you're confused about the language tokens, they're the proper way to add any strings used by the game.
            // We use AssetPlus API for that
            Languages.AddToken("CHARACTERNAME_SKILLSLOT_SKILLNAME_NAME", "The name of this skill");
            Languages.AddToken("CHARACTERNAME_SKILLSLOT_SKILLNAME_DESCRIPTION", "The description of this skill.");



            //If this is the default/first skill, copy this code and remove the //,
            //skillFamily.variants[0] = new SkillFamily.Variant
            //{
            //    skillDef = mySkillDef,
            //    unlockableName = "",
            //    viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)
            //};

            this.SkillSetup();

        }
        
        private void SkillSetup()
        {
            this.PrimarySkillSetup();
        }

        private void PrimarySkillSetup()
        {
            var mySkillDef = ScriptableObject.CreateInstance<SkillDef>();
            mySkillDef.activationState = new SerializableEntityStateType(typeof(ShadowLord.MyEntityStates.TargetedSumonSkill));
            mySkillDef.activationStateMachineName = "Weapon";
            mySkillDef.baseMaxStock = 1;
            mySkillDef.baseRechargeInterval = 0f;
            mySkillDef.beginSkillCooldownOnSkillEnd = true;
            mySkillDef.canceledFromSprinting = true;
            mySkillDef.fullRestockOnAssign = true;
            mySkillDef.interruptPriority = InterruptPriority.Any;
            mySkillDef.isBullets = false;
            mySkillDef.isCombatSkill = false;
            mySkillDef.mustKeyPress = false;
            mySkillDef.noSprint = true;
            mySkillDef.rechargeStock = 1;
            mySkillDef.requiredStock = 1;
            mySkillDef.shootDelay = 0.5f;
            mySkillDef.stockToConsume = 1;
            mySkillDef.icon = Resources.Load<Sprite>("NotAnActualPath");
            mySkillDef.skillDescriptionToken = "CHARACTERNAME_SKILLSLOT_SKILLNAME_DESCRIPTION";
            mySkillDef.skillName = "CHARACTERNAME_SKILLSLOT_SKILLNAME_NAME";
            mySkillDef.skillNameToken = "CHARACTERNAME_SKILLSLOT_SKILLNAME_NAME";

            LoadoutAPI.AddSkillDef(mySkillDef);
            //This adds our skilldef. If you don't do this, the skill will not work.

            var skillLocator = this.characterPrefab.GetComponent<SkillLocator>();

            //Note; you can change component.primary to component.secondary , component.utility and component.special
            var skillFamily = skillLocator.primary.skillFamily;

            //If this is an alternate skill, use this code.
            // Here, we add our skill as a variant to the exisiting Skill Family.
            Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
            skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
            {
                skillDef = mySkillDef,
                unlockableName = "",
                viewableNode = new ViewablesCatalog.Node(mySkillDef.skillNameToken, false, null)

            };
        }
    }
}