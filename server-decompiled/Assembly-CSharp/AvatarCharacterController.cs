using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public abstract class AvatarCharacterController : AvatarMultiBodyController
{
	// Token: 0x1700004E RID: 78
	// (get) Token: 0x0600041A RID: 1050 RVA: 0x0001F160 File Offset: 0x0001D360
	public BodyAnimator CharacterBody
	{
		get
		{
			return this.characterBody;
		}
	}

	// Token: 0x0600041B RID: 1051 RVA: 0x0001F168 File Offset: 0x0001D368
	public override void SwitchModelAndView(string _modelName, bool _bFPV, bool _bMale)
	{
		if (this.characterBody != null && this.modelName != _modelName)
		{
			if (this.characterBody.Parts.BodyObj != null)
			{
				this.characterBody.Parts.BodyObj.SetActive(false);
			}
			base.removeBodyAnimator(this.characterBody);
			this.characterBody = null;
		}
		if (this.characterBody == null)
		{
			this.modelName = _modelName;
			Transform transform = EModelBase.FindModel(base.transform);
			if (transform != null)
			{
				Transform transform2 = transform.Find(_modelName);
				if (transform2)
				{
					transform2.gameObject.SetActive(true);
					this.characterBody = base.addBodyAnimator(this.createCharacterBody(transform2));
				}
			}
		}
		if (this.characterBody != null)
		{
			this.initBodyAnimator(this.characterBody, _bFPV, _bMale);
		}
		base.SwitchModelAndView(_modelName, _bFPV, _bMale);
	}

	// Token: 0x0600041C RID: 1052 RVA: 0x0001F240 File Offset: 0x0001D440
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void initBodyAnimator(BodyAnimator _body, bool _bFPV, bool _bMale)
	{
		base._setBool("IsMale", _bMale, true);
		base._setFloat("IsMaleFloat", _bMale ? 1f : 0f, true);
		this.SetWalkType(this.entity.GetWalkType(), false);
		this._setBool(AvatarController.isDeadHash, this.entity.IsDead(), true);
		this._setBool(AvatarController.isFPVHash, _bFPV, true);
		this._setBool(AvatarController.isAliveHash, this.entity.IsAlive(), true);
		if (_body == this.characterBody)
		{
			this.characterBody.State = (_bFPV ? BodyAnimator.EnumState.OnlyColliders : BodyAnimator.EnumState.Visible);
		}
	}

	// Token: 0x0600041D RID: 1053
	[PublicizedFrom(EAccessModifier.Protected)]
	public abstract BodyAnimator createCharacterBody(Transform _bodyTransform);

	// Token: 0x0600041E RID: 1054 RVA: 0x0001F2DD File Offset: 0x0001D4DD
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual HashSet<int> getJumpStates()
	{
		return new HashSet<int> { Animator.StringToHash("Base Layer.Jump") };
	}

	// Token: 0x0600041F RID: 1055 RVA: 0x0001F2F5 File Offset: 0x0001D4F5
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual HashSet<int> getDeathStates()
	{
		HashSet<int> hashSet = new HashSet<int>();
		AvatarCharacterController.GetFirstPersonDeathStates(hashSet);
		AvatarCharacterController.GetThirdPersonDeathStates(hashSet);
		return hashSet;
	}

	// Token: 0x06000420 RID: 1056 RVA: 0x0001F308 File Offset: 0x0001D508
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual HashSet<int> getReloadStates()
	{
		HashSet<int> hashSet = new HashSet<int>();
		AvatarCharacterController.GetFirstPersonReloadStates(hashSet);
		AvatarCharacterController.GetThirdPersonReloadStates(hashSet);
		return hashSet;
	}

	// Token: 0x06000421 RID: 1057 RVA: 0x0001F31B File Offset: 0x0001D51B
	public override Animator GetAnimator()
	{
		return this.characterBody.Animator;
	}

	// Token: 0x06000422 RID: 1058 RVA: 0x0001F328 File Offset: 0x0001D528
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual HashSet<int> getHitStates()
	{
		HashSet<int> hashSet = new HashSet<int>();
		AvatarCharacterController.GetThirdPersonHitStates(hashSet);
		return hashSet;
	}

	// Token: 0x06000423 RID: 1059 RVA: 0x0001F338 File Offset: 0x0001D538
	public static void GetFirstPersonReloadStates(HashSet<int> hashSet)
	{
		hashSet.Add(Animator.StringToHash("Base Layer.fpvBlunderbussReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvSawedOffShotgunReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvPistolReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvMP5Reload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvSniperRifleReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvM136Reload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvCrossbowReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvHuntingRifleReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvAugerReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvChainsawReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvSawedOffShotgunReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvMagnumReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvBowReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvNailGunReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvAK47Reload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvBowReload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvAK47Reload"));
		hashSet.Add(Animator.StringToHash("Base Layer.fpvCompoundBowReload"));
	}

	// Token: 0x06000424 RID: 1060 RVA: 0x0001F478 File Offset: 0x0001D678
	public static void GetThirdPersonReloadStates(HashSet<int> hashSet)
	{
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemaleSawedOffShotgunReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemaleMP5Reload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemaleSniperRifleReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleM136Reload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemaleCrossbowReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleHuntingRifleReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleAugerReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleChainsawReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemaleSawedOffShotgunReloadIntro"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemaleSawedOffShotgunReloadExit"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.Female44MagnumReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemaleNailGunReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleBowReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleBlunderbussReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleBowReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.FemalePistolReload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleAk47Reload"));
		hashSet.Add(Animator.StringToHash("TwoHandedOverlays.femaleCompoundBowReload"));
	}

	// Token: 0x06000425 RID: 1061 RVA: 0x000027FC File Offset: 0x000009FC
	public static void GetFirstPersonHitStates(HashSet<int> hashSet)
	{
	}

	// Token: 0x06000426 RID: 1062 RVA: 0x0001F5B8 File Offset: 0x0001D7B8
	public static void GetThirdPersonHitStates(HashSet<int> hashSet)
	{
		hashSet.Add(Animator.StringToHash("PainOverlays.femaleTwitchHeadLeft"));
		hashSet.Add(Animator.StringToHash("PainOverlays.femaleTwitchHeadRight"));
		hashSet.Add(Animator.StringToHash("PainOverlays.femaleTwitchChestLeft"));
		hashSet.Add(Animator.StringToHash("PainOverlays.femaleTwitchChestRight"));
	}

	// Token: 0x06000427 RID: 1063 RVA: 0x000027FC File Offset: 0x000009FC
	public static void GetFirstPersonDeathStates(HashSet<int> hashSet)
	{
	}

	// Token: 0x06000428 RID: 1064 RVA: 0x0001F60C File Offset: 0x0001D80C
	public static void GetThirdPersonDeathStates(HashSet<int> hashSet)
	{
		hashSet.Add(Animator.StringToHash("Base Layer.generic"));
		hashSet.Add(Animator.StringToHash("Base Layer.FemaleDeath01"));
		hashSet.Add(Animator.StringToHash("Base Layer.idlingHead"));
		hashSet.Add(Animator.StringToHash("Base Layer.idlingChest"));
		hashSet.Add(Animator.StringToHash("Base Layer.idlingLeftArm"));
		hashSet.Add(Animator.StringToHash("Base Layer.idlingRightArm"));
		hashSet.Add(Animator.StringToHash("Base Layer.idlingLeftLeg"));
		hashSet.Add(Animator.StringToHash("Base Layer.idlingRightLeg"));
		hashSet.Add(Animator.StringToHash("Base Layer.meleeHeadFront"));
		hashSet.Add(Animator.StringToHash("Base Layer.meleeHeadLeft"));
		hashSet.Add(Animator.StringToHash("Base Layer.meleeHeadLeftA"));
		hashSet.Add(Animator.StringToHash("Base Layer.meleeHeadLeftB"));
		hashSet.Add(Animator.StringToHash("Base Layer.meleeHeadLeftC"));
		hashSet.Add(Animator.StringToHash("Base Layer.meleeHeadRight"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningChestA"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningChestB"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningHeadA"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningHeadB"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningLeftArmA"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningLeftArmB"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningRightArmA"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningRightArmB"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningLeftLeg"));
		hashSet.Add(Animator.StringToHash("Base Layer.runningRightLeg"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingChestA"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingChestB"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingHeadA"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingHeadB"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingLeftArm"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingRightArm"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingLeftLeg"));
		hashSet.Add(Animator.StringToHash("Base Layer.walkingRightLeg"));
	}

	// Token: 0x06000429 RID: 1065 RVA: 0x0001F83C File Offset: 0x0001DA3C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setTrigger(int _pid, bool _netsync = true)
	{
		base._setTrigger(_pid, _netsync);
		if (this.characterBody != null)
		{
			Animator animator = this.characterBody.Animator;
			if (AvatarMultiBodyController.animatorIsValid(animator))
			{
				animator.SetTrigger(_pid);
			}
		}
	}

	// Token: 0x0600042A RID: 1066 RVA: 0x0001F874 File Offset: 0x0001DA74
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _resetTrigger(int _pid, bool _netsync = true)
	{
		base._resetTrigger(_pid, _netsync);
		if (this.characterBody != null)
		{
			Animator animator = this.characterBody.Animator;
			if (animator)
			{
				animator.ResetTrigger(_pid);
			}
		}
	}

	// Token: 0x0600042B RID: 1067 RVA: 0x0001F8AC File Offset: 0x0001DAAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setFloat(int _pid, float _value, bool _netsync = true)
	{
		base._setFloat(_pid, _value, _netsync);
		if (this.characterBody != null)
		{
			Animator animator = this.characterBody.Animator;
			if (animator)
			{
				animator.SetFloat(_pid, _value);
			}
		}
	}

	// Token: 0x0600042C RID: 1068 RVA: 0x0001F8E8 File Offset: 0x0001DAE8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setBool(int _pid, bool _value, bool _netsync = true)
	{
		base._setBool(_pid, _value, _netsync);
		if (this.characterBody != null)
		{
			Animator animator = this.characterBody.Animator;
			if (animator)
			{
				animator.SetBool(_pid, _value);
			}
		}
	}

	// Token: 0x0600042D RID: 1069 RVA: 0x0001F924 File Offset: 0x0001DB24
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void _setInt(int _pid, int _value, bool _netsync = true)
	{
		base._setInt(_pid, _value, _netsync);
		if (this.characterBody != null)
		{
			Animator animator = this.characterBody.Animator;
			if (animator)
			{
				animator.SetInteger(_pid, _value);
			}
		}
	}

	// Token: 0x0600042E RID: 1070 RVA: 0x0001F95E File Offset: 0x0001DB5E
	[PublicizedFrom(EAccessModifier.Protected)]
	public AvatarCharacterController()
	{
	}

	// Token: 0x04000496 RID: 1174
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BodyAnimator characterBody;

	// Token: 0x04000497 RID: 1175
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string modelName;

	// Token: 0x020000C3 RID: 195
	public class AnimationStates
	{
		// Token: 0x0600042F RID: 1071 RVA: 0x0001F966 File Offset: 0x0001DB66
		public AnimationStates(HashSet<int> _jumpStates, HashSet<int> _deathStates, HashSet<int> _reloadStates, HashSet<int> _hitStates)
		{
			this.JumpStates = _jumpStates;
			this.DeathStates = _deathStates;
			this.ReloadStates = _reloadStates;
			this.HitStates = _hitStates;
		}

		// Token: 0x04000498 RID: 1176
		public readonly HashSet<int> JumpStates;

		// Token: 0x04000499 RID: 1177
		public readonly HashSet<int> DeathStates;

		// Token: 0x0400049A RID: 1178
		public readonly HashSet<int> ReloadStates;

		// Token: 0x0400049B RID: 1179
		public readonly HashSet<int> HitStates;
	}
}
