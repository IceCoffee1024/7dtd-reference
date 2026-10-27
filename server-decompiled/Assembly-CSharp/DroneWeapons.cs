using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020003EB RID: 1003
[Preserve]
public class DroneWeapons
{
	// Token: 0x0400143E RID: 5182
	public const string cSHOCK_BUFF_NAME = "buffShocked";

	// Token: 0x0400143F RID: 5183
	public const string cHealWeaponJoint = "WristLeft";

	// Token: 0x020003EC RID: 1004
	[Preserve]
	public class Weapon
	{
		// Token: 0x06001E42 RID: 7746 RVA: 0x000B83A0 File Offset: 0x000B65A0
		public Weapon(EntityAlive _entity)
		{
			this.entity = _entity;
			this.belongsPlayerId = this.entity.belongsPlayerId;
			this.entityProperties = _entity.EntityClass.Properties;
		}

		// Token: 0x06001E43 RID: 7747 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Init()
		{
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000B83F4 File Offset: 0x000B65F4
		public virtual void Update()
		{
			if (this.cooldownTimer > 0f)
			{
				this.cooldownTimer -= 0.05f;
				if (this.cooldownTimer <= 0f)
				{
					this.OnReadyToFire();
				}
				if (this.target && this.target.IsDead())
				{
					this.InvokeFireComplete();
				}
			}
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000B8454 File Offset: 0x000B6654
		public virtual void SetModProps(ItemValue itemValue)
		{
			this.modItem = itemValue;
			DynamicProperties properties = this.modItem.ItemClass.Properties;
			properties.ParseString("SoundWeaponCharged", ref this.soundWeaponCharged);
			properties.ParseString("SoundWeaponUseBark", ref this.soundWeaponUseBark);
			float value = EffectManager.GetValue(PassiveEffects.MaxRange, itemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			if (value != 0f)
			{
				this.range = value;
			}
			float value2 = EffectManager.GetValue(PassiveEffects.JunkDroneModCooldown, itemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
			if (value2 != 0f)
			{
				this.cooldown = value2;
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x000B84FC File Offset: 0x000B66FC
		public string ItemName
		{
			get
			{
				if (this.modItem == null)
				{
					return string.Empty;
				}
				return this.modItem.ItemClass.Name;
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001E47 RID: 7751 RVA: 0x000B851C File Offset: 0x000B671C
		public float TimeRemaning
		{
			get
			{
				return this.cooldownTimer;
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06001E48 RID: 7752 RVA: 0x000B8524 File Offset: 0x000B6724
		public float TimeLength
		{
			get
			{
				return this.actionTime + this.cooldown;
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x000B8533 File Offset: 0x000B6733
		public float Range
		{
			get
			{
				return this.range;
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x000B853B File Offset: 0x000B673B
		public float Cooldown
		{
			get
			{
				return this.cooldown;
			}
		}

		// Token: 0x06001E4B RID: 7755 RVA: 0x000B8543 File Offset: 0x000B6743
		public virtual bool canFire()
		{
			return this.cooldownTimer <= 0f;
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x000B8555 File Offset: 0x000B6755
		public virtual void Fire(EntityAlive _target)
		{
			this.target = _target;
			this.RefreshCooldown();
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000B8564 File Offset: 0x000B6764
		public void RefreshCooldown()
		{
			this.cooldownTimer = this.actionTime + this.cooldown;
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x000B8579 File Offset: 0x000B6779
		public virtual bool hasActionCompleted()
		{
			return this.cooldownTimer < this.cooldown;
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000B8589 File Offset: 0x000B6789
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnReadyToFire()
		{
			if (string.IsNullOrEmpty(this.soundWeaponCharged))
			{
				this.PlaySound(this.soundWeaponCharged);
			}
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000B85A4 File Offset: 0x000B67A4
		public void RegisterOnFireComplete(Action _onFireComplete)
		{
			this.onFireComplete = _onFireComplete;
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void OnFireComplete()
		{
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000B85AD File Offset: 0x000B67AD
		[PublicizedFrom(EAccessModifier.Protected)]
		public void InvokeFireComplete()
		{
			this.OnFireComplete();
			Action action = this.onFireComplete;
			if (action != null)
			{
				action();
			}
			this.onFireComplete = null;
			this.target = null;
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x000B85D4 File Offset: 0x000B67D4
		public virtual void Equip(ItemValue itemValue)
		{
			this.entity.MinEventContext.ItemValue = itemValue;
			itemValue.FireEvent(MinEventTypes.onSelfEquipStart, this.entity.MinEventContext);
			this.SetModProps(itemValue);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.entity.aiManager.AddItemTasks(itemValue.ItemClass);
			}
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000B8630 File Offset: 0x000B6830
		public void Unequip()
		{
			this.modItem.FireEvent(MinEventTypes.onSelfEquipStop, this.entity.MinEventContext);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				this.entity.aiManager.RemoveItemTasks(this.modItem.ItemClass.Name);
			}
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000B8681 File Offset: 0x000B6881
		public virtual void PlayBark()
		{
			if (!string.IsNullOrEmpty(this.soundWeaponUseBark))
			{
				this.PlayVO(this.soundWeaponUseBark);
			}
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000B869C File Offset: 0x000B689C
		[PublicizedFrom(EAccessModifier.Protected)]
		public void PlaySound(string soundGroupKey)
		{
			Manager.Play(this.entity, soundGroupKey, 1f, false);
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000B86B1 File Offset: 0x000B68B1
		[PublicizedFrom(EAccessModifier.Protected)]
		public void PlayVO(string soundGroupKey)
		{
			(this.entity as EntityDrone).BroadcastPlayVO(soundGroupKey, false, 1f);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000B86CC File Offset: 0x000B68CC
		[PublicizedFrom(EAccessModifier.Protected)]
		public void TargetApplyBuff(string _buff, bool _fromElectrical)
		{
			this.target.Buffs.AddBuff(_buff, World.worldToBlockPos(this.entity.position), (this.belongsPlayerId != -1) ? this.belongsPlayerId : this.entity.entityId, true, _fromElectrical, -1f);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000B8720 File Offset: 0x000B6920
		[PublicizedFrom(EAccessModifier.Protected)]
		public void SpawnParticleEffect(ParticleEffect _pe, int _entityId)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				if (!GameManager.IsDedicatedServer)
				{
					GameManager.Instance.SpawnParticleEffectClient(_pe, _entityId, false, false);
				}
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageParticleEffect>().Setup(_pe, _entityId, false, false), false, -1, _entityId, -1, null, 192, false);
				return;
			}
			SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageParticleEffect>().Setup(_pe, _entityId, false, false), false);
		}

		// Token: 0x06001E5A RID: 7770 RVA: 0x000B8798 File Offset: 0x000B6998
		[PublicizedFrom(EAccessModifier.Protected)]
		public Transform SpawnDroneParticleEffect(ParticleEffect _pe, int _entityId, DroneWeapons.NetPackageDroneParticleEffect.cActionType actionType)
		{
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<DroneWeapons.NetPackageDroneParticleEffect>().Setup(_pe, _entityId, actionType), false, -1, -1, -1, null, 192, false);
				if (!GameManager.IsDedicatedServer)
				{
					return GameManager.Instance.SpawnParticleEffectClientForceCreation(_pe, _entityId, false);
				}
			}
			else
			{
				SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<DroneWeapons.NetPackageDroneParticleEffect>().Setup(_pe, _entityId, actionType), false);
			}
			return null;
		}

		// Token: 0x04001440 RID: 5184
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityAlive entity;

		// Token: 0x04001441 RID: 5185
		[PublicizedFrom(EAccessModifier.Protected)]
		public int belongsPlayerId;

		// Token: 0x04001442 RID: 5186
		[PublicizedFrom(EAccessModifier.Protected)]
		public DynamicProperties entityProperties;

		// Token: 0x04001443 RID: 5187
		public Transform WeaponJoint;

		// Token: 0x04001444 RID: 5188
		[PublicizedFrom(EAccessModifier.Protected)]
		public float actionTime;

		// Token: 0x04001445 RID: 5189
		[PublicizedFrom(EAccessModifier.Protected)]
		public float cooldown = 1f;

		// Token: 0x04001446 RID: 5190
		[PublicizedFrom(EAccessModifier.Protected)]
		public float range = 10f;

		// Token: 0x04001447 RID: 5191
		[PublicizedFrom(EAccessModifier.Protected)]
		public EntityAlive target;

		// Token: 0x04001448 RID: 5192
		[PublicizedFrom(EAccessModifier.Private)]
		public float cooldownTimer;

		// Token: 0x04001449 RID: 5193
		[PublicizedFrom(EAccessModifier.Protected)]
		public ItemValue modItem;

		// Token: 0x0400144A RID: 5194
		public string soundWeaponCharged;

		// Token: 0x0400144B RID: 5195
		public string soundWeaponUseBark;

		// Token: 0x0400144C RID: 5196
		[PublicizedFrom(EAccessModifier.Private)]
		public Action onFireComplete;

		// Token: 0x0400144D RID: 5197
		public const string cSoundWeaponCharged = "SoundWeaponCharged";

		// Token: 0x0400144E RID: 5198
		public const string cSoundWeaponUseBark = "SoundWeaponUseBark";
	}

	// Token: 0x020003ED RID: 1005
	[Preserve]
	public class HealBeamWeapon : DroneWeapons.Weapon
	{
		// Token: 0x06001E5B RID: 7771 RVA: 0x000B880C File Offset: 0x000B6A0C
		public HealBeamWeapon(EntityAlive _entity)
			: base(_entity)
		{
		}

		// Token: 0x06001E5C RID: 7772 RVA: 0x000B885F File Offset: 0x000B6A5F
		public override void Init()
		{
			this.WeaponJoint = this.entity.transform.FindInChilds("WristLeft", false);
		}

		// Token: 0x06001E5D RID: 7773 RVA: 0x000B887D File Offset: 0x000B6A7D
		public override void SetModProps(ItemValue itemValue)
		{
			DynamicProperties properties = itemValue.ItemClass.Properties;
			properties.ParseFloat("HealActionTime", ref this.actionTime);
			properties.ParseFloat("HealDamageThreshold", ref this.HealDamageThreshold);
			base.SetModProps(itemValue);
		}

		// Token: 0x06001E5E RID: 7774 RVA: 0x000B88B4 File Offset: 0x000B6AB4
		public override void Equip(ItemValue itemValue)
		{
			base.Equip(itemValue);
			EntityDrone entityDrone = this.entity as EntityDrone;
			EntityAlive entityAlive = GameManager.Instance.World.GetEntity(entityDrone.belongsPlayerId) as EntityAlive;
			if (entityAlive && entityDrone && entityDrone.TimeSinceCreation != 0f)
			{
				entityAlive.Buffs.AddBuff("buffJunkDroneHealCooldownEffect", -1, true, false, -1f);
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000B8928 File Offset: 0x000B6B28
		public override void Fire(EntityAlive _target)
		{
			base.Fire(_target);
			if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				DroneWeapons.HealBeamWeapon.HealItemType healItemType = this.findNeededHealType(_target);
				if (healItemType == DroneWeapons.HealBeamWeapon.HealItemType.None)
				{
					base.InvokeFireComplete();
					Log.Out("HealBeamWeapon: failed to determine heal type");
					return;
				}
				ItemStack healingItemStack = this.getHealingItemStack(healItemType);
				if (healingItemStack == null)
				{
					base.InvokeFireComplete();
					return;
				}
				this.entity.inventory.SetItem(0, healingItemStack);
				this.entity.inventory.SetHoldingItemIdx(0);
				ItemAction itemAction = this.entity.inventory.holdingItem.Actions[1];
				ItemActionData itemActionData = this.entity.inventory.holdingItemData.actionData[1];
				ItemActionUseOther.FeedInventoryData feedInventoryData = itemActionData as ItemActionUseOther.FeedInventoryData;
				EntityDrone entityDrone = this.entity as EntityDrone;
				EntityAlive attackTarget = entityDrone.GetAttackTarget();
				if (feedInventoryData != null)
				{
					feedInventoryData.TargetEntity = attackTarget;
					itemActionData = feedInventoryData;
				}
				if (itemAction != null && itemAction.CanExecute(itemActionData))
				{
					itemAction.ExecuteAction(itemActionData, false);
					itemAction.ExecuteAction(itemActionData, true);
				}
				EntityAlive owner = entityDrone.Owner;
				if (owner)
				{
					owner.Buffs.AddBuff("buffJunkDroneHealCooldownEffect", -1, true, false, -1f);
				}
				base.PlaySound("drone_healeffect");
				ParticleEffect particleEffect = new ParticleEffect("drone_heal_beam", Vector3.zero, Quaternion.LookRotation(_target.getHeadPosition() - this.entity.position), 1f, Color.clear, null, this.entity.transform, 1f, "");
				Transform transform = base.SpawnDroneParticleEffect(particleEffect, this.entity.entityId, DroneWeapons.NetPackageDroneParticleEffect.cActionType.Heal);
				if (transform && !GameManager.IsDedicatedServer)
				{
					transform.GetComponent<DroneBeamParticle>().SetDisplayTime(this.actionTime);
				}
				ParticleEffect particleEffect2 = new ParticleEffect("drone_heal_player", Vector3.zero, Quaternion.identity, 1f, Color.clear, null, _target.transform, 1f, "");
				if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
				{
					SingletonMonoBehaviour<ConnectionManager>.Instance.SendPackage(NetPackageManager.GetPackage<NetPackageParticleEffect>().Setup(particleEffect2, _target.entityId, false, false), false, -1, -1, -1, null, 192, false);
					if (!GameManager.IsDedicatedServer)
					{
						GameManager.Instance.SpawnParticleEffectClient(particleEffect2, _target.entityId, false, false);
					}
				}
			}
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000B8B5A File Offset: 0x000B6D5A
		public override bool canFire()
		{
			return base.canFire() && this.hasHealingItem();
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000B8B6C File Offset: 0x000B6D6C
		public bool hasHealingItem()
		{
			return this.hasSupportedHealingItem();
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000B8B74 File Offset: 0x000B6D74
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasSupportedHealingItem()
		{
			for (int i = 0; i < this.supportedItems.Length; i++)
			{
				if (this.hasItem(this.supportedItems[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E63 RID: 7779 RVA: 0x000B8BA8 File Offset: 0x000B6DA8
		public bool targetCanBeHealed(EntityAlive _target)
		{
			return _target.IsAlive() && !_target.Buffs.HasBuff("buffHealHealth") && _target.GetCVar("medicalRegHealthAmount") == 0f && (float)_target.Health < _target.Stats.Health.ModifiedMax;
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000B8C00 File Offset: 0x000B6E00
		public bool isTargetInNeedOfMedical(EntityAlive _target)
		{
			if (!_target)
			{
				return false;
			}
			float num = (float)_target.GetMaxHealth();
			float modifiedMax = _target.Stats.Health.ModifiedMax;
			return ((num == modifiedMax && (float)_target.Health < num - this.HealDamageThreshold) || (float)_target.Health < modifiedMax * 0.67f) && _target.GetCVar("medicalRegHealthAmount") == 0f;
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000B8C69 File Offset: 0x000B6E69
		public bool targetNeedsHealing(EntityAlive _target)
		{
			return this.findNeededHealType(_target) > DroneWeapons.HealBeamWeapon.HealItemType.None;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000B8C75 File Offset: 0x000B6E75
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isTargetBleeding(EntityAlive _target)
		{
			return _target.Buffs.ActiveBuffs.Find((BuffValue b) => b.BuffName.ContainsCaseInsensitive("buffInjuryBleeding")) != null;
		}

		// Token: 0x06001E67 RID: 7783 RVA: 0x000B8CAC File Offset: 0x000B6EAC
		[PublicizedFrom(EAccessModifier.Private)]
		public DroneWeapons.HealBeamWeapon.HealItemType findNeededHealType(EntityAlive _entity)
		{
			bool flag = this.hasItem(DroneWeapons.HealBeamWeapon.HealItemType.Bandage);
			bool flag2 = this.hasItem(DroneWeapons.HealBeamWeapon.HealItemType.MedicalBandage);
			bool flag3 = this.hasItem(DroneWeapons.HealBeamWeapon.HealItemType.FirstAidKit);
			if (this.isTargetInNeedOfMedical(_entity) || this.targetCanBeHealed(_entity))
			{
				if (flag2)
				{
					return DroneWeapons.HealBeamWeapon.HealItemType.MedicalBandage;
				}
				if (flag3)
				{
					return DroneWeapons.HealBeamWeapon.HealItemType.FirstAidKit;
				}
				if (this.isTargetBleeding(_entity))
				{
					return DroneWeapons.HealBeamWeapon.HealItemType.Bandage;
				}
			}
			else if (this.isTargetBleeding(_entity))
			{
				if (flag)
				{
					return DroneWeapons.HealBeamWeapon.HealItemType.Bandage;
				}
				if (flag2)
				{
					return DroneWeapons.HealBeamWeapon.HealItemType.MedicalBandage;
				}
				if (flag3)
				{
					return DroneWeapons.HealBeamWeapon.HealItemType.FirstAidKit;
				}
			}
			return DroneWeapons.HealBeamWeapon.HealItemType.None;
		}

		// Token: 0x06001E68 RID: 7784 RVA: 0x000B8D14 File Offset: 0x000B6F14
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemStack getHealingItemStack(DroneWeapons.HealBeamWeapon.HealItemType healType)
		{
			Bag bag = this.entity.bag;
			for (int i = 0; i < bag.SlotCount; i++)
			{
				ItemStack itemStack = bag.ItemGrid[i];
				if (this.isItem(itemStack.itemValue, healType))
				{
					ItemStack itemStack2 = new ItemStack(itemStack.itemValue.Clone(), 1);
					bag.DecItem(itemStack.itemValue, 1, false, null);
					return itemStack2;
				}
			}
			return null;
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000B8D80 File Offset: 0x000B6F80
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasItem(string itemGroupOrName)
		{
			Bag bag = this.entity.bag;
			for (int i = 0; i < bag.SlotCount; i++)
			{
				ItemStack itemStack = bag.ItemGrid[i];
				if (this.isItem(itemStack.itemValue, itemGroupOrName) && bag.CanTakeItem(itemStack))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000B8DD2 File Offset: 0x000B6FD2
		[PublicizedFrom(EAccessModifier.Private)]
		public bool hasItem(DroneWeapons.HealBeamWeapon.HealItemType itemType)
		{
			return this.hasItem(this.supportedItems[(int)itemType]);
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000B8DE2 File Offset: 0x000B6FE2
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isItem(ItemValue iv, string itemGroupOrName)
		{
			return iv.ItemClass != null && iv.ItemClass.Name.Equals(itemGroupOrName);
		}

		// Token: 0x06001E6C RID: 7788 RVA: 0x000B8E02 File Offset: 0x000B7002
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isItem(ItemValue iv, DroneWeapons.HealBeamWeapon.HealItemType itemType)
		{
			return this.isItem(iv, this.supportedItems[(int)itemType]);
		}

		// Token: 0x0400144F RID: 5199
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cHealBeam = "drone_heal_beam";

		// Token: 0x04001450 RID: 5200
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cHealPlayer = "drone_heal_player";

		// Token: 0x04001451 RID: 5201
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cHealEffect = "drone_healeffect";

		// Token: 0x04001452 RID: 5202
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cIdxHealing = 0;

		// Token: 0x04001453 RID: 5203
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cActionIdx = 1;

		// Token: 0x04001454 RID: 5204
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cHealingBuff = "buffHealHealth";

		// Token: 0x04001455 RID: 5205
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cHealCVar = "medicalRegHealthAmount";

		// Token: 0x04001456 RID: 5206
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cModifiedHealthCutoff = 0.67f;

		// Token: 0x04001457 RID: 5207
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cBuffHealCooldown = "buffJunkDroneHealCooldownEffect";

		// Token: 0x04001458 RID: 5208
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cBuffInjuryBleeding = "buffInjuryBleeding";

		// Token: 0x04001459 RID: 5209
		[PublicizedFrom(EAccessModifier.Private)]
		public static FastTags<TagGroup.Global> healingItemTags = FastTags<TagGroup.Global>.Parse("medical");

		// Token: 0x0400145A RID: 5210
		[PublicizedFrom(EAccessModifier.Private)]
		public float HealDamageThreshold = 35f;

		// Token: 0x0400145B RID: 5211
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] supportedItems = new string[] { "none", "medicalAloeCream", "medicalBandage", "medicalFirstAidBandage", "medicalFirstAidKit" };

		// Token: 0x020003EE RID: 1006
		[PublicizedFrom(EAccessModifier.Private)]
		public enum HealItemType
		{
			// Token: 0x0400145D RID: 5213
			None,
			// Token: 0x0400145E RID: 5214
			AloeCream,
			// Token: 0x0400145F RID: 5215
			Bandage,
			// Token: 0x04001460 RID: 5216
			MedicalBandage,
			// Token: 0x04001461 RID: 5217
			FirstAidKit
		}
	}

	// Token: 0x020003F0 RID: 1008
	[Preserve]
	public class StunBeamWeapon : DroneWeapons.Weapon
	{
		// Token: 0x06001E71 RID: 7793 RVA: 0x000B8E42 File Offset: 0x000B7042
		public StunBeamWeapon(EntityAlive _entity)
			: base(_entity)
		{
		}

		// Token: 0x06001E72 RID: 7794 RVA: 0x000B8E4B File Offset: 0x000B704B
		public override void Init()
		{
			this.WeaponJoint = this.entity.transform.FindInChilds("WristRight", false);
			this.cooldown = 10f;
			this.range = 2f;
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x000B8E80 File Offset: 0x000B7080
		public override void Fire(EntityAlive _target)
		{
			base.Fire(_target);
			_target.SetCVar("_droneStunDamage", (float)this.modItem.Quality);
			base.TargetApplyBuff("buffShocked", true);
			base.PlaySound("drone_attackeffect");
			FireControllerUtils.SpawnParticleEffect(new ParticleEffect("nozzleflashuzi", this.WeaponJoint.position + Origin.position, Quaternion.Euler(0f, 180f, 0f), 1f, Color.white, "Electricity/Turret/turret_fire", this.WeaponJoint, 1f, ""), -1);
			float num = GameManager.Instance.World.GetLightBrightness(World.worldToBlockPos(this.entity.position)) / 2f;
			FireControllerUtils.SpawnParticleEffect(new ParticleEffect("nozzlesmokeuzi", this.WeaponJoint.position + Origin.position, num, new Color(1f, 1f, 1f, 0.3f), null, this.WeaponJoint, false, 1f, ""), -1);
		}

		// Token: 0x04001464 RID: 5220
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cStunWeaponJoint = "WristRight";

		// Token: 0x04001465 RID: 5221
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cAttackEffect = "drone_attackeffect";

		// Token: 0x04001466 RID: 5222
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cShockBuffDmgReq = "_droneStunDamage";

		// Token: 0x04001467 RID: 5223
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cShockMuzzleFlash = "nozzleflashuzi";

		// Token: 0x04001468 RID: 5224
		[PublicizedFrom(EAccessModifier.Private)]
		public const string cShockMuzzleSmoke = "nozzlesmokeuzi";
	}

	// Token: 0x020003F1 RID: 1009
	[Preserve]
	public class MachineGunWeapon : DroneWeapons.Weapon
	{
		// Token: 0x06001E74 RID: 7796 RVA: 0x000B8F91 File Offset: 0x000B7191
		public MachineGunWeapon(EntityAlive _entity)
			: base(_entity)
		{
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x000B8FB8 File Offset: 0x000B71B8
		public override void Init()
		{
			this.damageMultiplier = new DamageMultiplier(this.entityProperties);
			this.WeaponJoint = this.entity.transform.FindInChilds("WristRight", false);
			if (this.entityProperties.Values.ContainsKey("MaxDistance"))
			{
				this.range = StringParsers.ParseFloat(this.entityProperties.Values["MaxDistance"], 0, -1, NumberStyles.Any);
			}
			this.spreadHorizontal = new Vector2(-1f, 1f);
			this.spreadVertical = new Vector2(-1f, 1f);
			if (this.entityProperties.Values.ContainsKey("RaySpread"))
			{
				float num = StringParsers.ParseFloat(this.entityProperties.Values["RaySpread"], 0, -1, NumberStyles.Any);
				num *= 0.5f;
				this.spreadHorizontal = new Vector2(-num, num);
				this.spreadVertical = new Vector2(-num, num);
			}
			if (this.entityProperties.Values.ContainsKey("RayCount"))
			{
				this.RayCount = (float)int.Parse(this.entityProperties.Values["RayCount"]);
			}
			if (this.entityProperties.Values.ContainsKey("BurstRoundCount"))
			{
				this.burstRoundCountMax = int.Parse(this.entityProperties.Values["BurstRoundCount"]);
			}
			if (this.entityProperties.Values.ContainsKey("BurstFireRate"))
			{
				this.burstFireRate = Mathf.Max(StringParsers.ParseFloat(this.entityProperties.Values["BurstFireRate"], 0, -1, NumberStyles.Any), 0.1f);
			}
			this.actionTime = this.burstFireRate * (float)this.burstRoundCountMax;
			if (this.entityProperties.Values.ContainsKey("CooldownTime"))
			{
				this.cooldown = StringParsers.ParseFloat(this.entityProperties.Values["CooldownTime"], 0, -1, NumberStyles.Any);
			}
			if (this.entityProperties.Values.ContainsKey("EntityDamage"))
			{
				this.entityDamage = int.Parse(this.entityProperties.Values["EntityDamage"]);
			}
			this.buffActions = new List<string>();
			if (this.entityProperties.Values.ContainsKey("Buff"))
			{
				string[] array = this.entityProperties.Values["Buff"].Replace(" ", "").Split(',', StringSplitOptions.None);
				this.buffActions.AddRange(array);
			}
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000B9254 File Offset: 0x000B7454
		public override void Update()
		{
			base.Update();
			if (this.target != null && !this.target.IsDead() && this.burstRoundCount < this.burstRoundCountMax && base.TimeRemaning > 0f && base.TimeRemaning < base.TimeLength - this.burstFireRate * (float)this.burstRoundCount)
			{
				this._fireWeapon();
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000B92C0 File Offset: 0x000B74C0
		public override void Fire(EntityAlive _target)
		{
			if (!SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
			{
				return;
			}
			base.Fire(_target);
			this._fireWeapon();
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000B92DC File Offset: 0x000B74DC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnFireComplete()
		{
			base.OnFireComplete();
			this.burstRoundCount = 0;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000B92EC File Offset: 0x000B74EC
		[PublicizedFrom(EAccessModifier.Private)]
		public void _fireWeapon()
		{
			EntityDrone entityDrone = this.entity as EntityDrone;
			Vector3 position = this.WeaponJoint.transform.position;
			Vector3 vector = this.target.getChestPosition() - Origin.position;
			EntityAlive entityAlive = GameManager.Instance.World.GetEntity(entityDrone.belongsPlayerId) as EntityAlive;
			GameRandom gameRandom = GameManager.Instance.World.GetGameRandom();
			FastTags<TagGroup.Global> itemTags = entityDrone.OriginalItemValue.ItemClass.ItemTags;
			int num = (int)EffectManager.GetValue(PassiveEffects.RoundRayCount, entityDrone.OriginalItemValue, this.RayCount, entityAlive, null, entityDrone.OriginalItemValue.ItemClass.ItemTags, true, false, true, true, true, 1, true, false);
			float value = EffectManager.GetValue(PassiveEffects.MaxRange, entityDrone.OriginalItemValue, this.range, entityAlive, null, entityDrone.OriginalItemValue.ItemClass.ItemTags, true, false, true, true, true, 1, true, false);
			for (int i = 0; i < num; i++)
			{
				Vector3 vector2 = (vector - position).normalized;
				vector2 = Quaternion.Euler(gameRandom.RandomRange(this.spreadHorizontal.x, this.spreadHorizontal.y), gameRandom.RandomRange(this.spreadVertical.x, this.spreadVertical.y), 0f) * vector2;
				Ray ray = new Ray(position + Origin.position, vector2);
				int num2 = Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.EntityPenetrationCount, entityDrone.OriginalItemValue, 0f, entityAlive, null, entityDrone.OriginalItemValue.ItemClass.ItemTags, true, false, true, true, true, 1, true, false));
				num2++;
				int num3 = Mathf.FloorToInt(EffectManager.GetValue(PassiveEffects.BlockPenetrationFactor, entityDrone.OriginalItemValue, 1f, entityAlive, null, entityDrone.OriginalItemValue.ItemClass.ItemTags, true, false, true, true, true, 1, true, false));
				EntityAlive entityAlive2 = null;
				for (int j = 0; j < num2; j++)
				{
					if (Voxel.Raycast(GameManager.Instance.World, ray, value, -538751005, 8, 0f))
					{
						WorldRayHitInfo worldRayHitInfo = Voxel.voxelRayHitInfo.Clone();
						if (worldRayHitInfo.tag.StartsWith("E_"))
						{
							string text;
							EntityAlive entityAlive3 = ItemActionAttack.FindHitEntityNoTagCheck(worldRayHitInfo, out text) as EntityAlive;
							if (entityAlive2 == entityAlive3)
							{
								ray.origin = worldRayHitInfo.hit.pos + ray.direction * 0.1f;
								j--;
								goto IL_0338;
							}
							entityAlive2 = entityAlive3;
						}
						else
						{
							j += Mathf.FloorToInt((float)ItemActionAttack.GetBlockHit(GameManager.Instance.World, worldRayHitInfo).Block.MaxDamage / (float)num3);
						}
						ItemActionAttack.Hit(worldRayHitInfo, entityDrone.belongsPlayerId, EnumDamageTypes.Piercing, this.GetDamageBlock(entityDrone.OriginalItemValue, BlockValue.Air, GameManager.Instance.World.GetEntity(entityDrone.belongsPlayerId) as EntityAlive, 1), this.GetDamageEntity(entityDrone.OriginalItemValue, GameManager.Instance.World.GetEntity(entityDrone.belongsPlayerId) as EntityAlive, 1), 1f, entityDrone.OriginalItemValue.PercentUsesLeft, 0f, 0f, "bullet", this.damageMultiplier, this.buffActions, new ItemActionAttack.AttackHitInfo(), 1, 0, 0f, null, null, ItemActionAttack.EnumAttackMode.RealNoHarvesting, null, entityDrone.entityId, entityDrone.OriginalItemValue, false, false, true, null);
					}
					IL_0338:;
				}
			}
			ParticleEffect particleEffect = new ParticleEffect("nozzleflashuzi", this.WeaponJoint.position + Origin.position, Quaternion.Euler(0f, 180f, 0f), 1f, Color.white, "Electricity/Turret/turret_fire", this.WeaponJoint, 1f, "");
			base.SpawnParticleEffect(particleEffect, -1);
			float num4 = GameManager.Instance.World.GetLightBrightness(World.worldToBlockPos(this.entity.position)) / 2f;
			ParticleEffect particleEffect2 = new ParticleEffect("nozzlesmokeuzi", this.WeaponJoint.position + Origin.position, num4, new Color(1f, 1f, 1f, 0.3f), null, this.WeaponJoint, false, 1f, "");
			base.SpawnParticleEffect(particleEffect2, -1);
			this.burstRoundCount++;
			if ((int)EffectManager.GetValue(PassiveEffects.MagazineSize, entityDrone.OriginalItemValue, 0f, null, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false) > 0)
			{
				EntityDrone entityDrone2 = entityDrone;
				int ammoCount = entityDrone2.AmmoCount;
				entityDrone2.AmmoCount = ammoCount - 1;
			}
			entityDrone.OriginalItemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, entityDrone.OriginalItemValue, 1f, entityAlive, null, entityDrone.OriginalItemValue.ItemClass.ItemTags, true, false, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000B97B8 File Offset: 0x000B79B8
		[PublicizedFrom(EAccessModifier.Private)]
		public float GetDamageEntity(ItemValue _itemValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
		{
			return EffectManager.GetValue(PassiveEffects.EntityDamage, _itemValue, (float)this.entityDamage, _holdingEntity, null, _itemValue.ItemClass.ItemTags, true, false, true, true, true, 1, true, false);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000B97E8 File Offset: 0x000B79E8
		[PublicizedFrom(EAccessModifier.Private)]
		public float GetDamageBlock(ItemValue _itemValue, BlockValue _blockValue, EntityAlive _holdingEntity = null, int actionIndex = 0)
		{
			this.tmpTag = _itemValue.ItemClass.ItemTags;
			this.tmpTag |= _blockValue.Block.Tags;
			float value = EffectManager.GetValue(PassiveEffects.BlockDamage, _itemValue, (float)this.blockDamage, _holdingEntity, null, this.tmpTag, true, false, true, true, true, 1, true, false);
			return Utils.FastMin((float)_blockValue.Block.blockMaterial.MaxIncomingDamage, value);
		}

		// Token: 0x04001469 RID: 5225
		[PublicizedFrom(EAccessModifier.Private)]
		public float RayCount = 1f;

		// Token: 0x0400146A RID: 5226
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2 spreadHorizontal;

		// Token: 0x0400146B RID: 5227
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2 spreadVertical;

		// Token: 0x0400146C RID: 5228
		[PublicizedFrom(EAccessModifier.Private)]
		public int burstRoundCountMax = 1;

		// Token: 0x0400146D RID: 5229
		[PublicizedFrom(EAccessModifier.Private)]
		public int burstRoundCount;

		// Token: 0x0400146E RID: 5230
		[PublicizedFrom(EAccessModifier.Private)]
		public const float burstFireRateMax = 0.1f;

		// Token: 0x0400146F RID: 5231
		[PublicizedFrom(EAccessModifier.Private)]
		public float burstFireRate = 1f;

		// Token: 0x04001470 RID: 5232
		[PublicizedFrom(EAccessModifier.Private)]
		public int entityDamage;

		// Token: 0x04001471 RID: 5233
		[PublicizedFrom(EAccessModifier.Private)]
		public int blockDamage;

		// Token: 0x04001472 RID: 5234
		[PublicizedFrom(EAccessModifier.Private)]
		public DamageMultiplier damageMultiplier;

		// Token: 0x04001473 RID: 5235
		[PublicizedFrom(EAccessModifier.Private)]
		public List<string> buffActions;

		// Token: 0x04001474 RID: 5236
		[PublicizedFrom(EAccessModifier.Private)]
		public FastTags<TagGroup.Global> tmpTag;
	}

	// Token: 0x020003F2 RID: 1010
	[Preserve]
	public class NetPackageDroneParticleEffect : NetPackage
	{
		// Token: 0x06001E7C RID: 7804 RVA: 0x000B985C File Offset: 0x000B7A5C
		public DroneWeapons.NetPackageDroneParticleEffect Setup(ParticleEffect _pe, int _entityThatCausedIt, DroneWeapons.NetPackageDroneParticleEffect.cActionType _actionType)
		{
			this.pe = _pe;
			this.entityThatCausedIt = _entityThatCausedIt;
			this.actionType = _actionType;
			return this;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000B9874 File Offset: 0x000B7A74
		public override void read(PooledBinaryReader _br)
		{
			this.pe = new ParticleEffect();
			this.pe.Read(_br);
			this.entityThatCausedIt = _br.ReadInt32();
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000B9899 File Offset: 0x000B7A99
		public override void write(PooledBinaryWriter _bw)
		{
			base.write(_bw);
			this.pe.Write(_bw);
			_bw.Write(this.entityThatCausedIt);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x000B98BC File Offset: 0x000B7ABC
		public override void ProcessPackage(World _world, GameManager _callbacks)
		{
			if (_world == null)
			{
				return;
			}
			if (!_world.IsRemote())
			{
				_world.GetGameManager().SpawnParticleEffectServer(this.pe, this.entityThatCausedIt, false, false);
				return;
			}
			Transform transform = _world.GetGameManager().SpawnParticleEffectClientForceCreation(this.pe, this.entityThatCausedIt, false);
			if (transform != null)
			{
				EntityDrone entityDrone = _world.GetEntity(this.entityThatCausedIt) as EntityDrone;
				if (entityDrone != null)
				{
					DroneBeamParticle component = transform.GetComponent<DroneBeamParticle>();
					if (this.actionType == DroneWeapons.NetPackageDroneParticleEffect.cActionType.Attack && entityDrone.activeWeapon != null)
					{
						transform.parent = entityDrone.activeWeapon.WeaponJoint;
						component.SetDisplayTime(entityDrone.AttackActionTime);
					}
					else if (this.actionType == DroneWeapons.NetPackageDroneParticleEffect.cActionType.Heal)
					{
						transform.parent = entityDrone.healWeapon.WeaponJoint;
						component.SetDisplayTime(entityDrone.HealActionTime);
					}
					transform.localPosition = Vector3.zero;
					transform.localRotation = Quaternion.identity;
				}
			}
		}

		// Token: 0x04001475 RID: 5237
		[PublicizedFrom(EAccessModifier.Private)]
		public ParticleEffect pe;

		// Token: 0x04001476 RID: 5238
		[PublicizedFrom(EAccessModifier.Private)]
		public int entityThatCausedIt;

		// Token: 0x04001477 RID: 5239
		[PublicizedFrom(EAccessModifier.Private)]
		public DroneWeapons.NetPackageDroneParticleEffect.cActionType actionType;

		// Token: 0x020003F3 RID: 1011
		public enum cActionType
		{
			// Token: 0x04001479 RID: 5241
			Attack,
			// Token: 0x0400147A RID: 5242
			Heal
		}
	}
}
