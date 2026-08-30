using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using Audio;
using GamePath;
using Platform;
using PrefabVolumes;
using UAI;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200046F RID: 1135
[Preserve]
public abstract class EntityAlive : Entity
{
	// Token: 0x170003ED RID: 1005
	// (get) Token: 0x0600221F RID: 8735 RVA: 0x000CDF38 File Offset: 0x000CC138
	// (set) Token: 0x06002220 RID: 8736 RVA: 0x000CDF43 File Offset: 0x000CC143
	public bool IsEquipping
	{
		get
		{
			return this.equippingCount > 0;
		}
		set
		{
			if (value)
			{
				this.equippingCount++;
				return;
			}
			if (this.equippingCount > 0)
			{
				this.equippingCount--;
			}
		}
	}

	// Token: 0x170003EE RID: 1006
	// (get) Token: 0x06002221 RID: 8737 RVA: 0x000CDF6E File Offset: 0x000CC16E
	public bool HasAI
	{
		get
		{
			return this.hasAI;
		}
	}

	// Token: 0x170003EF RID: 1007
	// (get) Token: 0x06002222 RID: 8738 RVA: 0x000CDF76 File Offset: 0x000CC176
	public static bool IsHeadshotOnly
	{
		get
		{
			return EntityAlive.HeadshotMode == EntityAlive.HeadShotOnlyModes.HeadshotOnly;
		}
	}

	// Token: 0x170003F0 RID: 1008
	// (get) Token: 0x06002223 RID: 8739 RVA: 0x000CDF80 File Offset: 0x000CC180
	public static bool IsHeadshotFinisher
	{
		get
		{
			return EntityAlive.HeadshotMode == EntityAlive.HeadShotOnlyModes.HeadshotFinisher;
		}
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x06002224 RID: 8740 RVA: 0x000CDF8A File Offset: 0x000CC18A
	public static bool IsCelebrate
	{
		get
		{
			return EntityAlive.CelebrateMode == EntityAlive.CelebrateModes.Enabled;
		}
	}

	// Token: 0x170003F2 RID: 1010
	// (get) Token: 0x06002225 RID: 8741 RVA: 0x000CDF94 File Offset: 0x000CC194
	public static bool IsCelebrateHeadshot
	{
		get
		{
			return EntityAlive.CelebrateMode == EntityAlive.CelebrateModes.HeadshotOnly;
		}
	}

	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06002226 RID: 8742 RVA: 0x000CDF9E File Offset: 0x000CC19E
	// (set) Token: 0x06002227 RID: 8743 RVA: 0x000CDFA8 File Offset: 0x000CC1A8
	public bool IsDancing
	{
		get
		{
			return this.isDancing;
		}
		set
		{
			this.isDancing = value;
			if (value)
			{
				if (this.emodel != null && this.emodel.avatarController != null)
				{
					this.emodel.avatarController.UpdateInt("IsDancing", base.EntityClass.DanceTypeID, true);
					return;
				}
			}
			else if (this.emodel != null && this.emodel.avatarController != null)
			{
				this.emodel.avatarController.UpdateInt("IsDancing", 0, true);
			}
		}
	}

	// Token: 0x06002228 RID: 8744 RVA: 0x000CE03A File Offset: 0x000CC23A
	public void BeginDynamicRagdoll(DynamicRagdollFlags flags, FloatRange stunTime)
	{
		this._dynamicRagdoll = flags;
		this._dynamicRagdollRootMotion = Vector3.zero;
		this._dynamicRagdollStunTime = stunTime.Random(this.rand);
	}

	// Token: 0x06002229 RID: 8745 RVA: 0x000CE064 File Offset: 0x000CC264
	public void ActivateDynamicRagdoll()
	{
		if (this._dynamicRagdoll.HasFlag(DynamicRagdollFlags.Active))
		{
			DynamicRagdollFlags dynamicRagdoll = this._dynamicRagdoll;
			this._dynamicRagdoll = DynamicRagdollFlags.None;
			Vector3 vector = this._dynamicRagdollRootMotion * 20f;
			this.bodyDamage.StunDuration = this._dynamicRagdollStunTime;
			this.emodel.DoRagdoll(EModelBase.RagdollMode.Default, this._dynamicRagdollStunTime, EnumBodyPartHit.None, vector, Vector3.zero, true);
			if (dynamicRagdoll.HasFlag(DynamicRagdollFlags.UseBoneVelocities) && this._ragdollPositionsPrev.Count == this._ragdollPositionsCur.Count)
			{
				List<Vector3> list = new List<Vector3>();
				for (int i = 0; i < this._ragdollPositionsPrev.Count; i++)
				{
					Vector3 vector2 = this._ragdollPositionsCur[i] - this._ragdollPositionsPrev[i];
					list.Add(vector2 * 20f);
				}
				this.emodel.ApplyRagdollVelocities(list);
			}
		}
	}

	// Token: 0x0600222A RID: 8746 RVA: 0x000CE15C File Offset: 0x000CC35C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Awake()
	{
		base.Awake();
		this.entityName = base.GetType().Name;
		this.MinEventContext.Self = this;
		this.seeCache = new EntitySeeCache(this);
		this.maximumHomeDistance = -1;
		this.homePosition = new ChunkCoordinates(0, 0, 0);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !(this is EntityPlayer))
		{
			this.hasAI = true;
			this.navigator = AstarManager.CreateNavigator(this);
			this.aiManager = new EAIManager(this);
			this.lookHelper = new EntityLookHelper(this);
			this.moveHelper = new EntityMoveHelper(this);
		}
		this.equipment = new Equipment(this);
		this.InitInventory();
		this.stepHeight = 0.52f;
		this.soundDelayTicks = this.GetSoundRandomTicks() / 3 - 5;
		this.spawnPoints = new EntityBedrollPositionList(this);
		this.CreationTimeSinceLevelLoad = Time.timeSinceLevelLoad;
		this.Buffs = new EntityBuffs(this);
		this.droppedBackpackPositions = new List<Vector3i>();
	}

	// Token: 0x0600222B RID: 8747 RVA: 0x000CE253 File Offset: 0x000CC453
	public override void Init(int _entityClass, EntityInstanceAssets _assets, EModelInstanceAssets _eModelAssets)
	{
		base.Init(_entityClass, _assets, _eModelAssets);
		this.InitStats();
		this.switchModelView(EnumEntityModelView.ThirdPerson);
		this.InitPostCommon();
	}

	// Token: 0x0600222C RID: 8748 RVA: 0x000CE274 File Offset: 0x000CC474
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitPostCommon()
	{
		if (GameManager.IsDedicatedServer)
		{
			Transform modelTransform = this.emodel.GetModelTransform();
			if (modelTransform)
			{
				ServerHelper.SetupForServer(modelTransform.gameObject);
			}
		}
		this.AddCharacterController();
		this.wasSeenByPlayer = false;
		this.ticksToCheckSeenByPlayer = 20;
		if (EntityClass.list[this.entityClass].UseAIPackages)
		{
			this.hasAI = true;
			this.AIPackages = new List<string>();
			this.AIPackages.AddRange(EntityClass.list[this.entityClass].AIPackages);
			this.utilityAIContext = new Context(this);
		}
		List<string> buffs = EntityClass.list[this.entityClass].Buffs;
		if (buffs != null)
		{
			for (int i = 0; i < buffs.Count; i++)
			{
				string text = buffs[i];
				if (!this.Buffs.HasBuff(text))
				{
					this.Buffs.AddBuff(text, -1, true, false, -1f);
				}
			}
		}
		if ((this.entityFlags & EntityFlags.AIHearing) > EntityFlags.None)
		{
			this.emodel.SetVisible(false, false);
			this.emodel.SetFade(0f);
		}
	}

