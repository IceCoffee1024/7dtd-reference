using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020004B2 RID: 1202
[Preserve]
public class EntityHuman : EntityEnemy
{
	// Token: 0x17000440 RID: 1088
	// (get) Token: 0x060025FE RID: 9726 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override Entity.EnumPositionUpdateMovementType positionUpdateMovementType
	{
		[PublicizedFrom(EAccessModifier.Protected)]
		get
		{
			return Entity.EnumPositionUpdateMovementType.MoveTowards;
		}
	}

	// Token: 0x060025FF RID: 9727 RVA: 0x000E86C4 File Offset: 0x000E68C4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06002600 RID: 9728 RVA: 0x000E86CC File Offset: 0x000E68CC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitCommon()
	{
		base.InitCommon();
		if (this.walkType == 21)
		{
			this.TurnIntoCrawler();
		}
	}

	// Token: 0x06002601 RID: 9729 RVA: 0x000E86E4 File Offset: 0x000E68E4
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		string @string = EntityClass.list[this.entityClass].Properties.GetString(EntityClass.PropMoveSpeedPattern);
		if (@string.Length > 0)
		{
			this.moveSpeedPattern = StringParsers.ParseList<float>(@string, ',', (string _s, int _start, int _end) => StringParsers.ParseFloat(_s, _start, _end, NumberStyles.Any));
		}
	}

	// Token: 0x06002602 RID: 9730 RVA: 0x000E8750 File Offset: 0x000E6950
	public override void OnAddedToWorld()
	{
		base.OnAddedToWorld();
		if (base.GetSpawnerSource() == EnumSpawnerSource.Biome)
		{
			this.timeToDie = this.world.worldTime + 12000UL + (ulong)(10000f * this.rand.RandomFloat);
			if (this.IsFeral)
			{
				int num = (int)SkyManager.GetDawnTime();
				int num2 = (int)SkyManager.GetDuskTime();
				int num3 = GameUtils.WorldTimeToHours(this.WorldTimeBorn);
				if (num3 < num || num3 >= num2)
				{
					int num4 = GameUtils.WorldTimeToDays(this.world.worldTime);
					if (GameUtils.WorldTimeToHours(this.world.worldTime) >= num2)
					{
						num4++;
					}
					this.timeToDie = GameUtils.DayTimeToWorldTime(num4, num, 0);
				}
			}
		}
		else
		{
			this.timeToDie = this.world.worldTime + 4000UL + (ulong)(4000f * this.rand.RandomFloat);
			if (this.IsSleeper)
			{
				this.timeToDie += 24000UL;
			}
		}
		this.SetupHandItem();
	}

	// Token: 0x06002603 RID: 9731 RVA: 0x000E884B File Offset: 0x000E6A4B
	public virtual void SetupHandItem()
	{
		this.inventory.SetRightHandAsModel();
		this.ShowHoldingItem(false);
	}

	// Token: 0x06002604 RID: 9732 RVA: 0x000E8860 File Offset: 0x000E6A60
	public override void OnUpdateLive()
	{
		base.OnUpdateLive();
		if (this.moveSpeedRagePer > 0f && this.bodyDamage.CurrentStun == EnumEntityStunType.None)
		{
			this.moveSpeedScaleTime -= 0.05f;
			if (this.moveSpeedScaleTime <= 0f)
			{
				this.StopRage();
			}
		}
		if (!this.isEntityRemote && !this.IsDead())
		{
			if (this.smellPlayer)
			{
				int num = this.smellPlayerTimeoutTicks - 1;
				this.smellPlayerTimeoutTicks = num;
				if (num <= 0)
				{
					this.smellPlayer = null;
				}
			}
			this.MoveSpeedPatternTick();
			if (this.world.worldTime >= this.timeToDie && !this.attackTarget)
			{
				this.lootDropProb = 0f;
				this.Kill(DamageResponse.New(true));
			}
		}
		if (this.emodel)
		{
			AvatarController avatarController = this.emodel.avatarController;
			if (avatarController)
			{
				bool flag = this.onGround || this.isSwimming || this.bInElevator;
				if (flag)
				{
					this.fallTime = 0f;
					this.fallThresholdTime = 0f;
					if (this.bInElevator)
					{
						this.fallThresholdTime = 0.6f;
					}
				}
				else
				{
					if (this.fallThresholdTime == 0f)
					{
						this.fallThresholdTime = 0.1f + this.rand.RandomFloat * 0.3f;
					}
					this.fallTime += 0.05f;
				}
				bool flag2 = !this.emodel.IsRagdollActive && this.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.isSwimming && !this.bInElevator && this.jumpState == EntityAlive.JumpState.Off && !this.IsDead();
				if (this.fallTime <= this.fallThresholdTime)
				{
					flag2 = false;
				}
				avatarController.SetFallAndGround(flag2, flag);
			}
		}
	}

	// Token: 0x06002605 RID: 9733 RVA: 0x000E8A29 File Offset: 0x000E6C29
	public bool IsStormEffected()
	{
		return base.GetSpawnerSource() == EnumSpawnerSource.Biome && this.biomeStandingOn != null && this.biomeStandingOn.m_BiomeType != BiomeDefinition.BiomeType.PineForest && WeatherManager.Instance.IsStorming(this.biomeStandingOn.m_BiomeType);
	}

	// Token: 0x06002606 RID: 9734 RVA: 0x000E8A64 File Offset: 0x000E6C64
	public override Ray GetLookRay()
	{
		Ray ray;
		if (base.IsBreakingBlocks)
		{
			ray = new Ray(this.position + new Vector3(0f, this.GetEyeHeight(), 0f), this.GetLookVector());
		}
		else if (base.GetWalkType() == 22)
		{
			ray = new Ray(this.getHeadPosition(), this.GetLookVector());
		}
		else
		{
			ray = new Ray(this.getHeadPosition(), this.GetLookVector());
		}
		return ray;
	}

	// Token: 0x06002607 RID: 9735 RVA: 0x000E8ADB File Offset: 0x000E6CDB
	public override Vector3 GetLookVector()
	{
		if (this.lookAtPosition.Equals(Vector3.zero))
		{
			return base.GetLookVector();
		}
		return this.lookAtPosition - this.getHeadPosition();
	}

	// Token: 0x17000441 RID: 1089
	// (get) Token: 0x06002608 RID: 9736 RVA: 0x000E8B08 File Offset: 0x000E6D08
	public override bool IsRunning
	{
		get
		{
			EnumGamePrefs enumGamePrefs = EnumGamePrefs.ZombieMove;
			if (this.IsBloodMoon)
			{
				enumGamePrefs = EnumGamePrefs.ZombieBMMove;
			}
			else if (this.IsFeral)
			{
				enumGamePrefs = EnumGamePrefs.ZombieFeralMove;
			}
			else if (this.world.IsDark())
			{
				enumGamePrefs = EnumGamePrefs.ZombieMoveNight;
			}
			return GamePrefs.GetInt(enumGamePrefs) >= 2;
		}
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x000E8B50 File Offset: 0x000E6D50
	public override float GetMoveSpeed()
	{
		if (this.IsBloodMoon)
		{
			return EffectManager.GetValue(PassiveEffects.WalkSpeed, null, this.moveSpeedNight, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		if (this.smellPlayer)
		{
			return this.GetMoveSpeedAggro() * 0.85f;
		}
		if (this.IsStormEffected())
		{
			return this.GetMoveSpeedAggro() * 0.8f;
		}
		if (this.IsAlert)
		{
			return this.GetMoveSpeedAggro() * 0.65f;
		}
		if (this.world.IsDark())
		{
			return EffectManager.GetValue(PassiveEffects.WalkSpeed, null, this.moveSpeedNight, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		return EffectManager.GetValue(PassiveEffects.CrouchSpeed, null, this.moveSpeed, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x000E8C24 File Offset: 0x000E6E24
	public override float GetMoveSpeedAggro()
	{
		EnumGamePrefs enumGamePrefs = EnumGamePrefs.ZombieMove;
		if (this.IsBloodMoon)
		{
			enumGamePrefs = EnumGamePrefs.ZombieBMMove;
		}
		else if (this.IsFeral)
		{
			enumGamePrefs = EnumGamePrefs.ZombieFeralMove;
		}
		else if (this.world.IsDark())
		{
			enumGamePrefs = EnumGamePrefs.ZombieMoveNight;
		}
		else if (base.GetSpawnerSource() == EnumSpawnerSource.Biome && this.biomeStandingOn != null && this.biomeStandingOn.m_BiomeType != BiomeDefinition.BiomeType.PineForest && WeatherManager.Instance.IsStorming(this.biomeStandingOn.m_BiomeType))
		{
			enumGamePrefs = EnumGamePrefs.ZombieBMMove;
		}
		int num = GamePrefs.GetInt(enumGamePrefs);
		if (this.smellPlayer)
		{
			int @int = GamePrefs.GetInt(EnumGamePrefs.AISmellMode);
			num = Utils.FastMax(num, @int - 1);
		}
		float num2 = EntityHuman.moveSpeeds[num];
		if (this.moveSpeedRagePer > 1f)
		{
			num2 = EntityHuman.moveSuperRageSpeeds[num];
		}
		else if (this.moveSpeedRagePer > 0f)
		{
			float num3 = EntityHuman.moveRageSpeeds[num];
			num2 = num2 * (1f - this.moveSpeedRagePer) + num3 * this.moveSpeedRagePer;
		}
		if (num2 < 1f)
		{
			num2 = this.moveSpeedAggro * (1f - num2) + this.moveSpeedAggroMax * num2;
		}
		else
		{
			num2 = this.moveSpeedAggroMax * num2;
		}
		num2 *= this.moveSpeedPatternScale;
		return EffectManager.GetValue(PassiveEffects.RunSpeed, null, num2, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x000E8D68 File Offset: 0x000E6F68
	[PublicizedFrom(EAccessModifier.Private)]
	public void MoveSpeedPatternTick()
	{
		if (this.moveSpeedPattern == null)
		{
			return;
		}
		this.moveSpeedPatternDelay -= 0.05f;
		if (this.moveSpeedPatternDelay <= 0f)
		{
			this.moveSpeedPatternIndex += 2;
			if (this.moveSpeedPatternIndex >= this.moveSpeedPattern.Count)
			{
				this.moveSpeedPatternIndex = 0;
			}
			this.moveSpeedPatternDelay = this.moveSpeedPattern[this.moveSpeedPatternIndex];
			this.moveSpeedPatternScale = this.moveSpeedPattern[this.moveSpeedPatternIndex + 1];
		}
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x000E8DF5 File Offset: 0x000E6FF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override float getNextStepSoundDistance()
	{
		if (!this.IsRunning)
		{
			return 0.5f;
		}
		return 1.5f;
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateStepSound(float _distX, float _distZ, float _rotYDelta)
	{
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x000E8E0C File Offset: 0x000E700C
	public override void MoveEntityHeaded(Vector3 _direction, bool _isDirAbsolute)
	{
		if (this.walkType == 21 && this.Jumping)
		{
			this.motion = this.accumulatedRootMotion;
			this.accumulatedRootMotion = Vector3.zero;
			this.IsRotateToGroundFlat = true;
			if (this.moveHelper != null)
			{
				Vector3 vector = this.moveHelper.JumpToPos - this.position;
				if (Utils.FastAbs(vector.y) < 0.2f)
				{
					this.motion.y = vector.y * 0.2f;
				}
				if (Utils.FastAbs(vector.x) < 0.3f)
				{
					this.motion.x = vector.x * 0.2f;
				}
				if (Utils.FastAbs(vector.z) < 0.3f)
				{
					this.motion.z = vector.z * 0.2f;
				}
				if (vector.sqrMagnitude < 0.010000001f)
				{
					if (this.emodel && this.emodel.avatarController)
					{
						this.emodel.avatarController.StartAnimationJump(AnimJumpMode.Land);
					}
					this.Jumping = false;
				}
			}
			this.entityCollision(this.motion);
			return;
		}
		base.MoveEntityHeaded(_direction, _isDirAbsolute);
	}

	// Token: 0x0600260F RID: 9743 RVA: 0x000E8F48 File Offset: 0x000E7148
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void UpdateJump()
	{
		if (this.walkType == 21 && !this.isSwimming)
		{
			base.FaceJumpTo();
			this.jumpState = EntityAlive.JumpState.Climb;
			if (!this.emodel.avatarController || !this.emodel.avatarController.IsAnimationJumpRunning())
			{
				this.Jumping = false;
			}
			if (this.jumpTicks == 0 && this.accumulatedRootMotion.y > 0.005f)
			{
				this.jumpTicks = 30;
			}
			return;
		}
		base.UpdateJump();
		if (this.isSwimming)
		{
			return;
		}
		this.accumulatedRootMotion.y = 0f;
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x000E8FE1 File Offset: 0x000E71E1
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void EndJump()
	{
		base.EndJump();
		this.IsRotateToGroundFlat = false;
	}

	// Token: 0x06002611 RID: 9745 RVA: 0x000E8FF0 File Offset: 0x000E71F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool ExecuteFallBehavior(EntityAlive.FallBehavior behavior, float _distance, Vector3 _fallMotion)
	{
		if (behavior == null || !this.emodel)
		{
			return false;
		}
		AvatarController avatarController = this.emodel.avatarController;
		if (!avatarController)
		{
			return false;
		}
		avatarController.UpdateInt("RandomSelector", this.rand.RandomRange(0, 64), true);
		switch (behavior.ResponseOp)
		{
		case EntityAlive.FallBehavior.Op.None:
			avatarController.UpdateInt(AvatarController.jumpLandResponseHash, -1, true);
			break;
		case EntityAlive.FallBehavior.Op.Land:
			avatarController.UpdateInt(AvatarController.jumpLandResponseHash, 0, true);
			break;
		case EntityAlive.FallBehavior.Op.LandLow:
			avatarController.UpdateInt(AvatarController.jumpLandResponseHash, 1, true);
			break;
		case EntityAlive.FallBehavior.Op.LandHard:
			avatarController.UpdateInt(AvatarController.jumpLandResponseHash, 2, true);
			break;
		case EntityAlive.FallBehavior.Op.Stumble:
			avatarController.UpdateInt(AvatarController.jumpLandResponseHash, 3, true);
			break;
		case EntityAlive.FallBehavior.Op.Ragdoll:
			this.emodel.DoRagdoll(EModelBase.RagdollMode.Default, this.rand.RandomFloat * 2f, EnumBodyPartHit.None, _fallMotion * 20f, Vector3.zero, false);
			break;
		}
		if (this.attackTarget != null && behavior.RagePer.IsSet() && behavior.RageTime.IsSet() && this.StartRage(behavior.RagePer.Random(this.rand), behavior.RageTime.Random(this.rand)))
		{
			avatarController.StartAnimationRaging();
		}
		return true;
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x000E913C File Offset: 0x000E733C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool ExecuteDestroyBlockBehavior(EntityAlive.DestroyBlockBehavior behavior, ItemActionAttack.AttackHitInfo attackHitInfo)
	{
		if (behavior == null || attackHitInfo == null || this.moveHelper == null || this.emodel == null || this.emodel.avatarController == null)
		{
			return false;
		}
		if (this.walkType == 21)
		{
			return false;
		}
		this.moveHelper.ClearBlocked();
		this.moveHelper.ClearTempMove();
		this.emodel.avatarController.UpdateInt("RandomSelector", this.rand.RandomRange(0, 64), true);
		switch (behavior.ResponseOp)
		{
		case EntityAlive.DestroyBlockBehavior.Op.Ragdoll:
			this.emodel.avatarController.BeginStun(EnumEntityStunType.StumbleBreakThroughRagdoll, EnumBodyPartHit.LeftUpperLeg, Utils.EnumHitDirection.None, false, 1f);
			base.SetStun(EnumEntityStunType.StumbleBreakThroughRagdoll);
			break;
		case EntityAlive.DestroyBlockBehavior.Op.Stumble:
			this.emodel.avatarController.BeginStun(EnumEntityStunType.StumbleBreakThrough, EnumBodyPartHit.LeftUpperLeg, Utils.EnumHitDirection.None, false, 1f);
			base.SetStun(EnumEntityStunType.StumbleBreakThrough);
			this.bodyDamage.StunDuration = 1f;
			break;
		}
		if (this.attackTarget != null && behavior.RagePer.IsSet() && behavior.RageTime.IsSet())
		{
			this.StartRage(behavior.RagePer.Random(this.rand), behavior.RageTime.Random(this.rand));
		}
		return true;
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x000E9284 File Offset: 0x000E7484
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale)
	{
		if (_damageSource.GetDamageType() == EnumDamageTypes.Falling && this.rand.RandomFloat >= 0.25f)
		{
			_strength = (_strength + 1) / 2;
			int num = (this.GetMaxHealth() + 2) / 3;
			if (_strength > num)
			{
				_strength = num;
			}
		}
		return base.DamageEntity(_damageSource, _strength, _criticalHit, impulseScale);
	}

	// Token: 0x06002614 RID: 9748 RVA: 0x000E92D1 File Offset: 0x000E74D1
	public static void SetupRageChance(float chance, int index)
	{
		EntityHuman.rageChance = chance;
		EntityHuman.superRageChance = EntityHuman.superRageChances[index];
	}

	// Token: 0x06002615 RID: 9749 RVA: 0x000E92E8 File Offset: 0x000E74E8
	public override void ProcessDamageResponseLocal(DamageResponse _dmResponse)
	{
		base.ProcessDamageResponseLocal(_dmResponse);
		if (!this.isEntityRemote)
		{
			float num = (float)_dmResponse.Strength / 40f;
			if (num > 1f)
			{
				num = Mathf.Pow(num, 0.29f);
			}
			float num2 = EntityHuman.rageChance * num;
			if (this.rand.RandomFloat < num2)
			{
				if (this.rand.RandomFloat < EntityHuman.superRageChance)
				{
					this.StartRage(2f, 30f);
					this.PlayOneShot(this.GetSoundAlert(), false, false, false, null, 1f);
					return;
				}
				this.StartRage(0.5f + this.rand.RandomFloat * 0.5f, 4f + this.rand.RandomFloat * 6f);
			}
		}
	}

	// Token: 0x06002616 RID: 9750 RVA: 0x000E93AD File Offset: 0x000E75AD
	public bool StartRage(float speedPercent, float time)
	{
		if (speedPercent >= this.moveSpeedRagePer)
		{
			this.moveSpeedRagePer = speedPercent;
			this.moveSpeedScaleTime = time;
			return true;
		}
		return false;
	}

	// Token: 0x06002617 RID: 9751 RVA: 0x000E93C9 File Offset: 0x000E75C9
	public void StopRage()
	{
		this.moveSpeedRagePer = 0f;
		this.moveSpeedScaleTime = 0f;
	}

	// Token: 0x06002618 RID: 9752 RVA: 0x000E93E1 File Offset: 0x000E75E1
	public override void OnEntityDeath()
	{
		base.OnEntityDeath();
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x000E93EC File Offset: 0x000E75EC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void AnalyticsSendDeath(DamageResponse _dmResponse)
	{
		DamageSource source = _dmResponse.Source;
		if (source == null)
		{
			return;
		}
		string text;
		if (source.BuffClass != null)
		{
			text = source.BuffClass.Name;
		}
		else
		{
			if (source.ItemClass == null)
			{
				return;
			}
			text = source.ItemClass.Name;
		}
		GameSparksCollector.IncrementCounter(GameSparksCollector.GSDataKey.ZombiesKilledBy, text, 1, true, GameSparksCollector.GSDataCollection.SessionUpdates);
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x000E943C File Offset: 0x000E763C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void TurnIntoCrawler()
	{
		BoxCollider component = base.gameObject.GetComponent<BoxCollider>();
		if (component)
		{
			component.center = new Vector3(0f, 0.35f, 0f);
			component.size = new Vector3(0.8f, 0.8f, 0.8f);
		}
		base.SetupBounds();
		this.boundingBox.center = this.boundingBox.center + this.position;
		this.bCanClimbLadders = false;
	}

	// Token: 0x0600261B RID: 9755 RVA: 0x000E94BA File Offset: 0x000E76BA
	public override void BuffAdded(BuffValue _buff)
	{
		if (_buff.BuffClass.DamageType == EnumDamageTypes.Electrical)
		{
			this.Electrocuted = true;
		}
	}

	// Token: 0x04001C43 RID: 7235
	public ulong timeToDie;

	// Token: 0x04001C44 RID: 7236
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float moveSpeedRagePer;

	// Token: 0x04001C45 RID: 7237
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float moveSpeedScaleTime;

	// Token: 0x04001C46 RID: 7238
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fallTime;

	// Token: 0x04001C47 RID: 7239
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float fallThresholdTime;

	// Token: 0x04001C48 RID: 7240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] moveSpeeds = new float[] { 0f, 0.35f, 0.7f, 1f, 1.35f };

	// Token: 0x04001C49 RID: 7241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] moveRageSpeeds = new float[] { 0.75f, 0.8f, 0.9f, 1.15f, 1.7f };

	// Token: 0x04001C4A RID: 7242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] moveSuperRageSpeeds = new float[] { 0.88f, 0.92f, 1f, 1.2f, 1.7f };

	// Token: 0x04001C4B RID: 7243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int moveSpeedPatternIndex;

	// Token: 0x04001C4C RID: 7244
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float moveSpeedPatternScale = 1f;

	// Token: 0x04001C4D RID: 7245
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float moveSpeedPatternDelay;

	// Token: 0x04001C4E RID: 7246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<float> moveSpeedPattern;

	// Token: 0x04001C4F RID: 7247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float[] superRageChances = new float[] { 0f, 0.01f, 0.03f, 0.05f, 0.08f, 0.15f, 0.18f, 0.2f, 0.25f, 0.3f };

	// Token: 0x04001C50 RID: 7248
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float rageChance;

	// Token: 0x04001C51 RID: 7249
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static float superRageChance;
}
