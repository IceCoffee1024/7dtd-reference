using System;
using System.Collections.Generic;
using System.Text;
using Audio;
using UnityEngine;
using XMLData.Item;

// Token: 0x02000594 RID: 1428
public abstract class ItemAction : XMLData.Item.ItemActionData
{
	// Token: 0x06002EF1 RID: 12017 RVA: 0x0012919D File Offset: 0x0012739D
	public string GetSoundStart()
	{
		return this.soundStart;
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x001291A8 File Offset: 0x001273A8
	public virtual ItemClass.EnumCrosshairType GetCrosshairType(global::ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal.TPCameraCheckResult == eTPCameraCheckResult.Pass)
		{
			return ItemClass.EnumCrosshairType.Plus;
		}
		if (entityPlayerLocal.TPCameraCheckResult == eTPCameraCheckResult.LineOfSightCheckFailed)
		{
			return ItemClass.EnumCrosshairType.Blocked;
		}
		return ItemClass.EnumCrosshairType.None;
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEndDelayed()
	{
		return false;
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHUD(global::ItemActionData _actionData, int _x, int _y)
	{
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnScreenOverlay(global::ItemActionData _data)
	{
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeScrollWheel(global::ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeCameraFunction(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002EF8 RID: 12024 RVA: 0x001291DD File Offset: 0x001273DD
	public virtual global::ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new global::ItemActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002EF9 RID: 12025 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual RenderCubeType GetFocusType(global::ItemActionData _actionData)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsFocusBlockInside()
	{
		return true;
	}

	// Token: 0x06002EFB RID: 12027 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsHUDDisabled(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x001291E6 File Offset: 0x001273E6
	public virtual void StartHolding(global::ItemActionData _data)
	{
		this.OnModificationsChanged(_data);
	}

	// Token: 0x06002EFD RID: 12029 RVA: 0x001291EF File Offset: 0x001273EF
	public virtual void StopHolding(global::ItemActionData _data)
	{
		_data.invData.holdingEntity.StopAnimatorAudio(Entity.StopAnimatorAudioType.StopOnStopHolding);
	}

	// Token: 0x06002EFE RID: 12030 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingUpdate(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ShowItem(global::ItemActionData _actionData, bool _show)
	{
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnModificationsChanged(global::ItemActionData _data)
	{
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CanCancel(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x00129204 File Offset: 0x00127404
	public virtual void CancelReload(global::ItemActionData _actionData, bool holsterWeapon)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && holsterWeapon)
		{
			entityPlayerLocal.HolsterWeapon(true);
		}
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup(global::ItemActionData _data)
	{
	}

	// Token: 0x06002F05 RID: 12037 RVA: 0x00129238 File Offset: 0x00127438
	public virtual void ReadFrom(DynamicProperties _props)
	{
		this.Delay = 0f;
		_props.ParseFloat("Delay", ref this.Delay);
		_props.ParseString("Sound_start", ref this.soundStart);
		_props.ParseBool("Sound_in_head", ref this.Sound_in_head);
		if (_props.Values.ContainsKey("Particle_harvesting"))
		{
			this.bUseParticleHarvesting = StringParsers.ParseBool(_props.Values["Particle_harvesting"], 0, -1, true);
			this.particleHarvestingCategory = _props.Params1["Particle_harvesting"];
		}
		this.ActionExp = 2;
		_props.ParseInt("ActionExp", ref this.ActionExp);
		this.ActionExpBonusMultiplier = 10f;
		_props.ParseFloat("ActionExpBonusMultiplier", ref this.ActionExpBonusMultiplier);
		_props.ParseBool("UseAnimation", ref this.UseAnimation);
		this.BuffActions = new List<string>();
		if (_props.Values.ContainsKey("Buff"))
		{
			if (_props.Values["Buff"].Contains(","))
			{
				string[] array = _props.Values["Buff"].Replace(" ", "").Split(',', StringSplitOptions.None);
				this.BuffActions.AddRange(array);
			}
			else
			{
				this.BuffActions.Add(_props.Values["Buff"].Trim());
			}
		}
		else
		{
			this.ActionExpBonusMultiplier = 10f;
		}
		this.Properties = _props;
	}

	// Token: 0x06002F06 RID: 12038 RVA: 0x001293B1 File Offset: 0x001275B1
	public string GetDescription()
	{
		return this.Properties.GetString("Description");
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string CanInteract(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x001293C4 File Offset: 0x001275C4
	public static void ExecuteBuffActions(List<string> actions, int instigatorId, EntityAlive target, bool isCritical, EnumBodyPartHit hitLocation, string context)
	{
		if (target == null)
		{
			return;
		}
		EntityAlive entityAlive = GameManager.Instance.World.GetEntity(instigatorId) as EntityAlive;
		if (entityAlive == null)
		{
			return;
		}
		if (actions != null)
		{
			for (int i = 0; i < actions.Count; i++)
			{
				BuffClass buff = BuffManager.GetBuff(actions[i]);
				if (buff != null)
				{
					float num = 1f;
					num = EffectManager.GetValue(PassiveEffects.BuffProcChance, null, num, entityAlive, null, FastTags<TagGroup.Global>.Parse(buff.Name), true, true, true, true, true, 1, true, false);
					if (target.rand.RandomFloat <= num)
					{
						target.Buffs.AddBuff(actions[i], entityAlive.entityId, true, false, -1f);
					}
				}
			}
		}
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsActionRunning(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x00129478 File Offset: 0x00127678
	public virtual bool CanExecute(global::ItemActionData _actionData)
	{
		if (this.ExecutionRequirements == null)
		{
			return true;
		}
		_actionData.invData.holdingEntity.MinEventContext.ItemValue = _actionData.invData.itemValue;
		return this.ExecutionRequirements.IsValid(_actionData.invData.holdingEntity.MinEventContext);
	}

	// Token: 0x06002F0B RID: 12043
	public abstract void ExecuteAction(global::ItemActionData _actionData, bool _bReleased);

	// Token: 0x06002F0C RID: 12044 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ExecuteInstantAction(EntityAlive ent, ItemStack stack, bool isHeldItem, XUiC_ItemStack stackController)
	{
		return false;
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CancelAction(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual WorldRayHitInfo GetExecuteActionTarget(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x001294CF File Offset: 0x001276CF
	public virtual void GetIronSights(global::ItemActionData _actionData, out float _fov)
	{
		_fov = 0f;
	}

	// Token: 0x06002F10 RID: 12048 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual EnumCameraShake GetCameraShakeType(global::ItemActionData _actionData)
	{
		return EnumCameraShake.None;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x001294D8 File Offset: 0x001276D8
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x001294D8 File Offset: 0x001276D8
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool AllowItemLoopingSound(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsAimingGunPossible(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AimingSet(global::ItemActionData _actionData, bool _isAiming, bool _wasAiming)
	{
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ItemActionEffects(GameManager _gameManager, global::ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateNozzleParticlesPosAndRot(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetInitialMeta(ItemValue _itemValue)
	{
		return 0;
	}

	// Token: 0x06002F19 RID: 12057 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SwapAmmoType(EntityAlive _entity, int _ammoItemId = -1)
	{
	}

	// Token: 0x06002F1A RID: 12058 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool canShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002F1B RID: 12059 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool isShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x001294DF File Offset: 0x001276DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void getOverlayData(global::ItemActionData actionData, out float _perc, out string _text)
	{
		_perc = 0f;
		_text = "";
	}

	// Token: 0x06002F1D RID: 12061 RVA: 0x001294F0 File Offset: 0x001276F0
	public static float GetDismemberChance(global::ItemActionData _actionData, WorldRayHitInfo hitInfo)
	{
		FastTags<TagGroup.Global> fastTags = _actionData.ActionTags;
		if (hitInfo.tag == "E_BP_Head")
		{
			fastTags |= ItemAction.headTag;
		}
		else if (hitInfo.tag.ContainsCaseInsensitive("arm"))
		{
			fastTags |= ItemAction.armTag;
		}
		else if (hitInfo.tag.ContainsCaseInsensitive("leg"))
		{
			fastTags |= ItemAction.legTag;
		}
		return EffectManager.GetValue(PassiveEffects.DismemberChance, _actionData.invData.holdingEntity.inventory.holdingItemItemValue, 0f, _actionData.invData.holdingEntity, null, fastTags | _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false);
	}

	// Token: 0x06002F1E RID: 12062 RVA: 0x001295B0 File Offset: 0x001277B0
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual List<string> getBuffActions(global::ItemActionData _actionData)
	{
		if (this.BuffActions == null)
		{
			return new List<string>();
		}
		return this.BuffActions;
	}

	// Token: 0x06002F1F RID: 12063 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
	}

	// Token: 0x06002F20 RID: 12064 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEditingTool()
	{
		return false;
	}

	// Token: 0x06002F21 RID: 12065 RVA: 0x0004E500 File Offset: 0x0004C700
	public virtual string GetStat(global::ItemActionData _data)
	{
		return string.Empty;
	}

	// Token: 0x06002F22 RID: 12066 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsStatChanged()
	{
		return false;
	}

	// Token: 0x06002F23 RID: 12067 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool HasRadial()
	{
		return false;
	}

	// Token: 0x06002F24 RID: 12068 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
	}

	// Token: 0x06002F25 RID: 12069 RVA: 0x001295C6 File Offset: 0x001277C6
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string StringFormatHandler(string title, object value)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1}[-]\n", title, value);
	}

	// Token: 0x06002F26 RID: 12070 RVA: 0x001295D4 File Offset: 0x001277D4
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string BuffActionStrings(ItemAction itemAction, List<string> stringList)
	{
		if (itemAction.BuffActions == null || itemAction.BuffActions.Count == 0)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < itemAction.BuffActions.Count; i++)
		{
			BuffClass buff = BuffManager.GetBuff(itemAction.BuffActions[i]);
			if (buff != null && !string.IsNullOrEmpty(buff.Name))
			{
				stringList.Add(ItemAction.StringFormatHandler(Localization.Get("lblEffect", false, null), string.Format("{0}", buff.Name)));
			}
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06002F27 RID: 12071 RVA: 0x00129667 File Offset: 0x00127867
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string getColoredItemStat(string _title, float _value)
	{
		if (_value > 0f)
		{
			return string.Format("{0}: [00ff00]+{1}[-]", _title, _value.ToCultureInvariantString());
		}
		if (_value < 0f)
		{
			return string.Format("{0}: [ff0000]{1}[-]", _title, _value.ToCultureInvariantString());
		}
		return "";
	}

	// Token: 0x06002F28 RID: 12072 RVA: 0x001296A4 File Offset: 0x001278A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string getColoredItemStatPercentage(string _title, float _value)
	{
		if (_value > 0f)
		{
			return string.Format("{0}: [00ff00]+{1}%[-]", _title, _value.ToCultureInvariantString("0.0"));
		}
		if (_value < 0f)
		{
			return string.Format("{0}: [ff0000]{1}%[-]", _title, _value.ToCultureInvariantString("0.0"));
		}
		return "";
	}

	// Token: 0x06002F29 RID: 12073 RVA: 0x001296F4 File Offset: 0x001278F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleItemBreak(global::ItemActionData _actionData)
	{
		if (_actionData.invData.itemValue.MaxUseTimes > 0 && _actionData.invData.itemValue.UseTimes >= (float)_actionData.invData.itemValue.MaxUseTimes)
		{
			Manager.BroadcastPlay(_actionData.invData.holdingEntity, "itembreak", false, 1f);
		}
	}

	// Token: 0x06002F2A RID: 12074 RVA: 0x00129754 File Offset: 0x00127954
	public static void HandleDegradation(ItemValue _outItem, EntityPlayerLocal player = null)
	{
		if (ItemAction.ItemMaxDegrationAmount == 0f)
		{
			return;
		}
		float num;
		if (!_outItem.TryGetMetadata("DurabilityModifier", out num))
		{
			num = 1f;
			_outItem.SetMetadata("DurabilityModifier", num);
			if (player)
			{
				GameManager.ShowTooltip(player, Localization.Get("ttDegradationRepair", false, null), false, false, 0f);
			}
			return;
		}
		num -= ItemAction.ItemMaxDegrationAmount;
		num = Mathf.Max(num, 0.05f);
		_outItem.SetMetadata("DurabilityModifier", num);
	}

	// Token: 0x06002F2B RID: 12075 RVA: 0x001297D4 File Offset: 0x001279D4
	public void HandleJamSound(ItemValue itemValue, ItemClass itemClass, EntityPlayerLocal localPlayer)
	{
		ItemAction.CanRepairValues canRepairValues = ItemAction.CanRepair(itemValue);
		if (canRepairValues == ItemAction.CanRepairValues.Allowed)
		{
			if (itemClass.Properties.Values.ContainsKey(ItemClass.PropSoundJammed))
			{
				Manager.PlayInsidePlayerHead(itemClass.Properties.Values[ItemClass.PropSoundJammed], -1, 0f, false, false);
			}
			GameManager.ShowTooltip(localPlayer, "ttItemNeedsRepair", false, false, 0f);
			return;
		}
		if (canRepairValues == ItemAction.CanRepairValues.NoLongerRepairable)
		{
			if (itemClass.Properties.Values.ContainsKey(ItemClass.PropSoundJammed))
			{
				Manager.PlayInsidePlayerHead(itemClass.Properties.Values[ItemClass.PropSoundJammed], -1, 0f, false, false);
			}
			GameManager.ShowTooltip(localPlayer, "ttItemNoLongerRepairable", false, false, 0f);
		}
	}

	// Token: 0x06002F2C RID: 12076 RVA: 0x00129888 File Offset: 0x00127A88
	public static ItemAction.CanRepairValues CanRepair(ItemValue _itemToBeRepaired)
	{
		if (ItemAction.ItemMaxDegrationAmount == 0f)
		{
			return ItemAction.CanRepairValues.Allowed;
		}
		float num;
		if (!_itemToBeRepaired.TryGetMetadata("DurabilityModifier", out num))
		{
			return ItemAction.CanRepairValues.Allowed;
		}
		float num2 = num - ItemAction.ItemMaxDegrationAmount;
		if (num2 < ItemAction.ItemMaxDegrationAmount)
		{
			return ItemAction.CanRepairValues.NoLongerRepairable;
		}
		if (num2 * (float)_itemToBeRepaired.MaxUseTimes < (float)_itemToBeRepaired.MaxUseTimes - _itemToBeRepaired.UseTimes)
		{
			return ItemAction.CanRepairValues.DurabilityWouldBeReduced;
		}
		return ItemAction.CanRepairValues.Allowed;
	}

	// Token: 0x06002F2D RID: 12077 RVA: 0x001298E2 File Offset: 0x00127AE2
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemAction()
	{
	}

	// Token: 0x0400241E RID: 9246
	public static FastTags<TagGroup.Global> headTag = FastTags<TagGroup.Global>.Parse("head");

	// Token: 0x0400241F RID: 9247
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> armTag = FastTags<TagGroup.Global>.Parse("arm");

	// Token: 0x04002420 RID: 9248
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> legTag = FastTags<TagGroup.Global>.Parse("leg");

	// Token: 0x04002421 RID: 9249
	public static bool ShowDebugDisplayHit;

	// Token: 0x04002422 RID: 9250
	public static float DebugDisplayHitSize = 0.005f;

	// Token: 0x04002423 RID: 9251
	public static float DebugDisplayHitTime = 10f;

	// Token: 0x04002424 RID: 9252
	public static bool ShowDistanceDebugInfo = false;

	// Token: 0x04002425 RID: 9253
	public ItemClass item;

	// Token: 0x04002426 RID: 9254
	public List<string> BuffActions;

	// Token: 0x04002427 RID: 9255
	public new float Delay;

	// Token: 0x04002428 RID: 9256
	public new float Range;

	// Token: 0x04002429 RID: 9257
	public float SphereRadius;

	// Token: 0x0400242A RID: 9258
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x0400242B RID: 9259
	public RequirementGroup ExecutionRequirements;

	// Token: 0x0400242C RID: 9260
	public bool UseAnimation = true;

	// Token: 0x0400242D RID: 9261
	public int ActionIndex;

	// Token: 0x0400242E RID: 9262
	public static float ItemDegradationModifier = 1f;

	// Token: 0x0400242F RID: 9263
	public static float ItemMaxDegrationAmount = 0f;

	// Token: 0x04002430 RID: 9264
	public static ItemAction.RepairTypes RepairType = ItemAction.RepairTypes.Both;

	// Token: 0x04002431 RID: 9265
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundStart;

	// Token: 0x04002432 RID: 9266
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Sound_in_head;

	// Token: 0x04002433 RID: 9267
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseParticleHarvesting;

	// Token: 0x04002434 RID: 9268
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particleHarvestingCategory;

	// Token: 0x04002435 RID: 9269
	public int ActionExp;

	// Token: 0x04002436 RID: 9270
	public float ActionExpBonusMultiplier;

	// Token: 0x02000595 RID: 1429
	public enum RepairTypes
	{
		// Token: 0x04002438 RID: 9272
		None,
		// Token: 0x04002439 RID: 9273
		RepairOnly,
		// Token: 0x0400243A RID: 9274
		CombineOnly,
		// Token: 0x0400243B RID: 9275
		Both
	}

	// Token: 0x02000596 RID: 1430
	public enum CanRepairValues
	{
		// Token: 0x0400243D RID: 9277
		Allowed,
		// Token: 0x0400243E RID: 9278
		NoLongerRepairable,
		// Token: 0x0400243F RID: 9279
		DurabilityWouldBeReduced
	}
}