	// Token: 0x0600222D RID: 8749 RVA: 0x000CE390 File Offset: 0x000CC590
	public override void PostInit()
	{
		base.PostInit();
		this.ApplySpawnState();
		LODGroup componentInChildren = this.emodel.GetModelTransform().GetComponentInChildren<LODGroup>();
		if (componentInChildren)
		{
			LOD[] lods = componentInChildren.GetLODs();
			lods[lods.Length - 1].screenRelativeTransitionHeight = 0.003f;
			componentInChildren.SetLODs(lods);
		}
		this.disableFallBehaviorUntilOnGround = true;
		GameEventManager.Current.HandleSpawnModifier(this);
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x000CE3F7 File Offset: 0x000CC5F7
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplySpawnState()
	{
		if (this.Health <= 0 && this.isEntityRemote)
		{
			this.ClientKill(DamageResponse.New(true));
		}
		this.ExecuteDismember(true);
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x000CE41D File Offset: 0x000CC61D
	public virtual void InitInventory()
	{
		if (this.inventory == null)
		{
			this.inventory = new Inventory(GameManager.Instance, this);
		}
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000CE438 File Offset: 0x000CC638
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void switchModelView(EnumEntityModelView modelView)
	{
		this.emodel.SwitchModelAndView(modelView == EnumEntityModelView.FirstPerson, this.IsMale);
		this.ReassignEquipmentTransforms();
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000CE458 File Offset: 0x000CC658
	public virtual void ReassignEquipmentTransforms()
	{
		if (this.isFirstTimeEquipmentReassigned)
		{
			this.Buffs.SetCustomVar("_equipReload", 0f, true, CVarOperation.set);
			this.isFirstTimeEquipmentReassigned = false;
		}
		else
		{
			this.Buffs.SetCustomVar("_equipReload", 1f, true, CVarOperation.set);
		}
		this.equipment.InitializeEquipmentTransforms();
		this.Buffs.SetCustomVar("_equipReload", 0f, true, CVarOperation.set);
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000CE4C8 File Offset: 0x000CC6C8
	public override void CopyPropertiesFromEntityClass()
	{
		base.CopyPropertiesFromEntityClass();
		EntityClass entityClass = EntityClass.list[this.entityClass];
		string text = entityClass.Properties.GetString(EntityClass.PropHandItem);
		if (text.Length > 0)
		{
			int num = text.IndexOf(",");
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			this.handItem = ItemClass.GetItem(text, false);
			if (this.handItem.IsEmpty())
			{
				throw new Exception("HandItem missing " + text);
			}
		}
		else
		{
			this.handItem = ItemClass.GetItem("meleeHandPlayer", false).Clone();
		}
		if (this.inventory != null)
		{
			this.inventory.SetBareHandItem(this.handItem);
		}
		this.rightHandTransformName = "Gunjoint";
		if (this.emodel is EModelSDCS)
		{
			this.rightHandTransformName = "RightWeapon";
		}
		entityClass.Properties.ParseString(EntityClass.PropRightHandJointName, ref this.rightHandTransformName);
		if (!(this is EntityPlayer))
		{
			this.factionId = 0;
			this.factionRank = 0;
			string @string = entityClass.Properties.GetString("Faction");
			if (@string.Length > 0)
			{
				Faction factionByName = FactionManager.Instance.GetFactionByName(@string);
				if (factionByName != null)
				{
					this.factionId = factionByName.ID;
					string string2 = entityClass.Properties.GetString("FactionRank");
					if (string2.Length > 0)
					{
						this.factionRank = StringParsers.ParseUInt8(string2, 0, -1, NumberStyles.Integer);
					}
				}
			}
		}
		else if (FactionManager.Instance.GetFaction(this.factionId).ID == 0)
		{
			this.factionId = FactionManager.Instance.CreateFaction(this.entityName, true, "").ID;
			this.factionRank = byte.MaxValue;
		}
		this.maxViewAngle = 180f;
		entityClass.Properties.ParseFloat(EntityClass.PropMaxViewAngle, ref this.maxViewAngle);
		this.sightRangeBase = entityClass.SightRange;
		this.sightLightThreshold = entityClass.sightLightThreshold;
		this.SetSleeperSight(-1f, -1f);
		this.sightWakeThresholdAtRange.x = this.rand.RandomRange(entityClass.SleeperSightToWakeMin.x, entityClass.SleeperSightToWakeMin.y);
		this.sightWakeThresholdAtRange.y = this.rand.RandomRange(entityClass.SleeperSightToWakeMax.y, entityClass.SleeperSightToWakeMax.y);
		this.sightGroanThresholdAtRange.x = this.rand.RandomRange(entityClass.SleeperSightToSenseMin.x, entityClass.SleeperSightToSenseMin.y);
		this.sightGroanThresholdAtRange.y = this.rand.RandomRange(entityClass.SleeperSightToSenseMax.y, entityClass.SleeperSightToSenseMax.y);
		this.sleeperNoiseToSense = this.rand.RandomRange(entityClass.SleeperNoiseToSense.x, entityClass.SleeperNoiseToSense.y);
		this.sleeperNoiseToSenseSoundChance = entityClass.SleeperNoiseToSenseSoundChance;
		this.sleeperNoiseToWake = this.rand.RandomRange(entityClass.SleeperNoiseToWake.x, entityClass.SleeperNoiseToWake.y);
		float num2 = 1f;
		entityClass.Properties.ParseFloat(EntityClass.PropAttackTimeoutDay, ref num2);
		this.attackTimeoutDay = (int)(num2 * 20f);
		entityClass.Properties.ParseFloat(EntityClass.PropAttackTimeoutNight, ref num2);
		this.attackTimeoutNight = (int)(num2 * 20f);
		entityClass.Properties.ParseBool(EntityClass.PropStompsSpikes, ref this.stompsSpikes);
		this.weight = 1f;
		entityClass.Properties.ParseFloat(EntityClass.PropWeight, ref this.weight);
		this.weight = Utils.FastMax(this.weight, 0.5f);
		this.pushFactor = 1f;
		entityClass.Properties.ParseFloat(EntityClass.PropPushFactor, ref this.pushFactor);
		float num3 = 5f;
		entityClass.Properties.ParseFloat(EntityClass.PropTimeStayAfterDeath, ref num3);
		this.timeStayAfterDeath = (int)(num3 * 20f);
		this.IsMale = true;
		entityClass.Properties.ParseBool(EntityClass.PropIsMale, ref this.IsMale);
		this.IsFeral = entityClass.Tags.Test_Bit(EntityAlive.FeralTagBit);
		this.proneRefillRate = this.rand.RandomRange(entityClass.KnockdownProneRefillRate.x, entityClass.KnockdownProneRefillRate.y);
		this.kneelRefillRate = this.rand.RandomRange(entityClass.KnockdownKneelRefillRate.x, entityClass.KnockdownKneelRefillRate.y);
		this.moveSpeed = 1f;
		entityClass.Properties.ParseFloat(EntityClass.PropMoveSpeed, ref this.moveSpeed);
		this.moveSpeedNight = this.moveSpeed;
		entityClass.Properties.ParseFloat(EntityClass.PropMoveSpeedNight, ref this.moveSpeedNight);
		this.moveSpeedAggro = this.moveSpeed;
		this.moveSpeedAggroMax = this.moveSpeed;
		entityClass.Properties.ParseVec(EntityClass.PropMoveSpeedAggro, ref this.moveSpeedAggro, ref this.moveSpeedAggroMax);
		this.moveSpeedPanic = 1f;
		this.moveSpeedPanicMax = 1f;
		entityClass.Properties.ParseFloat(EntityClass.PropMoveSpeedPanic, ref this.moveSpeedPanic);
		if (this.moveSpeedPanic != 1f)
		{
			this.moveSpeedPanicMax = this.moveSpeedPanic;
		}
		entityClass.Properties.ParseFloat(EntityClass.PropSwimSpeed, ref this.swimSpeed);
		entityClass.Properties.ParseVec(EntityClass.PropSwimStrokeRate, ref this.swimStrokeRate);
		Vector2 negativeInfinity = Vector2.negativeInfinity;
		entityClass.Properties.ParseVec(EntityClass.PropMoveSpeedRand, ref negativeInfinity);
		if (negativeInfinity.x > -1f)
		{
			float num4 = this.rand.RandomRange(negativeInfinity.x, negativeInfinity.y);
			if (this.moveSpeedAggro < 1f)
			{
				this.moveSpeedAggro += num4;
				if (this.moveSpeedAggro < 0.1f)
				{
					this.moveSpeedAggro = 0.1f;
				}
				if (this.moveSpeedAggro > this.moveSpeedAggroMax)
				{
					this.moveSpeedAggro = this.moveSpeedAggroMax;
				}
			}
		}
		entityClass.Properties.ParseInt(EntityClass.PropCrouchType, ref this.crouchType);
		this.walkType = EntityAlive.GetSpawnWalkType(entityClass);
		entityClass.Properties.ParseBool(EntityClass.PropCanClimbLadders, ref this.bCanClimbLadders);
		entityClass.Properties.ParseBool(EntityClass.PropCanClimbVertical, ref this.bCanClimbVertical);
		Vector2 vector = new Vector2(1.9f, 2.1f);
		entityClass.Properties.ParseVec(EntityClass.PropJumpMaxDistance, ref vector);
		this.jumpMaxDistance = this.rand.RandomRange(vector.x, vector.y);
		this.jumpDelay = 1f;
		entityClass.Properties.ParseFloat(EntityClass.PropJumpDelay, ref this.jumpDelay);
		this.jumpDelay *= 20f;
		this.ExperienceValue = 20;
		entityClass.Properties.ParseInt(EntityClass.PropExperienceGain, ref this.ExperienceValue);
		if (this.aiManager != null)
		{
			this.aiManager.CopyPropertiesFromEntityClass(entityClass);
		}
		entityClass.Properties.ParseString(EntityClass.PropSoundSpawn, ref this.soundSpawn);
		entityClass.Properties.ParseString(EntityClass.PropSoundSleeperSense, ref this.soundSleeperGroan);
		entityClass.Properties.ParseString(EntityClass.PropSoundSleeperSnore, ref this.soundSleeperSnore);
		entityClass.Properties.ParseString(EntityClass.PropSoundDeath, ref this.soundDeath);
		entityClass.Properties.ParseString(EntityClass.PropSoundAlert, ref this.soundAlert);
		entityClass.Properties.ParseString(EntityClass.PropSoundAttack, ref this.soundAttack);
		entityClass.Properties.ParseString(EntityClass.PropSoundLiving, ref this.soundLiving);
		entityClass.Properties.ParseString(EntityClass.PropSoundRandom, ref this.soundRandom);
		entityClass.Properties.ParseString(EntityClass.PropSoundSense, ref this.soundSense);
		entityClass.Properties.ParseString(EntityClass.PropSoundGiveUp, ref this.soundGiveUp);
		this.soundStepType = "step";
		entityClass.Properties.ParseString(EntityClass.PropSoundStepType, ref this.soundStepType);
		entityClass.Properties.ParseString(EntityClass.PropSoundStamina, ref this.soundStamina);
		entityClass.Properties.ParseString(EntityClass.PropSoundJump, ref this.soundJump);
		entityClass.Properties.ParseString(EntityClass.PropSoundLand, ref this.soundLand);
		entityClass.Properties.ParseString(EntityClass.PropSoundPlayerLandThump, ref this.soundLandThump);
		entityClass.Properties.ParseString(EntityClass.PropSoundHurt, ref this.soundHurt);
		entityClass.Properties.ParseString(EntityClass.PropSoundHurtSmall, ref this.soundHurtSmall);
		entityClass.Properties.ParseString(EntityClass.PropSoundDrownPain, ref this.soundDrownPain);
		entityClass.Properties.ParseString(EntityClass.PropSoundDrownDeath, ref this.soundDrownDeath);
		entityClass.Properties.ParseString(EntityClass.PropSoundWaterSurface, ref this.soundWaterSurface);
		this.soundAlertTicks = 25;
		entityClass.Properties.ParseInt(EntityClass.PropSoundAlertTime, ref this.soundAlertTicks);
		this.soundAlertTicks *= 20;
		this.soundRandomTicks = 25;
		entityClass.Properties.ParseInt(EntityClass.PropSoundRandomTime, ref this.soundRandomTicks);
		this.soundRandomTicks *= 20;
		entityClass.Properties.ParseString(EntityClass.PropParticleOnDeath, ref this.particleOnDeath);
		entityClass.Properties.ParseString(EntityClass.PropParticleOnDestroy, ref this.particleOnDestroy);
		GameMode gameModeForId = GameMode.GetGameModeForId(GameStats.GetInt(EnumGameStats.GameModeId));
		if (gameModeForId != null)
		{
			string text2 = string.Empty;
			if (entityClass.Properties.Classes.ContainsKey(EntityClass.PropItemsOnEnterGame))
			{
				text2 = entityClass.Properties.Classes[EntityClass.PropItemsOnEnterGame].GetString(gameModeForId.GetTypeName());
			}
			if (text2.Length > 0)
			{
				foreach (string text3 in text2.Split(',', StringSplitOptions.None))
				{
					ItemStack itemStack = ItemStack.FromString(text3.Trim());
					if (itemStack.itemValue.IsEmpty())
					{
						throw new Exception("Item with name '" + text3 + "' not found in class " + EntityClass.list[this.entityClass].entityClassName);
					}
					if (itemStack.itemValue.ItemClass.CreativeMode != EnumCreativeMode.Console || (DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5).IsCurrent())
					{
						this.itemsOnEnterGame.Add(itemStack);
					}
				}
			}
		}
		DynamicProperties @class = entityClass.Properties.GetClass(EntityClass.PropFallLandBehavior);
		if (@class != null)
		{
			foreach (KeyValuePair<string, string> keyValuePair in @class.Data)
			{
				string key = keyValuePair.Key;
				Dictionary<string, string> dictionary = @class.ParseKeyData(key);
				if (dictionary != null)
				{
					FloatRange floatRange = default(FloatRange);
					FloatRange floatRange2 = default(FloatRange);
					FloatRange floatRange3 = default(FloatRange);
					IntRange intRange = new IntRange(0, 10);
					string text4;
					EntityAlive.FallBehavior.Op op;
					if (!dictionary.TryGetValue("anim", out text4) || !Enum.TryParse<EntityAlive.FallBehavior.Op>(text4, out op))
					{
						Log.Error("Expected 'anim' parameter as float for FallBehavior " + key + ", skipping");
					}
					else
					{
						float num5 = 0f;
						if (!dictionary.TryGetValue("weight", out text4) || !StringParsers.TryParseFloat(text4, out num5))
						{
							Log.Error("Expected 'weight' parameter as float for FallBehavior " + key + ", skipping");
						}
						else if (dictionary.TryGetValue("height", out text4))
						{
							FloatRange floatRange4;
							if (StringParsers.TryParseRange(text4, out floatRange4, new float?(3.4028235E+38f), ','))
							{
								floatRange = floatRange4;
								if (dictionary.TryGetValue("ragePer", out text4))
								{
									FloatRange floatRange5;
									if (!StringParsers.TryParseRange(text4, out floatRange5, null, ','))
									{
										Log.Error("Expected 'ragePer' parameter as range(min,min-max) " + key + ", skipping");
										continue;
									}
									floatRange2 = floatRange5;
								}
								if (dictionary.TryGetValue("rageTime", out text4))
								{
									FloatRange floatRange6;
									if (!StringParsers.TryParseRange(text4, out floatRange6, null, ','))
									{
										Log.Error("Expected 'rageTime' parameter as range(min,min-max) " + key + ", skipping");
										continue;
									}
									floatRange3 = floatRange6;
								}
								if (dictionary.TryGetValue("difficulty", out text4))
								{
									IntRange intRange2;
									if (!StringParsers.TryParseRange(text4, out intRange2, null, ','))
									{
										Log.Error("Expected 'difficulty' parameter as range(min,min-max) " + key + ", skipping");
										continue;
									}
									intRange = intRange2;
								}
								this.fallBehaviors.Add(new EntityAlive.FallBehavior(key, op, floatRange, num5, floatRange2, floatRange3, intRange));
							}
							else
							{
								Log.Error("Expected 'height' parameter as range(min,min-max) " + key + ", skipping");
							}
						}
						else
						{
							Log.Error("Expected 'height' parameter for FallBehavior " + key + ", skipping");
						}
					}
				}
			}
		}
		DynamicProperties class2 = entityClass.Properties.GetClass(EntityClass.PropDestroyBlockBehavior);
		if (class2 != null)
		{
			EntityAlive.DestroyBlockBehavior.Op[] array2 = Enum.GetValues(typeof(EntityAlive.DestroyBlockBehavior.Op)) as EntityAlive.DestroyBlockBehavior.Op[];
			for (int j = 0; j < array2.Length; j++)
			{
				string text5 = array2[j].ToStringCached<EntityAlive.DestroyBlockBehavior.Op>();
				Dictionary<string, string> dictionary2 = class2.ParseKeyData(array2[j].ToStringCached<EntityAlive.DestroyBlockBehavior.Op>());
				if (dictionary2 != null)
				{
					FloatRange floatRange7 = default(FloatRange);
					FloatRange floatRange8 = default(FloatRange);
					IntRange intRange3 = new IntRange(0, 10);
					string text6;
					float num6;
					if (!dictionary2.TryGetValue("weight", out text6) || !StringParsers.TryParseFloat(text6, out num6))
					{
						Log.Error(string.Format("Expected 'weight' parameter as float for FallBehavior {0}, skipping", array2[j]));
					}
					else
					{
						if (dictionary2.TryGetValue("ragePer", out text6))
						{
							FloatRange floatRange9;
							if (!StringParsers.TryParseRange(text6, out floatRange9, null, ','))
							{
								Log.Error(string.Format("Expected 'ragePer' parameter as range(min,min-max) {0}, skipping", array2[j]));
								goto IL_0E01;
							}
							floatRange7 = floatRange9;
						}
						if (dictionary2.TryGetValue("rageTime", out text6))
						{
							FloatRange floatRange10;
							if (!StringParsers.TryParseRange(text6, out floatRange10, null, ','))
							{
								Log.Error(string.Format("Expected 'rageTime' parameter as range(min,min-max) {0}, skipping", array2[j]));
								goto IL_0E01;
							}
							floatRange8 = floatRange10;
						}
						if (dictionary2.TryGetValue("difficulty", out text6))
						{
							IntRange intRange4;
							if (!StringParsers.TryParseRange(text6, out intRange4, null, ','))
							{
								Log.Error("Expected 'difficulty' parameter as range(min,min-max) " + text5 + ", skipping");
								goto IL_0E01;
							}
							intRange3 = intRange4;
						}
						this._destroyBlockBehaviors.Add(new EntityAlive.DestroyBlockBehavior(text5, array2[j], num6, floatRange7, floatRange8, intRange3));
					}
				}
				IL_0E01:;
			}
		}
		this.distractionResistance = EffectManager.GetValue(PassiveEffects.DistractionResistance, null, 0f, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		this.distractionResistanceWithTarget = EffectManager.GetValue(PassiveEffects.DistractionResistance, null, 0f, this, null, EntityAlive.DistractionResistanceWithTargetTags, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000CF34C File Offset: 0x000CD54C
	public void SetInventorySlots(string _handItemName)
	{
		if (_handItemName.Contains(" "))
		{
			_handItemName = _handItemName.Replace(" ", "");
		}
		string[] array = _handItemName.Split(",", StringSplitOptions.None);
		if (this.inventory != null)
		{
			ItemStack[] array2 = new ItemStack[array.Length - 1];
			for (int i = 1; i < array.Length; i++)
			{
				string text = array[i];
				ItemStack itemStack;
				if (text.Length == 0)
				{
					itemStack = ItemStack.Empty;
				}
				else
				{
					itemStack = ItemStack.FromString(text);
					if (itemStack.itemValue.IsEmpty())
					{
						Log.Error("HandItem missing " + text);
					}
				}
				array2[i - 1] = itemStack;
			}
			this.inventory.SetSlots(array2, true);
		}
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000CF3F8 File Offset: 0x000CD5F8
	public static int GetSpawnWalkType(EntityClass _entityClass)
	{
		int num = 0;
		_entityClass.Properties.ParseInt(EntityClass.PropWalkType, ref num);
		return num;
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000CF41C File Offset: 0x000CD61C
	public override void VisiblityCheck(float _distanceSqr, bool _isZoom)
	{
		if ((this.entityFlags & EntityFlags.AIHearing) > EntityFlags.None)
		{
			if (GameManager.IsDedicatedServer)
			{
				this.emodel.SetVisible(true, false);
				return;
			}
			if (_distanceSqr < (float)(_isZoom ? 14400 : 8100))
			{
				this.renderFadeTarget = this.renderFadeMax;
				return;
			}
			this.renderFadeTarget = 0f;
		}
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000CF477 File Offset: 0x000CD677
	public virtual void SetSleeper()
	{
		this.IsSleeper = true;
		this.aiManager.pathCostScale += 0.2f;
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000CF497 File Offset: 0x000CD697
	public void SetSleeperSight(float angle, float range)
	{
		if (angle < 0f)
		{
			angle = this.maxViewAngle;
		}
		this.sleeperViewAngle = angle;
		if (range < 0f)
		{
			range = Utils.FastMax(3f, this.sightRangeBase * 0.2f);
		}
		this.sleeperSightRange = range;
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x000CF4D7 File Offset: 0x000CD6D7
	public void SetSleeperHearing(float percent)
	{
		if (percent < 0.001f)
		{
			percent = 0.001f;
		}
		percent = 1f / percent;
		this.sleeperNoiseToSense *= percent;
		this.sleeperNoiseToWake *= percent;
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000CF510 File Offset: 0x000CD710
	public int GetSleeperDisturbedLevel(float dist, float lightLevel)
	{
		float num = dist / this.sightRangeBase;
		if (num <= 1f)
		{
			float num2 = Mathf.Lerp(this.sightWakeThresholdAtRange.x, this.sightWakeThresholdAtRange.y, num);
			if (lightLevel > num2)
			{
				return 2;
			}
			float num3 = Mathf.Lerp(this.sightGroanThresholdAtRange.x, this.sightGroanThresholdAtRange.y, num);
			if (lightLevel > num3)
			{
				return 1;
			}
		}
		return 0;
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x000CF578 File Offset: 0x000CD778
	public void GetSleeperDebugScale(float dist, out float wake, out float groan)
	{
		float num = dist / this.sightRangeBase;
		wake = Mathf.Lerp(this.sightWakeThresholdAtRange.x, this.sightWakeThresholdAtRange.y, num);
		groan = Mathf.Lerp(this.sightGroanThresholdAtRange.x, this.sightGroanThresholdAtRange.y, num);
	}

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x0600223B RID: 8763 RVA: 0x000CF5CA File Offset: 0x000CD7CA
	public bool sleepingOrWakingUp
	{
		get
		{
			return this.IsSleeping;
		}
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x000CF5D4 File Offset: 0x000CD7D4
	public void TriggerSleeperPose(int _pose, bool _returningToSleep = false)
	{
		if (this.IsDead())
		{
			return;
		}
		if (this.emodel && this.emodel.avatarController)
		{
			this.emodel.avatarController.TriggerSleeperPose(_pose, _returningToSleep);
			this.pendingSleepTrigger = -1;
			if (_pose != 5)
			{
				this.physicsHeight = 0.85f;
			}
		}
		else
		{
			this.pendingSleepTrigger = _pose;
		}
		this.lastSleeperPose = _pose;
		this.IsSleeping = true;
		this.SleeperSupressLivingSounds = true;
		this.sleeperLookDir = Quaternion.AngleAxis(this.rotation.y, Vector3.up) * this.SleeperSpawnLookDir;
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x000CF675 File Offset: 0x000CD875
	public void ResumeSleeperPose()
	{
		this.TriggerSleeperPose(this.lastSleeperPose, true);
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000CF684 File Offset: 0x000CD884
	public void ConditionalTriggerSleeperWakeUp()
	{
		if (this.IsSleeping && !this.IsDead())
		{
			this.IsSleeping = false;
			this.IsSleeperPassive = false;
			int num = ((this.physicsHeight < 1f && !this.IsWalkTypeACrawl()) ? (-2) : (-1));
			this.emodel.avatarController.TriggerSleeperPose(num, false);
			if (this.aiManager != null)
			{
				this.aiManager.SleeperWokeUp();
			}
			if (!this.world.IsRemote())
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSleeperWakeup>().Setup(this.entityId), false, -1, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000CF730 File Offset: 0x000CD930
	public void SetSleeperActive()
	{
		if (this.IsSleeperPassive)
		{
			this.IsSleeperPassive = false;
			if (!this.world.IsRemote())
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageSleeperPassiveChange>().Setup(this.entityId), false, -1, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x000CF786 File Offset: 0x000CD986
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void InitStats()
	{
		this.entityStats = new EntityStats(this);
		this.startOfFrameStats = new EntityStats(this);
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x000CF7A0 File Offset: 0x000CD9A0
	public void SetStats(EntityStats _stats)
	{
		this.entityStats.CopyFrom(_stats);
	}

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06002242 RID: 8770 RVA: 0x000CF7AE File Offset: 0x000CD9AE
	public EntityStats Stats
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.entityStats;
		}
	}

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06002243 RID: 8771 RVA: 0x000CF7B6 File Offset: 0x000CD9B6
	public EntityStats StartOfFrameStats
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			return this.startOfFrameStats;
		}
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000CF7BE File Offset: 0x000CD9BE
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual ItemValue GetHandItem()
	{
		return this.handItem;
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x000CF7C6 File Offset: 0x000CD9C6
	public bool IsHoldingLight()
	{
		return this.inventory.IsFlashlightOn;
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x000027FC File Offset: 0x000009FC
	public void CycleActivatableItems()
	{
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000CF7D4 File Offset: 0x000CD9D4
	public List<ItemValue> GetActivatableItemPool()
	{
		List<ItemValue> list = new List<ItemValue>();
		this.CollectActivatableItems(list);
		return list;
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000CF7F0 File Offset: 0x000CD9F0
	public void CollectActivatableItems(List<ItemValue> _pool)
	{
		if (this.inventory != null)
		{
			EntityAlive.GetActivatableItems(this.inventory.holdingItemItemValue, _pool);
		}
		if (this.equipment != null)
		{
			int slotCount = this.equipment.GetSlotCount();
			for (int i = 0; i < slotCount; i++)
			{
				EntityAlive.GetActivatableItems(this.equipment.GetSlotItemOrNone(i), _pool);
			}
		}
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000CF848 File Offset: 0x000CDA48
	[PublicizedFrom(EAccessModifier.Private)]
	public static void GetActivatableItems(ItemValue _item, List<ItemValue> _itemPool)
	{
		ItemClass itemClass = _item.ItemClass;
		if (itemClass == null)
		{
			return;
		}
		if (itemClass.HasTrigger(MinEventTypes.onSelfItemActivate))
		{
			_itemPool.Add(_item);
		}
		for (int i = 0; i < _item.Modifications.Length; i++)
		{
			ItemValue itemValue = _item.Modifications[i];
			if (itemValue != null)
			{
				ItemClass itemClass2 = itemValue.ItemClass;
				if (itemClass2 != null && itemClass2.HasTrigger(MinEventTypes.onSelfItemActivate))
				{
					_itemPool.Add(itemValue);
				}
			}
		}
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000CF8AC File Offset: 0x000CDAAC
	public override void OnUpdatePosition(float _partialTicks)
	{
		float num = Utils.DeltaAngle(this.rotation.y, this.prevRotation.y);
		base.OnUpdatePosition(_partialTicks);
		Vector3 vector = Vector3.zero;
		for (int i = 0; i < this.lastTickPos.Length - 1; i++)
		{
			vector.x += this.lastTickPos[i].x - this.lastTickPos[i + 1].x;
			vector.z += this.lastTickPos[i].z - this.lastTickPos[i + 1].z;
		}
		vector += this.position - this.lastTickPos[0];
		vector /= (float)this.lastTickPos.Length;
		if (this.AttachedToEntity == null)
		{
			this.updateStepSound(vector.x, vector.z, num);
		}
		if (!this.RootMotion && !this.isEntityRemote)
		{
			this.updateSpeedForwardAndStrafe(vector, _partialTicks);
		}
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x000CF9C0 File Offset: 0x000CDBC0
	public void Snore()
	{
		if (!this.isSnore && this.isGroan && this.snoreGroanCD <= 0)
		{
			this.isSnore = true;
			this.isGroan = false;
			this.snoreGroanCD = this.rand.RandomRange(20, 21);
			if (this.soundSleeperSnore != null && !this.isGroanSilent)
			{
				Manager.BroadcastPlay(this, this.soundSleeperSnore, false, 1f);
			}
		}
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x000CFA2C File Offset: 0x000CDC2C
	public void Groan()
	{
		if (!this.isGroan && this.snoreGroanCD <= 0)
		{
			this.isGroan = true;
			this.isSnore = false;
			this.snoreGroanCD = this.rand.RandomRange(20, 21);
			if (this.sleeperNoiseToSenseSoundChance >= 1f || this.rand.RandomFloat <= this.sleeperNoiseToSenseSoundChance)
			{
				this.isGroanSilent = false;
				if (this.soundSleeperGroan != null)
				{
					Manager.BroadcastPlay(this, this.soundSleeperGroan, false, 1f);
					return;
				}
			}
			else
			{
				this.isGroanSilent = true;
			}
		}
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x000CFAB8 File Offset: 0x000CDCB8
	public override void OnUpdateEntity()
	{
		base.OnUpdateEntity();
		this.Buffs.SetCustomVar("_underwater", this.inWaterPercent, true, CVarOperation.set);
		if (this.Buffs != null)
		{
			this.Buffs.Tick();
		}
		this.OnUpdateLive();
		if (!this.IsSleeping && (!this.isEntityRemote || !(this is EntityPlayer)) && this.inventory != null)
		{
			this.inventory.OnUpdate();
		}
		if (this.Health <= 0 && !this.IsDead() && !this.isEntityRemote && !this.IsGodMode.Value)
		{
			if (this.Buffs.HasBuff("drowning"))
			{
				this.DamageEntity(DamageSource.suffocating, 1, false, 1f);
			}
			else
			{
				this.DamageEntity(DamageSource.disease, 1, false, 1f);
			}
		}
		if (base.IsAlive() && this.bPlayHurtSound)
		{
			string text = this.GetSoundHurt(this.woundedDamageSource, this.woundedStrength);
			if (text != null)
			{
				this.PlayOneShot(text, false, false, false, null, 1f);
			}
		}
		this.bPlayHurtSound = false;
		this.bBeenWounded = false;
		this.woundedStrength = 0;
		this.woundedDamageSource = null;
		if (this.snoreGroanCD > 0)
		{
			this.snoreGroanCD--;
		}
		if (!this.IsDead() && !this.isEntityRemote)
		{
			if (this.isRadiationSensitive() && this.biomeStandingOn != null && this.biomeStandingOn.m_RadiationLevel > 0 && !this.IsGodMode.Value && this.world.worldTime % 20UL == 0UL)
			{
				this.DamageEntity(DamageSource.radiation, this.biomeStandingOn.m_RadiationLevel, false, 1f);
			}
			if (this.hasAI)
			{
				if (this.IsSleeping && this.pendingSleepTrigger > -1)
				{
					this.TriggerSleeperPose(this.pendingSleepTrigger, false);
				}
				this.soundDelayTicks--;
				if (this.attackingTime <= 0 && this.soundDelayTicks <= 0 && this.aiClosestPlayerDistSq <= 400f && this.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.SleeperSupressLivingSounds)
				{
					if (this.targetAlertChanged)
					{
						this.targetAlertChanged = false;
						this.soundDelayTicks = this.GetSoundAlertTicks();
						if (this.GetSoundAlert() != null && !this.IsScoutZombie)
						{
							this.PlayOneShot(this.GetSoundAlert(), false, false, false, null, 1f);
						}
						this.OnEntityTargeted(this.attackTarget);
					}
					else
					{
						this.soundDelayTicks = this.GetSoundRandomTicks();
						this.attackTargetLast = null;
						if (this.GetSoundRandom() != null)
						{
							this.PlayOneShot(this.GetSoundRandom(), false, false, false, null, 1f);
						}
					}
				}
			}
		}
		if (this.hasBeenAttackedTime > 0)
		{
			this.hasBeenAttackedTime--;
		}
		if (this.painResistPercent > 0f)
		{
			this.painResistPercent -= 0.010000001f;
			if (this.painResistPercent <= 0f)
			{
				this.painHitsFelt = 0f;
			}
		}
		if (this.attackingTime > 0)
		{
			this.attackingTime--;
		}
		if (this.investigatePositionTicks > 0)
		{
			int num = this.investigatePositionTicks - 1;
			this.investigatePositionTicks = num;
			if (num == 0)
			{
				this.ClearInvestigatePosition();
			}
		}
		bool flag = this.IsDead();
		if (this.alertEnabled)
		{
			this.isAlert = this.bReplicatedAlertFlag;
			if (!this.isEntityRemote)
			{
				if (this.alertTicks > 0)
				{
					this.alertTicks--;
				}
				this.isAlert = !flag && (this.alertTicks > 0 || this.attackTarget || (this.HasInvestigatePosition && this.isInvestigateAlert));
				if (this.bReplicatedAlertFlag != this.isAlert)
				{
					this.bReplicatedAlertFlag = this.isAlert;
					this.bEntityAliveFlagsChanged = true;
				}
			}
			if (!this.isAlert && !flag)
			{
				this.Buffs.SetCustomVar(EntityAlive.notAlertedId, 1f, true, CVarOperation.set);
				this.notAlertDelayTicks = 4;
			}
			else
			{
				if (this.notAlertDelayTicks > 0)
				{
					this.notAlertDelayTicks--;
				}
				if (this.notAlertDelayTicks == 0)
				{
					this.Buffs.SetCustomVar(EntityAlive.notAlertedId, 0f, true, CVarOperation.set);
				}
			}
		}
		if (flag)
		{
			this.OnDeathUpdate();
		}
		if (this.revengeEntity != null)
		{
			if (!this.revengeEntity.IsAlive())
			{
				this.SetRevengeTarget(null);
				return;
			}
			if (this.revengeTimer > 0)
			{
				this.revengeTimer--;
				return;
			}
			this.SetRevengeTarget(null);
		}
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x000CFF1C File Offset: 0x000CE11C
	public override void KillLootContainer()
	{
		if (!this.isEntityRemote && this.IsDead() && !this.corpseBlockValue.isair && this.deathUpdateTime < this.timeStayAfterDeath)
		{
			this.deathUpdateTime = this.timeStayAfterDeath - 1;
		}
		base.KillLootContainer();
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x000CFF68 File Offset: 0x000CE168
	public override void Kill(DamageResponse _dmResponse)
	{
		this.NotifySleeperDeath();
		if (this.AttachedToEntity != null)
		{
			this.Detach();
		}
		if (this.deathUpdateTime == 0)
		{
			string text = this.GetSoundDeath(_dmResponse.Source);
			if (text != null)
			{
				this.PlayOneShot(text, false, false, false, null, 1f);
			}
		}
		if (this.IsDead())
		{
			this.SetDead();
			return;
		}
		this.ClientKill(_dmResponse);
		base.Kill(_dmResponse);
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x000CFFD4 File Offset: 0x000CE1D4
	public override void SetDead()
	{
		base.SetDead();
		this.Stats.Health.Value = 0f;
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x000CFFF1 File Offset: 0x000CE1F1
	public void NotifySleeperDeath()
	{
		if (!this.isEntityRemote && this.IsSleeper)
		{
			this.world.NotifySleeperVolumesEntityDied(this);
		}
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x000D000F File Offset: 0x000CE20F
	public void ClearEntityThatKilledMe()
	{
		this.entityThatKilledMe = null;
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x000D0018 File Offset: 0x000CE218
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void ClientKill(DamageResponse _dmResponse)
	{
		this.lastHitDirection = Utils.EnumHitDirection.Back;
		if (this.entityThatKilledMe == null && _dmResponse.Source != null)
		{
			Entity entity = ((_dmResponse.Source.getEntityId() != -1) ? this.world.GetEntity(_dmResponse.Source.getEntityId()) : null);
			if (this.Spawned && entity is EntityAlive)
			{
				this.entityThatKilledMe = (EntityAlive)entity;
			}
		}
		if (!this.IsDead())
		{
			this.SetDead();
			if (this.Buffs != null)
			{
				this.Buffs.OnDeath(this.entityThatKilledMe, (_dmResponse.Source == null) ? null : _dmResponse.Source.AttackingItem, _dmResponse.Source != null && _dmResponse.Source.damageType == EnumDamageTypes.Crushing, (_dmResponse.Source == null) ? FastTags<TagGroup.Global>.Parse("crushing") : _dmResponse.Source.DamageTypeTag);
			}
			if (this.Progression != null)
			{
				this.Progression.OnDeath();
			}
			global::UnityEngine.Object @object = this as EntityPlayer;
			this.AnalyticsSendDeath(_dmResponse);
			EntityAlive entityAlive = this.entityThatKilledMe;
			this.HandleClientDeath((_dmResponse.Source != null) ? _dmResponse.Source.BlockPosition : base.GetBlockPosition());
			this.OnEntityDeath();
			if (@object == null && base.EntityClass.bIsEnemyEntity && entityAlive is EntityPlayer && (EffectManager.GetValue(PassiveEffects.CelebrationKill, null, 0f, this.entityThatKilledMe, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0f || EntityAlive.IsCelebrate || (EntityAlive.IsCelebrateHeadshot && _dmResponse.Source != null && _dmResponse.HitBodyPart == EnumBodyPartHit.Head)))
			{
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					float lightBrightness = this.world.GetLightBrightness(base.GetBlockPosition());
					this.world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect("confetti", this.position, lightBrightness, Color.white, null, null, false, 1f, ""), this.entityId, false, true);
					Manager.BroadcastPlay(entityAlive, this.position, "twitch_celebrate");
					GameManager.Instance.World.RemoveEntity(this.entityId, EnumRemoveEntityReason.Killed);
					return;
				}
			}
			else
			{
				this.emodel.OnDeath(_dmResponse, this.world.ChunkCache);
			}
		}
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleClientDeath(Vector3i attackPos)
	{
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void OnEntityTargeted(EntityAlive target)
	{
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x000D0268 File Offset: 0x000CE468
	public void ForceHoldingWeaponUpdate()
	{
		if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
		{
			return;
		}
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageHoldingItem>().Setup(this), false, -1, this.entityId, -1, null, 192, false);
			return;
		}
		if (this.entityId > 0 && this as EntityPlayerLocal != null)
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageHoldingItem>().Setup(this), false);
		}
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000D02E9 File Offset: 0x000CE4E9
	public virtual void SetHoldingItemTransform(Transform _transform)
	{
		this.emodel.SetInRightHand(_transform);
		this.ForceHoldingWeaponUpdate();
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingItemChanged()
	{
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateCameraFOV(bool _bLerpPosition)
	{
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000D02FD File Offset: 0x000CE4FD
	public virtual int GetCameraFOV()
	{
		return GamePrefs.GetInt(EnumGamePrefs.OptionsGfxFOV);
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x000D0306 File Offset: 0x000CE506
	public virtual float GetWetnessRate()
	{
		return this.inWaterPercent;
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x000D0310 File Offset: 0x000CE510
	public float GetAmountEnclosed()
	{
		Vector3 position = this.position;
		position.y += 0.5f;
		Vector3i vector3i = World.worldToBlockPos(position);
		if (vector3i.y < 255)
		{
			IChunk chunkFromWorldPos = this.world.GetChunkFromWorldPos(vector3i);
			if (chunkFromWorldPos != null)
			{
				float num = (float)chunkFromWorldPos.GetLight(vector3i.x, vector3i.y, vector3i.z, Chunk.LIGHT_TYPE.SUN);
				float num2 = (float)chunkFromWorldPos.GetLight(vector3i.x, vector3i.y + 1, vector3i.z, Chunk.LIGHT_TYPE.SUN);
				float num3 = Utils.FastMax(num, num2) / 15f;
				return 1f - num3;
			}
		}
		return 1f;
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x000D03AB File Offset: 0x000CE5AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void OnHeadUnderwaterStateChanged(bool _bUnderwater)
	{
		base.OnHeadUnderwaterStateChanged(_bUnderwater);
		if (_bUnderwater)
		{
			this.FireEvent(MinEventTypes.onSelfWaterSubmerge, true);
			return;
		}
		this.FireEvent(MinEventTypes.onSelfWaterSurface, true);
	}

	// Token: 0x170003F7 RID: 1015
	// (get) Token: 0x0600225E RID: 8798 RVA: 0x000D03CA File Offset: 0x000CE5CA
	// (set) Token: 0x0600225F RID: 8799 RVA: 0x000D03D2 File Offset: 0x000CE5D2
	public virtual bool JetpackActive
	{
		get
		{
			return this.bJetpackActive;
		}
		set
		{
			if (value != this.bJetpackActive)
			{
				this.bJetpackActive = value;
				this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06002260 RID: 8800 RVA: 0x000D03FA File Offset: 0x000CE5FA
	// (set) Token: 0x06002261 RID: 8801 RVA: 0x000D0402 File Offset: 0x000CE602
	public virtual bool JetpackWearing
	{
		get
		{
			return this.bJetpackWearing;
		}
		set
		{
			if (value != this.bJetpackWearing)
			{
				this.bJetpackWearing = value;
				this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06002262 RID: 8802 RVA: 0x000D042A File Offset: 0x000CE62A
	// (set) Token: 0x06002263 RID: 8803 RVA: 0x000D0432 File Offset: 0x000CE632
	public virtual bool ParachuteWearing
	{
		get
		{
			return this.bParachuteWearing;
		}
		set
		{
			if (value != this.bParachuteWearing)
			{
				this.bParachuteWearing = value;
				this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06002264 RID: 8804 RVA: 0x000D045A File Offset: 0x000CE65A
	// (set) Token: 0x06002265 RID: 8805 RVA: 0x000D0494 File Offset: 0x000CE694
	public virtual bool AimingGun
	{
		get
		{
			return this.emodel.avatarController != null && this.emodel.avatarController.TryGetBool(AvatarController.isAimingHash, out this.bAimingGun) && this.bAimingGun;
		}
		set
		{
			bool aimingGun = this.AimingGun;
			if (value != aimingGun)
			{
				if (this.emodel.avatarController != null)
				{
					this.emodel.avatarController.UpdateBool(AvatarController.isAimingHash, value, true);
				}
				this.UpdateCameraFOV(true);
			}
			if (this is EntityPlayerLocal && this.inventory != null)
			{
				ItemAction itemAction = this.inventory.holdingItem.Actions[1];
				if (itemAction != null)
				{
					itemAction.AimingSet(this.inventory.holdingItemData.actionData[1], value, aimingGun);
				}
			}
			EntityPlayerLocal entityPlayerLocal = this as EntityPlayerLocal;
			if (entityPlayerLocal != null && value)
			{
				entityPlayerLocal.StartTPCameraLockTimer();
			}
		}
	}

	// Token: 0x06002266 RID: 8806 RVA: 0x000D0538 File Offset: 0x000CE738
	public virtual Vector3 GetChestTransformPosition()
	{
		if (this.IsCrouching || this.bodyDamage.CurrentStun == EnumEntityStunType.Kneel || this.bodyDamage.CurrentStun == EnumEntityStunType.Prone)
		{
			return base.transform.position + new Vector3(0f, this.GetEyeHeight() * 0.25f, 0f);
		}
		return base.transform.position + new Vector3(0f, this.GetEyeHeight() * 0.95f, 0f);
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06002267 RID: 8807 RVA: 0x000D05C0 File Offset: 0x000CE7C0
	// (set) Token: 0x06002268 RID: 8808 RVA: 0x000D05C8 File Offset: 0x000CE7C8
	public virtual bool MovementRunning
	{
		get
		{
			return this.bMovementRunning;
		}
		set
		{
			if (value != this.bMovementRunning)
			{
				this.bMovementRunning = value;
			}
		}
	}

	// Token: 0x170003FC RID: 1020
	// (get) Token: 0x06002269 RID: 8809 RVA: 0x000D05DA File Offset: 0x000CE7DA
	// (set) Token: 0x0600226A RID: 8810 RVA: 0x000D05E4 File Offset: 0x000CE7E4
	public virtual bool Crouching
	{
		get
		{
			return this.bCrouching;
		}
		set
		{
			if (value != this.bCrouching)
			{
				this.bCrouching = value;
				if (this.emodel.avatarController != null)
				{
					this.emodel.avatarController.SetCrouching(value);
				}
				this.CurrentStanceTag = (this.bCrouching ? EntityAlive.StanceTagCrouching : EntityAlive.StanceTagStanding);
				this.Buffs.SetCustomVar("_crouching", (float)(this.bCrouching ? 1 : 0), true, CVarOperation.set);
				this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x170003FD RID: 1021
	// (get) Token: 0x0600226B RID: 8811 RVA: 0x000D0674 File Offset: 0x000CE874
	public bool IsCrouching
	{
		get
		{
			return this.Crouching || this.CrouchingLocked;
		}
	}

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x0600226C RID: 8812 RVA: 0x000D0688 File Offset: 0x000CE888
	// (set) Token: 0x0600226D RID: 8813 RVA: 0x000D06CC File Offset: 0x000CE8CC
	public virtual bool Jumping
	{
		get
		{
			return this.bJumping && EffectManager.GetValue(PassiveEffects.JumpStrength, null, 1f, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) != 0f;
		}
		set
		{
			if (value != this.bJumping)
			{
				this.bJumping = value;
				if (this.Jumping)
				{
					this.StartJump();
					this.CurrentMovementTag &= EntityAlive.MovementTagIdle;
					this.CurrentMovementTag |= EntityAlive.MovementTagJumping;
				}
				else
				{
					this.EndJump();
					this.CurrentMovementTag &= EntityAlive.MovementTagJumping;
					this.bJumping = false;
				}
				this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x0600226E RID: 8814 RVA: 0x000D075E File Offset: 0x000CE95E
	// (set) Token: 0x0600226F RID: 8815 RVA: 0x000D0768 File Offset: 0x000CE968
	public bool Climbing
	{
		get
		{
			return this.bClimbing;
		}
		set
		{
			if (value != this.bClimbing)
			{
				this.bClimbing = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
				if (this.bClimbing)
				{
					this.CurrentMovementTag &= EntityAlive.MovementTagIdle;
					this.CurrentMovementTag |= EntityAlive.MovementTagClimbing;
					return;
				}
				this.CurrentMovementTag &= EntityAlive.MovementTagClimbing;
			}
		}
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x000D07E6 File Offset: 0x000CE9E6
	public virtual bool CanNavigatePath()
	{
		return this.onGround || this.isSwimming || this.bInElevator || this.Climbing;
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x000D0808 File Offset: 0x000CEA08
	public AvatarController.ActionState GetAnimActionState()
	{
		if (this.emodel.avatarController)
		{
			return this.emodel.avatarController.GetActionState();
		}
		return AvatarController.ActionState.None;
	}

	// Token: 0x06002272 RID: 8818 RVA: 0x000D0830 File Offset: 0x000CEA30
	public virtual void StartAnimAction(int _animType)
	{
		if (this.emodel.avatarController)
		{
			this.emodel.avatarController.GetActionState();
			if (_animType != 9999)
			{
				if (this.emodel.avatarController.IsActionActive())
				{
					return;
				}
			}
			else if (!this.emodel.avatarController.IsActionActive())
			{
				return;
			}
			this.bPlayerStatsChanged |= !this.isEntityRemote;
			this.emodel.avatarController.StartAction(_animType);
		}
	}

	// Token: 0x06002273 RID: 8819 RVA: 0x000D08B5 File Offset: 0x000CEAB5
	public virtual void ContinueAnimAction(int _animType)
	{
		if (this.emodel.avatarController)
		{
			this.bPlayerStatsChanged |= !this.isEntityRemote;
			this.emodel.avatarController.StartAction(_animType);
		}
	}

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06002274 RID: 8820 RVA: 0x000D08F0 File Offset: 0x000CEAF0
	// (set) Token: 0x06002275 RID: 8821 RVA: 0x000D0917 File Offset: 0x000CEB17
	public virtual bool RightArmAnimationAttack
	{
		get
		{
			return this.emodel.avatarController != null && this.emodel.avatarController.IsAnimationAttackPlaying();
		}
		set
		{
			if (this.emodel.avatarController != null && value && !this.emodel.avatarController.IsAnimationAttackPlaying())
			{
				this.emodel.avatarController.StartAnimationAttack();
			}
		}
	}

	// Token: 0x17000401 RID: 1025
	// (get) Token: 0x06002276 RID: 8822 RVA: 0x000D0951 File Offset: 0x000CEB51
	// (set) Token: 0x06002277 RID: 8823 RVA: 0x000D0978 File Offset: 0x000CEB78
	public virtual bool RightArmAnimationUse
	{
		get
		{
			return this.emodel.avatarController != null && this.emodel.avatarController.IsAnimationUsePlaying();
		}
		set
		{
			if (this.emodel.avatarController != null && value != this.emodel.avatarController.IsAnimationUsePlaying())
			{
				this.emodel.avatarController.StartAnimationUse();
			}
		}
	}

	// Token: 0x17000402 RID: 1026
	// (get) Token: 0x06002278 RID: 8824 RVA: 0x000D09B0 File Offset: 0x000CEBB0
	// (set) Token: 0x06002279 RID: 8825 RVA: 0x000D09D8 File Offset: 0x000CEBD8
	public virtual bool SpecialAttack
	{
		get
		{
			return this.emodel.avatarController != null && this.emodel.avatarController.IsAnimationSpecialAttackPlaying();
		}
		set
		{
			if (this.emodel.avatarController != null && value != this.emodel.avatarController.IsAnimationSpecialAttackPlaying())
			{
				this.bPlayerStatsChanged |= !this.isEntityRemote;
				this.emodel.avatarController.StartAnimationSpecialAttack(value, 0);
			}
		}
	}

	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x0600227A RID: 8826 RVA: 0x000D0A33 File Offset: 0x000CEC33
	// (set) Token: 0x0600227B RID: 8827 RVA: 0x000D0A5A File Offset: 0x000CEC5A
	public virtual bool SpecialAttack2
	{
		get
		{
			return this.emodel.avatarController != null && this.emodel.avatarController.IsAnimationSpecialAttack2Playing();
		}
		set
		{
			if (this.emodel.avatarController != null && value)
			{
				this.bPlayerStatsChanged |= !this.isEntityRemote;
				this.emodel.avatarController.StartAnimationSpecialAttack2();
			}
		}
	}

	// Token: 0x17000404 RID: 1028
	// (get) Token: 0x0600227C RID: 8828 RVA: 0x000D0A97 File Offset: 0x000CEC97
	// (set) Token: 0x0600227D RID: 8829 RVA: 0x000D0ABE File Offset: 0x000CECBE
	public virtual bool Raging
	{
		get
		{
			return this.emodel.avatarController != null && this.emodel.avatarController.IsAnimationRagingPlaying();
		}
		set
		{
			if (this.emodel.avatarController != null && value && !this.emodel.avatarController.IsAnimationRagingPlaying())
			{
				this.emodel.avatarController.StartAnimationRaging();
			}
		}
	}

	// Token: 0x17000405 RID: 1029
	// (get) Token: 0x0600227E RID: 8830 RVA: 0x000D0AF8 File Offset: 0x000CECF8
	// (set) Token: 0x0600227F RID: 8831 RVA: 0x000D0B34 File Offset: 0x000CED34
	public virtual bool Electrocuted
	{
		get
		{
			return this.emodel != null && this.emodel.avatarController != null && this.emodel.avatarController.GetAnimationElectrocuteRemaining() > 0f;
		}
		set
		{
			if (this.emodel != null && this.emodel.avatarController != null && value != this.emodel.avatarController.GetAnimationElectrocuteRemaining() > 0.4f)
			{
				this.bPlayerStatsChanged |= !this.isEntityRemote;
				if (value)
				{
					this.emodel.avatarController.StartAnimationElectrocute(0.6f);
					this.emodel.avatarController.Electrocute(true);
				}
			}
		}
	}

	// Token: 0x17000406 RID: 1030
	// (get) Token: 0x06002280 RID: 8832 RVA: 0x000D0BBB File Offset: 0x000CEDBB
	// (set) Token: 0x06002281 RID: 8833 RVA: 0x000D0BE2 File Offset: 0x000CEDE2
	public virtual bool HarvestingAnimation
	{
		get
		{
			return this.emodel.avatarController != null && this.emodel.avatarController.IsAnimationHarvestingPlaying();
		}
		set
		{
			this.emodel.avatarController.UpdateBool("Harvesting", value, true);
		}
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x000D0BFB File Offset: 0x000CEDFB
	public virtual void StartHarvestingAnim(float _length, bool _weaponFireTrigger)
	{
		if (this.emodel != null && this.emodel.avatarController != null)
		{
			this.emodel.avatarController.StartAnimationHarvesting(_length, _weaponFireTrigger);
		}
	}

	// Token: 0x17000407 RID: 1031
	// (get) Token: 0x06002283 RID: 8835 RVA: 0x000D0C30 File Offset: 0x000CEE30
	// (set) Token: 0x06002284 RID: 8836 RVA: 0x000D0C38 File Offset: 0x000CEE38
	public bool IsEating
	{
		get
		{
			return this.m_isEating;
		}
		set
		{
			if (value != this.m_isEating)
			{
				this.m_isEating = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
				if (this.emodel != null && this.emodel.avatarController != null)
				{
					if (this.m_isEating)
					{
						this.emodel.avatarController.StartEating();
						return;
					}
					this.emodel.avatarController.StopEating();
				}
			}
		}
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x000D0CB8 File Offset: 0x000CEEB8
	public virtual void SetVehicleAnimation(int _animHash, int _pose)
	{
		if (this.emodel && this.emodel.avatarController)
		{
			this.emodel.avatarController.SetVehicleAnimation(_animHash, _pose);
			this.bPlayerStatsChanged = !this.isEntityRemote;
			if (_pose == -1)
			{
				AvatarLocalPlayerController avatarLocalPlayerController = this.emodel.avatarController as AvatarLocalPlayerController;
				if (avatarLocalPlayerController != null)
				{
					avatarLocalPlayerController.TPVResetAnimPose();
				}
			}
		}
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x000D0D23 File Offset: 0x000CEF23
	public virtual int GetVehicleAnimation()
	{
		if (this.emodel && this.emodel.avatarController)
		{
			return this.emodel.avatarController.GetVehicleAnimation();
		}
		return -1;
	}

	// Token: 0x17000408 RID: 1032
	// (get) Token: 0x06002287 RID: 8839 RVA: 0x000D0D56 File Offset: 0x000CEF56
	// (set) Token: 0x06002288 RID: 8840 RVA: 0x000D0D5E File Offset: 0x000CEF5E
	public virtual int Died
	{
		get
		{
			return this.died;
		}
		set
		{
			if (value != this.died)
			{
				this.died = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x17000409 RID: 1033
	// (get) Token: 0x06002289 RID: 8841 RVA: 0x000D0D86 File Offset: 0x000CEF86
	// (set) Token: 0x0600228A RID: 8842 RVA: 0x000D0D8E File Offset: 0x000CEF8E
	public virtual int Score
	{
		get
		{
			return this.score;
		}
		set
		{
			if (value != this.score)
			{
				this.score = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x1700040A RID: 1034
	// (get) Token: 0x0600228B RID: 8843 RVA: 0x000D0DB6 File Offset: 0x000CEFB6
	// (set) Token: 0x0600228C RID: 8844 RVA: 0x000D0DBE File Offset: 0x000CEFBE
	public virtual int KilledZombies
	{
		get
		{
			return this.killedZombies;
		}
		set
		{
			if (value != this.killedZombies)
			{
				this.killedZombies = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x1700040B RID: 1035
	// (get) Token: 0x0600228D RID: 8845 RVA: 0x000D0DE6 File Offset: 0x000CEFE6
	// (set) Token: 0x0600228E RID: 8846 RVA: 0x000D0DEE File Offset: 0x000CEFEE
	public virtual int KilledPlayers
	{
		get
		{
			return this.killedPlayers;
		}
		set
		{
			if (value != this.killedPlayers)
			{
				this.killedPlayers = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x1700040C RID: 1036
	// (get) Token: 0x0600228F RID: 8847 RVA: 0x000D0E16 File Offset: 0x000CF016
	// (set) Token: 0x06002290 RID: 8848 RVA: 0x000D0E1E File Offset: 0x000CF01E
	public virtual int TeamNumber
	{
		get
		{
			return this.teamNumber;
		}
		set
		{
			if (value != this.teamNumber)
			{
				this.teamNumber = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
				if (!this.isEntityRemote)
				{
					GameManager.Instance.GameMessage(EnumGameMessages.ChangedTeam, this, null);
				}
			}
		}
	}

	// Token: 0x1700040D RID: 1037
	// (get) Token: 0x06002291 RID: 8849 RVA: 0x000D0E5B File Offset: 0x000CF05B
	public virtual string EntityName
	{
		get
		{
			return this.entityName;
		}
	}

	// Token: 0x06002292 RID: 8850 RVA: 0x000D0E63 File Offset: 0x000CF063
	public override void SetEntityName(string _name)
	{
		if (!_name.Equals(this.entityName))
		{
			this.entityName = _name;
			this.bPlayerStatsChanged |= !this.isEntityRemote;
			this.HandleSetNavName();
		}
	}

	// Token: 0x1700040E RID: 1038
	// (get) Token: 0x06002293 RID: 8851 RVA: 0x000D0E96 File Offset: 0x000CF096
	// (set) Token: 0x06002294 RID: 8852 RVA: 0x000D0E9E File Offset: 0x000CF09E
	public virtual int DeathHealth
	{
		get
		{
			return this.deathHealth;
		}
		set
		{
			if (value != this.deathHealth)
			{
				this.deathHealth = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x1700040F RID: 1039
	// (get) Token: 0x06002295 RID: 8853 RVA: 0x000D0EC6 File Offset: 0x000CF0C6
	// (set) Token: 0x06002296 RID: 8854 RVA: 0x000D0ECE File Offset: 0x000CF0CE
	public virtual bool Spawned
	{
		get
		{
			return this.bSpawned;
		}
		set
		{
			if (value != this.bSpawned)
			{
				this.bSpawned = value;
				this.onSpawnStateChanged();
				this.bEntityAliveFlagsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x17000410 RID: 1040
	// (get) Token: 0x06002297 RID: 8855 RVA: 0x000D0EFC File Offset: 0x000CF0FC
	// (set) Token: 0x06002298 RID: 8856 RVA: 0x000D0F04 File Offset: 0x000CF104
	public bool IsBreakingBlocks
	{
		get
		{
			return this.m_isBreakingBlocks;
		}
		set
		{
			if (value != this.m_isBreakingBlocks)
			{
				this.m_isBreakingBlocks = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
			}
		}
	}

	// Token: 0x06002299 RID: 8857 RVA: 0x000D0EC6 File Offset: 0x000CF0C6
	public override bool IsSpawned()
	{
		return this.bSpawned;
	}

	// Token: 0x17000411 RID: 1041
	// (get) Token: 0x0600229A RID: 8858 RVA: 0x000D0F2C File Offset: 0x000CF12C
	public virtual EntityBedrollPositionList SpawnPoints
	{
		get
		{
			return this.spawnPoints;
		}
	}

	// Token: 0x0600229B RID: 8859 RVA: 0x000D0F34 File Offset: 0x000CF134
	public virtual void RemoveIKTargets()
	{
		this.emodel.RemoveIKController();
	}

	// Token: 0x0600229C RID: 8860 RVA: 0x000D0F44 File Offset: 0x000CF144
	public virtual void SetIKTargets(List<IKController.Target> targets)
	{
		IKController ikcontroller = this.emodel.AddIKController();
		if (ikcontroller)
		{
			ikcontroller.SetTargets(targets);
		}
	}

	// Token: 0x0600229D RID: 8861 RVA: 0x000D0F6C File Offset: 0x000CF16C
	public virtual List<Vector3i> GetDroppedBackpackPositions()
	{
		return this.droppedBackpackPositions;
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x000D0F74 File Offset: 0x000CF174
	public virtual Vector3i GetLastDroppedBackpackPosition()
	{
		if (this.droppedBackpackPositions == null)
		{
			return Vector3i.zero;
		}
		if (this.droppedBackpackPositions.Count == 0)
		{
			return Vector3i.zero;
		}
		List<Vector3i> list = this.droppedBackpackPositions;
		return list[list.Count - 1];
	}

	// Token: 0x0600229F RID: 8863 RVA: 0x000D0FAC File Offset: 0x000CF1AC
	public virtual bool EqualsDroppedBackpackPositions(Vector3i position)
	{
		if (this.droppedBackpackPositions != null)
		{
			foreach (Vector3i vector3i in this.droppedBackpackPositions)
			{
				if (position.Equals(vector3i))
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	// Token: 0x060022A0 RID: 8864 RVA: 0x000D1014 File Offset: 0x000CF214
	public virtual void SetDroppedBackpackPositions(List<Vector3i> positions)
	{
		this.droppedBackpackPositions.Clear();
		if (positions != null)
		{
			this.droppedBackpackPositions.AddRange(positions);
		}
	}

	// Token: 0x060022A1 RID: 8865 RVA: 0x000D1030 File Offset: 0x000CF230
	public virtual void ClearDroppedBackpackPositions()
	{
		this.droppedBackpackPositions.Clear();
	}

	// Token: 0x17000412 RID: 1042
	// (get) Token: 0x060022A2 RID: 8866 RVA: 0x000D103D File Offset: 0x000CF23D
	// (set) Token: 0x060022A3 RID: 8867 RVA: 0x000D1050 File Offset: 0x000CF250
	public virtual int Health
	{
		get
		{
			return (int)this.Stats.Health.Value;
		}
		set
		{
			this.Stats.Health.Value = (float)value;
		}
	}

	// Token: 0x17000413 RID: 1043
	// (get) Token: 0x060022A4 RID: 8868 RVA: 0x000D1064 File Offset: 0x000CF264
	// (set) Token: 0x060022A5 RID: 8869 RVA: 0x000D1076 File Offset: 0x000CF276
	public virtual float Stamina
	{
		get
		{
			return this.Stats.Stamina.Value;
		}
		set
		{
			this.Stats.Stamina.Value = value;
		}
	}

	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x060022A6 RID: 8870 RVA: 0x000D1089 File Offset: 0x000CF289
	// (set) Token: 0x060022A7 RID: 8871 RVA: 0x000D109B File Offset: 0x000CF29B
	public virtual float Water
	{
		get
		{
			return this.Stats.Water.Value;
		}
		set
		{
			this.Stats.Water.Value = value;
		}
	}

	// Token: 0x060022A8 RID: 8872 RVA: 0x000D10AE File Offset: 0x000CF2AE
	public virtual int GetMaxHealth()
	{
		return (int)this.Stats.Health.Max;
	}

	// Token: 0x060022A9 RID: 8873 RVA: 0x000D10C1 File Offset: 0x000CF2C1
	public virtual int GetMaxStamina()
	{
		return (int)this.Stats.Stamina.Max;
	}

	// Token: 0x060022AA RID: 8874 RVA: 0x000D10D4 File Offset: 0x000CF2D4
	public virtual int GetMaxWater()
	{
		return (int)this.Stats.Water.Max;
	}

	// Token: 0x17000415 RID: 1045
	// (get) Token: 0x060022AB RID: 8875 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsValidAimAssistSlowdownTarget
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000416 RID: 1046
	// (get) Token: 0x060022AC RID: 8876 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsValidAimAssistSnapTarget
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000417 RID: 1047
	// (get) Token: 0x060022AD RID: 8877 RVA: 0x000D10E7 File Offset: 0x000CF2E7
	// (set) Token: 0x060022AE RID: 8878 RVA: 0x000D10EF File Offset: 0x000CF2EF
	public virtual EModelBase.HeadStates CurrentHeadState
	{
		get
		{
			return this.currentHeadState;
		}
		set
		{
			if (value != this.currentHeadState)
			{
				this.currentHeadState = value;
				this.bPlayerStatsChanged |= !this.isEntityRemote;
			}
			this.emodel.ForceHeadState(value);
		}
	}

	// Token: 0x060022AF RID: 8879 RVA: 0x00040D24 File Offset: 0x0003EF24
	public virtual float GetStaminaMultiplier()
	{
		return 1f;
	}

	// Token: 0x060022B0 RID: 8880 RVA: 0x000D1124 File Offset: 0x000CF324
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void SetMovementState()
	{
		float num = this.speedStrafe;
		if (num >= 1234f)
		{
			num = 0f;
		}
		float num2 = this.speedForward * this.speedForward + num * num;
		this.MovementState = ((num2 > this.moveSpeedAggro * this.moveSpeedAggro) ? 3 : ((num2 > this.moveSpeed * this.moveSpeed) ? 2 : ((num2 > 0.001f) ? 1 : 0)));
	}

	// Token: 0x060022B1 RID: 8881 RVA: 0x000D1190 File Offset: 0x000CF390
	public virtual void OnUpdateLive()
	{
		this.Stats.Health.RegenerationAmount = 0f;
		if (!this.isEntityRemote && !this.IsDead())
		{
			this.Stats.Tick(this.world.worldTime);
			if (this.attractPlayer)
			{
				int num = this.attractPlayerTimeoutTicks - 1;
				this.attractPlayerTimeoutTicks = num;
				if (num <= 0)
				{
					this.attractPlayer = null;
				}
			}
		}
		if (this.jumpTicks > 0)
		{
			this.jumpTicks--;
		}
		if (this.attackTargetTime > 0)
		{
			this.attackTargetTime--;
			if (this.attackTarget != null && this.attackTargetTime == 0)
			{
				this.attackTarget = null;
				if (!this.isEntityRemote)
				{
					this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, -1, NetPackageManager.GetPackage<NetPackageSetAttackTarget>().Setup(this.entityId, -1), false);
				}
			}
		}
		this.updateCurrentBlockPosAndValue();
		if (this.AttachedToEntity == null)
		{
			if (this.isEntityRemote)
			{
				if (this.RootMotion)
				{
					this.MoveEntityHeaded(Vector3.zero, false);
				}
			}
			else
			{
				if (this.Health <= 0)
				{
					this.bJumping = false;
					this.bClimbing = false;
					this.moveDirection = Vector3.zero;
					this.renderFadeMax = 1f;
				}
				else if (!this.world.IsRemote() && !this.IsDead() && !this.IsClientControlled() && this.hasAI)
				{
					this.updateTasks();
				}
				this.noisePlayer = null;
				this.noisePlayerDistance = 0f;
				this.noisePlayerVolume = 0f;
				if (this.bJumping)
				{
					this.UpdateJump();
				}
				else
				{
					this.jumpTicks = 0;
				}
				float num2 = this.landMovementFactor;
				this.landMovementFactor *= this.GetSpeedModifier();
				this.MoveEntityHeaded(this.moveDirection, this.isMoveDirAbsolute);
				this.landMovementFactor = num2;
			}
			if (this.moveDirection.x > 0f || this.moveDirection.z > 0f)
			{
				if (this.bMovementRunning)
				{
					this.CurrentMovementTag = EntityAlive.MovementTagRunning;
				}
				else
				{
					this.CurrentMovementTag = EntityAlive.MovementTagWalking;
				}
			}
			else
			{
				this.CurrentMovementTag = EntityAlive.MovementTagIdle;
			}
		}
		if (this.bodyDamage.CurrentStun != EnumEntityStunType.None && !this.emodel.IsRagdollActive && !this.IsDead())
		{
			if (this.bodyDamage.CurrentStun == EnumEntityStunType.Getup)
			{
				if (!this.emodel.avatarController || !this.emodel.avatarController.IsAnimationStunRunning())
				{
					this.ClearStun();
				}
			}
			else
			{
				this.bodyDamage.StunDuration = this.bodyDamage.StunDuration - 0.05f;
				if (this.bodyDamage.StunDuration <= 0f)
				{
					this.SetStun(EnumEntityStunType.Getup);
					if (this.emodel.avatarController)
					{
						this.emodel.avatarController.EndStun();
					}
				}
			}
		}
		this.proneRefillCounter += 0.05f * this.proneRefillRate;
		while (this.proneRefillCounter >= 1f)
		{
			this.bodyDamage.StunProne = Mathf.Max(0, this.bodyDamage.StunProne - 1);
			this.proneRefillCounter -= 1f;
		}
		this.kneelRefillCounter += 0.05f * this.kneelRefillRate;
		while (this.kneelRefillCounter >= 1f)
		{
			this.bodyDamage.StunKnee = Mathf.Max(0, this.bodyDamage.StunKnee - 1);
			this.kneelRefillCounter -= 1f;
		}
		EntityPlayer primaryPlayer = this.world.GetPrimaryPlayer();
		if (primaryPlayer != null && primaryPlayer != this)
		{
			int num = this.ticksToCheckSeenByPlayer - 1;
			this.ticksToCheckSeenByPlayer = num;
			if (num <= 0)
			{
				this.wasSeenByPlayer = primaryPlayer.CanSee(this);
				if (this.wasSeenByPlayer)
				{
					this.ticksToCheckSeenByPlayer = 200;
				}
				else
				{
					this.ticksToCheckSeenByPlayer = 20;
				}
			}
			else if (this.wasSeenByPlayer)
			{
				primaryPlayer.SetCanSee(this);
			}
		}
		if (this.onGround)
		{
			this.disableFallBehaviorUntilOnGround = false;
		}
		this.UpdateDynamicRagdoll();
		this.checkForTeleportOutOfTraderArea();
	}

	// Token: 0x060022B2 RID: 8882 RVA: 0x000D15B8 File Offset: 0x000CF7B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void checkForTeleportOutOfTraderArea()
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && !GameManager.Instance.IsEditMode() && !this.IsGodMode.Value)
		{
			EntityPlayer entityPlayer = this as EntityPlayer;
			if (entityPlayer != null && Time.time - this.lastTimeTraderStationChecked > 0.1f)
			{
				this.lastTimeTraderStationChecked = Time.time;
				Vector3 position = this.position;
				position.y += 0.5f;
				Vector3i vector3i = World.worldToBlockPos(position);
				TraderArea traderAreaAt = this.world.GetTraderAreaAt(vector3i);
				if (traderAreaAt != null && traderAreaAt.IsInitialized)
				{
					Vector3 vector = default(Vector3);
					int num = 0;
					PrefabTeleportVolume prefabTeleportVolume;
					if (TraderInfo.TraderHoursPreset != TraderInfo.TraderHourPresets.AlwaysOpen && this.world.IsWorldEvent(World.WorldEvent.BloodMoon) && traderAreaAt.IsWithinProtectArea(position))
					{
						vector = traderAreaAt.ProtectPosition + traderAreaAt.ProtectSize * 0.5f;
						num = Math.Max(traderAreaAt.ProtectSize.x, traderAreaAt.ProtectSize.z) / 2;
					}
					else if (traderAreaAt.IsWithinTeleportArea(position, out prefabTeleportVolume) && (traderAreaAt.IsClosed || EffectManager.GetValue(PassiveEffects.NoTrader, null, 0f, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) == 1f))
					{
						PrefabInstance poiatPosition = this.world.GetPOIAtPosition(vector3i, null, null);
						if (poiatPosition == null)
						{
							return;
						}
						vector = poiatPosition.boundingBoxPosition + traderAreaAt.PrefabSize * 0.5f;
						num = Math.Max(traderAreaAt.PrefabSize.x, traderAreaAt.PrefabSize.z) / 2;
					}
					if (num > 0)
					{
						num += this.traderTeleportStreak;
						this.traderTeleportStreak++;
						Vector3 vector2;
						if (!this.world.GetRandomSpawnPositionMinMaxToPosition(vector, num, num + 1, 1, false, out vector2, this.entityId, true, 20, true, EnumLandClaimOwner.Ally, true))
						{
							Log.Warning("Trader teleport: Could not find a valid teleport position, returning original position");
							return;
						}
						if (this.isEntityRemote)
						{
							SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.ForEntityId(this.entityId).SendPackage(NetPackageManager.GetPackage<NetPackageTeleportPlayer>().Setup(vector2, null, false));
						}
						else if (entityPlayer)
						{
							entityPlayer.Teleport(vector2, float.MinValue);
						}
						else if (this.AttachedToEntity != null)
						{
							this.AttachedToEntity.SetPosition(vector2, true);
						}
						else
						{
							this.SetPosition(vector2, true);
						}
						if (entityPlayer)
						{
							GameEventManager.Current.HandleAction("game_on_trader_teleport", entityPlayer, entityPlayer, false, "", "", false, true, "", null);
							return;
						}
					}
				}
				else
				{
					this.traderTeleportStreak = 1;
				}
			}
		}
	}

	// Token: 0x060022B3 RID: 8883 RVA: 0x000D187C File Offset: 0x000CFA7C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void StartJump()
	{
		this.jumpState = EntityAlive.JumpState.Leap;
		this.jumpStateTicks = 0;
		this.jumpDistance = 1f;
		this.jumpHeightDiff = 0f;
		this.disableFallBehaviorUntilOnGround = true;
		if (this.isSwimming)
		{
			this.jumpState = EntityAlive.JumpState.SwimStart;
			if (this.emodel.avatarController != null)
			{
				this.emodel.avatarController.SetSwim(true);
				return;
			}
		}
		else if (this.emodel.avatarController != null)
		{
			this.emodel.avatarController.StartAnimationJump(AnimJumpMode.Start);
		}
	}

	// Token: 0x060022B4 RID: 8884 RVA: 0x000D190C File Offset: 0x000CFB0C
	public virtual void SetJumpDistance(float _distance, float _heightDiff)
	{
		this.jumpDistance = _distance;
		this.jumpHeightDiff = _heightDiff;
	}

	// Token: 0x060022B5 RID: 8885 RVA: 0x000D191C File Offset: 0x000CFB1C
	public virtual void SetSwimValues(float _durationTicks, Vector3 _motion)
	{
		this.jumpSwimDurationTicks = Mathf.Clamp(_durationTicks / this.swimSpeed - 6f, 3f, 20f);
		this.jumpSwimMotion = _motion;
	}

	// Token: 0x060022B6 RID: 8886 RVA: 0x000D1948 File Offset: 0x000CFB48
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void UpdateJump()
	{
		if (this.IsFlyMode.Value)
		{
			this.Jumping = false;
			return;
		}
		this.jumpStateTicks++;
		switch (this.jumpState)
		{
		case EntityAlive.JumpState.Leap:
			if (this.accumulatedRootMotion.y > 0.005f || (float)this.jumpStateTicks >= this.jumpDelay)
			{
				this.StartJumpMotion();
				this.jumpTicks = 200;
				this.jumpState = EntityAlive.JumpState.Air;
				this.jumpStateTicks = 0;
				this.jumpIsMoving = true;
				return;
			}
			break;
		case EntityAlive.JumpState.Air:
			if (this.onGround || (this.motionMultiplier < 0.45f && this.jumpStateTicks > 40))
			{
				this.jumpState = EntityAlive.JumpState.Land;
				this.jumpStateTicks = 0;
				this.jumpIsMoving = false;
				return;
			}
			break;
		case EntityAlive.JumpState.Land:
			if (this.jumpStateTicks > 5)
			{
				this.Jumping = false;
				return;
			}
			break;
		case EntityAlive.JumpState.SwimStart:
			if ((float)this.jumpStateTicks > 6f)
			{
				this.jumpTicks = 100;
				this.jumpState = EntityAlive.JumpState.Swim;
				this.jumpStateTicks = 0;
				this.jumpIsMoving = true;
				this.StartJumpSwimMotion();
				return;
			}
			break;
		case EntityAlive.JumpState.Swim:
			if (!this.isSwimming || (float)this.jumpStateTicks >= this.jumpSwimDurationTicks)
			{
				this.Jumping = false;
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x060022B7 RID: 8887 RVA: 0x000D1A80 File Offset: 0x000CFC80
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void StartJumpSwimMotion()
	{
		if (this.inWaterPercent > 0.65f)
		{
			float num = Mathf.Sqrt(this.jumpSwimMotion.x * this.jumpSwimMotion.x + this.jumpSwimMotion.z * this.jumpSwimMotion.z) + 0.001f;
			float num2 = Mathf.Lerp(-0.6f, -0.05f, num * 0.8f);
			this.jumpSwimMotion.y = Utils.FastClamp(this.jumpSwimMotion.y, num2, 1f);
			float num3 = this.jumpSwimDurationTicks;
			float num4 = (num3 - 1f) * this.world.Gravity * 0.025f * 0.4999f;
			num4 /= Mathf.Pow(0.91f, (num3 - 3f) * 0.91f * 0.115f);
			float num5 = (num3 - 1f) / 15f;
			float num6 = Mathf.LerpUnclamped(0.46f, 0.41860002f, num5);
			float num7 = Mathf.Pow(0.91f, (num3 - 1f) * num6);
			float num8 = 1f / num3 / num7;
			num4 += this.jumpSwimMotion.y * num8;
			num8 /= Utils.FastMax(1f, num);
			this.motion.x = this.jumpSwimMotion.x * num8;
			this.motion.z = this.jumpSwimMotion.z * num8;
			this.motion.y = num4;
			return;
		}
		this.motion.y = 0f;
	}

	// Token: 0x060022B8 RID: 8888 RVA: 0x000D1C0C File Offset: 0x000CFE0C
	[PublicizedFrom(EAccessModifier.Protected)]
	public void FaceJumpTo()
	{
		Vector3 vector = this.moveHelper.JumpToPos - this.position;
		float num = Mathf.Round(Mathf.Atan2(vector.x, vector.z) * 57.29578f / 90f) * 90f;
		base.SeekYaw(num, 0f, 0f);
	}

	// Token: 0x060022B9 RID: 8889 RVA: 0x000D1C6C File Offset: 0x000CFE6C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void StartJumpMotion()
	{
		base.SetAirBorne(true);
		float num = (float)((int)(5f + Mathf.Pow(this.jumpDistance * 8f, 0.5f)));
		this.motion = this.GetForwardVector() * (this.jumpDistance / num);
		float num2 = num * this.world.Gravity * 0.5f;
		this.motion.y = Utils.FastMax(num2 * 0.5f, num2 + this.jumpHeightDiff / num);
	}

	// Token: 0x060022BA RID: 8890 RVA: 0x000D1CF0 File Offset: 0x000CFEF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void JumpMove()
	{
		this.accumulatedRootMotion = Vector3.zero;
		Vector3 motion = this.motion;
		this.entityCollision(this.motion);
		this.motion.x = motion.x;
		this.motion.z = motion.z;
		if (this.motion.y != 0f)
		{
			this.motion.y = motion.y;
		}
		if (this.jumpState == EntityAlive.JumpState.Air)
		{
			this.motion.y = this.motion.y - this.world.Gravity;
			return;
		}
		this.motion.x = this.motion.x * 0.91f;
		this.motion.z = this.motion.z * 0.91f;
		this.motion.y = this.motion.y - this.world.Gravity * 0.025f;
		this.motion.y = this.motion.y * 0.91f;
	}

	// Token: 0x060022BB RID: 8891 RVA: 0x000D1DE0 File Offset: 0x000CFFE0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void EndJump()
	{
		this.jumpState = EntityAlive.JumpState.Off;
		this.jumpIsMoving = false;
		if (!this.isEntityRemote && this.emodel.avatarController != null)
		{
			this.emodel.avatarController.StartAnimationJump(AnimJumpMode.Land);
		}
	}

	// Token: 0x060022BC RID: 8892 RVA: 0x000D1E1C File Offset: 0x000D001C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override bool CalcIfSwimming()
	{
		float num = ((this.onGround || this.Jumping) ? 0.7f : 0.5f);
		return this.inWaterPercent >= num;
	}

	// Token: 0x060022BD RID: 8893 RVA: 0x000D1E52 File Offset: 0x000D0052
	public override void SwimChanged()
	{
		if (this.emodel.avatarController)
		{
			this.emodel.avatarController.SetSwim(this.isSwimming);
		}
	}

	// Token: 0x060022BE RID: 8894 RVA: 0x000D1E7C File Offset: 0x000D007C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Update()
	{
		base.Update();
		this.updateNetworkStats();
		if (!this.isEntityRemote && this.RootMotion && this.lerpForwardSpeed)
		{
			float num = Utils.FastAbs(this.speedForward);
			float num2 = 0.06935714f;
			if (num > 0.01942f)
			{
				num2 = this.speedForwardTargetStep;
			}
			float num3 = Utils.FastMoveTowards(this.speedForward, this.speedForwardTarget, num2 * Time.deltaTime);
			if (num > 0.01942f && Utils.FastAbs(num3) <= 0.01942f)
			{
				num3 = 0.01942f;
			}
			this.speedForward = num3;
		}
		if (this.isHeadUnderwater != (this.Buffs.GetCustomVar("_underwater") == 1f))
		{
			this.Buffs.SetCustomVar("_underwater", (float)(this.isHeadUnderwater ? 1 : 0), true, CVarOperation.set);
		}
		this.MinEventContext.Area = this.boundingBox;
		this.MinEventContext.Biome = this.biomeStandingOn;
		this.MinEventContext.ItemValue = this.inventory.holdingItemItemValue;
		this.MinEventContext.BlockValue = this.blockValueStandingOn;
		this.MinEventContext.ItemInventoryData = this.inventory.holdingItemData;
		this.MinEventContext.Position = this.position;
		this.MinEventContext.Seed = this.entityId + Mathf.Abs(GameManager.Instance.World.Seed);
		this.MinEventContext.Transform = base.transform;
		FastTags<TagGroup.Global>.CombineTags(EntityClass.list[this.entityClass].Tags, this.inventory.holdingItem.ItemTags, this.CurrentStanceTag, this.CurrentMovementTag, ref this.MinEventContext.Tags);
		if (this.Progression != null)
		{
			this.Progression.Update();
		}
		if (this.renderFade != this.renderFadeTarget)
		{
			this.renderFade = Mathf.MoveTowards(this.renderFade, this.renderFadeTarget, Time.deltaTime);
			this.emodel.SetFade(this.renderFade);
			bool flag = this.renderFade > 0.01f;
			if (this.emodel.visible != flag)
			{
				this.emodel.SetVisible(flag, false);
			}
		}
	}

	// Token: 0x060022BF RID: 8895 RVA: 0x000D20A4 File Offset: 0x000D02A4
	[PublicizedFrom(EAccessModifier.Private)]
	public void LateUpdate()
	{
		this.startOfFrameStats.CopyFrom(this.entityStats);
	}

	// Token: 0x060022C0 RID: 8896 RVA: 0x000D20B8 File Offset: 0x000D02B8
	public virtual void OnDeathUpdate()
	{
		if (this.deathUpdateTime < this.timeStayAfterDeath)
		{
			this.deathUpdateTime++;
		}
		int deadBodyHitPoints = EntityClass.list[this.entityClass].DeadBodyHitPoints;
		if (deadBodyHitPoints > 0 && this.DeathHealth <= -deadBodyHitPoints)
		{
			this.deathUpdateTime = this.timeStayAfterDeath;
		}
		if (this.deathUpdateTime < this.timeStayAfterDeath)
		{
			return;
		}
		if (!this.isEntityRemote && !this.markedForUnload && this.particleOnDestroy != null && this.particleOnDestroy.Length > 0)
		{
			float lightBrightness = this.world.GetLightBrightness(base.GetBlockPosition());
			this.world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect(this.particleOnDestroy, this.getHeadPosition(), lightBrightness, Color.white, null, null, false, 1f, ""), this.entityId, false, false);
		}
	}

	// Token: 0x060022C1 RID: 8897 RVA: 0x000D2194 File Offset: 0x000D0394
	public void NotifyRootMotion(Animator animator)
	{
		this.accumulatedRootMotion += animator.deltaPosition;
	}

	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x060022C2 RID: 8898 RVA: 0x000D21AD File Offset: 0x000D03AD
	public virtual float MaxVelocity
	{
		get
		{
			return 5f;
		}
	}

	// Token: 0x060022C3 RID: 8899 RVA: 0x000D21B4 File Offset: 0x000D03B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void DefaultMoveEntity(Vector3 _direction, bool _isDirAbsolute)
	{
		float num = 0.91f;
		if (AIDirector.debugFreezePos && this.aiManager != null)
		{
			this.motion = Vector3.zero;
		}
		if (this.onGround)
		{
			num = 0.546f;
			if (!this.IsDead() && this is EntityPlayer)
			{
				BlockValue blockValue = this.world.GetBlock(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.boundingBox.min.y), Utils.Fastfloor(this.position.z));
				if (blockValue.isair || blockValue.Block.blockMaterial.IsGroundCover)
				{
					blockValue = this.world.GetBlock(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.boundingBox.min.y - 1f), Utils.Fastfloor(this.position.z));
				}
				if (!blockValue.isair)
				{
					num = Mathf.Clamp(1f - blockValue.Block.blockMaterial.Friction, 0.01f, 1f);
				}
			}
		}
		if (!this.RootMotion || (!this.onGround && this.jumpTicks > 0))
		{
			float num2;
			if (this.onGround)
			{
				num2 = this.landMovementFactor;
				float num3 = 0.163f / (num * num * num);
				num2 *= num3;
			}
			else
			{
				num2 = this.jumpMovementFactor;
			}
			this.Move(_direction, _isDirAbsolute, num2, this.MaxVelocity);
		}
		if (this.Climbing)
		{
			this.fallDistance = 0f;
			this.entityCollision(this.motion);
			this.distanceClimbed += this.motion.magnitude;
			if (this.distanceClimbed > 0.5f)
			{
				this.internalPlayStepSound(1f);
				this.distanceClimbed = 0f;
			}
		}
		else
		{
			if (base.IsInElevator())
			{
				if (!this.RootMotion)
				{
					float num4 = 0.15f;
					if (this.motion.x < -num4)
					{
						this.motion.x = -num4;
					}
					if (this.motion.x > num4)
					{
						this.motion.x = num4;
					}
					if (this.motion.z < -num4)
					{
						this.motion.z = -num4;
					}
					if (this.motion.z > num4)
					{
						this.motion.z = num4;
					}
				}
				this.fallDistance = 0f;
			}
			if (this.IsSleeping)
			{
				this.motion.x = 0f;
				this.motion.z = 0f;
			}
			this.entityCollision(this.motion);
		}
		if (this.isSwimming)
		{
			this.motion.x = this.motion.x * 0.91f;
			this.motion.z = this.motion.z * 0.91f;
			this.motion.y = this.motion.y - this.world.Gravity * 0.025f;
			this.motion.y = this.motion.y * 0.91f;
			return;
		}
		this.motion.x = this.motion.x * num;
		this.motion.z = this.motion.z * num;
		if (!this.bInElevator)
		{
			this.motion.y = this.motion.y - this.world.Gravity;
		}
		this.motion.y = this.motion.y * 0.98f;
	}

	// Token: 0x060022C4 RID: 8900 RVA: 0x000D2518 File Offset: 0x000D0718
	public virtual void MoveEntityHeaded(Vector3 _direction, bool _isDirAbsolute)
	{
		if (this.AttachedToEntity != null)
		{
			return;
		}
		if (this.jumpIsMoving)
		{
			this.JumpMove();
			return;
		}
		if (this.RootMotion)
		{
			if (this.isEntityRemote && this.bodyDamage.CurrentStun == EnumEntityStunType.None && !this.IsDead() && (!(this.emodel != null) || !(this.emodel.avatarController != null) || !this.emodel.avatarController.IsAnimationHitRunning()))
			{
				this.accumulatedRootMotion = Vector3.zero;
				return;
			}
			bool flag = this.emodel && this.emodel.IsRagdollActive;
			if (this.isSwimming && !flag)
			{
				this.motion += this.accumulatedRootMotion * 0.001f;
			}
			else if (this.onGround || this.jumpTicks > 0)
			{
				if (flag)
				{
					this.motion.x = 0f;
					this.motion.z = 0f;
				}
				else
				{
					float y = this.motion.y;
					this.motion = this.accumulatedRootMotion;
					this.motion.y = this.motion.y + y;
				}
			}
			this.accumulatedRootMotion = Vector3.zero;
		}
		if (this.IsFlyMode.Value)
		{
			EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			float num = ((primaryPlayer != null) ? primaryPlayer.GodModeSpeedModifier : 1f);
			float num2 = 2f * (this.MovementRunning ? 0.35f : 0.12f) * num;
			if (!this.RootMotion)
			{
				this.Move(_direction, _isDirAbsolute, this.GetPassiveEffectSpeedModifier() * num2, this.GetPassiveEffectSpeedModifier() * num2);
			}
			if (!this.IsNoCollisionMode.Value)
			{
				this.entityCollision(this.motion);
				this.motion *= base.ConditionalScalePhysicsMulConstant(0.546f);
			}
			else
			{
				this.SetPosition(this.position + this.motion, true);
				this.motion = Vector3.zero;
			}
		}
		else
		{
			this.DefaultMoveEntity(_direction, _isDirAbsolute);
		}
		if (!this.isEntityRemote && this.RootMotion)
		{
			float num3 = this.landMovementFactor;
			num3 *= 2.5f;
			if (this.inWaterPercent > 0.3f)
			{
				if (num3 > 0.01f)
				{
					float num4 = (this.inWaterPercent - 0.3f) * 1.4285715f;
					num3 = Mathf.Lerp(num3, 0.01f + (num3 - 0.01f) * 0.1f, num4);
				}
				if (this.isSwimming)
				{
					num3 = this.landMovementFactor * 5f;
				}
			}
			float magnitude = _direction.magnitude;
			if (magnitude > 1f)
			{
				num3 /= magnitude;
			}
			Vector3 vector = _direction;
			if (_isDirAbsolute)
			{
				vector = base.transform.InverseTransformDirection(this.moveDirection);
			}
			float num5 = vector.z * num3;
			if (this.lerpForwardSpeed)
			{
				if (Utils.FastAbs(this.speedForwardTarget - num5) > 0.05f)
				{
					this.speedForwardTargetStep = Utils.FastAbs(num5 - this.speedForward) / 0.18f;
				}
				this.speedForwardTarget = num5;
			}
			else
			{
				this.speedForward = num5;
			}
			this.speedStrafe = vector.x * num3;
			this.SetMovementState();
			base.ReplicateSpeeds();
		}
	}

	// Token: 0x060022C5 RID: 8901 RVA: 0x000D2870 File Offset: 0x000D0A70
	public float GetPassiveEffectSpeedModifier()
	{
		if (this.IsCrouching)
		{
			if (this.MovementRunning)
			{
				return EffectManager.GetValue(PassiveEffects.WalkSpeed, null, Constants.cPlayerSpeedModifierWalking, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			}
			return EffectManager.GetValue(PassiveEffects.CrouchSpeed, null, Constants.cPlayerSpeedModifierCrouching, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		else
		{
			if (this.MovementRunning)
			{
				return EffectManager.GetValue(PassiveEffects.RunSpeed, null, Constants.cPlayerSpeedModifierRunning, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			}
			return EffectManager.GetValue(PassiveEffects.WalkSpeed, null, Constants.cPlayerSpeedModifierWalking, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
	}

	// Token: 0x060022C6 RID: 8902 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CalcStrafeYawOffset(float _moveX, float _moveZ, ref float _desiredyaw, ref float _yawOffset)
	{
		return false;
	}

	// Token: 0x060022C7 RID: 8903 RVA: 0x000D2924 File Offset: 0x000D0B24
	public void SetMoveForward(float _moveForward)
	{
		this.moveDirection.x = 0f;
		this.moveDirection.z = _moveForward;
		this.isMoveDirAbsolute = false;
		this.Climbing = false;
		this.lerpForwardSpeed = true;
		this.motion.x = 0f;
		this.motion.z = 0f;
		this.accumulatedRootMotion.x = 0f;
		this.accumulatedRootMotion.z = 0f;
		if (this.bInElevator)
		{
			this.motion.y = 0f;
		}
	}

	// Token: 0x060022C8 RID: 8904 RVA: 0x000D29BC File Offset: 0x000D0BBC
	public void SetMoveForwardWithModifiers(float _speedModifier, float _speedScale, float _strafeAngle, bool _climb)
	{
		Vector3 vector = new Vector3(0f, 0f, 1f);
		vector = Quaternion.Euler(0f, _strafeAngle, 0f) * vector;
		this.moveDirection.x = vector.x;
		this.moveDirection.z = vector.z;
		this.isMoveDirAbsolute = false;
		this.Climbing = _climb;
		this.lerpForwardSpeed = true;
		float num = this.speedModifier;
		this.speedModifier = _speedModifier * _speedScale;
		if (num > 0.2f)
		{
			num = this.speedModifier / num;
			this.accumulatedRootMotion.x = this.accumulatedRootMotion.x * num;
			this.accumulatedRootMotion.z = this.accumulatedRootMotion.z * num;
		}
	}

	// Token: 0x060022C9 RID: 8905 RVA: 0x000D2A70 File Offset: 0x000D0C70
	public void AddMotion(float dir, float speed)
	{
		float num = dir * 0.017453292f;
		this.accumulatedRootMotion.x = this.accumulatedRootMotion.x + Mathf.Sin(num) * speed;
		this.accumulatedRootMotion.z = this.accumulatedRootMotion.z + Mathf.Cos(num) * speed;
	}

	// Token: 0x060022CA RID: 8906 RVA: 0x000D2AB4 File Offset: 0x000D0CB4
	public void MakeMotionMoveToward(float x, float z, float minMotion, float maxMotion)
	{
		if (this.RootMotion)
		{
			float num = Mathf.Sqrt(x * x + z * z);
			if (num > 0f)
			{
				num = Utils.FastClamp(Mathf.Sqrt(this.accumulatedRootMotion.x * this.accumulatedRootMotion.x + this.accumulatedRootMotion.z * this.accumulatedRootMotion.z), minMotion, maxMotion) / num;
				if (num < 1f)
				{
					x *= num;
					z *= num;
				}
			}
			this.accumulatedRootMotion.x = x;
			this.accumulatedRootMotion.z = z;
			return;
		}
		this.moveDirection.x = x;
		this.moveDirection.z = z;
		this.isMoveDirAbsolute = true;
	}

	// Token: 0x060022CB RID: 8907 RVA: 0x000D2B68 File Offset: 0x000D0D68
	public bool IsInFrontOfMe(Vector3 _position)
	{
		Vector3 headPosition = this.getHeadPosition();
		Vector3 vector = _position - headPosition;
		Vector3 forwardVector = this.GetForwardVector();
		float angleBetween = Utils.GetAngleBetween(vector, forwardVector);
		float num = this.GetMaxViewAngle() * 0.5f;
		return angleBetween >= -num && angleBetween <= num;
	}

	// Token: 0x060022CC RID: 8908 RVA: 0x000D2BAC File Offset: 0x000D0DAC
	public bool IsInViewCone(Vector3 _position)
	{
		Vector3 headPosition = this.getHeadPosition();
		Vector3 vector = _position - headPosition;
		Vector3 lookVector;
		float num;
		if (this.IsSleeping)
		{
			lookVector = this.sleeperLookDir;
			num = this.sleeperViewAngle;
		}
		else
		{
			lookVector = this.GetLookVector();
			num = this.GetMaxViewAngle();
		}
		num *= 0.5f;
		float angleBetween = Utils.GetAngleBetween(vector, lookVector);
		return angleBetween >= -num && angleBetween <= num;
	}

	// Token: 0x060022CD RID: 8909 RVA: 0x000D2C08 File Offset: 0x000D0E08
	public void DrawViewCone()
	{
		Vector3 vector;
		float num;
		if (this.IsSleeping)
		{
			vector = this.sleeperLookDir;
			num = this.sleeperViewAngle;
		}
		else
		{
			vector = this.GetLookVector();
			num = this.GetMaxViewAngle();
		}
		vector *= this.GetSeeDistance();
		num *= 0.5f;
		Vector3 vector2 = this.getHeadPosition() - Origin.position;
		Debug.DrawRay(vector2, vector, new Color(0.9f, 0.9f, 0.5f), 0.1f);
		Vector3 vector3 = Quaternion.Euler(0f, -num, 0f) * vector;
		Debug.DrawRay(vector2, vector3, new Color(0.6f, 0.6f, 0.3f), 0.1f);
		Vector3 vector4 = Quaternion.Euler(0f, num, 0f) * vector;
		Debug.DrawRay(vector2, vector4, new Color(0.6f, 0.6f, 0.3f), 0.1f);
	}

	// Token: 0x060022CE RID: 8910 RVA: 0x000D2CF0 File Offset: 0x000D0EF0
	public bool CanSee(Vector3 _pos)
	{
		Vector3 headPosition = this.getHeadPosition();
		Vector3 vector = _pos - headPosition;
		float seeDistance = this.GetSeeDistance();
		if (vector.magnitude > seeDistance)
		{
			return false;
		}
		if (!this.IsInViewCone(_pos))
		{
			return false;
		}
		Ray ray = new Ray(headPosition, vector);
		ray.origin += vector.normalized * 0.2f;
		int modelLayer = this.GetModelLayer();
		this.SetModelLayer(2, false, null);
		bool flag = true;
		if (Voxel.Raycast(this.world, ray, seeDistance, false, false))
		{
			flag = false;
		}
		this.SetModelLayer(modelLayer, false, null);
		return flag;
	}

	// Token: 0x060022CF RID: 8911 RVA: 0x000D2D8C File Offset: 0x000D0F8C
	public bool CanEntityBeSeen(Entity _other, bool checkViewCone = true)
	{
		Vector3 headPosition = this.getHeadPosition();
		Vector3 headPosition2 = _other.getHeadPosition();
		Vector3 vector = headPosition2 - headPosition;
		float magnitude = vector.magnitude;
		float num = this.GetSeeDistance();
		EntityPlayer entityPlayer = _other as EntityPlayer;
		if (entityPlayer != null)
		{
			num *= entityPlayer.DetectUsScale(this);
		}
		if (magnitude > num)
		{
			return false;
		}
		if (checkViewCone && !this.IsInViewCone(headPosition2))
		{
			return false;
		}
		bool flag = false;
		Ray ray = new Ray(headPosition, vector);
		ray.origin += vector.normalized * -0.1f;
		int modelLayer = this.GetModelLayer();
		this.SetModelLayer(2, false, null);
		if (Voxel.Raycast(this.world, ray, num, -1612492829, 64, 0f))
		{
			if (Voxel.voxelRayHitInfo.tag == "E_Vehicle")
			{
				EntityVehicle entityVehicle = EntityVehicle.FindCollisionEntity(Voxel.voxelRayHitInfo.transform);
				if (entityVehicle && entityVehicle.IsAttached(_other))
				{
					flag = true;
				}
			}
			else
			{
				if (Voxel.voxelRayHitInfo.tag == "E_Enemy")
				{
					EntityDrone entityDrone = EntityDrone.FindCollisionEntity(Voxel.voxelRayHitInfo.transform);
					if (entityDrone)
					{
						entityDrone.IgnoreCollisionEntity(ray, num);
					}
				}
				if (Voxel.voxelRayHitInfo.tag.StartsWith("E_BP_"))
				{
					Voxel.voxelRayHitInfo.transform = GameUtils.GetHitRootTransform(Voxel.voxelRayHitInfo.tag, Voxel.voxelRayHitInfo.transform);
				}
				if (_other.transform == Voxel.voxelRayHitInfo.transform)
				{
					flag = true;
				}
			}
		}
		this.SetModelLayer(modelLayer, false, null);
		return flag;
	}

	// Token: 0x060022D0 RID: 8912 RVA: 0x000D2F28 File Offset: 0x000D1128
	public virtual float GetSeeDistance()
	{
		this.senseScale = 1f;
		if (this.IsSleeping)
		{
			this.sightRange = this.sleeperSightRange;
			return this.sleeperSightRange;
		}
		this.sightRange = this.sightRangeBase;
		if (this.aiManager != null)
		{
			float num = EAIManager.CalcSenseScale();
			this.senseScale = 1f + num * this.aiManager.feralSense;
			this.sightRange = this.sightRangeBase * this.senseScale;
		}
		return this.sightRange;
	}

	// Token: 0x060022D1 RID: 8913 RVA: 0x000D2FA8 File Offset: 0x000D11A8
	public bool CanSeeStealth(float dist, float lightLevel)
	{
		float num = dist / this.sightRange;
		float num2 = Utils.FastLerp(this.sightLightThreshold.x, this.sightLightThreshold.y, num);
		return lightLevel > num2;
	}

	// Token: 0x060022D2 RID: 8914 RVA: 0x000D2FE4 File Offset: 0x000D11E4
	public float GetSeeStealthDebugScale(float dist)
	{
		float num = dist / this.sightRange;
		return Utils.FastLerp(this.sightLightThreshold.x, this.sightLightThreshold.y, num);
	}

	// Token: 0x060022D3 RID: 8915 RVA: 0x000D3018 File Offset: 0x000D1218
	public override void SetAlive()
	{
		if (this.IsDead())
		{
			this.lastAliveTime = Time.time;
		}
		base.SetAlive();
		if (!this.isEntityRemote)
		{
			this.Stats.ResetStats();
		}
		this.Stats.Health.MaxModifier = 0f;
		this.Health = (int)this.Stats.Health.ModifiedMax;
		this.Stamina = this.Stats.Stamina.ModifiedMax;
		this.deathUpdateTime = 0;
		this.bDead = false;
		this.RecordedDamage.Fatal = false;
		this.emodel.SetAlive();
	}

	// Token: 0x060022D4 RID: 8916 RVA: 0x000D30B8 File Offset: 0x000D12B8
	public float YawForTarget(Entity _otherEntity)
	{
		return this.YawForTarget(_otherEntity.GetPosition());
	}

	// Token: 0x060022D5 RID: 8917 RVA: 0x000D30C8 File Offset: 0x000D12C8
	public float YawForTarget(Vector3 target)
	{
		float num = target.x - this.position.x;
		return -(float)(Math.Atan2((double)(target.z - this.position.z), (double)num) * 180.0 / 3.141592653589793) + 90f;
	}

	// Token: 0x060022D6 RID: 8918 RVA: 0x000D3120 File Offset: 0x000D1320
	public void RotateTo(Entity _otherEntity, float _dYaw, float _dPitch)
	{
		float num = _otherEntity.position.x - this.position.x;
		float num2 = _otherEntity.position.z - this.position.z;
		float num3;
		if (_otherEntity is EntityAlive)
		{
			EntityAlive entityAlive = (EntityAlive)_otherEntity;
			num3 = this.position.y + this.GetEyeHeight() - (entityAlive.position.y + entityAlive.GetEyeHeight());
		}
		else
		{
			num3 = (_otherEntity.boundingBox.min.y + _otherEntity.boundingBox.max.y) / 2f - (this.position.y + this.GetEyeHeight());
		}
		float num4 = Mathf.Sqrt(num * num + num2 * num2);
		float num5 = -(float)(Math.Atan2((double)num2, (double)num) * 180.0 / 3.141592653589793) + 90f;
		float num6 = (float)(-(float)(Math.Atan2((double)num3, (double)num4) * 180.0 / 3.141592653589793));
		this.rotation.x = EntityAlive.UpdateRotation(this.rotation.x, num6, _dPitch);
		this.rotation.y = EntityAlive.UpdateRotation(this.rotation.y, num5, _dYaw);
	}

	// Token: 0x060022D7 RID: 8919 RVA: 0x000D3268 File Offset: 0x000D1468
	public void RotateTo(float _x, float _y, float _z, float _dYaw, float _dPitch)
	{
		float num = _x - this.position.x;
		float num2 = _z - this.position.z;
		float num3 = Mathf.Sqrt(num * num + num2 * num2);
		float num4 = -(float)(Math.Atan2((double)num2, (double)num) * 180.0 / 3.141592653589793) + 90f;
		this.rotation.y = EntityAlive.UpdateRotation(this.rotation.y, num4, _dYaw);
		if (_dPitch > 0f)
		{
			float num5 = (float)(-(float)(Math.Atan2((double)(_y - this.position.y), (double)num3) * 180.0 / 3.141592653589793));
			this.rotation.x = -EntityAlive.UpdateRotation(this.rotation.x, num5, _dPitch);
		}
	}

	// Token: 0x060022D8 RID: 8920 RVA: 0x000D3338 File Offset: 0x000D1538
	public static float UpdateRotation(float _curRotation, float _intendedRotation, float _maxIncr)
	{
		float num;
		for (num = _intendedRotation - _curRotation; num < -180f; num += 360f)
		{
		}
		while (num >= 180f)
		{
			num -= 360f;
		}
		if (num > _maxIncr)
		{
			num = _maxIncr;
		}
		if (num < -_maxIncr)
		{
			num = -_maxIncr;
		}
		return _curRotation + num;
	}

	// Token: 0x060022D9 RID: 8921 RVA: 0x000D3380 File Offset: 0x000D1580
	public override float GetEyeHeight()
	{
		if (this.walkType == 21)
		{
			return 0.15f;
		}
		if (this.walkType == 22)
		{
			return 0.6f;
		}
		if (!this.IsCrouching)
		{
			return base.height * 0.8f;
		}
		return base.height * 0.5f;
	}

	// Token: 0x060022DA RID: 8922 RVA: 0x000D33CE File Offset: 0x000D15CE
	public virtual float GetSpeedModifier()
	{
		return this.speedModifier;
	}

	// Token: 0x060022DB RID: 8923 RVA: 0x000D33D8 File Offset: 0x000D15D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void fallHitGround(float _distance, Vector3 _fallMotion)
	{
		base.fallHitGround(_distance, _fallMotion);
		if (_distance > 2f)
		{
			int num = (int)((-_fallMotion.y - 0.85f) * 160f);
			if (num > 0)
			{
				this.DamageEntity(DamageSource.fall, num, false, 1f);
			}
			this.PlayHitGroundSound(1f);
		}
		if (!this.IsDead() && !this.emodel.IsRagdollActive && (this.disableFallBehaviorUntilOnGround || !this.ChooseFallBehavior(_distance, _fallMotion)) && this.emodel && this.emodel.avatarController)
		{
			this.emodel.avatarController.StartAnimationJump(AnimJumpMode.Land);
		}
		if (this.aiManager != null)
		{
			this.aiManager.FallHitGround(_distance);
		}
	}

	// Token: 0x060022DC RID: 8924 RVA: 0x000D3498 File Offset: 0x000D1698
	public bool NotifyDestroyedBlock(ItemActionAttack.AttackHitInfo attackHitInfo)
	{
		if (attackHitInfo == null || this.moveHelper == null || this.moveHelper.BlockedFlags <= 0)
		{
			return false;
		}
		if (this.moveHelper.HitInfo.hit.blockValueRef == attackHitInfo.hitRef)
		{
			this.moveHelper.ClearBlocked();
		}
		if (this._destroyBlockBehaviors.Count == 0)
		{
			return false;
		}
		float num = 0f;
		EntityAlive.weightBehaviorTemp.Clear();
		int num2 = 1;
		for (int i = 0; i < this._destroyBlockBehaviors.Count; i++)
		{
			EntityAlive.DestroyBlockBehavior destroyBlockBehavior = this._destroyBlockBehaviors[i];
			if (num2 >= destroyBlockBehavior.Difficulty.min && num2 <= destroyBlockBehavior.Difficulty.max)
			{
				EntityAlive.WeightBehavior weightBehavior;
				weightBehavior.weight = destroyBlockBehavior.Weight + num;
				weightBehavior.index = i;
				EntityAlive.weightBehaviorTemp.Add(weightBehavior);
				num += destroyBlockBehavior.Weight;
			}
		}
		bool flag = false;
		if (num > 0f)
		{
			EntityAlive.DestroyBlockBehavior destroyBlockBehavior2 = null;
			float num3 = this.rand.RandomFloat * num;
			for (int j = 0; j < EntityAlive.weightBehaviorTemp.Count; j++)
			{
				if (num3 <= EntityAlive.weightBehaviorTemp[j].weight)
				{
					destroyBlockBehavior2 = this._destroyBlockBehaviors[EntityAlive.weightBehaviorTemp[j].index];
					break;
				}
			}
			if (destroyBlockBehavior2 != null)
			{
				flag = this.ExecuteDestroyBlockBehavior(destroyBlockBehavior2, attackHitInfo);
			}
		}
		return flag;
	}

	// Token: 0x060022DD RID: 8925 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ExecuteDestroyBlockBehavior(EntityAlive.DestroyBlockBehavior behavior, ItemActionAttack.AttackHitInfo attackHitInfo)
	{
		return false;
	}

	// Token: 0x060022DE RID: 8926 RVA: 0x000D3608 File Offset: 0x000D1808
	[PublicizedFrom(EAccessModifier.Private)]
	public bool ChooseFallBehavior(float _distance, Vector3 _fallMotion)
	{
		if (this.fallBehaviors.Count == 0)
		{
			return false;
		}
		float num = 0f;
		EntityAlive.weightBehaviorTemp.Clear();
		for (int i = 0; i < this.fallBehaviors.Count; i++)
		{
			EntityAlive.FallBehavior fallBehavior = this.fallBehaviors[i];
			if (_distance >= fallBehavior.Height.min && _distance <= fallBehavior.Height.max && 1 >= fallBehavior.Difficulty.min && 1 <= fallBehavior.Difficulty.max)
			{
				EntityAlive.WeightBehavior weightBehavior;
				weightBehavior.weight = fallBehavior.Weight + num;
				weightBehavior.index = i;
				EntityAlive.weightBehaviorTemp.Add(weightBehavior);
				num += fallBehavior.Weight;
			}
		}
		bool flag = false;
		if (num > 0f)
		{
			EntityAlive.FallBehavior fallBehavior2 = null;
			float num2 = this.rand.RandomFloat * num;
			for (int j = 0; j < EntityAlive.weightBehaviorTemp.Count; j++)
			{
				if (num2 <= EntityAlive.weightBehaviorTemp[j].weight)
				{
					fallBehavior2 = this.fallBehaviors[EntityAlive.weightBehaviorTemp[j].index];
					break;
				}
			}
			if (fallBehavior2 != null)
			{
				flag = this.ExecuteFallBehavior(fallBehavior2, _distance, _fallMotion);
			}
		}
		return flag;
	}

	// Token: 0x060022DF RID: 8927 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool ExecuteFallBehavior(EntityAlive.FallBehavior behavior, float _distance, Vector3 _fallMotion)
	{
		return false;
	}

	// Token: 0x060022E0 RID: 8928 RVA: 0x000D3740 File Offset: 0x000D1940
	public virtual void PlayHitGroundSound(float impactSpeed = 1f)
	{
		float num = Mathf.Lerp(0.3f, 1f, impactSpeed);
		if (!string.IsNullOrEmpty(this.soundLand))
		{
			this.PlayOneShot(this.soundLand, false, false, false, null, num);
			return;
		}
		if (!string.IsNullOrEmpty(this.soundLandThump))
		{
			this.PlayOneShot(this.soundLandThump, false, false, false, null, num);
			return;
		}
		this.PlayOneShot("entityhitsground", false, false, false, null, num);
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool FriendlyFireCheck(EntityAlive other)
	{
		return true;
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool HasImmunity(BuffClass _buffClass)
	{
		return false;
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000D37AC File Offset: 0x000D19AC
	public int CalculateBlockDamage(BlockDamage block, int defaultBlockDamage, out bool bypassMaxDamage)
	{
		if (this.stompsSpikes && block.HasTag(BlockTags.Spike))
		{
			bypassMaxDamage = true;
			return 999;
		}
		bypassMaxDamage = false;
		return defaultBlockDamage;
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000D37CC File Offset: 0x000D19CC
	public override int DamageEntity(DamageSource _damageSource, int _strength, bool _criticalHit, float _impulseScale = 1f)
	{
		if (_damageSource.damageType == EnumDamageTypes.Suicide && this.emodel && this.emodel.avatarController is AvatarZombieController)
		{
			(this.emodel.avatarController as AvatarZombieController).CleanupDismemberedLimbs();
		}
		EnumDamageSource source = _damageSource.GetSource();
		if (_damageSource.IsIgnoreConsecutiveDamages() && source != EnumDamageSource.Internal)
		{
			if (this.damageSourceTimeouts.ContainsKey(source) && GameTimer.Instance.ticks - this.damageSourceTimeouts[source] < 30UL)
			{
				return -1;
			}
			this.damageSourceTimeouts[source] = GameTimer.Instance.ticks;
		}
		EntityAlive entityAlive = this.world.GetEntity(_damageSource.getEntityId()) as EntityAlive;
		if (!this.FriendlyFireCheck(entityAlive))
		{
			return -1;
		}
		bool flag = _damageSource.GetDamageType() == EnumDamageTypes.Heat;
		if (!flag && entityAlive && (this.entityFlags & entityAlive.entityFlags & EntityFlags.Zombie) > EntityFlags.None)
		{
			return -1;
		}
		if (this.IsGodMode.Value)
		{
			return -1;
		}
		if (!this.IsDead() && entityAlive)
		{
			PassiveEffects passiveEffects = PassiveEffects.DamageBonus;
			EntityAlive entityAlive2 = entityAlive;
			float value = EffectManager.GetValue(passiveEffects, _damageSource.AttackingItem, 0f, entityAlive2, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			if (value > 0f)
			{
				_damageSource.DamageMultiplier = value;
				_damageSource.BonusDamageType = EnumDamageBonusType.Sneak;
			}
		}
		this.MinEventContext.Other = entityAlive;
		float num = Utils.FastMin(1f, EffectManager.GetValue(PassiveEffects.GeneralDamageResist, null, 0f, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
		float num2 = (float)_strength * num + this.accumulatedDamageResisted;
		int num3 = Utils.FastMin(_strength, (int)num2);
		this.accumulatedDamageResisted = num2 - (float)num3;
		_strength -= num3;
		DamageResponse damageResponse = this.damageEntityLocal(_damageSource, _strength, _criticalHit, _impulseScale);
		NetPackage netPackage = NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, damageResponse);
		if (this.world.IsRemote())
		{
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(netPackage, false);
		}
		else
		{
			int num4 = -1;
			if (!flag && _damageSource.CreatorEntityId != -2)
			{
				num4 = _damageSource.getEntityId();
				if (_damageSource.CreatorEntityId != -1)
				{
					Entity entity = this.world.GetEntity(_damageSource.CreatorEntityId);
					if (entity && !entity.isEntityRemote)
					{
						num4 = -1;
					}
				}
			}
			this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, num4, netPackage, false);
		}
		return damageResponse.ModStrength;
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000D3A27 File Offset: 0x000D1C27
	public virtual void SetDamagedTarget(EntityAlive _attackTarget)
	{
		this.damagedTarget = _attackTarget;
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000D3A30 File Offset: 0x000D1C30
	public virtual void ClearDamagedTarget()
	{
		this.damagedTarget = null;
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000D3A39 File Offset: 0x000D1C39
	public EntityAlive GetDamagedTarget()
	{
		return this.damagedTarget;
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000D3A41 File Offset: 0x000D1C41
	public override bool IsDead()
	{
		return base.IsDead() || this.RecordedDamage.Fatal;
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000D3A58 File Offset: 0x000D1C58
	public override bool CanLockOnServer(int _lockingPlayerID, ILockContext _context, ushort _channel)
	{
		return !this.IsDead() && base.CanLockOnServer(_lockingPlayerID, _context, _channel);
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000D3A6D File Offset: 0x000D1C6D
	public override bool CanLockLocally(ILockContext _context, ushort _channel)
	{
		return !this.IsDead() && base.CanLockLocally(_context, _channel);
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000D3A84 File Offset: 0x000D1C84
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual DamageResponse damageEntityLocal(DamageSource _damageSource, int _strength, bool _criticalHit, float impulseScale)
	{
		DamageResponse damageResponse = default(DamageResponse);
		damageResponse.Source = _damageSource;
		damageResponse.Strength = _strength;
		damageResponse.Critical = _criticalHit;
		damageResponse.HitDirection = Utils.EnumHitDirection.None;
		damageResponse.MovementState = this.MovementState;
		damageResponse.Random = this.rand.RandomFloat;
		damageResponse.ImpulseScale = impulseScale;
		damageResponse.HitBodyPart = _damageSource.GetEntityDamageBodyPart(this);
		damageResponse.ArmorSlot = _damageSource.GetEntityDamageEquipmentSlot(this);
		damageResponse.ArmorSlotGroup = _damageSource.GetEntityDamageEquipmentSlotGroup(this);
		if (_strength > 0)
		{
			damageResponse.HitDirection = (_damageSource.Equals(DamageSource.fall) ? Utils.EnumHitDirection.Back : ((Utils.EnumHitDirection)Utils.Get4HitDirectionAsInt(_damageSource.getDirection(), this.GetLookVector())));
		}
		if (_damageSource.AffectedByArmor())
		{
			this.equipment.CalcDamage(ref damageResponse.Strength, ref damageResponse.ArmorDamage, damageResponse.Source.DamageTypeTag, this.MinEventContext.Other, damageResponse.Source.AttackingItem);
		}
		float num = this.GetDamageFraction((float)damageResponse.Strength);
		if (damageResponse.Fatal || damageResponse.Strength >= this.Health)
		{
			if ((damageResponse.HitBodyPart & EnumBodyPartHit.Head) > EnumBodyPartHit.None)
			{
				if (num >= 0.2f)
				{
					damageResponse.Source.DismemberChance = Utils.FastMax(damageResponse.Source.DismemberChance * 0.5f, 0.3f);
				}
			}
			else if (num >= 0.12f)
			{
				damageResponse.Source.DismemberChance = Utils.FastMax(damageResponse.Source.DismemberChance * 0.5f, 0.5f);
			}
			num = 1f;
			if (this.canDisintegrate)
			{
				this.Disintegrate();
			}
		}
		this.CheckDismember(ref damageResponse, num);
		int num2 = this.bodyDamage.StunKnee;
		int num3 = this.bodyDamage.StunProne;
		if ((damageResponse.HitBodyPart & EnumBodyPartHit.Head) > EnumBodyPartHit.None && damageResponse.Dismember)
		{
			if (this.Health > 0)
			{
				damageResponse.Strength = this.Health;
			}
		}
		else if (_damageSource.CanStun && this.GetWalkType() != 21 && this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
		{
			if ((damageResponse.HitBodyPart & (EnumBodyPartHit.Torso | EnumBodyPartHit.Head | EnumBodyPartHit.LeftUpperArm | EnumBodyPartHit.RightUpperArm | EnumBodyPartHit.LeftLowerArm | EnumBodyPartHit.RightLowerArm)) > EnumBodyPartHit.None)
			{
				num3 += _strength;
			}
			else if (damageResponse.HitBodyPart.IsLeg())
			{
				num2 += _strength * (_criticalHit ? 2 : 1);
			}
		}
		if ((!damageResponse.HitBodyPart.IsLeg() || !damageResponse.Dismember) && this.GetWalkType() != 21 && !this.sleepingOrWakingUp)
		{
			EntityClass entityClass = EntityClass.list[this.entityClass];
			if (this.GetDamageFraction((float)num3) >= entityClass.KnockdownProneDamageThreshold && entityClass.KnockdownProneDamageThreshold > 0f)
			{
				if (this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
				{
					damageResponse.Stun = EnumEntityStunType.Prone;
					damageResponse.StunDuration = this.rand.RandomRange(entityClass.KnockdownProneStunDuration.x, entityClass.KnockdownProneStunDuration.y);
				}
			}
			else if (this.GetDamageFraction((float)num2) >= entityClass.KnockdownKneelDamageThreshold && entityClass.KnockdownKneelDamageThreshold > 0f && this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
			{
				damageResponse.Stun = EnumEntityStunType.Kneel;
				damageResponse.StunDuration = this.rand.RandomRange(entityClass.KnockdownKneelStunDuration.x, entityClass.KnockdownKneelStunDuration.y);
			}
		}
		bool flag = false;
		int num4 = damageResponse.Strength + damageResponse.ArmorDamage / 2;
		if (num4 > 0 && !this.IsGodMode.Value && damageResponse.Stun == EnumEntityStunType.None && !this.sleepingOrWakingUp)
		{
			flag = damageResponse.Strength < this.Health;
			if (flag)
			{
				flag = this.GetWalkType() == 21 || !damageResponse.Dismember || !damageResponse.HitBodyPart.IsLeg();
			}
			if (flag && damageResponse.Source.GetDamageType() != EnumDamageTypes.Bashing)
			{
				flag = num4 >= 6;
			}
			if (damageResponse.Source.GetDamageType() == EnumDamageTypes.BarbedWire)
			{
				flag = true;
			}
		}
		damageResponse.PainHit = flag;
		if (damageResponse.Strength >= this.Health)
		{
			damageResponse.Fatal = true;
		}
		if (damageResponse.Fatal)
		{
			damageResponse.Stun = EnumEntityStunType.None;
		}
		if (this.isEntityRemote)
		{
			damageResponse.ModStrength = 0;
		}
		else
		{
			if (this.Health <= damageResponse.Strength)
			{
				_strength -= this.Health;
			}
			damageResponse.ModStrength = _strength;
		}
		if (damageResponse.Dismember)
		{
			EntityAlive entityAlive = this.world.GetEntity(damageResponse.Source.getEntityId()) as EntityAlive;
			if (entityAlive != null)
			{
				entityAlive.FireEvent(MinEventTypes.onDismember, true);
			}
		}
		if (this.MinEventContext.Other != null)
		{
			this.MinEventContext.Other.MinEventContext.DamageResponse = damageResponse;
			float value = EffectManager.GetValue(PassiveEffects.HealthSteal, null, 0f, this.MinEventContext.Other, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			if (value != 0f)
			{
				int num5 = (int)((float)num4 * value);
				if (num5 + this.MinEventContext.Other.Health <= 0)
				{
					num5 = (this.MinEventContext.Other.Health - 1) * -1;
				}
				this.MinEventContext.Other.AddHealth(num5);
				if (num5 < 0 && this.MinEventContext.Other is EntityPlayerLocal)
				{
					((EntityPlayerLocal)this.MinEventContext.Other).ForceBloodSplatter();
				}
			}
		}
		this.FireAttackedEvents(damageResponse);
		this.ProcessDamageResponseLocal(damageResponse);
		return damageResponse;
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000D3FDC File Offset: 0x000D21DC
	public override void FireAttackedEvents(DamageResponse _dmResponse)
	{
		base.FireAttackedEvents(_dmResponse);
		if (_dmResponse.Source.BuffClass == null || this.Progression != null)
		{
			this.MinEventContext.DamageResponse = _dmResponse;
			EntityAlive entityAlive = this.world.GetEntity(_dmResponse.Source.getEntityId()) as EntityAlive;
			if (entityAlive && !entityAlive.isEntityRemote)
			{
				this.MinEventContext.IsLocal = this is EntityPlayer && this.isEntityRemote;
			}
			if (_dmResponse.Source.BuffClass == null)
			{
				this.FireEvent(MinEventTypes.onOtherAttackedSelf, true);
			}
			else if (this.Progression != null)
			{
				this.Progression.FireEvent(MinEventTypes.onOtherAttackedSelf, this.MinEventContext);
			}
			this.MinEventContext.IsLocal = false;
		}
	}

	// Token: 0x17000419 RID: 1049
	// (get) Token: 0x060022ED RID: 8941 RVA: 0x000D409C File Offset: 0x000D229C
	public virtual bool IsImmuneToLegDamage
	{
		get
		{
			EntityClass entityClass = EntityClass.list[this.entityClass];
			return this.GetWalkType() == 21 || !this.bodyDamage.HasLeftLeg || !this.bodyDamage.HasRightLeg || (entityClass.LowerLegDismemberThreshold <= 0f && entityClass.UpperLegDismemberThreshold <= 0f);
		}
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000D4100 File Offset: 0x000D2300
	public override void ProcessDamageResponse(DamageResponse _dmResponse)
	{
		if (Time.time - this.lastAliveTime < 1f)
		{
			return;
		}
		base.ProcessDamageResponse(_dmResponse);
		this.ProcessDamageResponseLocal(_dmResponse);
		if (!this.world.IsRemote())
		{
			Entity entity = this.world.GetEntity(_dmResponse.Source.getEntityId());
			if (entity && !entity.isEntityRemote && this.isEntityRemote && this is EntityPlayer)
			{
				this.world.entityDistributer.SendPacketToTrackedPlayers(this.entityId, this.entityId, NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, _dmResponse), false);
				return;
			}
			if (_dmResponse.Source.BuffClass != null)
			{
				this.world.entityDistributer.SendPacketToTrackedPlayers(this.entityId, this.entityId, NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, _dmResponse), false);
				return;
			}
			this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, _dmResponse.Source.getEntityId(), NetPackageManager.GetPackage<NetPackageDamageEntity>().Setup(this.entityId, _dmResponse), false);
		}
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000D4214 File Offset: 0x000D2414
	public virtual void ProcessDamageResponseLocal(DamageResponse _dmResponse)
	{
		if (this.emodel == null)
		{
			return;
		}
		if (_dmResponse.Source.BonusDamageType != EnumDamageBonusType.None)
		{
			EntityPlayerLocal primaryPlayer = this.world.GetPrimaryPlayer();
			if (primaryPlayer && primaryPlayer.entityId == _dmResponse.Source.getEntityId())
			{
				EnumDamageBonusType bonusDamageType = _dmResponse.Source.BonusDamageType;
				if (bonusDamageType != EnumDamageBonusType.Sneak)
				{
					if (bonusDamageType == EnumDamageBonusType.Stun)
					{
						primaryPlayer.NotifyDamageMultiplier(_dmResponse.Source.DamageMultiplier);
					}
				}
				else
				{
					primaryPlayer.NotifySneakDamage(_dmResponse.Source.DamageMultiplier);
				}
			}
		}
		EntityAlive entityAlive = this.world.GetEntity(_dmResponse.Source.getEntityId()) as EntityAlive;
		if (entityAlive != null)
		{
			entityAlive.SetDamagedTarget(this);
		}
		if (this.IsSleeperPassive)
		{
			this.world.CheckSleeperVolumeNoise(this.position);
		}
		this.ConditionalTriggerSleeperWakeUp();
		this.SleeperSupressLivingSounds = false;
		this.bPlayHurtSound = false;
		if (this.equipment != null && _dmResponse.ArmorDamage > 0)
		{
			List<ItemValue> armor = this.equipment.GetArmor();
			if (armor.Count > 0)
			{
				float num = (float)_dmResponse.ArmorDamage / (float)armor.Count;
				if (num < 1f && num != 0f)
				{
					num = 1f;
				}
				for (int i = 0; i < armor.Count; i++)
				{
					armor[i].UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, armor[i], num, this, null, armor[i].ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
				}
			}
		}
		this.ApplyLocalBodyDamage(_dmResponse);
		this.lastHitRanged = false;
		this.lastDamageResponse = _dmResponse;
		bool flag = EffectManager.GetValue(PassiveEffects.NegateDamageSelf, null, 0f, this, null, FastTags<TagGroup.Global>.Parse(_dmResponse.HitBodyPart.ToString()), true, true, true, true, true, 1, true, false) > 0f || EffectManager.GetValue(PassiveEffects.NegateDamageOther, (entityAlive != null) ? entityAlive.inventory.holdingItemItemValue : null, 0f, entityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0f;
		if (_dmResponse.Dismember && !flag)
		{
			this.lastHitImpactDir = _dmResponse.Source.getDirection();
			if (entityAlive != null)
			{
				this.lastHitEntityFwd = entityAlive.GetForwardVector();
			}
			if (_dmResponse.Source.ItemClass != null && _dmResponse.Source.ItemClass.HasAnyTags(DismembermentManager.rangedTags))
			{
				this.lastHitRanged = true;
			}
			if (_dmResponse.Source.ItemClass != null)
			{
				float num2 = (float)_dmResponse.ModStrength / (float)this.GetMaxHealth();
				this.lastHitForce = DismembermentManager.GetImpactForce(_dmResponse.Source.ItemClass, num2);
			}
			this.ExecuteDismember(false);
		}
		bool flag2 = _dmResponse.Stun > EnumEntityStunType.None;
		bool flag3 = this.bodyDamage.CurrentStun > EnumEntityStunType.None;
		bool flag4 = this.Health <= 0;
		int num3 = _dmResponse.Strength;
		if (!(this is EntityPlayer) && base.EntityClass.bIsEnemyEntity)
		{
			if (!flag4 && ((EntityAlive.IsHeadshotOnly && entityAlive is EntityPlayer) || EffectManager.GetValue(PassiveEffects.HeadShotOnly, null, 0f, entityAlive, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0f) && !base.HasAnyTags(EntityAlive.noheadTag) && (_dmResponse.HitBodyPart & EnumBodyPartHit.Head) == EnumBodyPartHit.None)
			{
				num3 = 0;
				_dmResponse.Fatal = false;
			}
			if (!flag4 && EntityAlive.IsHeadshotFinisher && entityAlive is EntityPlayer && _dmResponse.Fatal && !base.HasAnyTags(EntityAlive.noheadTag) && (_dmResponse.HitBodyPart & EnumBodyPartHit.Head) == EnumBodyPartHit.None)
			{
				num3 = this.Health - 1;
				_dmResponse.Fatal = false;
			}
		}
		if (!flag && _dmResponse.Fatal && this.isEntityRemote)
		{
			this.ClientKill(_dmResponse);
		}
		else if (flag2 && this.emodel.avatarController)
		{
			if (_dmResponse.Stun == EnumEntityStunType.Prone)
			{
				if (this.bodyDamage.CurrentStun == EnumEntityStunType.None)
				{
					if ((_dmResponse.Critical && _dmResponse.Source.damageType == EnumDamageTypes.Bashing) || this.rand.RandomFloat < 0.6f)
					{
						this.DoRagdoll(in _dmResponse);
					}
					else
					{
						this.emodel.avatarController.BeginStun(EnumEntityStunType.Prone, _dmResponse.HitBodyPart, _dmResponse.HitDirection, _dmResponse.Critical, _dmResponse.Random);
					}
					this.SetStun(EnumEntityStunType.Prone);
					this.bodyDamage.StunDuration = _dmResponse.StunDuration;
				}
				else if (this.bodyDamage.CurrentStun != EnumEntityStunType.Prone)
				{
					this.DoRagdoll(in _dmResponse);
					this.SetStun(EnumEntityStunType.Prone);
					this.bodyDamage.StunDuration = _dmResponse.StunDuration * 0.5f;
				}
			}
			else if (_dmResponse.Stun == EnumEntityStunType.Kneel)
			{
				bool flag5 = false;
				if (this.bodyDamage.CurrentStun == EnumEntityStunType.None)
				{
					if (_dmResponse.Critical || this.rand.RandomFloat < 0.25f)
					{
						flag5 = true;
					}
					else
					{
						this.SetStun(EnumEntityStunType.Kneel);
						this.emodel.avatarController.BeginStun(EnumEntityStunType.Kneel, _dmResponse.HitBodyPart, _dmResponse.HitDirection, _dmResponse.Critical, _dmResponse.Random);
					}
				}
				else if (this.bodyDamage.CurrentStun == EnumEntityStunType.Kneel)
				{
					flag5 = true;
				}
				if (flag5)
				{
					this.DoRagdoll(in _dmResponse);
					this.SetStun(EnumEntityStunType.Prone);
				}
				this.bodyDamage.StunDuration = _dmResponse.StunDuration;
			}
		}
		else if (_dmResponse.PainHit && !flag3 && this.emodel.avatarController)
		{
			EntityClass entityClass = EntityClass.list[this.entityClass];
			float num4 = entityClass.PainResistPerHit;
			if (num4 >= 0f)
			{
				float num5 = (float)this.GetMaxHealth();
				if ((float)this.Health / num5 < entityClass.PainResistPerHitLowHealthPercent)
				{
					num4 = entityClass.PainResistPerHitLowHealth;
				}
				this.painResistPercent = Utils.FastMin(this.painResistPercent + num4, 3f);
				float num6 = float.MaxValue;
				if (this.painResistPercent >= 3f && num4 >= 1f)
				{
					num6 = 0f;
					this.painHitsFelt += 0.15f;
				}
				else if (this.painResistPercent >= 1f)
				{
					num6 = Utils.FastLerp(0.5f, 0.15f, (this.painResistPercent - 1f) * 0.75f);
					this.painHitsFelt += 0.3f;
				}
				else
				{
					this.painHitsFelt += Utils.FastLerp(1f, 0.3f, this.painResistPercent);
				}
				this.emodel.avatarController.StartAnimationHit(_dmResponse.HitBodyPart, (int)_dmResponse.HitDirection, (int)((float)_dmResponse.Strength * 100f / num5), _dmResponse.Critical, _dmResponse.MovementState, _dmResponse.Random, num6);
			}
		}
		if (this.bodyDamage.CurrentStun == EnumEntityStunType.None)
		{
			if (_dmResponse.Source.CanStun)
			{
				if ((_dmResponse.HitBodyPart & (EnumBodyPartHit.Torso | EnumBodyPartHit.Head | EnumBodyPartHit.LeftUpperArm | EnumBodyPartHit.RightUpperArm | EnumBodyPartHit.LeftLowerArm | EnumBodyPartHit.RightLowerArm)) > EnumBodyPartHit.None)
				{
					this.bodyDamage.StunProne = this.bodyDamage.StunProne + _dmResponse.Strength;
				}
				else if (_dmResponse.HitBodyPart.IsLeg())
				{
					this.bodyDamage.StunKnee = this.bodyDamage.StunKnee + _dmResponse.Strength;
				}
			}
		}
		else
		{
			this.bodyDamage.StunProne = 0;
			this.bodyDamage.StunKnee = 0;
		}
		if (this.Health <= 0 && this.deathUpdateTime > 0)
		{
			this.DeathHealth -= _dmResponse.Strength;
		}
		if (flag)
		{
			num3 = 0;
			_dmResponse.Fatal = false;
		}
		if (this.isEntityRemote)
		{
			this.Health -= num3;
			this.RecordedDamage = _dmResponse;
		}
		else
		{
			if (!this.IsGodMode.Value)
			{
				this.Health -= num3;
				if (_dmResponse.Fatal && this.Health > 0)
				{
					this.Health = 0;
				}
				this.hasBeenAttackedTime = 0;
				if (_dmResponse.PainHit)
				{
					this.hasBeenAttackedTime = this.GetMaxAttackTime();
				}
			}
			this.bPlayHurtSound = (this.bBeenWounded = num3 > 0);
			if (this.bBeenWounded)
			{
				base.setBeenAttacked();
				this.MinEventContext.Other = GameManager.Instance.World.GetEntity(_dmResponse.Source.getEntityId()) as EntityAlive;
				this.FireEvent(MinEventTypes.onOtherDamagedSelf, true);
			}
			if (num3 > this.woundedStrength)
			{
				this.woundedStrength = _dmResponse.Strength;
				this.woundedDamageSource = _dmResponse.Source;
			}
			this.lastHitDirection = _dmResponse.HitDirection;
			if (this.Health <= 0)
			{
				_dmResponse.Source.getDirection();
				_dmResponse.Strength += this.Health;
				Entity entity = ((_dmResponse.Source.getEntityId() != -1) ? this.world.GetEntity(_dmResponse.Source.getEntityId()) : null);
				if (this.Spawned && !flag4)
				{
					if (entity is EntityAlive)
					{
						this.entityThatKilledMe = (EntityAlive)entity;
					}
					else
					{
						this.entityThatKilledMe = null;
					}
				}
				this.Kill(_dmResponse);
				if (!_dmResponse.Fatal && this.world.IsRemote())
				{
					this.DamageEntity(DamageSource.disease, 1, false, 1f);
				}
			}
		}
		Entity entity2 = ((_dmResponse.Source.getEntityId() != -1) ? this.world.GetEntity(_dmResponse.Source.getEntityId()) : null);
		if (entity2 != null && entity2 != this)
		{
			if (entity2 is EntityAlive && !this.isEntityRemote && !entity2.IsIgnoredByAI())
			{
				this.SetRevengeTarget((EntityAlive)entity2);
				if (this.aiManager != null)
				{
					this.aiManager.DamagedByEntity();
				}
			}
			if (entity2 is EntityPlayer)
			{
				((EntityPlayer)entity2).FireEvent(MinEventTypes.onCombatEntered, true);
			}
			this.FireEvent(MinEventTypes.onCombatEntered, true);
		}
		if (_dmResponse.Strength > 0 && _dmResponse.Source.GetDamageType() == EnumDamageTypes.Electrical)
		{
			this.Electrocuted = true;
		}
		this.RecordedDamage = _dmResponse;
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000D4C04 File Offset: 0x000D2E04
	public string GetArmorImpactSound(EquipmentSlots _slot, bool _grazingHit, bool _includeCosmetic = true)
	{
		ItemClass itemClass = null;
		if (_includeCosmetic)
		{
			itemClass = this.equipment.GetCosmeticSlot((int)_slot, false);
		}
		if (itemClass == null)
		{
			ItemValue slotItem = this.equipment.GetSlotItem((int)_slot);
			itemClass = ((slotItem != null) ? slotItem.ItemClass : null);
		}
		if (itemClass == null)
		{
			return string.Empty;
		}
		if (_grazingHit)
		{
			return itemClass.SoundImpactGraze;
		}
		return itemClass.SoundImpactHit;
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000D4C5C File Offset: 0x000D2E5C
	public string GetArmorMaterial(EquipmentSlots _slot, bool _includeCosmetic = true)
	{
		ItemClass itemClass = null;
		if (_includeCosmetic)
		{
			itemClass = this.equipment.GetCosmeticSlot((int)_slot, false);
		}
		if (itemClass == null)
		{
			ItemValue slotItem = this.equipment.GetSlotItem((int)_slot);
			itemClass = ((slotItem != null) ? slotItem.ItemClass : null);
		}
		if (itemClass == null)
		{
			return string.Empty;
		}
		if (itemClass != null)
		{
			return itemClass.MadeOfMaterial.SurfaceCategory;
		}
		return string.Empty;
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000D4CB5 File Offset: 0x000D2EB5
	public EntityAlive GetRevengeTarget()
	{
		return this.revengeEntity;
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000D4CBD File Offset: 0x000D2EBD
	public void SetRevengeTarget(EntityAlive _other)
	{
		this.revengeEntity = _other;
		this.revengeTimer = ((this.revengeEntity == null) ? 0 : 500);
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000D4CE2 File Offset: 0x000D2EE2
	public void SetRevengeTimer(int ticks)
	{
		this.revengeTimer = ticks;
	}

	// Token: 0x060022F5 RID: 8949 RVA: 0x000BBB3C File Offset: 0x000B9D3C
	public override bool CanBePushed()
	{
		return !this.IsDead();
	}

	// Token: 0x060022F6 RID: 8950 RVA: 0x000D4CEB File Offset: 0x000D2EEB
	public override bool CanCollideWith(Entity _other)
	{
		return !this.IsDead() && !(_other is EntityItem) && !(_other is EntitySupplyCrate);
	}

	// Token: 0x060022F7 RID: 8951 RVA: 0x000D4D0B File Offset: 0x000D2F0B
	public override bool CanCollideWithBlocks()
	{
		return !this.IsSleeping;
	}

	// Token: 0x060022F8 RID: 8952 RVA: 0x000D4D18 File Offset: 0x000D2F18
	public void DoRagdoll(in DamageResponse _dmResponse)
	{
		this.emodel.DoRagdoll(in _dmResponse, EModelBase.RagdollMode.Default, _dmResponse.StunDuration);
	}

	// Token: 0x060022F9 RID: 8953 RVA: 0x000D4D2D File Offset: 0x000D2F2D
	public void DoRagdoll(in DamageResponse _dmResponse, EModelBase.RagdollMode _mode)
	{
		this.emodel.DoRagdoll(in _dmResponse, _mode, _dmResponse.StunDuration);
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x000D4D44 File Offset: 0x000D2F44
	public void AddScore(int _diedMySelfTimes, int _zombieKills, int _playerKills, int _otherTeamnumber, int _conditions)
	{
		this.KilledZombies += _zombieKills;
		this.KilledPlayers += _playerKills;
		this.Died += _diedMySelfTimes;
		this.Score += _zombieKills * GameStats.GetInt(EnumGameStats.ScoreZombieKillMultiplier) + _playerKills * GameStats.GetInt(EnumGameStats.ScorePlayerKillMultiplier) + _diedMySelfTimes * GameStats.GetInt(EnumGameStats.ScoreDiedMultiplier);
		if (this.Score < 0)
		{
			this.Score = 0;
		}
		if (this is EntityPlayerLocal)
		{
			if (_diedMySelfTimes > 0)
			{
				IAchievementManager achievementManager = PlatformManager.NativePlatform.AchievementManager;
				if (achievementManager != null)
				{
					achievementManager.SetAchievementStat(EnumAchievementDataStat.Deaths, _diedMySelfTimes);
				}
			}
			if (_zombieKills > 0)
			{
				IAchievementManager achievementManager2 = PlatformManager.NativePlatform.AchievementManager;
				if (achievementManager2 != null)
				{
					achievementManager2.SetAchievementStat(EnumAchievementDataStat.ZombiesKilled, _zombieKills);
				}
			}
			if (_playerKills > 0)
			{
				IAchievementManager achievementManager3 = PlatformManager.NativePlatform.AchievementManager;
				if (achievementManager3 != null)
				{
					achievementManager3.SetAchievementStat(EnumAchievementDataStat.PlayersKilled, _playerKills);
				}
			}
			if ((_conditions & 2) != 0)
			{
				IAchievementManager achievementManager4 = PlatformManager.NativePlatform.AchievementManager;
				if (achievementManager4 == null)
				{
					return;
				}
				achievementManager4.SetAchievementStat(EnumAchievementDataStat.KilledWith44Magnum, 1);
			}
		}
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x000D4E2C File Offset: 0x000D302C
	public virtual void AwardKill(EntityAlive killer)
	{
		if (killer != null && killer != this)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			EntityType entityType = this.entityType;
			if (entityType != EntityType.Player)
			{
				if (entityType == EntityType.Zombie)
				{
					num++;
				}
			}
			else
			{
				num2++;
			}
			EntityPlayer entityPlayer = killer as EntityPlayer;
			if (entityPlayer)
			{
				GameManager.Instance.AwardKill(killer, this);
				if (entityPlayer.inventory.IsHoldingGun() && entityPlayer.inventory.holdingItem.Name.Equals("gunHandgunT2Magnum44"))
				{
					num3 = 2;
				}
				GameManager.Instance.AddScoreServer(killer.entityId, num, num2, this.TeamNumber, num3);
			}
		}
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x000D4ED4 File Offset: 0x000D30D4
	public virtual void OnEntityDeath()
	{
		if (this.deathUpdateTime != 0)
		{
			return;
		}
		this.AddScore(1, 0, 0, -1, 0);
		if (this.soundLiving != null && this.soundLivingID >= 0)
		{
			Manager.Stop(this.entityId, this.soundLiving);
			this.soundLivingID = -1;
		}
		if (this.AttachedToEntity)
		{
			this.Detach();
		}
		if (this.isEntityRemote)
		{
			return;
		}
		this.AwardKill(this.entityThatKilledMe);
		if (this.particleOnDeath != null && this.particleOnDeath.Length > 0)
		{
			float lightBrightness = this.world.GetLightBrightness(base.GetBlockPosition());
			this.world.GetGameManager().SpawnParticleEffectServer(new ParticleEffect(this.particleOnDeath, this.getHeadPosition(), lightBrightness, Color.white, null, null, false, 1f, ""), this.entityId, false, false);
		}
		if (this.isGameMessageOnDeath())
		{
			GameManager.Instance.GameMessage(EnumGameMessages.EntityWasKilled, this, this.entityThatKilledMe);
		}
		if (this.entityThatKilledMe != null)
		{
			Log.Out("Entity {0} {1} killed by {2} {3}", new object[]
			{
				base.GetDebugName(),
				this.entityId,
				this.entityThatKilledMe.GetDebugName(),
				this.entityThatKilledMe.entityId
			});
		}
		else
		{
			Log.Out("Entity {0} {1} killed", new object[]
			{
				base.GetDebugName(),
				this.entityId
			});
		}
		ModEvents.SEntityKilledData sentityKilledData = new ModEvents.SEntityKilledData(this, this.entityThatKilledMe);
		ModEvents.EntityKilled.Invoke(ref sentityKilledData);
		this.dropItemOnDeath();
		this.entityThatKilledMe = null;
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x000D506A File Offset: 0x000D326A
	public void Disintegrate()
	{
		this.timeStayAfterDeath = 0;
		this.isDisintegrated = true;
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x000D507A File Offset: 0x000D327A
	public virtual void PlayGiveUpSound()
	{
		if (this.soundGiveUp != null)
		{
			this.PlayOneShot(this.soundGiveUp, false, false, false, null, 1f);
		}
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x000D5099 File Offset: 0x000D3299
	public virtual Vector3 GetCameraLook(float _t)
	{
		return this.GetLookVector();
	}

	// Token: 0x06002300 RID: 8960 RVA: 0x000D50A4 File Offset: 0x000D32A4
	public Vector3 GetForwardVector()
	{
		float num = Mathf.Cos(this.rotation.y * 0.0175f - 3.1415927f);
		float num2 = Mathf.Sin(this.rotation.y * 0.0175f - 3.1415927f);
		float num3 = -Mathf.Cos(0f);
		float num4 = Mathf.Sin(0f);
		return new Vector3(num2 * num3, num4, num * num3);
	}

	// Token: 0x06002301 RID: 8961 RVA: 0x000D510C File Offset: 0x000D330C
	public Vector2 GetForwardVector2()
	{
		float num = this.rotation.y * 0.017453292f;
		float num2 = Mathf.Cos(num);
		return new Vector2(Mathf.Sin(num), num2);
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x000D513C File Offset: 0x000D333C
	public virtual Vector3 GetLookVector()
	{
		float num = Mathf.Cos(this.rotation.y * 0.0175f - 3.1415927f);
		float num2 = Mathf.Sin(this.rotation.y * 0.0175f - 3.1415927f);
		float num3 = -Mathf.Cos(this.rotation.x * 0.0175f);
		float num4 = Mathf.Sin(this.rotation.x * 0.0175f);
		return new Vector3(num2 * num3, num4, num * num3);
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x000D5099 File Offset: 0x000D3299
	public virtual Vector3 GetLookVector(Vector3 _altLookVector)
	{
		return this.GetLookVector();
	}

	// Token: 0x06002304 RID: 8964 RVA: 0x000D51BC File Offset: 0x000D33BC
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetSoundRandomTicks()
	{
		return this.rand.RandomRange(this.soundRandomTicks / 2, this.soundRandomTicks);
	}

	// Token: 0x06002305 RID: 8965 RVA: 0x000D51D7 File Offset: 0x000D33D7
	[PublicizedFrom(EAccessModifier.Protected)]
	public int GetSoundAlertTicks()
	{
		return this.rand.RandomRange(this.soundAlertTicks / 2, this.soundAlertTicks);
	}

	// Token: 0x06002306 RID: 8966 RVA: 0x000D51F2 File Offset: 0x000D33F2
	[PublicizedFrom(EAccessModifier.Protected)]
	public string GetSoundRandom()
	{
		return this.soundRandom;
	}

	// Token: 0x06002307 RID: 8967 RVA: 0x000D51FA File Offset: 0x000D33FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string GetSoundJump()
	{
		return this.soundJump;
	}

	// Token: 0x06002308 RID: 8968 RVA: 0x000D5202 File Offset: 0x000D3402
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string GetSoundHurt(DamageSource _damageSource, int _damageStrength)
	{
		return this.soundHurt;
	}

	// Token: 0x06002309 RID: 8969 RVA: 0x000D520A File Offset: 0x000D340A
	[PublicizedFrom(EAccessModifier.Protected)]
	public string GetSoundHurtSmall()
	{
		return this.soundHurtSmall;
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000D5202 File Offset: 0x000D3402
	[PublicizedFrom(EAccessModifier.Protected)]
	public string GetSoundHurt()
	{
		return this.soundHurt;
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000D5212 File Offset: 0x000D3412
	[PublicizedFrom(EAccessModifier.Protected)]
	public string GetSoundDrownPain()
	{
		return this.soundDrownPain;
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000D521A File Offset: 0x000D341A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string GetSoundDeath(DamageSource _damageSource)
	{
		return this.soundDeath;
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000D5222 File Offset: 0x000D3422
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string GetSoundAttack()
	{
		return this.soundAttack;
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x000D522A File Offset: 0x000D342A
	public virtual string GetSoundAlert()
	{
		return this.soundAlert;
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x000D5232 File Offset: 0x000D3432
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual string GetSoundStamina()
	{
		return this.soundStamina;
	}

	// Token: 0x06002310 RID: 8976 RVA: 0x000D523A File Offset: 0x000D343A
	public virtual string GetSoundSense()
	{
		return this.soundSense;
	}

	// Token: 0x06002311 RID: 8977 RVA: 0x000D5242 File Offset: 0x000D3442
	public virtual Ray GetLookRay()
	{
		return new Ray(this.position + new Vector3(0f, this.GetEyeHeight(), 0f), this.GetLookVector());
	}

	// Token: 0x06002312 RID: 8978 RVA: 0x000D526F File Offset: 0x000D346F
	public virtual Ray GetMeleeRay()
	{
		return this.GetLookRay();
	}

	// Token: 0x06002313 RID: 8979 RVA: 0x000D5278 File Offset: 0x000D3478
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void dropItemOnDeath()
	{
		if (!this.hasAI)
		{
			for (int i = 0; i < this.inventory.GetItemCount(); i++)
			{
				ItemStack item = this.inventory.GetItem(i);
				ItemClass forId = ItemClass.GetForId(item.itemValue.type);
				if (forId != null && forId.CanDrop(null))
				{
					this.world.GetGameManager().ItemDropServer(item, this.position, new Vector3(0.5f, 0f, 0.5f), -1, Constants.cItemDroppedOnDeathLifetime, false);
					this.inventory.SetItem(i, ItemValue.None, 0, true);
				}
			}
			this.inventory.SetFlashlight(false);
			this.equipment.DropItems();
		}
		if (this.entityThatKilledMe)
		{
			this.lootDropProb = EffectManager.GetValue(PassiveEffects.LootDropProb, this.entityThatKilledMe.inventory.holdingItemItemValue, this.lootDropProb, this.entityThatKilledMe, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		this.lootDropProb *= LootContainer.LootBagChance;
		if (!LootContainer.NoLoot && this.lootDropProb > this.rand.RandomFloat)
		{
			base.DropBagServer();
		}
	}

	// Token: 0x06002314 RID: 8980 RVA: 0x000D53AC File Offset: 0x000D35AC
	public virtual Vector3 GetDropPosition()
	{
		if (this.ParachuteWearing || this.JetpackWearing)
		{
			return base.transform.position + base.transform.forward - Vector3.up * 0.3f + Origin.position;
		}
		return base.transform.position + base.transform.forward + Vector3.up + Origin.position;
	}

	// Token: 0x06002315 RID: 8981 RVA: 0x000D5432 File Offset: 0x000D3632
	public virtual void OnFired()
	{
		if (this.emodel.avatarController != null)
		{
			this.emodel.avatarController.StartAnimationFiring();
		}
	}

	// Token: 0x06002316 RID: 8982 RVA: 0x000D5457 File Offset: 0x000D3657
	public virtual void OnReloadStart()
	{
		if (this.emodel.avatarController != null)
		{
			this.emodel.avatarController.StartAnimationReloading();
		}
	}

	// Token: 0x06002317 RID: 8983 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnReloadEnd()
	{
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool WillForceToFollow(EntityAlive _other)
	{
		return false;
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000D547C File Offset: 0x000D367C
	public void AddHealth(int _v)
	{
		if (this.Health <= 0)
		{
			return;
		}
		this.Health += _v;
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000D5496 File Offset: 0x000D3696
	public void AddStamina(float _v)
	{
		if (this.entityStats.Stamina != null && this.Health > 0)
		{
			this.entityStats.Stamina.Value += _v;
		}
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000D54C6 File Offset: 0x000D36C6
	public void AddWater(float _v)
	{
		this.Stats.Water.Value += _v;
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000D54E0 File Offset: 0x000D36E0
	public int GetTicksNoPlayerAdjacent()
	{
		return this.ticksNoPlayerAdjacent;
	}

	// Token: 0x0600231D RID: 8989 RVA: 0x000D54E8 File Offset: 0x000D36E8
	public bool CanSee(EntityAlive _other)
	{
		return this.seeCache.CanSee(_other);
	}

	// Token: 0x0600231E RID: 8990 RVA: 0x000D54F6 File Offset: 0x000D36F6
	public void SetCanSee(EntityAlive _other)
	{
		this.seeCache.SetCanSee(_other);
	}

	// Token: 0x0600231F RID: 8991 RVA: 0x000D5504 File Offset: 0x000D3704
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateTasks()
	{
		if (GamePrefs.GetBool(EnumGamePrefs.DebugStopEnemiesMoving) && !(this is EntityDrone))
		{
			this.SetMoveForwardWithModifiers(0f, 0f, 0f, false);
			if (this.aiManager != null)
			{
				this.aiManager.UpdateDebugName();
			}
			return;
		}
		this.CheckDespawn();
		this.seeCache.ClearIfExpired();
		bool useAIPackages = EntityClass.list[this.entityClass].UseAIPackages;
		this.aiActiveDelay -= this.aiActiveScale;
		if (this.aiActiveDelay <= 0f)
		{
			this.aiActiveDelay = 1f;
			if (!useAIPackages)
			{
				this.aiManager.Update();
			}
			else
			{
				UAIBase.Update(this.utilityAIContext);
			}
		}
		PathInfo path = PathFinderThread.Instance.GetPath(this.entityId);
		if (((path != null) ? path.path : null) != null)
		{
			bool flag = true;
			if (!useAIPackages)
			{
				flag = this.aiManager.CheckPath(path);
			}
			if (flag)
			{
				this.navigator.SetPath(path, path.speed);
			}
		}
		this.navigator.UpdateNavigation();
		this.moveHelper.UpdateMoveHelper();
		this.lookHelper.onUpdateLook();
		if (this.distraction != null && (this.distraction.IsDead() || this.distraction.IsMarkedForUnload()))
		{
			this.distraction = null;
		}
		if (this.pendingDistraction != null && (this.pendingDistraction.IsDead() || this.pendingDistraction.IsMarkedForUnload()))
		{
			this.pendingDistraction = null;
		}
	}

	// Token: 0x06002320 RID: 8992 RVA: 0x000D567F File Offset: 0x000D387F
	public PathNavigate getNavigator()
	{
		return this.navigator;
	}

	// Token: 0x06002321 RID: 8993 RVA: 0x000D5688 File Offset: 0x000D3888
	public void FindPath(Vector3 targetPos, float moveSpeed, bool canBreak, EAIBase behavior)
	{
		Vector3 vector = targetPos - this.position;
		if (vector.x * vector.x + vector.z * vector.z > 1225f)
		{
			if (vector.y > 45f)
			{
				targetPos.y = this.position.y + 45f;
			}
			else if (vector.y < -45f)
			{
				targetPos.y = this.position.y - 45f;
			}
		}
		PathFinderThread.Instance.FindPath(this, targetPos, moveSpeed, canBreak, behavior);
	}

	// Token: 0x06002322 RID: 8994 RVA: 0x000D5720 File Offset: 0x000D3920
	public bool isWithinHomeDistanceCurrentPosition()
	{
		return this.isWithinHomeDistance(Utils.Fastfloor(this.position.x), Utils.Fastfloor(this.position.y), Utils.Fastfloor(this.position.z));
	}

	// Token: 0x06002323 RID: 8995 RVA: 0x000D5758 File Offset: 0x000D3958
	public bool isWithinHomeDistance(int _x, int _y, int _z)
	{
		return this.maximumHomeDistance < 0 || this.homePosition.getDistanceSquared(_x, _y, _z) < (float)(this.maximumHomeDistance * this.maximumHomeDistance);
	}

	// Token: 0x06002324 RID: 8996 RVA: 0x000D5783 File Offset: 0x000D3983
	public void setHomeArea(Vector3i _pos, int _maxDistance)
	{
		this.homePosition.position = _pos;
		this.maximumHomeDistance = _maxDistance;
	}

	// Token: 0x06002325 RID: 8997 RVA: 0x000D5798 File Offset: 0x000D3998
	public ChunkCoordinates getHomePosition()
	{
		return this.homePosition;
	}

	// Token: 0x06002326 RID: 8998 RVA: 0x000D57A0 File Offset: 0x000D39A0
	public int getMaximumHomeDistance()
	{
		return this.maximumHomeDistance;
	}

	// Token: 0x06002327 RID: 8999 RVA: 0x000D57A8 File Offset: 0x000D39A8
	public void detachHome()
	{
		this.maximumHomeDistance = -1;
	}

	// Token: 0x06002328 RID: 9000 RVA: 0x000D57B1 File Offset: 0x000D39B1
	public bool hasHome()
	{
		return this.maximumHomeDistance >= 0;
	}

	// Token: 0x06002329 RID: 9001 RVA: 0x000D57BF File Offset: 0x000D39BF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool canDespawn()
	{
		return !this.IsClientControlled() && base.GetSpawnerSource() != EnumSpawnerSource.StaticSpawner && !this.IsSleeping;
	}

	// Token: 0x0600232A RID: 9002 RVA: 0x000D57DD File Offset: 0x000D39DD
	public void ResetDespawnTime()
	{
		this.ticksNoPlayerAdjacent = 0;
		this.seeCache.SetLastTimePlayerSeen();
	}

	// Token: 0x0600232B RID: 9003 RVA: 0x000D57F4 File Offset: 0x000D39F4
	public void CheckDespawn()
	{
		if (this.isEntityRemote)
		{
			return;
		}
		if (!this.CanUpdateEntity() && this.bIsChunkObserver && this.world.GetClosestPlayer(this, -1f, false) == null)
		{
			this.MarkToUnload();
			return;
		}
		if (!this.canDespawn())
		{
			return;
		}
		int num = this.despawnDelayCounter + 1;
		this.despawnDelayCounter = num;
		if (num < 20)
		{
			return;
		}
		this.despawnDelayCounter = 0;
		this.ticksNoPlayerAdjacent += 20;
		EnumSpawnerSource spawnerSource = base.GetSpawnerSource();
		EntityPlayer closestPlayer = this.world.GetClosestPlayer(this, -1f, false);
		if (spawnerSource == EnumSpawnerSource.Dynamic)
		{
			if (!closestPlayer)
			{
				if (!this.world.GetClosestPlayer(this, -1f, true))
				{
					this.Despawn();
				}
				return;
			}
		}
		else if (spawnerSource == EnumSpawnerSource.Biome && !this.world.GetClosestPlayer(this, 130f, false))
		{
			if (this.world.GetClosestPlayer(this, 20f, true))
			{
				this.isDespawnWhenPlayerFar = true;
			}
			else if (this.isDespawnWhenPlayerFar)
			{
				this.Despawn();
			}
		}
		if (!closestPlayer)
		{
			return;
		}
		float sqrMagnitude = (closestPlayer.position - this.position).sqrMagnitude;
		if (sqrMagnitude < 6400f)
		{
			this.ticksNoPlayerAdjacent = 0;
		}
		int num2 = int.MaxValue;
		float lastTimePlayerSeen = this.seeCache.GetLastTimePlayerSeen();
		if (lastTimePlayerSeen > 0f)
		{
			num2 = (int)(Time.time - lastTimePlayerSeen);
		}
		switch (spawnerSource)
		{
		case EnumSpawnerSource.Biome:
			if (this.ticksNoPlayerAdjacent > 100 && sqrMagnitude > 16384f)
			{
				this.Despawn();
				return;
			}
			if (this.ticksNoPlayerAdjacent > 1800)
			{
				this.Despawn();
				return;
			}
			break;
		case EnumSpawnerSource.StaticSpawner:
			break;
		case EnumSpawnerSource.Dynamic:
			if (this.attackTarget)
			{
				num2 = 0;
			}
			if (this.IsSleeper && !this.IsSleeping)
			{
				if (sqrMagnitude > 9216f && num2 > 80)
				{
					this.Despawn();
					return;
				}
			}
			else
			{
				if (sqrMagnitude > 2304f && num2 > 60 && !this.HasInvestigatePosition)
				{
					this.Despawn();
					return;
				}
				if (this.ticksNoPlayerAdjacent > 1800)
				{
					this.Despawn();
					return;
				}
			}
			break;
		default:
			return;
		}
	}

	// Token: 0x0600232C RID: 9004 RVA: 0x000D5A04 File Offset: 0x000D3C04
	[PublicizedFrom(EAccessModifier.Private)]
	public void Despawn()
	{
		this.IsDespawned = true;
		this.MarkToUnload();
	}

	// Token: 0x0600232D RID: 9005 RVA: 0x000D5A13 File Offset: 0x000D3C13
	public void ForceDespawn()
	{
		this.Despawn();
	}

	// Token: 0x0600232E RID: 9006 RVA: 0x000D5A1B File Offset: 0x000D3C1B
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public EntityAlive GetAttackTarget()
	{
		return this.attackTarget;
	}

	// Token: 0x0600232F RID: 9007 RVA: 0x000D5A23 File Offset: 0x000D3C23
	public virtual Vector3 GetAttackTargetHitPosition()
	{
		return this.attackTarget.getChestPosition();
	}

	// Token: 0x06002330 RID: 9008 RVA: 0x000D5A30 File Offset: 0x000D3C30
	public EntityAlive GetAttackTargetLocal()
	{
		if (this.isEntityRemote)
		{
			return this.attackTargetClient;
		}
		return this.attackTarget;
	}

	// Token: 0x06002331 RID: 9009 RVA: 0x000D5A48 File Offset: 0x000D3C48
	public void SetAttackTarget(EntityAlive _attackTarget, int _attackTargetTime)
	{
		if (_attackTarget == this.attackTarget)
		{
			this.attackTargetTime = _attackTargetTime;
			return;
		}
		if (this.attackTarget)
		{
			this.attackTargetLast = this.attackTarget;
		}
		this.targetAlertChanged = false;
		if (_attackTarget)
		{
			if (_attackTarget != this.attackTargetLast)
			{
				this.targetAlertChanged = true;
				this.soundDelayTicks = this.rand.RandomRange(5, 20);
			}
			this.investigatePositionTicks = 0;
		}
		if (!this.isEntityRemote)
		{
			this.world.entityDistributer.SendPacketToTrackedPlayersAndTrackedEntity(this.entityId, -1, NetPackageManager.GetPackage<NetPackageSetAttackTarget>().Setup(this.entityId, _attackTarget ? _attackTarget.entityId : (-1)), false);
		}
		this.attackTarget = _attackTarget;
		this.attackTargetTime = _attackTargetTime;
	}

	// Token: 0x06002332 RID: 9010 RVA: 0x000D5B12 File Offset: 0x000D3D12
	public void SetAttackTargetClient(EntityAlive _attackTarget)
	{
		this.attackTargetClient = _attackTarget;
	}

	// Token: 0x1700041A RID: 1050
	// (get) Token: 0x06002333 RID: 9011 RVA: 0x000D5B1B File Offset: 0x000D3D1B
	public bool HasInvestigatePosition
	{
		get
		{
			return this.investigatePositionTicks > 0;
		}
	}

	// Token: 0x1700041B RID: 1051
	// (get) Token: 0x06002334 RID: 9012 RVA: 0x000D5B26 File Offset: 0x000D3D26
	public Vector3 InvestigatePosition
	{
		get
		{
			return this.investigatePos;
		}
	}

	// Token: 0x06002335 RID: 9013 RVA: 0x000D5B2E File Offset: 0x000D3D2E
	public int GetInvestigatePositionTicks()
	{
		return this.investigatePositionTicks;
	}

	// Token: 0x06002336 RID: 9014 RVA: 0x000D5B38 File Offset: 0x000D3D38
	public void ClearInvestigatePosition()
	{
		this.investigatePos = Vector3.zero;
		this.investigatePositionTicks = 0;
		this.ResetDespawnTime();
		int num = this.rand.RandomRange(20, 35) * 20;
		if (this.entityType == EntityType.Zombie)
		{
			num /= 2;
		}
		this.SetAlertTicks(num);
	}

	// Token: 0x06002337 RID: 9015 RVA: 0x000D5B84 File Offset: 0x000D3D84
	public int CalcInvestigateTicks(int _ticks, EntityAlive _investigateEntity)
	{
		float value = EffectManager.GetValue(PassiveEffects.EnemySearchDuration, null, 1f, _investigateEntity, null, EntityClass.list[this.entityClass].Tags, true, true, true, true, true, 1, true, false);
		return (int)((float)_ticks / value);
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x000D5BC6 File Offset: 0x000D3DC6
	public void SetInvestigatePosition(Vector3 pos, int ticks, bool isAlert = true)
	{
		this.investigatePos = pos;
		this.investigatePositionTicks = ticks;
		this.isInvestigateAlert = isAlert;
	}

	// Token: 0x06002339 RID: 9017 RVA: 0x000D5BDD File Offset: 0x000D3DDD
	public int GetAlertTicks()
	{
		return this.alertTicks;
	}

	// Token: 0x0600233A RID: 9018 RVA: 0x000D5BE5 File Offset: 0x000D3DE5
	public void SetAlertTicks(int ticks)
	{
		this.alertTicks = ticks;
	}

	// Token: 0x1700041C RID: 1052
	// (get) Token: 0x0600233B RID: 9019 RVA: 0x000D5BEE File Offset: 0x000D3DEE
	public virtual bool IsAlert
	{
		get
		{
			if (this.isEntityRemote)
			{
				return this.bReplicatedAlertFlag;
			}
			return this.isAlert;
		}
	}

	// Token: 0x0600233C RID: 9020 RVA: 0x000D5C05 File Offset: 0x000D3E05
	public EntitySeeCache GetEntitySenses()
	{
		return this.seeCache;
	}

	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x0600233D RID: 9021 RVA: 0x000D5C0D File Offset: 0x000D3E0D
	public virtual bool IsRunning
	{
		get
		{
			return this.IsBloodMoon || this.world.IsDark();
		}
	}

	// Token: 0x0600233E RID: 9022 RVA: 0x000D5C24 File Offset: 0x000D3E24
	public virtual float GetMoveSpeed()
	{
		if (this.IsBloodMoon || this.world.IsDark())
		{
			return EffectManager.GetValue(PassiveEffects.WalkSpeed, null, this.moveSpeedNight, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		return EffectManager.GetValue(PassiveEffects.CrouchSpeed, null, this.moveSpeed, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x0600233F RID: 9023 RVA: 0x000D5C90 File Offset: 0x000D3E90
	public virtual float GetMoveSpeedAggro()
	{
		if (this.IsBloodMoon || this.world.IsDark())
		{
			return EffectManager.GetValue(PassiveEffects.RunSpeed, null, this.moveSpeedAggroMax, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		}
		return EffectManager.GetValue(PassiveEffects.WalkSpeed, null, this.moveSpeedAggro, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002340 RID: 9024 RVA: 0x000D5CFC File Offset: 0x000D3EFC
	public float GetMoveSpeedPanic()
	{
		return EffectManager.GetValue(PassiveEffects.RunSpeed, null, this.moveSpeedPanic, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002341 RID: 9025 RVA: 0x000D5D2D File Offset: 0x000D3F2D
	public override float GetWeight()
	{
		return this.weight;
	}

	// Token: 0x06002342 RID: 9026 RVA: 0x000D5D35 File Offset: 0x000D3F35
	public override float GetPushFactor()
	{
		return this.pushFactor;
	}

	// Token: 0x06002343 RID: 9027 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool CanEntityJump()
	{
		return true;
	}

	// Token: 0x06002344 RID: 9028 RVA: 0x000D5D3D File Offset: 0x000D3F3D
	public void SetMaxViewAngle(float _angle)
	{
		this.maxViewAngle = _angle;
	}

	// Token: 0x06002345 RID: 9029 RVA: 0x000D5D46 File Offset: 0x000D3F46
	public virtual float GetMaxViewAngle()
	{
		return this.maxViewAngle;
	}

	// Token: 0x06002346 RID: 9030 RVA: 0x000D5D4E File Offset: 0x000D3F4E
	public void SetSightLightThreshold(Vector2 _threshold)
	{
		this.sightLightThreshold = _threshold;
	}

	// Token: 0x06002347 RID: 9031 RVA: 0x000D5D57 File Offset: 0x000D3F57
	public int GetModelLayer()
	{
		return this.emodel.GetModelTransform().gameObject.layer;
	}

	// Token: 0x06002348 RID: 9032 RVA: 0x000D5D6E File Offset: 0x000D3F6E
	public virtual void SetModelLayer(int _layerId, bool force = false, string[] excludeTags = null)
	{
		Utils.SetLayerRecursively(this.emodel.GetModelTransform().gameObject, _layerId);
	}

	// Token: 0x06002349 RID: 9033 RVA: 0x000D5D86 File Offset: 0x000D3F86
	public virtual void SetColliderLayer(int _layerId, bool _force = false)
	{
		Utils.SetColliderLayerRecursively(this.emodel.GetModelTransform().gameObject, _layerId);
	}

	// Token: 0x0600234A RID: 9034 RVA: 0x00080B3A File Offset: 0x0007ED3A
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual int GetMaxAttackTime()
	{
		return 10;
	}

	// Token: 0x0600234B RID: 9035 RVA: 0x000D5D9E File Offset: 0x000D3F9E
	public int GetAttackTimeoutTicks()
	{
		if (!this.world.IsDark())
		{
			return this.attackTimeoutDay;
		}
		return this.attackTimeoutNight;
	}

	// Token: 0x0600234C RID: 9036 RVA: 0x000D5DBA File Offset: 0x000D3FBA
	public override void MarkToUnload()
	{
		base.MarkToUnload();
		this.deathUpdateTime = this.timeStayAfterDeath;
	}

	// Token: 0x0600234D RID: 9037 RVA: 0x000D5DCE File Offset: 0x000D3FCE
	public override bool IsMarkedForUnload()
	{
		return base.IsMarkedForUnload() && this.deathUpdateTime >= this.timeStayAfterDeath;
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x000D5DEC File Offset: 0x000D3FEC
	public virtual bool IsAttackValid()
	{
		if (!(this is EntityPlayer))
		{
			if (this.Electrocuted)
			{
				return false;
			}
			if (this.bodyDamage.CurrentStun == EnumEntityStunType.Kneel || this.bodyDamage.CurrentStun == EnumEntityStunType.Prone)
			{
				return false;
			}
		}
		return (!(this.emodel != null) || !(this.emodel.avatarController != null) || !this.emodel.avatarController.IsAttackPrevented()) && !this.IsDead() && (this.painResistPercent >= 1f || (this.hasBeenAttackedTime <= 0 && (this.emodel.avatarController == null || !this.emodel.avatarController.IsAnimationHitRunning())));
	}

	// Token: 0x0600234F RID: 9039 RVA: 0x000D5EAA File Offset: 0x000D40AA
	public virtual bool IsAttackImpact()
	{
		return this.emodel && this.emodel.avatarController && this.emodel.avatarController.IsAttackImpact();
	}

	// Token: 0x06002350 RID: 9040 RVA: 0x000D5EDD File Offset: 0x000D40DD
	public virtual void ShowHoldingItem(bool _show)
	{
		this.inventory.ShowRightHand(_show);
	}

	// Token: 0x06002351 RID: 9041 RVA: 0x000D5EEB File Offset: 0x000D40EB
	public virtual bool IsHoldingItemInUse(int _actionIndex)
	{
		ItemAction itemAction = this.inventory.holdingItem.Actions[_actionIndex];
		return itemAction != null && itemAction.IsActionRunning(this.inventory.holdingItemData.actionData[_actionIndex]);
	}

	// Token: 0x06002352 RID: 9042 RVA: 0x000D5F20 File Offset: 0x000D4120
	public virtual bool UseHoldingItem(int _actionIndex, bool _isReleased)
	{
		if (!_isReleased)
		{
			if (_actionIndex == 0 && this.emodel && this.emodel.avatarController && this.emodel.avatarController.IsAnimationAttackPlaying())
			{
				return false;
			}
			if (!this.IsAttackValid())
			{
				return false;
			}
		}
		if (_actionIndex == 0 && _isReleased && this.GetSoundAttack() != null)
		{
			this.PlayOneShot(this.GetSoundAttack(), false, false, false, null, 1f);
		}
		this.attackingTime = 60;
		ItemAction itemAction = this.inventory.holdingItem.Actions[_actionIndex];
		if (itemAction != null)
		{
			itemAction.ExecuteAction(this.inventory.holdingItemData.actionData[_actionIndex], _isReleased);
		}
		return true;
	}

	// Token: 0x06002353 RID: 9043 RVA: 0x000D5FD0 File Offset: 0x000D41D0
	public bool Attack(bool _isReleased)
	{
		return this.UseHoldingItem(0, _isReleased);
	}

	// Token: 0x06002354 RID: 9044 RVA: 0x000D5FDC File Offset: 0x000D41DC
	public Entity GetTargetIfAttackedNow()
	{
		if (!this.IsAttackValid())
		{
			return null;
		}
		ItemClass holdingItem = this.inventory.holdingItem;
		if (holdingItem != null)
		{
			ItemAction itemAction = holdingItem.Actions[0];
			if (itemAction != null)
			{
				WorldRayHitInfo executeActionTarget = itemAction.GetExecuteActionTarget(this.inventory.holdingItemData.actionData[0]);
				if (executeActionTarget != null && executeActionTarget.bHitValid && executeActionTarget.transform)
				{
					float num = itemAction.Range;
					if (num == 0f)
					{
						ItemValue holdingItemItemValue = this.inventory.holdingItemItemValue;
						num = EffectManager.GetItemValue(PassiveEffects.MaxRange, holdingItemItemValue, 0f);
					}
					num += 0.3f;
					if (executeActionTarget.hit.distanceSq <= num * num)
					{
						Transform transform = executeActionTarget.transform;
						if (executeActionTarget.tag.StartsWith("E_BP_"))
						{
							transform = GameUtils.GetHitRootTransform(Voxel.voxelRayHitInfo.tag, executeActionTarget.transform);
						}
						if (transform != null)
						{
							Entity component = transform.GetComponent<Entity>();
							if (component)
							{
								return component;
							}
						}
						if (executeActionTarget.tag == "E_Vehicle")
						{
							return EntityVehicle.FindCollisionEntity(transform);
						}
					}
				}
			}
		}
		return null;
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x000D6100 File Offset: 0x000D4300
	public virtual float GetBlockDamageScale(bool isTerrain)
	{
		if (this.IsBloodMoon)
		{
			return ItemActionAttack.BMBlockDamagePercent;
		}
		return ItemActionAttack.EntityBlockDamagePercent;
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x000D6115 File Offset: 0x000D4315
	public virtual void PlayStepSound(float _volume)
	{
		this.internalPlayStepSound(_volume);
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x000D6120 File Offset: 0x000D4320
	[PublicizedFrom(EAccessModifier.Protected)]
	public void internalPlayStepSound(float _volume)
	{
		if (this.blockValueStandingOn.isair)
		{
			return;
		}
		if ((!this.onGround && !base.IsInElevator()) || this.isHeadUnderwater)
		{
			if (!(this is EntityPlayerLocal) && (this.isHeadUnderwater || this.world.IsWater(this.blockPosStandingOn)))
			{
				Manager.Play(this, "player_swim", 1f, false);
			}
			return;
		}
		BlockValue blockValue = this.blockValueStandingOn;
		Vector3i vector3i = this.blockPosStandingOn;
		vector3i.y++;
		BlockValue blockValue2 = this.world.GetBlock(vector3i);
		if (blockValue2.Block.blockMaterial.stepSound != null)
		{
			blockValue = blockValue2;
		}
		else if (blockValue.Block.blockMaterial.stepSound == null)
		{
			BlockValue blockValue3;
			blockValue2 = (blockValue3 = this.world.GetBlock(vector3i + Vector3i.right));
			if (!blockValue3.isair && blockValue2.Block.blockMaterial.stepSound != null)
			{
				blockValue = blockValue2;
			}
			else
			{
				blockValue2 = (blockValue3 = this.world.GetBlock(vector3i - Vector3i.right));
				if (!blockValue3.isair && blockValue2.Block.blockMaterial.stepSound != null)
				{
					blockValue = blockValue2;
				}
				else
				{
					blockValue2 = (blockValue3 = this.world.GetBlock(vector3i + Vector3i.forward));
					if (!blockValue3.isair && blockValue2.Block.blockMaterial.stepSound != null)
					{
						blockValue = blockValue2;
					}
					else
					{
						blockValue2 = (blockValue3 = this.world.GetBlock(vector3i - Vector3i.forward));
						if (!blockValue3.isair && blockValue2.Block.blockMaterial.stepSound != null)
						{
							blockValue = blockValue2;
						}
					}
				}
			}
		}
		if (blockValue.isair)
		{
			return;
		}
		Block block = blockValue.Block;
		if (EffectManager.GetValue(PassiveEffects.SilenceBlockSteps, null, 0f, this, null, block.Tags, true, true, true, true, true, 1, true, false) > 0f)
		{
			return;
		}
		MaterialBlock materialForSide = block.GetMaterialForSide(blockValue, BlockFace.Top);
		if (materialForSide != null && materialForSide.stepSound != null)
		{
			string name = materialForSide.stepSound.name;
			if (name.Length > 0)
			{
				string text = this.soundStepType + name;
				this.PlayStepSound(text, _volume);
			}
		}
	}

	// Token: 0x06002358 RID: 9048 RVA: 0x000D634C File Offset: 0x000D454C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateStepSound(float _distX, float _distZ, float _rotYDelta)
	{
		if (this.blockValueStandingOn.isair)
		{
			return;
		}
		float num = Mathf.Sqrt(_distX * _distX + _distZ * _distZ);
		if (!this.onGround || this.isHeadUnderwater)
		{
			this.distanceSwam += num;
			if (this.distanceSwam > this.nextSwimDistance)
			{
				this.nextSwimDistance += 1f;
				if (this.nextSwimDistance < this.distanceSwam || this.nextSwimDistance > this.distanceSwam + 1f)
				{
					this.nextSwimDistance = this.distanceSwam + 1f;
				}
				this.internalPlayStepSound(1f);
			}
			return;
		}
		this.distanceWalked += num;
		if (num == 0f)
		{
			this.stepSoundDistanceRemaining = 0.25f;
		}
		else
		{
			this.stepSoundDistanceRemaining -= num;
			if (this.stepSoundDistanceRemaining <= 0f)
			{
				this.stepSoundDistanceRemaining = this.getNextStepSoundDistance();
				this.internalPlayStepSound(1f);
			}
		}
		this.stepSoundRotYRemaining -= Utils.FastAbs(_rotYDelta);
		if (this.stepSoundRotYRemaining <= 0f)
		{
			this.stepSoundRotYRemaining = 90f;
			this.internalPlayStepSound(1f);
		}
	}

	// Token: 0x06002359 RID: 9049 RVA: 0x000D6480 File Offset: 0x000D4680
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updatePlayerLandSound(float _distXZ, float _diffY)
	{
		if (this.blockValueStandingOn.isair)
		{
			return;
		}
		if (_distXZ >= 0.025f || Utils.FastAbs(_diffY) >= 0.015f)
		{
			float num = this.inWaterPercent * 2f;
			float num2 = num - this.landWaterLevel;
			this.landWaterLevel = num;
			float num3 = Utils.FastAbs(num2);
			if (num > 0f)
			{
				num3 = Utils.FastMax(num3, _distXZ);
			}
			if (num3 >= 0.02f)
			{
				float num4 = Utils.FastMin(num3 * 2.2f + 0.01f, 1f);
				Manager.Play(this, "player_swim", num4, false);
			}
		}
	}

	// Token: 0x0600235A RID: 9050 RVA: 0x000D6510 File Offset: 0x000D4710
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updateCurrentBlockPosAndValue()
	{
		Vector3i vector3i = base.GetBlockPosition();
		BlockValue blockValue = this.world.GetBlock(vector3i);
		if (blockValue.isair)
		{
			vector3i.y--;
			blockValue = this.world.GetBlock(vector3i);
		}
		if (blockValue.ischild)
		{
			vector3i += blockValue.parent;
			blockValue = this.world.GetBlock(vector3i);
		}
		if (this.blockPosStandingOn != vector3i || !this.blockValueStandingOn.Equals(blockValue) || (this.onGround && !this.wasOnGround))
		{
			this.blockPosStandingOn = vector3i;
			this.blockValueStandingOn = blockValue;
			this.blockStandingOnChanged = !this.world.IsRemote();
			BiomeDefinition biome = this.world.GetBiome(this.blockPosStandingOn.x, this.blockPosStandingOn.z);
			if (biome != null && this.biomeStandingOn != biome && (this.biomeStandingOn == null || this.biomeStandingOn.m_Id != biome.m_Id))
			{
				this.onNewBiomeEntered(biome);
			}
		}
		this.CalcIfInElevator();
		Block block = this.blockValueStandingOn.Block;
		if (block.BuffsWhenWalkedOn != null && block.UseBuffsWhenWalkedOn(this.world, this.blockPosStandingOn, this.blockValueStandingOn))
		{
			bool flag = true;
			TileEntityWorkstation tileEntityWorkstation = this.world.GetTileEntity(this.blockPosStandingOn) as TileEntityWorkstation;
			if (tileEntityWorkstation != null)
			{
				flag = tileEntityWorkstation.IsBurning;
			}
			if (flag)
			{
				for (int i = 0; i < block.BuffsWhenWalkedOn.Length; i++)
				{
					BuffValue buff = this.Buffs.GetBuff(block.BuffsWhenWalkedOn[i]);
					if (buff == null || buff.DurationInSeconds >= 1f)
					{
						this.Buffs.AddBuff(block.BuffsWhenWalkedOn[i], vector3i, -1, true, false, -1f);
					}
				}
			}
		}
		if (this.onGround && !this.IsFlyMode.Value)
		{
			if (block.MovementFactor != 1f && block.HasCollidingAABB(this.blockValueStandingOn, this.blockPosStandingOn.x, this.blockPosStandingOn.y, this.blockPosStandingOn.z, 0f, this.boundingBox))
			{
				this.SetMotionMultiplier(EffectManager.GetValue(PassiveEffects.MovementFactorMultiplier, null, block.MovementFactor, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false));
			}
			if (this.blockStandingOnChanged)
			{
				this.blockStandingOnChanged = false;
				if (!this.blockValueStandingOn.isair)
				{
					block.OnEntityWalking(this.world, this.blockPosStandingOn.x, this.blockPosStandingOn.y, this.blockPosStandingOn.z, this.blockValueStandingOn, this);
					if (GameManager.bPhysicsActive && !this.blockValueStandingOn.ischild && !this.blockValueStandingOn.Block.isOversized && this.world.GetStability(this.blockPosStandingOn) == 0 && Block.CanFallBelow(this.world, this.blockPosStandingOn.x, this.blockPosStandingOn.y, this.blockPosStandingOn.z))
					{
						Log.Warning("EntityAlive {0} AddFallingBlock stab 0 happens?", new object[] { this.EntityName });
						this.world.AddFallingBlock(this.blockPosStandingOn, false);
					}
				}
				BlockValue block2 = this.world.GetBlock(this.blockPosStandingOn + Vector3i.up);
				if (!block2.isair)
				{
					block2.Block.OnEntityWalking(this.world, this.blockPosStandingOn.x, this.blockPosStandingOn.y + 1, this.blockPosStandingOn.z, block2, this);
				}
			}
		}
		this.HandleLootStageMaxCheck();
	}

	// Token: 0x0600235B RID: 9051 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void HandleLootStageMaxCheck()
	{
	}

	// Token: 0x0600235C RID: 9052 RVA: 0x000D68B4 File Offset: 0x000D4AB4
	[PublicizedFrom(EAccessModifier.Private)]
	public void CalcIfInElevator()
	{
		if (!this.bCanClimbLadders)
		{
			this.bInElevator = false;
			return;
		}
		ChunkCluster chunkCache = this.world.ChunkCache;
		Vector3i vector3i = new Vector3i(this.blockPosStandingOn.x, Utils.Fastfloor(this.boundingBox.min.y), this.blockPosStandingOn.z);
		BlockValue blockValue = chunkCache.GetBlock(vector3i);
		Block block = blockValue.Block;
		this.bInElevator = block.IsElevator((int)blockValue.rotation);
		vector3i.y++;
		blockValue = chunkCache.GetBlock(vector3i);
		block = blockValue.Block;
		this.bInElevator |= block.IsElevator((int)blockValue.rotation);
	}

	// Token: 0x0600235D RID: 9053 RVA: 0x000D6966 File Offset: 0x000D4B66
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual float getNextStepSoundDistance()
	{
		return 1.5f;
	}

	// Token: 0x0600235E RID: 9054 RVA: 0x000D696D File Offset: 0x000D4B6D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onNewBiomeEntered(BiomeDefinition _biome)
	{
		this.biomeStandingOn = _biome;
	}

	// Token: 0x0600235F RID: 9055 RVA: 0x000D6978 File Offset: 0x000D4B78
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void updateSpeedForwardAndStrafe(Vector3 _dist, float _partialTicks)
	{
		if (this.isEntityRemote && _partialTicks > 1f)
		{
			_dist /= _partialTicks;
		}
		this.speedForward *= 0.5f;
		this.speedStrafe *= 0.5f;
		this.speedVertical *= 0.5f;
		if (Mathf.Abs(_dist.x) > 0.001f || Mathf.Abs(_dist.z) > 0.001f)
		{
			float num = Mathf.Sin(-this.rotation.y * 3.1415927f / 180f);
			float num2 = Mathf.Cos(-this.rotation.y * 3.1415927f / 180f);
			this.speedForward += num2 * _dist.z - num * _dist.x;
			this.speedStrafe += num2 * _dist.x + num * _dist.z;
		}
		if (Mathf.Abs(_dist.y) > 0.001f)
		{
			this.speedVertical += _dist.y;
		}
		this.SetMovementState();
	}

	// Token: 0x06002360 RID: 9056 RVA: 0x000D6A9B File Offset: 0x000D4C9B
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void PlayStepSound(string stepSound, float _volume)
	{
		if (this is EntityPlayerLocal)
		{
			Manager.BroadcastPlay(this, stepSound, false, 1f);
			return;
		}
		Manager.Play(this, stepSound, 1f, false);
	}

	// Token: 0x06002361 RID: 9057 RVA: 0x000D6AC4 File Offset: 0x000D4CC4
	public void SetLookPosition(Vector3 _lookPos)
	{
		if ((this.lookAtPosition - _lookPos).sqrMagnitude < 0.0016f)
		{
			return;
		}
		this.lookAtPosition = _lookPos;
		if (this.world.entityDistributer != null)
		{
			this.world.entityDistributer.SendPacketToTrackedPlayers(this.entityId, this.world.GetPrimaryPlayerId(), NetPackageManager.GetPackage<NetPackageEntityLookAt>().Setup(this.entityId, _lookPos), false);
		}
		if (this.emodel.avatarController)
		{
			this.emodel.avatarController.SetLookPosition(_lookPos);
		}
	}

	// Token: 0x06002362 RID: 9058 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool isRadiationSensitive()
	{
		return true;
	}

	// Token: 0x06002363 RID: 9059 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsAimingGunPossible()
	{
		return true;
	}

	// Token: 0x06002364 RID: 9060 RVA: 0x000D6B57 File Offset: 0x000D4D57
	public int GetDeathTime()
	{
		return this.deathUpdateTime;
	}

	// Token: 0x06002365 RID: 9061 RVA: 0x000D6B5F File Offset: 0x000D4D5F
	public void SetDeathTime(int _deathTime)
	{
		this.deathUpdateTime = _deathTime;
	}

	// Token: 0x06002366 RID: 9062 RVA: 0x000D6B68 File Offset: 0x000D4D68
	public int GetTimeStayAfterDeath()
	{
		return this.timeStayAfterDeath;
	}

	// Token: 0x06002367 RID: 9063 RVA: 0x000D6B70 File Offset: 0x000D4D70
	public bool IsCorpse()
	{
		return this.emodel && this.emodel.IsRagdollDead && (float)this.deathUpdateTime > 70f;
	}

	// Token: 0x06002368 RID: 9064 RVA: 0x000D6BA0 File Offset: 0x000D4DA0
	public override void OnAddedToWorld()
	{
		if (!(this is EntityPlayerLocal))
		{
			OcclusionManager.AddEntity(this, 7f);
		}
		this.m_addedToWorld = true;
		if (!this.isEntityRemote)
		{
			this.bSpawned = true;
		}
		if (this as EntityPlayer == null)
		{
			this.FireEvent(MinEventTypes.onSelfFirstSpawn, true);
		}
		this.StartStopLivingSound();
	}

	// Token: 0x06002369 RID: 9065 RVA: 0x000D6BF4 File Offset: 0x000D4DF4
	public override void OnEntityUnload()
	{
		if (!(this is EntityPlayerLocal))
		{
			OcclusionManager.RemoveEntity(this);
		}
		if (this.navigator != null)
		{
			this.navigator.SetPath(null, 0f);
			this.navigator = null;
		}
		base.OnEntityUnload();
		this.lookHelper = null;
		this.moveHelper = null;
		this.seeCache = null;
	}

	// Token: 0x0600236A RID: 9066 RVA: 0x000D6C4B File Offset: 0x000D4E4B
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetDamageFraction(float _damage)
	{
		return _damage / (float)this.GetMaxHealth();
	}

	// Token: 0x0600236B RID: 9067 RVA: 0x000D6C58 File Offset: 0x000D4E58
	[PublicizedFrom(EAccessModifier.Private)]
	public float GetDismemberChance(ref DamageResponse _dmResponse, float damagePer)
	{
		EnumBodyPartHit hitBodyPart = _dmResponse.HitBodyPart;
		EntityClass entityClass = EntityClass.list[this.entityClass];
		float num = 0f;
		switch (hitBodyPart.ToPrimary())
		{
		case BodyPrimaryHit.Head:
			num = entityClass.DismemberMultiplierHead;
			break;
		case BodyPrimaryHit.LeftUpperArm:
		case BodyPrimaryHit.RightUpperArm:
		case BodyPrimaryHit.LeftLowerArm:
		case BodyPrimaryHit.RightLowerArm:
			num = entityClass.DismemberMultiplierArms;
			break;
		case BodyPrimaryHit.LeftUpperLeg:
		case BodyPrimaryHit.RightUpperLeg:
		case BodyPrimaryHit.LeftLowerLeg:
		case BodyPrimaryHit.RightLowerLeg:
			num = entityClass.DismemberMultiplierLegs;
			break;
		}
		num = EffectManager.GetValue(PassiveEffects.DismemberSelfChance, null, num, this, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
		float dismemberChance = _dmResponse.Source.DismemberChance;
		float num2 = ((dismemberChance < 100f) ? (dismemberChance * damagePer * num) : 100f);
		EntityPlayerLocal entityPlayerLocal = this.world.GetEntity(_dmResponse.Source.getEntityId()) as EntityPlayerLocal;
		if (entityPlayerLocal && entityPlayerLocal.DebugDismembermentChance)
		{
			num2 = 1f;
		}
		if (DismembermentManager.DebugLogEnabled && num2 > 0f)
		{
			Log.Out("[EntityAlive.GetDismemberChance] - {0}, primary {1}, damage {2}, chance {3} * damage% {4} * multiplier {5} = {6}", new object[]
			{
				hitBodyPart,
				hitBodyPart.ToPrimary(),
				_dmResponse.Strength,
				dismemberChance.ToCultureInvariantString(),
				damagePer.ToCultureInvariantString(),
				num.ToCultureInvariantString(),
				num2.ToCultureInvariantString()
			});
		}
		return num2;
	}

	// Token: 0x0600236C RID: 9068 RVA: 0x000D6DBC File Offset: 0x000D4FBC
	public virtual void CheckDismember(ref DamageResponse _dmResponse, float damagePer)
	{
		bool flag = _dmResponse.HitBodyPart.IsLeg();
		if (flag && base.IsAlive() && (this.bodyDamage.CurrentStun != EnumEntityStunType.None || this.sleepingOrWakingUp))
		{
			return;
		}
		float dismemberChance = this.GetDismemberChance(ref _dmResponse, damagePer);
		if (dismemberChance > 0f && this.rand.RandomFloat <= dismemberChance)
		{
			_dmResponse.Dismember = true;
			if (flag)
			{
				_dmResponse.TurnIntoCrawler = true;
			}
			return;
		}
		if (flag)
		{
			EntityClass entityClass = EntityClass.list[this.entityClass];
			if (entityClass.LegCrawlerThreshold > 0f && this.GetDamageFraction((float)_dmResponse.Strength) >= entityClass.LegCrawlerThreshold)
			{
				_dmResponse.TurnIntoCrawler = true;
			}
			if (!this.bodyDamage.ShouldBeCrawler && !_dmResponse.TurnIntoCrawler && entityClass.LegCrippleScale > 0f)
			{
				float num = this.GetDamageFraction((float)_dmResponse.Strength) * entityClass.LegCrippleScale;
				if (num >= 0.05f)
				{
					if ((this.bodyDamage.Flags & 4096U) == 0U && _dmResponse.HitBodyPart.IsLeftLeg() && this.rand.RandomFloat < num)
					{
						_dmResponse.CrippleLegs = true;
					}
					if ((this.bodyDamage.Flags & 8192U) == 0U && _dmResponse.HitBodyPart.IsRightLeg() && this.rand.RandomFloat < num)
					{
						_dmResponse.CrippleLegs = true;
					}
				}
			}
		}
	}

	// Token: 0x0600236D RID: 9069 RVA: 0x000D6F1C File Offset: 0x000D511C
	[PublicizedFrom(EAccessModifier.Private)]
	public void ApplyLocalBodyDamage(DamageResponse _dmResponse)
	{
		EnumBodyPartHit enumBodyPartHit = _dmResponse.HitBodyPart;
		this.bodyDamage.bodyPartHit = enumBodyPartHit;
		this.bodyDamage.damageType = _dmResponse.Source.damageType;
		if (_dmResponse.Dismember)
		{
			if (DismembermentManager.DebugBodyPartHit != EnumBodyPartHit.None)
			{
				enumBodyPartHit = DismembermentManager.DebugBodyPartHit;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.Head) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 1U;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.LeftUpperArm) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 2U;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.LeftLowerArm) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 4U;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.RightUpperArm) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 8U;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.RightLowerArm) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 16U;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.LeftUpperLeg) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 32U;
				this.bodyDamage.ShouldBeCrawler = true;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.LeftLowerLeg) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 64U;
				this.bodyDamage.ShouldBeCrawler = true;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.RightUpperLeg) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 128U;
				this.bodyDamage.ShouldBeCrawler = true;
			}
			if ((enumBodyPartHit & EnumBodyPartHit.RightLowerLeg) > EnumBodyPartHit.None)
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 256U;
				this.bodyDamage.ShouldBeCrawler = true;
			}
		}
		if (_dmResponse.TurnIntoCrawler)
		{
			this.bodyDamage.ShouldBeCrawler = true;
		}
		if (_dmResponse.CrippleLegs)
		{
			if (_dmResponse.HitBodyPart.IsLeftLeg())
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 4096U;
			}
			if (_dmResponse.HitBodyPart.IsRightLeg())
			{
				this.bodyDamage.Flags = this.bodyDamage.Flags | 8192U;
			}
		}
	}

	// Token: 0x0600236E RID: 9070 RVA: 0x000D70D8 File Offset: 0x000D52D8
	[PublicizedFrom(EAccessModifier.Protected)]
	public void ExecuteDismember(bool restoreState)
	{
		if (this.emodel == null || this.emodel.avatarController == null)
		{
			return;
		}
		if (this.bodyDamage.IsCrippled && this.bodyDamage.bodyPartHit.IsLeg() && base.IsAlive() && this.walkType != 5 && this.walkType < 20)
		{
			this.SetWalkType(5);
		}
		this.emodel.avatarController.DismemberLimb(this.bodyDamage, restoreState);
		if (this.bodyDamage.ShouldBeCrawler)
		{
			this.SetupCrawler();
		}
	}

	// Token: 0x0600236F RID: 9071 RVA: 0x000D7174 File Offset: 0x000D5374
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetupCrawler()
	{
		if (base.IsAlive())
		{
			this.SetWalkType(21);
			base.SetMaxHeight(0.5f);
			ItemValue itemValue = null;
			if (EntityClass.list[this.entityClass].Properties.Values.ContainsKey(EntityClass.PropHandItemCrawler))
			{
				itemValue = ItemClass.GetItem(EntityClass.list[this.entityClass].Properties.Values[EntityClass.PropHandItemCrawler], false);
				if (itemValue.IsEmpty())
				{
					itemValue = null;
				}
			}
			if (itemValue == null)
			{
				itemValue = ItemClass.GetItem("meleeHandZombie02", false);
			}
			this.inventory.SetBareHandItem(itemValue);
			this.TurnIntoCrawler();
		}
	}

	// Token: 0x06002370 RID: 9072 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void TurnIntoCrawler()
	{
	}

	// Token: 0x06002371 RID: 9073 RVA: 0x000D721D File Offset: 0x000D541D
	public void ClearStun()
	{
		this.bodyDamage.CurrentStun = EnumEntityStunType.None;
		this.bodyDamage.StunDuration = 0f;
		this.SetCVar("_stunned", 0f);
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x000D724B File Offset: 0x000D544B
	public void SetStun(EnumEntityStunType stun)
	{
		this.bodyDamage.CurrentStun = stun;
		this.SetCVar("_stunned", 1f);
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x000D7269 File Offset: 0x000D5469
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void onSpawnStateChanged()
	{
		if (!this.m_addedToWorld)
		{
			return;
		}
		this.StartStopLivingSound();
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x000D727C File Offset: 0x000D547C
	[PublicizedFrom(EAccessModifier.Private)]
	public void StartStopLivingSound()
	{
		if (this.soundLiving != null)
		{
			if (this.Spawned)
			{
				if (!this.IsDead() && this.Health > 0)
				{
					Manager.Play(this, this.soundLiving, 1f, false);
					this.soundLivingID = 0;
				}
			}
			else if (this.soundLivingID >= 0)
			{
				Manager.Stop(this.entityId, this.soundLiving);
				this.soundLivingID = -1;
			}
		}
		if (this.Spawned && this.soundSpawn != null && !this.SleeperSupressLivingSounds)
		{
			this.PlayOneShot(this.soundSpawn, false, false, false, null, 1f);
		}
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x000D7314 File Offset: 0x000D5514
	public void CrouchHeightFixedUpdate()
	{
		if (this.crouchType == 0)
		{
			return;
		}
		if (this.physicsBaseHeight <= 1.3f)
		{
			return;
		}
		float num = this.physicsBaseHeight;
		if (base.IsInElevator())
		{
			num *= 1.06f;
		}
		if (this.emodel.IsRagdollMovement || this.bodyDamage.CurrentStun == EnumEntityStunType.Prone)
		{
			num = this.physicsBaseHeight * 0.08f;
		}
		float num2 = this.m_characterController.GetRadius() * 0.9f;
		float num3 = num2 + 0.3f;
		float num4 = num + 0.01f - num3 - num2;
		Vector3 vector = this.PhysicsTransform.position;
		vector.y += num3;
		if (this.moveHelper != null && (this.moveHelper.BlockedFlags & 3) == 2)
		{
			vector += this.ModelTransform.forward * 0.15f;
		}
		RaycastHit raycastHit;
		if (Physics.SphereCast(vector, num2, Vector3.up, out raycastHit, num4, 1083277320))
		{
			Transform transform = raycastHit.transform;
			if (transform && transform.CompareTag("Physics"))
			{
				Entity component = transform.GetComponent<Entity>();
				if (component)
				{
					component.PhysicsPush(transform.forward * (0.1f * Time.fixedDeltaTime), raycastHit.point, true);
				}
				return;
			}
			if (this.world.GetBlock(new Vector3i(raycastHit.point + Origin.position)).Block.Damage <= 0f)
			{
				num = raycastHit.point.y - (vector.y - num3) - 0.21f;
			}
		}
		if (num < this.physicsHeight)
		{
			if (base.IsInElevator())
			{
				return;
			}
			num = Utils.FastMoveTowards(this.physicsHeight, num, 0.099999994f);
		}
		else
		{
			num = Mathf.MoveTowards(this.physicsHeight, num, 0.016666666f);
		}
		base.SetHeight(num);
		float num5 = this.physicsBaseHeight * 0.7f;
		if (num <= num5)
		{
			this.crouchBendPerTarget = 0f;
			if (this.walkType != 8 && this.walkType != 21)
			{
				int num6 = this.walkType;
				this.SetWalkType(8);
				this.walkTypeBeforeCrouch = num6;
			}
		}
		else
		{
			this.crouchBendPerTarget = 1f - (num - num5) / (this.physicsBaseHeight - num5);
			if (this.walkTypeBeforeCrouch != 0)
			{
				int num7 = this.walkTypeBeforeCrouch;
				this.walkTypeBeforeCrouch = 0;
				this.SetWalkType(num7);
			}
		}
		this.crouchBendPer = Mathf.MoveTowards(this.crouchBendPer, this.crouchBendPerTarget, 0.099999994f);
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x000D7590 File Offset: 0x000D5790
	public void SetWalkType(int _walkType)
	{
		if (this.walkType == 21)
		{
			return;
		}
		if (_walkType == 21)
		{
			this.walkType = _walkType;
			this.emodel.avatarController.TurnIntoCrawler();
			this.walkTypeBeforeCrouch = 0;
			return;
		}
		if (this.walkTypeBeforeCrouch != 0)
		{
			this.walkTypeBeforeCrouch = _walkType;
			return;
		}
		this.walkType = _walkType;
		this.emodel.avatarController.SetWalkType(_walkType, true);
	}

	// Token: 0x06002377 RID: 9079 RVA: 0x000D75F5 File Offset: 0x000D57F5
	public int GetWalkType()
	{
		return this.walkType;
	}

	// Token: 0x06002378 RID: 9080 RVA: 0x000D75FD File Offset: 0x000D57FD
	public bool IsWalkTypeACrawl()
	{
		return this.walkType >= 20;
	}

	// Token: 0x06002379 RID: 9081 RVA: 0x000D760C File Offset: 0x000D580C
	public string GetRightHandTransformName()
	{
		return this.rightHandTransformName;
	}

	// Token: 0x0600237A RID: 9082 RVA: 0x0001FF5D File Offset: 0x0001E15D
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool isGameMessageOnDeath()
	{
		return true;
	}

	// Token: 0x0600237B RID: 9083 RVA: 0x000D7614 File Offset: 0x000D5814
	public override float GetLightBrightness()
	{
		Vector3i blockPosition = base.GetBlockPosition();
		Vector3i vector3i = blockPosition;
		vector3i.y += Mathf.RoundToInt(base.height + 0.5f);
		return Utils.FastMax(this.world.GetLightBrightness(blockPosition), this.world.GetLightBrightness(vector3i));
	}

	// Token: 0x0600237C RID: 9084 RVA: 0x000D7664 File Offset: 0x000D5864
	public virtual float GetLightLevel()
	{
		EntityAlive entityAlive = this.AttachedToEntity as EntityAlive;
		if (entityAlive)
		{
			return entityAlive.GetLightLevel();
		}
		return this.inventory.GetLightLevel();
	}

	// Token: 0x0600237D RID: 9085 RVA: 0x000D7698 File Offset: 0x000D5898
	public override int AttachToEntity(Entity _other, int slot = -1)
	{
		slot = base.AttachToEntity(_other, slot);
		if (slot >= 0)
		{
			this.CurrentMovementTag = EntityAlive.MovementTagIdle;
			this.Crouching = false;
			if (!this.isEntityRemote)
			{
				this.saveInventory = null;
				if (_other is EntityAlive && _other.GetAttachedToInfo(slot).bReplaceLocalInventory)
				{
					this.saveInventory = this.inventory;
					this.saveHoldingItemIdxBeforeAttach = this.inventory.holdingItemIdx;
					this.inventory.SetHoldingItemIdxNoHolsterTime(this.inventory.DUMMY_SLOT_IDX);
					this.inventory = ((EntityAlive)_other).inventory;
				}
				this.bPlayerStatsChanged |= true;
			}
			else
			{
				this.ShowHoldingItem(false);
			}
		}
		return slot;
	}

	// Token: 0x0600237E RID: 9086 RVA: 0x000D774C File Offset: 0x000D594C
	public override void Detach()
	{
		if (this.saveInventory != null)
		{
			this.inventory = this.saveInventory;
			this.inventory.SetHoldingItemIdxNoHolsterTime(this.saveHoldingItemIdxBeforeAttach);
			this.saveInventory = null;
		}
		base.Detach();
		this.bPlayerStatsChanged |= !this.isEntityRemote;
	}

	// Token: 0x0600237F RID: 9087 RVA: 0x000D77A1 File Offset: 0x000D59A1
	public override void Write(BinaryWriter _bw, bool _bNetworkWrite)
	{
		base.Write(_bw, _bNetworkWrite);
		_bw.Write(this.deathHealth);
	}

	// Token: 0x06002380 RID: 9088 RVA: 0x000D77B7 File Offset: 0x000D59B7
	public override void Read(byte _version, BinaryReader _br)
	{
		base.Read(_version, _br);
		if (_version > 24)
		{
			this.deathHealth = _br.ReadInt32();
		}
	}

	// Token: 0x06002381 RID: 9089 RVA: 0x000D77D2 File Offset: 0x000D59D2
	public override string ToString()
	{
		return string.Format("[type={0}, name={1}, id={2}]", base.GetType().Name, GameUtils.SafeStringFormat(this.EntityName), this.entityId);
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x000D7800 File Offset: 0x000D5A00
	public virtual void FireEvent(MinEventTypes _eventType, bool useInventory = true)
	{
		MinEffectController effects = EntityClass.list[this.entityClass].Effects;
		if (effects != null)
		{
			effects.FireEvent(_eventType, this.MinEventContext);
		}
		if (this.Progression != null)
		{
			this.Progression.FireEvent(_eventType, this.MinEventContext);
		}
		if (this.challengeJournal != null)
		{
			this.challengeJournal.FireEvent(_eventType, this.MinEventContext);
		}
		if (this.inventory != null && useInventory)
		{
			this.inventory.FireEvent(_eventType, this.MinEventContext);
		}
		this.equipment.FireEvent(_eventType, this.MinEventContext);
		this.Buffs.FireEvent(_eventType, this.MinEventContext);
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x000D78AA File Offset: 0x000D5AAA
	public float GetCVar(string _varName)
	{
		if (this.Buffs == null)
		{
			return 0f;
		}
		return this.Buffs.GetCustomVar(_varName);
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000D78C6 File Offset: 0x000D5AC6
	public void SetCVar(string _varName, float _value)
	{
		if (this.Buffs == null)
		{
			return;
		}
		this.Buffs.SetCustomVar(_varName, _value, true, CVarOperation.set);
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void BuffAdded(BuffValue _buff)
	{
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000D78E0 File Offset: 0x000D5AE0
	public override void OnCollisionForward(Transform t, Collision collision, bool isStay)
	{
		if (!this.emodel.IsRagdollActive)
		{
			return;
		}
		if (collision.relativeVelocity.sqrMagnitude < 0.0625f)
		{
			return;
		}
		float sqrMagnitude = collision.impulse.sqrMagnitude;
		if (sqrMagnitude < 400f)
		{
			return;
		}
		if (this.IsDead())
		{
			EntityAlive.ImpactData impactData;
			this.impacts.TryGetValue(t, out impactData);
			impactData.count++;
			this.impacts[t] = impactData;
			if (impactData.count >= 10)
			{
				if (impactData.count == 10)
				{
					Rigidbody component = t.GetComponent<Rigidbody>();
					if (component)
					{
						component.velocity = Vector3.zero;
						component.angularVelocity = Vector3.zero;
						component.drag = 0.5f;
						component.angularDrag = 0.5f;
					}
					CharacterJoint component2 = t.GetComponent<CharacterJoint>();
					if (component2)
					{
						component2.enableProjection = false;
					}
				}
				if (impactData.count == 25 && !t.gameObject.CompareTag("E_BP_Body"))
				{
					t.GetComponent<Collider>().enabled = false;
				}
				return;
			}
		}
		if (Time.time - this.impactSoundTime < 0.25f)
		{
			return;
		}
		this.impactSoundTime = Time.time;
		if (t.lossyScale.x == 0f)
		{
			return;
		}
		string text = "impactbodylight";
		if (sqrMagnitude >= 3600f)
		{
			text = "impactbodyheavy";
		}
		Vector3 vector = Vector3.zero;
		int contactCount = collision.contactCount;
		for (int i = 0; i < contactCount; i++)
		{
			vector += collision.GetContact(i).point;
		}
		vector *= 1f / (float)contactCount;
		Manager.BroadcastPlay(vector + Origin.position, text, 0f);
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000D7A9C File Offset: 0x000D5C9C
	public void AddParticle(string _name, Transform _t)
	{
		if (this.particles.ContainsKey(_name))
		{
			this.particles[_name] = _t;
			return;
		}
		this.particles.Add(_name, _t);
	}

	// Token: 0x06002388 RID: 9096 RVA: 0x000D7AC8 File Offset: 0x000D5CC8
	public bool RemoveParticle(string _name)
	{
		Transform transform;
		if (this.particles.Remove(_name, out transform))
		{
			if (transform)
			{
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06002389 RID: 9097 RVA: 0x000D7AFC File Offset: 0x000D5CFC
	public bool HasParticle(string _name)
	{
		Transform transform;
		return this.particles.TryGetValue(_name, out transform);
	}

	// Token: 0x0600238A RID: 9098 RVA: 0x000D7B1C File Offset: 0x000D5D1C
	public void AddPart(string _name, Transform _t)
	{
		if (this.parts.ContainsKey(_name))
		{
			this.parts[_name] = _t;
			return;
		}
		this.parts.Add(_name, _t);
	}

	// Token: 0x0600238B RID: 9099 RVA: 0x000D7B48 File Offset: 0x000D5D48
	public void RemovePart(string _name)
	{
		Transform transform;
		if (this.parts.TryGetValue(_name, out transform))
		{
			this.parts.Remove(_name);
			if (transform)
			{
				transform.gameObject.name = ".";
				global::UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x0600238C RID: 9100 RVA: 0x000D7B98 File Offset: 0x000D5D98
	public void SetPartActive(string _name, bool isActive)
	{
		Transform transform;
		if (this.parts.TryGetValue(_name, out transform) && transform)
		{
			bool flag = true;
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				Transform child = transform.GetChild(i);
				if (child.CompareTag("ModOn"))
				{
					child.gameObject.SetActive(isActive);
					flag = false;
				}
				else if (child.CompareTag("ModMesh"))
				{
					if (transform.parent.name == "CameraNode")
					{
						child.gameObject.SetActive(false);
					}
					flag = false;
				}
			}
			if (flag)
			{
				transform.gameObject.SetActive(isActive);
			}
		}
	}

	// Token: 0x0600238D RID: 9101 RVA: 0x000D7C3C File Offset: 0x000D5E3C
	public OwnedEntityData GetOwnedEntity(int _entityId)
	{
		return this.ownedEntities.Find((OwnedEntityData e) => e.Id == _entityId);
	}

	// Token: 0x0600238E RID: 9102 RVA: 0x000D7C6D File Offset: 0x000D5E6D
	public bool HasOwnedEntity(int _entityId)
	{
		return this.GetOwnedEntity(_entityId) != null;
	}

	// Token: 0x0600238F RID: 9103 RVA: 0x000D7C7C File Offset: 0x000D5E7C
	public void AddOwnedEntity(OwnedEntityData _data)
	{
		if (!this.HasOwnedEntity(_data.Id) && _data != null)
		{
			this.ownedEntities.Add(_data);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageOwnedEntitySync>().Setup(this.entityId, _data.Id, _data.ClassId, NetPackageOwnedEntitySync.SyncType.Add), false, -1, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06002390 RID: 9104 RVA: 0x000D7CEC File Offset: 0x000D5EEC
	public void AddOwnedEntity(Entity _entity)
	{
		this.AddOwnedEntity(new OwnedEntityData(_entity));
	}

	// Token: 0x06002391 RID: 9105 RVA: 0x000D7CFC File Offset: 0x000D5EFC
	public void RemoveOwnedEntity(OwnedEntityData _data)
	{
		if (_data != null)
		{
			this.ownedEntities.Remove(_data);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageOwnedEntitySync>().Setup(this.entityId, _data.Id, _data.ClassId, NetPackageOwnedEntitySync.SyncType.Remove), false, -1, -1, -1, null, 192, false);
			}
		}
	}

	// Token: 0x06002392 RID: 9106 RVA: 0x000D7D5F File Offset: 0x000D5F5F
	public void RemoveOwnedEntity(int _entityId)
	{
		this.RemoveOwnedEntity(this.GetOwnedEntity(_entityId));
	}

	// Token: 0x06002393 RID: 9107 RVA: 0x000D7D6E File Offset: 0x000D5F6E
	public void RemoveOwnedEntity(Entity _entity)
	{
		this.RemoveOwnedEntity(_entity.entityId);
	}

	// Token: 0x06002394 RID: 9108 RVA: 0x000D7D7C File Offset: 0x000D5F7C
	public List<OwnedEntityData> GetOwnedEntities(int _classId)
	{
		List<OwnedEntityData> list = new List<OwnedEntityData>();
		for (int i = 0; i < this.ownedEntities.Count; i++)
		{
			OwnedEntityData ownedEntityData = this.ownedEntities[i];
			if (ownedEntityData.ClassId == _classId)
			{
				list.Add(ownedEntityData);
			}
		}
		return list;
	}

	// Token: 0x06002395 RID: 9109 RVA: 0x000D7DC3 File Offset: 0x000D5FC3
	public void HandleSetNavName()
	{
		if (this.NavObject != null)
		{
			this.NavObject.name = this.entityName;
		}
	}

	// Token: 0x06002396 RID: 9110 RVA: 0x000D7DE0 File Offset: 0x000D5FE0
	[PublicizedFrom(EAccessModifier.Private)]
	public void UpdateDynamicRagdoll()
	{
		if (this._dynamicRagdoll.HasFlag(DynamicRagdollFlags.Active))
		{
			if (this.accumulatedRootMotion != Vector3.zero)
			{
				this._dynamicRagdollRootMotion = this.accumulatedRootMotion;
			}
			if (this._dynamicRagdoll.HasFlag(DynamicRagdollFlags.UseBoneVelocities))
			{
				this._ragdollPositionsPrev.Clear();
				this._ragdollPositionsCur.CopyTo<Vector3>(this._ragdollPositionsPrev);
				this.emodel.CaptureRagdollPositions(this._ragdollPositionsCur);
			}
			if (this._dynamicRagdoll.HasFlag(DynamicRagdollFlags.RagdollOnFall) && !this.onGround)
			{
				this.ActivateDynamicRagdoll();
				return;
			}
		}
	}

	// Token: 0x06002397 RID: 9111 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void AnalyticsSendDeath(DamageResponse _dmResponse)
	{
	}

	// Token: 0x06002398 RID: 9112 RVA: 0x0004E124 File Offset: 0x0004C324
	public virtual string MakeDebugNameInfo()
	{
		return string.Empty;
	}

	// Token: 0x06002399 RID: 9113 RVA: 0x000D7E94 File Offset: 0x000D6094
	public static void SetupAllDebugNameHUDs(bool _isAdd)
	{
		List<Entity> list = GameManager.Instance.World.Entities.list;
		for (int i = 0; i < list.Count; i++)
		{
			EntityAlive entityAlive = list[i] as EntityAlive;
			if (entityAlive)
			{
				entityAlive.SetupDebugNameHUD(_isAdd);
			}
		}
	}

	// Token: 0x0600239A RID: 9114 RVA: 0x000D7EE4 File Offset: 0x000D60E4
	public void SetupDebugNameHUD(bool _isAdd)
	{
		if (this is EntityPlayer)
		{
			return;
		}
		GUIHUDEntityName component = this.ModelTransform.GetComponent<GUIHUDEntityName>();
		if (_isAdd)
		{
			if (!component)
			{
				this.ModelTransform.gameObject.AddComponent<GUIHUDEntityName>();
				return;
			}
		}
		else if (component)
		{
			global::UnityEngine.Object.Destroy(component);
		}
	}

	// Token: 0x0600239B RID: 9115 RVA: 0x000D7F31 File Offset: 0x000D6131
	public EModelBase.HeadStates GetHeadState()
	{
		if (base.EntityClass.CanBigHead)
		{
			return this.emodel.HeadState;
		}
		return EModelBase.HeadStates.Standard;
	}

	// Token: 0x0600239C RID: 9116 RVA: 0x000D7F50 File Offset: 0x000D6150
	public void SetBigHead()
	{
		if ((this is EntityAnimal || this is EntityEnemy || this is EntityTrader) && base.EntityClass.CanBigHead && this.emodel.HeadState == EModelBase.HeadStates.Standard)
		{
			this.emodel.HeadState = EModelBase.HeadStates.Growing;
			Manager.BroadcastPlayByLocalPlayer(this.position, "twitch_bighead_inflate");
		}
	}

	// Token: 0x0600239D RID: 9117 RVA: 0x000D7FAB File Offset: 0x000D61AB
	public void ForceBigHead()
	{
		if ((this is EntityAnimal || this is EntityEnemy || this is EntityTrader) && base.EntityClass.CanBigHead && this.emodel.HeadState == EModelBase.HeadStates.Standard)
		{
			this.emodel.HeadState = EModelBase.HeadStates.BigHead;
		}
	}

	// Token: 0x0600239E RID: 9118 RVA: 0x000D7FEC File Offset: 0x000D61EC
	public void ResetHead()
	{
		if ((this is EntityAnimal || this is EntityEnemy || this is EntityTrader) && base.EntityClass.CanBigHead && (this.emodel.HeadState == EModelBase.HeadStates.BigHead || this.emodel.HeadState == EModelBase.HeadStates.Growing))
		{
			base.StartCoroutine(this.resetHeadLater(this.emodel));
		}
	}

	// Token: 0x0600239F RID: 9119 RVA: 0x000D8050 File Offset: 0x000D6250
	public void ForceResetHead()
	{
		if ((this is EntityAnimal || this is EntityEnemy || this is EntityTrader) && base.EntityClass.CanBigHead && (this.emodel.HeadState == EModelBase.HeadStates.BigHead || this.emodel.HeadState == EModelBase.HeadStates.Growing))
		{
			this.emodel.HeadState = EModelBase.HeadStates.Standard;
		}
	}

	// Token: 0x060023A0 RID: 9120 RVA: 0x000D80AA File Offset: 0x000D62AA
	public void SetDancing(bool enabled)
	{
		if (base.EntityClass.DanceTypeID != 0)
		{
			this.IsDancing = enabled;
			return;
		}
		this.IsDancing = false;
	}

	// Token: 0x060023A1 RID: 9121 RVA: 0x000D80C8 File Offset: 0x000D62C8
	[PublicizedFrom(EAccessModifier.Protected)]
	public IEnumerator resetHeadLater(EModelBase model)
	{
		yield return new WaitForSeconds(0.25f);
		if (this.emodel != null && this.emodel.GetHeadTransform() != null && this.emodel.GetHeadTransform().localScale.x > 1f)
		{
			this.emodel.HeadState = EModelBase.HeadStates.Shrinking;
			Manager.BroadcastPlayByLocalPlayer(this.position, "twitch_bighead_deflate");
		}
		yield break;
	}

	// Token: 0x060023A2 RID: 9122 RVA: 0x000D80D7 File Offset: 0x000D62D7
	public void SetSpawnByData(int newSpawnByID, string newSpawnByName)
	{
		this.spawnById = newSpawnByID;
		this.spawnByName = newSpawnByName;
		this.bPlayerStatsChanged |= !this.isEntityRemote;
	}

	// Token: 0x060023A3 RID: 9123 RVA: 0x000D80FD File Offset: 0x000D62FD
	[PublicizedFrom(EAccessModifier.Internal)]
	public void SetHeadSize(float overrideHeadSize)
	{
		this.OverrideHeadSize = overrideHeadSize;
		this.emodel.SetHeadScale(overrideHeadSize);
	}

	// Token: 0x060023A4 RID: 9124 RVA: 0x000D8112 File Offset: 0x000D6312
	public void SetVehiclePoseMode(int _pose)
	{
		this.vehiclePoseMode = _pose;
		if (_pose != this.GetVehicleAnimation())
		{
			this.Crouching = false;
			this.SetVehicleAnimation(AvatarController.vehiclePoseHash, _pose);
		}
	}

	// Token: 0x060023A5 RID: 9125 RVA: 0x000D8138 File Offset: 0x000D6338
	[PublicizedFrom(EAccessModifier.Protected)]
	public void updateNetworkStats()
	{
		if (this.networkStatsUpdateQueue.Count > 0)
		{
			EntityAlive.NetworkStatChange networkStatChange = this.networkStatsUpdateQueue[0];
			this.networkStatsUpdateQueue.RemoveAt(0);
			if (networkStatChange.m_NetworkStats != null)
			{
				networkStatChange.m_NetworkStats.ToEntity(this);
				return;
			}
			EntityAlive.EntityNetworkHoldingData holdingData = networkStatChange.m_HoldingData;
			if (holdingData != null)
			{
				ItemStack holdingItemStack = holdingData.m_HoldingItemStack;
				byte holdingItemIndex = holdingData.m_HoldingItemIndex;
				if (!this.inventory.GetItem((int)holdingItemIndex).Equals(holdingItemStack))
				{
					this.inventory.SetItem((int)holdingItemIndex, holdingItemStack);
				}
				if (this.inventory.holdingItemIdx != (int)holdingItemIndex)
				{
					this.inventory.SetHoldingItemIdxNoHolsterTime((int)holdingItemIndex);
				}
			}
		}
	}

	// Token: 0x060023A6 RID: 9126 RVA: 0x000D81D8 File Offset: 0x000D63D8
	public void EnqueueNetworkStats(EntityAlive.EntityNetworkStats netStats)
	{
		EntityAlive.NetworkStatChange networkStatChange = new EntityAlive.NetworkStatChange();
		networkStatChange.m_NetworkStats = netStats;
		this.networkStatsUpdateQueue.Add(networkStatChange);
	}

	// Token: 0x060023A7 RID: 9127 RVA: 0x000D8200 File Offset: 0x000D6400
	public void EnqueueNetworkHoldingData(ItemStack holdingItemStack, byte holdingItemIndex)
	{
		EntityAlive.NetworkStatChange networkStatChange = new EntityAlive.NetworkStatChange();
		networkStatChange.m_HoldingData = new EntityAlive.EntityNetworkHoldingData
		{
			m_HoldingItemStack = holdingItemStack,
			m_HoldingItemIndex = holdingItemIndex
		};
		this.networkStatsUpdateQueue.Add(networkStatChange);
	}

	// Token: 0x060023A8 RID: 9128 RVA: 0x000D823C File Offset: 0x000D643C
	public virtual bool GetAimTarget(out Vector3 aimTarget)
	{
		EntityAlive attackTargetLocal = this.GetAttackTargetLocal();
		if (attackTargetLocal)
		{
			aimTarget = attackTargetLocal.getChestPosition();
			return true;
		}
		aimTarget = Vector3.zero;
		return false;
	}

	// Token: 0x060023A9 RID: 9129 RVA: 0x000D8274 File Offset: 0x000D6474
	public virtual bool GetHeadLookTarget(out Vector3 lookTarget)
	{
		EntityAlive attackTargetLocal = this.GetAttackTargetLocal();
		if (attackTargetLocal && this.CanSee(attackTargetLocal))
		{
			lookTarget = attackTargetLocal.getHeadPosition();
			return true;
		}
		lookTarget = Vector3.zero;
		return false;
	}

	// Token: 0x060023AA RID: 9130 RVA: 0x000D82B4 File Offset: 0x000D64B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public EntityAlive()
	{
	}

	// Token: 0x040017EA RID: 6122
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cTraderTeleportCheckTime = 0.1f;

	// Token: 0x040017EB RID: 6123
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cDamageImmunityOnRespawnSeconds = 1f;

	// Token: 0x040017EC RID: 6124
	public static readonly FastTags<TagGroup.Global> DistractionResistanceWithTargetTags = FastTags<TagGroup.Global>.GetTag("with_target");

	// Token: 0x040017ED RID: 6125
	public static readonly FastTags<TagGroup.Global> CoverWorthTags = FastTags<TagGroup.Global>.GetTag("coverWorthy");

	// Token: 0x040017EE RID: 6126
	public static readonly int FeralTagBit = FastTags<TagGroup.Global>.GetBit("feral");

	// Token: 0x040017EF RID: 6127
	public static readonly int FallingBuffTagBit = FastTags<TagGroup.Global>.GetBit("buffPlayerFallingDamage");

	// Token: 0x040017F0 RID: 6128
	public static readonly FastTags<TagGroup.Global> StanceTagCrouching = FastTags<TagGroup.Global>.GetTag("crouching");

	// Token: 0x040017F1 RID: 6129
	public static readonly FastTags<TagGroup.Global> StanceTagStanding = FastTags<TagGroup.Global>.GetTag("standing");

	// Token: 0x040017F2 RID: 6130
	public static readonly FastTags<TagGroup.Global> MovementTagIdle = FastTags<TagGroup.Global>.GetTag("idle");

	// Token: 0x040017F3 RID: 6131
	public static readonly FastTags<TagGroup.Global> MovementTagWalking = FastTags<TagGroup.Global>.GetTag("walking");

	// Token: 0x040017F4 RID: 6132
	public static readonly FastTags<TagGroup.Global> MovementTagRunning = FastTags<TagGroup.Global>.GetTag("running");

	// Token: 0x040017F5 RID: 6133
	public static readonly FastTags<TagGroup.Global> MovementTagFloating = FastTags<TagGroup.Global>.GetTag("floating");

	// Token: 0x040017F6 RID: 6134
	public static readonly FastTags<TagGroup.Global> MovementTagSwimming = FastTags<TagGroup.Global>.GetTag("swimming");

	// Token: 0x040017F7 RID: 6135
	public static readonly FastTags<TagGroup.Global> MovementTagSwimmingRun = FastTags<TagGroup.Global>.GetTag("swimmingRun");

	// Token: 0x040017F8 RID: 6136
	public static readonly FastTags<TagGroup.Global> MovementTagJumping = FastTags<TagGroup.Global>.GetTag("jumping");

	// Token: 0x040017F9 RID: 6137
	public static readonly FastTags<TagGroup.Global> MovementTagFalling = FastTags<TagGroup.Global>.GetTag("falling");

	// Token: 0x040017FA RID: 6138
	public static readonly FastTags<TagGroup.Global> MovementTagClimbing = FastTags<TagGroup.Global>.GetTag("climbing");

	// Token: 0x040017FB RID: 6139
	public static readonly FastTags<TagGroup.Global> MovementTagDriving = FastTags<TagGroup.Global>.GetTag("driving");

	// Token: 0x040017FC RID: 6140
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly float[] moveSpeedRandomness = new float[] { 0.2f, 1f, 1.1f, 1.2f, 1.35f, 1.5f };

	// Token: 0x040017FD RID: 6141
	public const float CLIMB_LADDER_SPEED = 1234f;

	// Token: 0x040017FE RID: 6142
	public static ulong HitDelay = 11000UL;

	// Token: 0x040017FF RID: 6143
	public static float HitSoundDistance = 10f;

	// Token: 0x04001800 RID: 6144
	public MinEventParams MinEventContext = new MinEventParams();

	// Token: 0x04001801 RID: 6145
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int equippingCount;

	// Token: 0x04001802 RID: 6146
	public bool IsSleeper;

	// Token: 0x04001803 RID: 6147
	public bool IsSleeping;

	// Token: 0x04001804 RID: 6148
	public bool IsSleeperPassive;

	// Token: 0x04001805 RID: 6149
	public bool SleeperSupressLivingSounds;

	// Token: 0x04001806 RID: 6150
	public Vector3 SleeperSpawnPosition;

	// Token: 0x04001807 RID: 6151
	public Vector3 SleeperSpawnLookDir;

	// Token: 0x04001808 RID: 6152
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float accumulatedDamageResisted;

	// Token: 0x04001809 RID: 6153
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int pendingSleepTrigger = -1;

	// Token: 0x0400180A RID: 6154
	public int lastSleeperPose;

	// Token: 0x0400180B RID: 6155
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 sleeperLookDir;

	// Token: 0x0400180C RID: 6156
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float sleeperSightRange;

	// Token: 0x0400180D RID: 6157
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float sleeperViewAngle;

	// Token: 0x0400180E RID: 6158
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 sightLightThreshold;

	// Token: 0x0400180F RID: 6159
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 sightWakeThresholdAtRange;

	// Token: 0x04001810 RID: 6160
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector2 sightGroanThresholdAtRange;

	// Token: 0x04001811 RID: 6161
	public float sleeperNoiseToSense;

	// Token: 0x04001812 RID: 6162
	public float sleeperNoiseToWake;

	// Token: 0x04001813 RID: 6163
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isSnore;

	// Token: 0x04001814 RID: 6164
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isGroan;

	// Token: 0x04001815 RID: 6165
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isGroanSilent;

	// Token: 0x04001816 RID: 6166
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float sleeperNoiseToSenseSoundChance;

	// Token: 0x04001817 RID: 6167
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int snoreGroanCD;

	// Token: 0x04001818 RID: 6168
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const int kSnoreGroanMinCD = 20;

	// Token: 0x04001819 RID: 6169
	public EntityPlayer noisePlayer;

	// Token: 0x0400181A RID: 6170
	public float noisePlayerDistance;

	// Token: 0x0400181B RID: 6171
	public float noisePlayerVolume;

	// Token: 0x0400181C RID: 6172
	public EntityPlayer attractPlayer;

	// Token: 0x0400181D RID: 6173
	public float attractPlayerDistance;

	// Token: 0x0400181E RID: 6174
	public int attractPlayerTimeoutTicks;

	// Token: 0x0400181F RID: 6175
	public EntityPlayer smellPlayer;

	// Token: 0x04001820 RID: 6176
	public float smellPlayerDistance;

	// Token: 0x04001821 RID: 6177
	public int smellPlayerTimeoutTicks;

	// Token: 0x04001822 RID: 6178
	public EntityItem pendingDistraction;

	// Token: 0x04001823 RID: 6179
	public float pendingDistractionDistanceSq;

	// Token: 0x04001824 RID: 6180
	public EntityItem distraction;

	// Token: 0x04001825 RID: 6181
	public float distractionResistance;

	// Token: 0x04001826 RID: 6182
	public float distractionResistanceWithTarget;

	// Token: 0x04001827 RID: 6183
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cSwimGravityPer = 0.025f;

	// Token: 0x04001828 RID: 6184
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cSwimDragY = 0.91f;

	// Token: 0x04001829 RID: 6185
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cSwimDrag = 0.91f;

	// Token: 0x0400182A RID: 6186
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cSwimAnimDelay = 6f;

	// Token: 0x0400182B RID: 6187
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int jumpTicks;

	// Token: 0x0400182C RID: 6188
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public EntityAlive.JumpState jumpState;

	// Token: 0x0400182D RID: 6189
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int jumpStateTicks;

	// Token: 0x0400182E RID: 6190
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float jumpDistance;

	// Token: 0x0400182F RID: 6191
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float jumpHeightDiff;

	// Token: 0x04001830 RID: 6192
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float jumpSwimDurationTicks;

	// Token: 0x04001831 RID: 6193
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 jumpSwimMotion;

	// Token: 0x04001832 RID: 6194
	public float jumpDelay;

	// Token: 0x04001833 RID: 6195
	public float jumpMaxDistance;

	// Token: 0x04001834 RID: 6196
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool jumpIsMoving;

	// Token: 0x04001835 RID: 6197
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float landWaterLevel;

	// Token: 0x04001836 RID: 6198
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksNoPlayerAdjacent;

	// Token: 0x04001837 RID: 6199
	public int hasBeenAttackedTime;

	// Token: 0x04001838 RID: 6200
	public float painHitsFelt;

	// Token: 0x04001839 RID: 6201
	public float painResistPercent;

	// Token: 0x0400183A RID: 6202
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int attackingTime;

	// Token: 0x0400183B RID: 6203
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive revengeEntity;

	// Token: 0x0400183C RID: 6204
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int revengeTimer;

	// Token: 0x0400183D RID: 6205
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool targetAlertChanged;

	// Token: 0x0400183E RID: 6206
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastAliveTime;

	// Token: 0x0400183F RID: 6207
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool alertEnabled = true;

	// Token: 0x04001840 RID: 6208
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int alertTicks;

	// Token: 0x04001841 RID: 6209
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static string notAlertedId = "_notAlerted";

	// Token: 0x04001842 RID: 6210
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int notAlertDelayTicks;

	// Token: 0x04001843 RID: 6211
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isAlert;

	// Token: 0x04001844 RID: 6212
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 investigatePos;

	// Token: 0x04001845 RID: 6213
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int investigatePositionTicks;

	// Token: 0x04001846 RID: 6214
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isInvestigateAlert;

	// Token: 0x04001847 RID: 6215
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool hasAI;

	// Token: 0x04001848 RID: 6216
	public EAIManager aiManager;

	// Token: 0x04001849 RID: 6217
	public List<string> AIPackages;

	// Token: 0x0400184A RID: 6218
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Context utilityAIContext;

	// Token: 0x0400184B RID: 6219
	public EntityPlayer aiClosestPlayer;

	// Token: 0x0400184C RID: 6220
	public float aiClosestPlayerDistSq;

	// Token: 0x0400184D RID: 6221
	public float aiActiveScale;

	// Token: 0x0400184E RID: 6222
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float aiActiveDelay;

	// Token: 0x0400184F RID: 6223
	public bool IsBloodMoon;

	// Token: 0x04001850 RID: 6224
	public bool IsFeral;

	// Token: 0x04001851 RID: 6225
	public bool IsBreakingDoors;

	// Token: 0x04001852 RID: 6226
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_isBreakingBlocks;

	// Token: 0x04001853 RID: 6227
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_isEating;

	// Token: 0x04001854 RID: 6228
	public Vector3 ChaseReturnLocation;

	// Token: 0x04001855 RID: 6229
	public bool IsScoutZombie;

	// Token: 0x04001856 RID: 6230
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityLookHelper lookHelper;

	// Token: 0x04001857 RID: 6231
	public EntityMoveHelper moveHelper;

	// Token: 0x04001858 RID: 6232
	public PathNavigate navigator;

	// Token: 0x04001859 RID: 6233
	public bool bCanClimbLadders;

	// Token: 0x0400185A RID: 6234
	public bool bCanClimbVertical;

	// Token: 0x0400185B RID: 6235
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive damagedTarget;

	// Token: 0x0400185C RID: 6236
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public EntityAlive attackTarget;

	// Token: 0x0400185D RID: 6237
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int attackTargetTime;

	// Token: 0x0400185E RID: 6238
	public EntityAlive attackTargetClient;

	// Token: 0x0400185F RID: 6239
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntityAlive attackTargetLast;

	// Token: 0x04001860 RID: 6240
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EntitySeeCache seeCache;

	// Token: 0x04001861 RID: 6241
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ChunkCoordinates homePosition;

	// Token: 0x04001862 RID: 6242
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int maximumHomeDistance;

	// Token: 0x04001863 RID: 6243
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float jumpMovementFactor = 0.02f;

	// Token: 0x04001864 RID: 6244
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float landMovementFactor = 0.1f;

	// Token: 0x04001865 RID: 6245
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float jumpMotionYValue = 0.419f;

	// Token: 0x04001866 RID: 6246
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float stepSoundDistanceRemaining;

	// Token: 0x04001867 RID: 6247
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float stepSoundRotYRemaining;

	// Token: 0x04001868 RID: 6248
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float nextSwimDistance;

	// Token: 0x04001869 RID: 6249
	public Inventory inventory;

	// Token: 0x0400186A RID: 6250
	public Inventory saveInventory;

	// Token: 0x0400186B RID: 6251
	public Equipment equipment;

	// Token: 0x0400186C RID: 6252
	public ChallengeJournal challengeJournal;

	// Token: 0x0400186D RID: 6253
	public int ExperienceValue;

	// Token: 0x0400186E RID: 6254
	public int deathUpdateTime;

	// Token: 0x0400186F RID: 6255
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public EntityAlive entityThatKilledMe;

	// Token: 0x04001870 RID: 6256
	public bool bPlayerStatsChanged;

	// Token: 0x04001871 RID: 6257
	public bool bEntityAliveFlagsChanged;

	// Token: 0x04001872 RID: 6258
	public bool bPlayerTwitchChanged;

	// Token: 0x04001873 RID: 6259
	public bool bPlayerEquipmentChanged;

	// Token: 0x04001874 RID: 6260
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Dictionary<EnumDamageSource, ulong> damageSourceTimeouts = new EnumDictionary<EnumDamageSource, ulong>();

	// Token: 0x04001875 RID: 6261
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int traderTeleportStreak = 1;

	// Token: 0x04001876 RID: 6262
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bJetpackWearing;

	// Token: 0x04001877 RID: 6263
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bJetpackActive;

	// Token: 0x04001878 RID: 6264
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bParachuteWearing;

	// Token: 0x04001879 RID: 6265
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bAimingGun;

	// Token: 0x0400187A RID: 6266
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bMovementRunning;

	// Token: 0x0400187B RID: 6267
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bCrouching;

	// Token: 0x0400187C RID: 6268
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bJumping;

	// Token: 0x0400187D RID: 6269
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bClimbing;

	// Token: 0x0400187E RID: 6270
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int died;

	// Token: 0x0400187F RID: 6271
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int score;

	// Token: 0x04001880 RID: 6272
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int killedZombies;

	// Token: 0x04001881 RID: 6273
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int killedPlayers;

	// Token: 0x04001882 RID: 6274
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int teamNumber;

	// Token: 0x04001883 RID: 6275
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string entityName = string.Empty;

	// Token: 0x04001884 RID: 6276
	public string DebugNameInfo = string.Empty;

	// Token: 0x04001885 RID: 6277
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int damageLocationBits;

	// Token: 0x04001886 RID: 6278
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bSpawned;

	// Token: 0x04001887 RID: 6279
	public bool bReplicatedAlertFlag;

	// Token: 0x04001888 RID: 6280
	public int vehiclePoseMode = -1;

	// Token: 0x04001889 RID: 6281
	public byte factionId;

	// Token: 0x0400188A RID: 6282
	public byte factionRank;

	// Token: 0x0400188B RID: 6283
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int ticksToCheckSeenByPlayer;

	// Token: 0x0400188C RID: 6284
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool wasSeenByPlayer;

	// Token: 0x0400188D RID: 6285
	public DamageResponse RecordedDamage;

	// Token: 0x0400188E RID: 6286
	public float moveSpeed;

	// Token: 0x0400188F RID: 6287
	public float moveSpeedNight;

	// Token: 0x04001890 RID: 6288
	public float moveSpeedAggro;

	// Token: 0x04001891 RID: 6289
	public float moveSpeedAggroMax;

	// Token: 0x04001892 RID: 6290
	public float moveSpeedPanic;

	// Token: 0x04001893 RID: 6291
	public float moveSpeedPanicMax;

	// Token: 0x04001894 RID: 6292
	public float swimSpeed;

	// Token: 0x04001895 RID: 6293
	public Vector2 swimStrokeRate;

	// Token: 0x04001896 RID: 6294
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public ItemValue handItem;

	// Token: 0x04001897 RID: 6295
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundSpawn;

	// Token: 0x04001898 RID: 6296
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundSleeperGroan;

	// Token: 0x04001899 RID: 6297
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundSleeperSnore;

	// Token: 0x0400189A RID: 6298
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundDeath;

	// Token: 0x0400189B RID: 6299
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundAlert;

	// Token: 0x0400189C RID: 6300
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundAttack;

	// Token: 0x0400189D RID: 6301
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundLiving;

	// Token: 0x0400189E RID: 6302
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundRandom;

	// Token: 0x0400189F RID: 6303
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundSense;

	// Token: 0x040018A0 RID: 6304
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundGiveUp;

	// Token: 0x040018A1 RID: 6305
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundStepType;

	// Token: 0x040018A2 RID: 6306
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundStamina;

	// Token: 0x040018A3 RID: 6307
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundJump;

	// Token: 0x040018A4 RID: 6308
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundLand;

	// Token: 0x040018A5 RID: 6309
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundLandThump;

	// Token: 0x040018A6 RID: 6310
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundHurt;

	// Token: 0x040018A7 RID: 6311
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundHurtSmall;

	// Token: 0x040018A8 RID: 6312
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string soundDrownPain;

	// Token: 0x040018A9 RID: 6313
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string soundDrownDeath;

	// Token: 0x040018AA RID: 6314
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public string soundWaterSurface;

	// Token: 0x040018AB RID: 6315
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int soundDelayTicks;

	// Token: 0x040018AC RID: 6316
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int soundLivingID = -1;

	// Token: 0x040018AD RID: 6317
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public const float cSoundRandomMaxDist = 20f;

	// Token: 0x040018AE RID: 6318
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int soundAlertTicks;

	// Token: 0x040018AF RID: 6319
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int soundRandomTicks;

	// Token: 0x040018B0 RID: 6320
	public int classMaxHealth;

	// Token: 0x040018B1 RID: 6321
	public int classMaxStamina;

	// Token: 0x040018B2 RID: 6322
	public int classMaxFood;

	// Token: 0x040018B3 RID: 6323
	public int classMaxWater;

	// Token: 0x040018B4 RID: 6324
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float weight;

	// Token: 0x040018B5 RID: 6325
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float pushFactor;

	// Token: 0x040018B6 RID: 6326
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float maxViewAngle;

	// Token: 0x040018B7 RID: 6327
	public float sightRangeBase;

	// Token: 0x040018B8 RID: 6328
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float sightRange;

	// Token: 0x040018B9 RID: 6329
	public float senseScale;

	// Token: 0x040018BA RID: 6330
	public int timeStayAfterDeath;

	// Token: 0x040018BB RID: 6331
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public BlockValue corpseBlockValue;

	// Token: 0x040018BC RID: 6332
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float corpseBlockChance;

	// Token: 0x040018BD RID: 6333
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int attackTimeoutDay;

	// Token: 0x040018BE RID: 6334
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int attackTimeoutNight;

	// Token: 0x040018BF RID: 6335
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string particleOnDeath;

	// Token: 0x040018C0 RID: 6336
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string particleOnDestroy;

	// Token: 0x040018C1 RID: 6337
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public EntityBedrollPositionList spawnPoints;

	// Token: 0x040018C2 RID: 6338
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<Vector3i> droppedBackpackPositions;

	// Token: 0x040018C3 RID: 6339
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public float speedModifier = 1f;

	// Token: 0x040018C4 RID: 6340
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public Vector3 accumulatedRootMotion;

	// Token: 0x040018C5 RID: 6341
	public Vector3 moveDirection;

	// Token: 0x040018C6 RID: 6342
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isMoveDirAbsolute;

	// Token: 0x040018C7 RID: 6343
	public Vector3 lookAtPosition;

	// Token: 0x040018C8 RID: 6344
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3i blockPosStandingOn;

	// Token: 0x040018C9 RID: 6345
	public BlockValue blockValueStandingOn;

	// Token: 0x040018CA RID: 6346
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool blockStandingOnChanged;

	// Token: 0x040018CB RID: 6347
	public BiomeDefinition biomeStandingOn;

	// Token: 0x040018CC RID: 6348
	public bool IsMale;

	// Token: 0x040018CD RID: 6349
	public int crouchType;

	// Token: 0x040018CE RID: 6350
	public float crouchBendPer;

	// Token: 0x040018CF RID: 6351
	public float crouchBendPerTarget;

	// Token: 0x040018D0 RID: 6352
	public const int cWalkTypeSwim = -1;

	// Token: 0x040018D1 RID: 6353
	public const int cWalkTypeFat = 1;

	// Token: 0x040018D2 RID: 6354
	public const int cWalkTypeCripple = 5;

	// Token: 0x040018D3 RID: 6355
	public const int cWalkTypeCrouch = 8;

	// Token: 0x040018D4 RID: 6356
	public const int cWalkTypeBandit = 15;

	// Token: 0x040018D5 RID: 6357
	public const int cWalkTypeCrawlFirst = 20;

	// Token: 0x040018D6 RID: 6358
	public const int cWalkTypeCrawler = 21;

	// Token: 0x040018D7 RID: 6359
	public const int cWalkTypeSpider = 22;

	// Token: 0x040018D8 RID: 6360
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int walkType;

	// Token: 0x040018D9 RID: 6361
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int walkTypeBeforeCrouch;

	// Token: 0x040018DA RID: 6362
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public string rightHandTransformName;

	// Token: 0x040018DB RID: 6363
	public int pingToServer;

	// Token: 0x040018DC RID: 6364
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public List<ItemStack> itemsOnEnterGame = new List<ItemStack>();

	// Token: 0x040018DD RID: 6365
	public Utils.EnumHitDirection lastHitDirection = Utils.EnumHitDirection.None;

	// Token: 0x040018DE RID: 6366
	public Vector3 lastHitImpactDir = Vector3.zero;

	// Token: 0x040018DF RID: 6367
	public Vector3 lastHitEntityFwd = Vector3.zero;

	// Token: 0x040018E0 RID: 6368
	public bool lastHitRanged;

	// Token: 0x040018E1 RID: 6369
	public float lastHitForce;

	// Token: 0x040018E2 RID: 6370
	public DamageResponse lastDamageResponse;

	// Token: 0x040018E3 RID: 6371
	public bool canDisintegrate;

	// Token: 0x040018E4 RID: 6372
	public bool isDisintegrated;

	// Token: 0x040018E5 RID: 6373
	public float CreationTimeSinceLevelLoad;

	// Token: 0x040018E6 RID: 6374
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public EntityStats entityStats;

	// Token: 0x040018E7 RID: 6375
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public EntityStats startOfFrameStats;

	// Token: 0x040018E8 RID: 6376
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float proneRefillRate;

	// Token: 0x040018E9 RID: 6377
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float kneelRefillRate;

	// Token: 0x040018EA RID: 6378
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float proneRefillCounter;

	// Token: 0x040018EB RID: 6379
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float kneelRefillCounter;

	// Token: 0x040018EC RID: 6380
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int deathHealth;

	// Token: 0x040018ED RID: 6381
	public BodyDamage bodyDamage;

	// Token: 0x040018EE RID: 6382
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool stompsSpikes;

	// Token: 0x040018EF RID: 6383
	public float OverrideSize = 1f;

	// Token: 0x040018F0 RID: 6384
	public float OverrideHeadSize = 1f;

	// Token: 0x040018F1 RID: 6385
	public float OverrideHeadDismemberScaleTime = 1.5f;

	// Token: 0x040018F2 RID: 6386
	public float OverridePitch;

	// Token: 0x040018F3 RID: 6387
	public static EntityAlive.HeadShotOnlyModes HeadshotMode = EntityAlive.HeadShotOnlyModes.None;

	// Token: 0x040018F4 RID: 6388
	public static EntityAlive.CelebrateModes CelebrateMode = EntityAlive.CelebrateModes.Disabled;

	// Token: 0x040018F5 RID: 6389
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDancing;

	// Token: 0x040018F6 RID: 6390
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float lastTimeTraderStationChecked;

	// Token: 0x040018F7 RID: 6391
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool lerpForwardSpeed;

	// Token: 0x040018F8 RID: 6392
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedForwardTarget;

	// Token: 0x040018F9 RID: 6393
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float speedForwardTargetStep = 1f;

	// Token: 0x040018FA RID: 6394
	public EntityBuffs Buffs;

	// Token: 0x040018FB RID: 6395
	public Progression Progression;

	// Token: 0x040018FC RID: 6396
	public FastTags<TagGroup.Global> CurrentStanceTag = EntityAlive.StanceTagStanding;

	// Token: 0x040018FD RID: 6397
	public FastTags<TagGroup.Global> CurrentMovementTag = FastTags<TagGroup.Global>.none;

	// Token: 0x040018FE RID: 6398
	public float renderFadeMax = 1f;

	// Token: 0x040018FF RID: 6399
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float renderFade;

	// Token: 0x04001900 RID: 6400
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float renderFadeTarget;

	// Token: 0x04001901 RID: 6401
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<EntityAlive.FallBehavior> fallBehaviors = new List<EntityAlive.FallBehavior>();

	// Token: 0x04001902 RID: 6402
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool disableFallBehaviorUntilOnGround;

	// Token: 0x04001903 RID: 6403
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<EntityAlive.DestroyBlockBehavior> _destroyBlockBehaviors = new List<EntityAlive.DestroyBlockBehavior>();

	// Token: 0x04001904 RID: 6404
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public DynamicRagdollFlags _dynamicRagdoll;

	// Token: 0x04001905 RID: 6405
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float _dynamicRagdollStunTime;

	// Token: 0x04001906 RID: 6406
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Vector3 _dynamicRagdollRootMotion;

	// Token: 0x04001907 RID: 6407
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector3> _ragdollPositionsPrev = new List<Vector3>();

	// Token: 0x04001908 RID: 6408
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public readonly List<Vector3> _ragdollPositionsCur = new List<Vector3>();

	// Token: 0x04001909 RID: 6409
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isFirstTimeEquipmentReassigned = true;

	// Token: 0x0400190A RID: 6410
	public bool CrouchingLocked;

	// Token: 0x0400190B RID: 6411
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public EModelBase.HeadStates currentHeadState;

	// Token: 0x0400190C RID: 6412
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static readonly List<EntityAlive.WeightBehavior> weightBehaviorTemp = new List<EntityAlive.WeightBehavior>();

	// Token: 0x0400190D RID: 6413
	public static bool ShowDebugDisplayHit = false;

	// Token: 0x0400190E RID: 6414
	public static float DebugDisplayHitSize = 0.005f;

	// Token: 0x0400190F RID: 6415
	public static float DebugDisplayHitTime = 10f;

	// Token: 0x04001910 RID: 6416
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public static FastTags<TagGroup.Global> noheadTag = FastTags<TagGroup.Global>.Parse("noHead");

	// Token: 0x04001911 RID: 6417
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool bPlayHurtSound;

	// Token: 0x04001912 RID: 6418
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public bool bBeenWounded;

	// Token: 0x04001913 RID: 6419
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public int woundedStrength;

	// Token: 0x04001914 RID: 6420
	[PublicizedFrom(EAccessModifier.Protected)]
	[NonSerialized]
	public DamageSource woundedDamageSource;

	// Token: 0x04001915 RID: 6421
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int despawnDelayCounter;

	// Token: 0x04001916 RID: 6422
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool isDespawnWhenPlayerFar;

	// Token: 0x04001917 RID: 6423
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public bool m_addedToWorld;

	// Token: 0x04001918 RID: 6424
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public int saveHoldingItemIdxBeforeAttach;

	// Token: 0x04001919 RID: 6425
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public float impactSoundTime;

	// Token: 0x0400191A RID: 6426
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<Transform, EntityAlive.ImpactData> impacts = new Dictionary<Transform, EntityAlive.ImpactData>();

	// Token: 0x0400191B RID: 6427
	public const string cParticlePrefix = "Ptl_";

	// Token: 0x0400191C RID: 6428
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, Transform> particles = new Dictionary<string, Transform>();

	// Token: 0x0400191D RID: 6429
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public Dictionary<string, Transform> parts = new Dictionary<string, Transform>();

	// Token: 0x0400191E RID: 6430
	public List<OwnedEntityData> ownedEntities = new List<OwnedEntityData>();

	// Token: 0x0400191F RID: 6431
	[PublicizedFrom(EAccessModifier.Private)]
	[NonSerialized]
	public List<EntityAlive.NetworkStatChange> networkStatsUpdateQueue = new List<EntityAlive.NetworkStatChange>();

	// Token: 0x02000470 RID: 1136
	public enum JumpState
	{
		// Token: 0x04001921 RID: 6433
		Off,
		// Token: 0x04001922 RID: 6434
		Climb,
		// Token: 0x04001923 RID: 6435
		Leap,
		// Token: 0x04001924 RID: 6436
		Air,
		// Token: 0x04001925 RID: 6437
		Land,
		// Token: 0x04001926 RID: 6438
		SwimStart,
		// Token: 0x04001927 RID: 6439
		Swim
	}

	// Token: 0x02000471 RID: 1137
	public enum HeadShotOnlyModes
	{
		// Token: 0x04001929 RID: 6441
		None,
		// Token: 0x0400192A RID: 6442
		HeadshotOnly,
		// Token: 0x0400192B RID: 6443
		HeadshotFinisher
	}

	// Token: 0x02000472 RID: 1138
	public enum CelebrateModes
	{
		// Token: 0x0400192D RID: 6445
		Disabled,
		// Token: 0x0400192E RID: 6446
		Enabled,
		// Token: 0x0400192F RID: 6447
		HeadshotOnly
	}

	// Token: 0x02000473 RID: 1139
	[PublicizedFrom(EAccessModifier.Protected)]
	public class FallBehavior
	{
		// Token: 0x060023AC RID: 9132 RVA: 0x000D8595 File Offset: 0x000D6795
		public FallBehavior(string name, EntityAlive.FallBehavior.Op type, FloatRange height, float weight, FloatRange ragePer, FloatRange rageTime, IntRange difficulty)
		{
			this.Name = name;
			this.ResponseOp = type;
			this.Height = height;
			this.Weight = weight;
			this.RagePer = ragePer;
			this.RageTime = rageTime;
			this.Difficulty = difficulty;
		}

		// Token: 0x04001930 RID: 6448
		public string Name;

		// Token: 0x04001931 RID: 6449
		public readonly EntityAlive.FallBehavior.Op ResponseOp;

		// Token: 0x04001932 RID: 6450
		public readonly FloatRange Height;

		// Token: 0x04001933 RID: 6451
		public readonly float Weight;

		// Token: 0x04001934 RID: 6452
		public readonly FloatRange RagePer;

		// Token: 0x04001935 RID: 6453
		public readonly FloatRange RageTime;

		// Token: 0x04001936 RID: 6454
		public readonly IntRange Difficulty;

		// Token: 0x02000474 RID: 1140
		public enum Op
		{
			// Token: 0x04001938 RID: 6456
			None,
			// Token: 0x04001939 RID: 6457
			Land,
			// Token: 0x0400193A RID: 6458
			LandLow,
			// Token: 0x0400193B RID: 6459
			LandHard,
			// Token: 0x0400193C RID: 6460
			Stumble,
			// Token: 0x0400193D RID: 6461
			Ragdoll
		}
	}

	// Token: 0x02000475 RID: 1141
	[PublicizedFrom(EAccessModifier.Protected)]
	public class DestroyBlockBehavior
	{
		// Token: 0x060023AD RID: 9133 RVA: 0x000D85D4 File Offset: 0x000D67D4
		public DestroyBlockBehavior(string name, EntityAlive.DestroyBlockBehavior.Op type, float weight, FloatRange ragePer, FloatRange rageTime, IntRange difficulty)
		{
			this.Name = name;
			this.ResponseOp = type;
			this.Weight = weight;
			this.RagePer = ragePer;
			this.RageTime = rageTime;
			this.Difficulty = difficulty;
		}

		// Token: 0x0400193E RID: 6462
		public string Name;

		// Token: 0x0400193F RID: 6463
		public readonly EntityAlive.DestroyBlockBehavior.Op ResponseOp;

		// Token: 0x04001940 RID: 6464
		public readonly float Weight;

		// Token: 0x04001941 RID: 6465
		public readonly FloatRange RagePer;

		// Token: 0x04001942 RID: 6466
		public readonly FloatRange RageTime;

		// Token: 0x04001943 RID: 6467
		public readonly IntRange Difficulty = new IntRange(int.MinValue, int.MaxValue);

		// Token: 0x02000476 RID: 1142
		public enum Op
		{
			// Token: 0x04001945 RID: 6469
			None,
			// Token: 0x04001946 RID: 6470
			Ragdoll,
			// Token: 0x04001947 RID: 6471
			Stumble
		}
	}

	// Token: 0x02000477 RID: 1143
	public enum EnumApproachState
	{
		// Token: 0x04001949 RID: 6473
		Ok,
		// Token: 0x0400194A RID: 6474
		TooFarAway,
		// Token: 0x0400194B RID: 6475
		BlockedByWorldMesh,
		// Token: 0x0400194C RID: 6476
		BlockedByEntity,
		// Token: 0x0400194D RID: 6477
		Unknown
	}

	// Token: 0x02000478 RID: 1144
	[PublicizedFrom(EAccessModifier.Private)]
	public struct WeightBehavior
	{
		// Token: 0x0400194E RID: 6478
		public float weight;

		// Token: 0x0400194F RID: 6479
		public int index;
	}

	// Token: 0x02000479 RID: 1145
	[PublicizedFrom(EAccessModifier.Private)]
	public struct ImpactData
	{
		// Token: 0x04001950 RID: 6480
		public int count;
	}

	// Token: 0x0200047A RID: 1146
	[PublicizedFrom(EAccessModifier.Private)]
	public class NetworkStatChange
	{
		// Token: 0x04001951 RID: 6481
		public EntityAlive.EntityNetworkStats m_NetworkStats;

		// Token: 0x04001952 RID: 6482
		public EntityAlive.EntityNetworkHoldingData m_HoldingData;
	}

	// Token: 0x0200047B RID: 1147
	public class EntityNetworkHoldingData
	{
		// Token: 0x04001953 RID: 6483
		public ItemStack m_HoldingItemStack;

		// Token: 0x04001954 RID: 6484
		public byte m_HoldingItemIndex;
	}

	// Token: 0x0200047C RID: 1148
	public class EntityNetworkStats
	{
		// Token: 0x060023B1 RID: 9137 RVA: 0x000D862C File Offset: 0x000D682C
		public void FillFromEntity(EntityAlive _entity)
		{
			this.killed = _entity.Died;
			this.holdingItemStack = _entity.inventory.holdingItemStack;
			this.holdingItemIndex = (byte)_entity.inventory.holdingItemIdx;
			this.deathHealth = _entity.DeathHealth;
			this.teamNumber = _entity.TeamNumber;
			if (GameManager.Instance.World.GetPrimaryPlayer() == _entity)
			{
				_entity.inventory.TurnOffLightFlares();
			}
			if (_entity.Progression != null && _entity.Progression.bProgressionStatsChanged)
			{
				_entity.Progression.bProgressionStatsChanged = false;
				this.hasProgression = true;
				this.progressionsData = _entity.Progression.ToBytes(false);
			}
			this.attachedToEntityId = ((_entity.AttachedToEntity != null) ? _entity.AttachedToEntity.entityId : (-1));
			this.entityName = _entity.EntityName;
			EntityPlayer entityPlayer = _entity as EntityPlayer;
			if (entityPlayer != null)
			{
				this.isPlayer = true;
				this.killedPlayers = _entity.KilledPlayers;
				this.killedZombies = _entity.KilledZombies;
				this.experience = entityPlayer.Progression.ExpToNextLevel;
				this.level = entityPlayer.Progression.Level;
				this.totalItemsCrafted = entityPlayer.totalItemsCrafted;
				this.distanceWalked = entityPlayer.distanceWalked;
				this.longestLife = entityPlayer.longestLife;
				this.currentLife = entityPlayer.currentLife;
				this.totalTimePlayed = entityPlayer.totalTimePlayed;
				this.vehiclePose = entityPlayer.GetVehicleAnimation();
				this.isSpectator = entityPlayer.IsSpectator;
				return;
			}
			this.isPlayer = false;
			this.experience = 0;
			this.level = 1;
			this.distanceWalked = 0f;
			this.totalItemsCrafted = 0U;
			this.longestLife = 0f;
			this.currentLife = 0f;
			this.totalTimePlayed = 0f;
		}

		// Token: 0x060023B2 RID: 9138 RVA: 0x000D87FC File Offset: 0x000D69FC
		public void ToEntity(EntityAlive _entity)
		{
			_entity.Died = this.killed;
			_entity.DeathHealth = this.deathHealth;
			_entity.TeamNumber = this.teamNumber;
			_entity.inventory.bResetLightLevelWhenChanged = true;
			if (!_entity.inventory.GetItem((int)this.holdingItemIndex).Equals(this.holdingItemStack))
			{
				_entity.inventory.SetItem((int)this.holdingItemIndex, this.holdingItemStack);
				_entity.inventory.ForceHoldingItemUpdate();
			}
			if (_entity.inventory.holdingItemIdx != (int)this.holdingItemIndex)
			{
				_entity.inventory.SetHoldingItemIdxNoHolsterTime((int)this.holdingItemIndex);
			}
			if (this.hasProgression)
			{
				_entity.Progression = Progression.FromBytes(this.progressionsData, _entity);
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer && _entity.Progression != null)
				{
					_entity.Progression.bProgressionStatsChanged = true;
				}
			}
			_entity.SetEntityName(this.entityName);
			EntityPlayer entityPlayer = _entity as EntityPlayer;
			if (entityPlayer != null && this.isPlayer)
			{
				if (_entity.NavObject != null)
				{
					_entity.NavObject.name = this.entityName;
				}
				_entity.KilledZombies = this.killedZombies;
				_entity.KilledPlayers = this.killedPlayers;
				entityPlayer.Progression.ExpToNextLevel = this.experience;
				entityPlayer.Progression.Level = this.level;
				entityPlayer.totalItemsCrafted = this.totalItemsCrafted;
				entityPlayer.distanceWalked = this.distanceWalked;
				entityPlayer.longestLife = this.longestLife;
				entityPlayer.currentLife = this.currentLife;
				entityPlayer.totalTimePlayed = this.totalTimePlayed;
				entityPlayer.SetVehiclePoseMode(this.vehiclePose);
				entityPlayer.IsSpectator = this.isSpectator;
			}
		}

		// Token: 0x060023B3 RID: 9139 RVA: 0x000D89A8 File Offset: 0x000D6BA8
		public void read(PooledBinaryReader _reader)
		{
			this.killed = _reader.ReadInt32();
			this.holdingItemStack = new ItemStack();
			this.holdingItemStack.Read(_reader);
			this.holdingItemIndex = _reader.ReadByte();
			this.deathHealth = _reader.ReadInt32();
			this.teamNumber = (int)_reader.ReadByte();
			this.attachedToEntityId = _reader.ReadInt32();
			this.entityName = _reader.ReadString();
			this.isPlayer = _reader.ReadBoolean();
			if (this.isPlayer)
			{
				this.killedZombies = _reader.ReadInt32();
				this.killedPlayers = _reader.ReadInt32();
				this.experience = _reader.ReadInt32();
				this.level = _reader.ReadInt32();
				this.totalItemsCrafted = _reader.ReadUInt32();
				this.distanceWalked = _reader.ReadSingle();
				this.longestLife = _reader.ReadSingle();
				this.currentLife = _reader.ReadSingle();
				this.totalTimePlayed = _reader.ReadSingle();
				this.vehiclePose = _reader.ReadInt32();
				this.isSpectator = _reader.ReadBoolean();
			}
			this.hasProgression = _reader.ReadBoolean();
			if (this.hasProgression)
			{
				int num = (int)_reader.ReadInt16();
				this.progressionsData = new byte[num];
				_reader.Read(this.progressionsData, 0, num);
			}
		}

		// Token: 0x060023B4 RID: 9140 RVA: 0x000D8AE8 File Offset: 0x000D6CE8
		public void write(PooledBinaryWriter _writer)
		{
			_writer.Write(this.killed);
			this.holdingItemStack.Write(_writer);
			_writer.Write(this.holdingItemIndex);
			_writer.Write(this.deathHealth);
			_writer.Write((byte)this.teamNumber);
			_writer.Write(this.attachedToEntityId);
			_writer.Write(this.entityName);
			_writer.Write(this.isPlayer);
			if (this.isPlayer)
			{
				_writer.Write(this.killedZombies);
				_writer.Write(this.killedPlayers);
				_writer.Write(this.experience);
				_writer.Write(this.level);
				_writer.Write(this.totalItemsCrafted);
				_writer.Write(this.distanceWalked);
				_writer.Write(this.longestLife);
				_writer.Write(this.currentLife);
				_writer.Write(this.totalTimePlayed);
				_writer.Write(this.vehiclePose);
				_writer.Write(this.isSpectator);
			}
			_writer.Write(this.hasProgression);
			if (this.hasProgression)
			{
				_writer.Write((short)this.progressionsData.Length);
				_writer.Write(this.progressionsData, 0, this.progressionsData.Length);
			}
		}

		// Token: 0x060023B5 RID: 9141 RVA: 0x000D8C1D File Offset: 0x000D6E1D
		public void SetName(string name)
		{
			this.entityName = name;
		}

		// Token: 0x04001955 RID: 6485
		[PublicizedFrom(EAccessModifier.Private)]
		public int experience;

		// Token: 0x04001956 RID: 6486
		[PublicizedFrom(EAccessModifier.Private)]
		public int level;

		// Token: 0x04001957 RID: 6487
		[PublicizedFrom(EAccessModifier.Private)]
		public int killed;

		// Token: 0x04001958 RID: 6488
		[PublicizedFrom(EAccessModifier.Private)]
		public int killedZombies;

		// Token: 0x04001959 RID: 6489
		[PublicizedFrom(EAccessModifier.Private)]
		public int killedPlayers;

		// Token: 0x0400195A RID: 6490
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemStack holdingItemStack;

		// Token: 0x0400195B RID: 6491
		[PublicizedFrom(EAccessModifier.Private)]
		public byte holdingItemIndex;

		// Token: 0x0400195C RID: 6492
		[PublicizedFrom(EAccessModifier.Private)]
		public int deathHealth;

		// Token: 0x0400195D RID: 6493
		[PublicizedFrom(EAccessModifier.Private)]
		public int teamNumber;

		// Token: 0x0400195E RID: 6494
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasProgression;

		// Token: 0x0400195F RID: 6495
		[PublicizedFrom(EAccessModifier.Private)]
		public byte[] progressionsData;

		// Token: 0x04001960 RID: 6496
		[PublicizedFrom(EAccessModifier.Private)]
		public int attachedToEntityId;

		// Token: 0x04001961 RID: 6497
		[PublicizedFrom(EAccessModifier.Private)]
		public string entityName;

		// Token: 0x04001962 RID: 6498
		[PublicizedFrom(EAccessModifier.Private)]
		public float distanceWalked;

		// Token: 0x04001963 RID: 6499
		[PublicizedFrom(EAccessModifier.Private)]
		public uint totalItemsCrafted;

		// Token: 0x04001964 RID: 6500
		[PublicizedFrom(EAccessModifier.Private)]
		public float longestLife;

		// Token: 0x04001965 RID: 6501
		[PublicizedFrom(EAccessModifier.Private)]
		public float currentLife;

		// Token: 0x04001966 RID: 6502
		[PublicizedFrom(EAccessModifier.Private)]
		public float totalTimePlayed;

		// Token: 0x04001967 RID: 6503
		[PublicizedFrom(EAccessModifier.Private)]
		public int vehiclePose;

		// Token: 0x04001968 RID: 6504
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isSpectator;

		// Token: 0x04001969 RID: 6505
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPlayer;
	}
}
