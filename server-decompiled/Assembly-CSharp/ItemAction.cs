using System;
using System.Collections.Generic;
using System.Text;
using Audio;
using UnityEngine;
using XMLData.Item;

// Token: 0x0200058D RID: 1421
public abstract class ItemAction : XMLData.Item.ItemActionData
{
	// Token: 0x06002EE3 RID: 12003 RVA: 0x001285A9 File Offset: 0x001267A9
	public string GetSoundStart()
	{
		return this.soundStart;
	}

	// Token: 0x06002EE4 RID: 12004 RVA: 0x001285B4 File Offset: 0x001267B4
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

	// Token: 0x06002EE5 RID: 12005 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEndDelayed()
	{
		return false;
	}

	// Token: 0x06002EE6 RID: 12006 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHUD(global::ItemActionData _actionData, int _x, int _y)
	{
	}

	// Token: 0x06002EE7 RID: 12007 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnScreenOverlay(global::ItemActionData _data)
	{
	}

	// Token: 0x06002EE8 RID: 12008 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeScrollWheel(global::ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06002EE9 RID: 12009 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeCameraFunction(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002EEA RID: 12010 RVA: 0x001285E9 File Offset: 0x001267E9
	public virtual global::ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new global::ItemActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002EEB RID: 12011 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual RenderCubeType GetFocusType(global::ItemActionData _actionData)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06002EEC RID: 12012 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsFocusBlockInside()
	{
		return true;
	}

	// Token: 0x06002EED RID: 12013 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsHUDDisabled(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002EEE RID: 12014 RVA: 0x001285F2 File Offset: 0x001267F2
	public virtual void StartHolding(global::ItemActionData _data)
	{
		this.OnModificationsChanged(_data);
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x001285FB File Offset: 0x001267FB
	public virtual void StopHolding(global::ItemActionData _data)
	{
		_data.invData.holdingEntity.StopAnimatorAudio(Entity.StopAnimatorAudioType.StopOnStopHolding);
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingUpdate(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ShowItem(global::ItemActionData _actionData, bool _show)
	{
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnModificationsChanged(global::ItemActionData _data)
	{
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CanCancel(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002EF4 RID: 12020 RVA: 0x00128610 File Offset: 0x00126810
	public virtual void CancelReload(global::ItemActionData _actionData, bool holsterWeapon)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && holsterWeapon)
		{
			entityPlayerLocal.HolsterWeapon(true);
		}
	}

	// Token: 0x06002EF5 RID: 12021 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x06002EF6 RID: 12022 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup(global::ItemActionData _data)
	{
	}

	// Token: 0x06002EF7 RID: 12023 RVA: 0x00128644 File Offset: 0x00126844
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

	// Token: 0x06002EF8 RID: 12024 RVA: 0x001287BD File Offset: 0x001269BD
	public string GetDescription()
	{
		return this.Properties.GetString("Description");
	}

	// Token: 0x06002EF9 RID: 12025 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual string CanInteract(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002EFA RID: 12026 RVA: 0x001287D0 File Offset: 0x001269D0
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

	// Token: 0x06002EFB RID: 12027 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsActionRunning(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002EFC RID: 12028 RVA: 0x00128884 File Offset: 0x00126A84
	public virtual bool CanExecute(global::ItemActionData _actionData)
	{
		if (this.ExecutionRequirements == null)
		{
			return true;
		}
		_actionData.invData.holdingEntity.MinEventContext.ItemValue = _actionData.invData.itemValue;
		return this.ExecutionRequirements.IsValid(_actionData.invData.holdingEntity.MinEventContext);
	}

	// Token: 0x06002EFD RID: 12029
	public abstract void ExecuteAction(global::ItemActionData _actionData, bool _bReleased);

	// Token: 0x06002EFE RID: 12030 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ExecuteInstantAction(EntityAlive ent, ItemStack stack, bool isHeldItem, XUiC_ItemStack stackController)
	{
		return false;
	}

	// Token: 0x06002EFF RID: 12031 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CancelAction(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002F00 RID: 12032 RVA: 0x000200DA File Offset: 0x0001E2DA
	public virtual WorldRayHitInfo GetExecuteActionTarget(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002F01 RID: 12033 RVA: 0x001288DB File Offset: 0x00126ADB
	public virtual void GetIronSights(global::ItemActionData _actionData, out float _fov)
	{
		_fov = 0f;
	}

	// Token: 0x06002F02 RID: 12034 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual EnumCameraShake GetCameraShakeType(global::ItemActionData _actionData)
	{
		return EnumCameraShake.None;
	}

	// Token: 0x06002F03 RID: 12035 RVA: 0x001288E4 File Offset: 0x00126AE4
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002F04 RID: 12036 RVA: 0x001288E4 File Offset: 0x00126AE4
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002F05 RID: 12037 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool AllowItemLoopingSound(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002F06 RID: 12038 RVA: 0x00020119 File Offset: 0x0001E319
	public virtual bool IsAimingGunPossible(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002F07 RID: 12039 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AimingSet(global::ItemActionData _actionData, bool _isAiming, bool _wasAiming)
	{
	}

	// Token: 0x06002F08 RID: 12040 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ItemActionEffects(GameManager _gameManager, global::ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
	}

	// Token: 0x06002F09 RID: 12041 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateNozzleParticlesPosAndRot(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002F0A RID: 12042 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetInitialMeta(ItemValue _itemValue)
	{
		return 0;
	}

	// Token: 0x06002F0B RID: 12043 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SwapAmmoType(EntityAlive _entity, int _ammoItemId = -1)
	{
	}

	// Token: 0x06002F0C RID: 12044 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool canShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002F0D RID: 12045 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool isShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002F0E RID: 12046 RVA: 0x001288EB File Offset: 0x00126AEB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void getOverlayData(global::ItemActionData actionData, out float _perc, out string _text)
	{
		_perc = 0f;
		_text = "";
	}

	// Token: 0x06002F0F RID: 12047 RVA: 0x001288FC File Offset: 0x00126AFC
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

	// Token: 0x06002F10 RID: 12048 RVA: 0x001289BC File Offset: 0x00126BBC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual List<string> getBuffActions(global::ItemActionData _actionData)
	{
		if (this.BuffActions == null)
		{
			return new List<string>();
		}
		return this.BuffActions;
	}

	// Token: 0x06002F11 RID: 12049 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
	}

	// Token: 0x06002F12 RID: 12050 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEditingTool()
	{
		return false;
	}

	// Token: 0x06002F13 RID: 12051 RVA: 0x0004E540 File Offset: 0x0004C740
	public virtual string GetStat(global::ItemActionData _data)
	{
		return string.Empty;
	}

	// Token: 0x06002F14 RID: 12052 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsStatChanged()
	{
		return false;
	}

	// Token: 0x06002F15 RID: 12053 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool HasRadial()
	{
		return false;
	}

	// Token: 0x06002F16 RID: 12054 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
	}

	// Token: 0x06002F17 RID: 12055 RVA: 0x001289D2 File Offset: 0x00126BD2
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string StringFormatHandler(string title, object value)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1}[-]\n", title, value);
	}

	// Token: 0x06002F18 RID: 12056 RVA: 0x001289E0 File Offset: 0x00126BE0
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

	// Token: 0x06002F19 RID: 12057 RVA: 0x00128A73 File Offset: 0x00126C73
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

	// Token: 0x06002F1A RID: 12058 RVA: 0x00128AB0 File Offset: 0x00126CB0
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

	// Token: 0x06002F1B RID: 12059 RVA: 0x00128B00 File Offset: 0x00126D00
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleItemBreak(global::ItemActionData _actionData)
	{
		if (_actionData.invData.itemValue.MaxUseTimes > 0 && _actionData.invData.itemValue.UseTimes >= (float)_actionData.invData.itemValue.MaxUseTimes)
		{
			Manager.BroadcastPlay(_actionData.invData.holdingEntity, "itembreak", false, 1f);
		}
	}

	// Token: 0x06002F1C RID: 12060 RVA: 0x00128B60 File Offset: 0x00126D60
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

	// Token: 0x06002F1D RID: 12061 RVA: 0x00128BE0 File Offset: 0x00126DE0
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

	// Token: 0x06002F1E RID: 12062 RVA: 0x00128C94 File Offset: 0x00126E94
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

	// Token: 0x06002F1F RID: 12063 RVA: 0x00128CEE File Offset: 0x00126EEE
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemAction()
	{
	}

	// Token: 0x04002403 RID: 9219
	public static FastTags<TagGroup.Global> headTag = FastTags<TagGroup.Global>.Parse("head");

	// Token: 0x04002404 RID: 9220
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> armTag = FastTags<TagGroup.Global>.Parse("arm");

	// Token: 0x04002405 RID: 9221
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> legTag = FastTags<TagGroup.Global>.Parse("leg");

	// Token: 0x04002406 RID: 9222
	public static bool ShowDebugDisplayHit;

	// Token: 0x04002407 RID: 9223
	public static float DebugDisplayHitSize = 0.005f;

	// Token: 0x04002408 RID: 9224
	public static float DebugDisplayHitTime = 10f;

	// Token: 0x04002409 RID: 9225
	public static bool ShowDistanceDebugInfo = false;

	// Token: 0x0400240A RID: 9226
	public ItemClass item;

	// Token: 0x0400240B RID: 9227
	public List<string> BuffActions;

	// Token: 0x0400240C RID: 9228
	public new float Delay;

	// Token: 0x0400240D RID: 9229
	public new float Range;

	// Token: 0x0400240E RID: 9230
	public float SphereRadius;

	// Token: 0x0400240F RID: 9231
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04002410 RID: 9232
	public RequirementGroup ExecutionRequirements;

	// Token: 0x04002411 RID: 9233
	public bool UseAnimation = true;

	// Token: 0x04002412 RID: 9234
	public int ActionIndex;

	// Token: 0x04002413 RID: 9235
	public static float ItemDegradationModifier = 1f;

	// Token: 0x04002414 RID: 9236
	public static float ItemMaxDegrationAmount = 0f;

	// Token: 0x04002415 RID: 9237
	public static ItemAction.RepairTypes RepairType = ItemAction.RepairTypes.Both;

	// Token: 0x04002416 RID: 9238
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundStart;

	// Token: 0x04002417 RID: 9239
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Sound_in_head;

	// Token: 0x04002418 RID: 9240
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseParticleHarvesting;

	// Token: 0x04002419 RID: 9241
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particleHarvestingCategory;

	// Token: 0x0400241A RID: 9242
	public int ActionExp;

	// Token: 0x0400241B RID: 9243
	public float ActionExpBonusMultiplier;

	// Token: 0x0200058E RID: 1422
	public enum RepairTypes
	{
		// Token: 0x0400241D RID: 9245
		None,
		// Token: 0x0400241E RID: 9246
		RepairOnly,
		// Token: 0x0400241F RID: 9247
		CombineOnly,
		// Token: 0x04002420 RID: 9248
		Both
	}

	// Token: 0x0200058F RID: 1423
	public enum CanRepairValues
	{
		// Token: 0x04002422 RID: 9250
		Allowed,
		// Token: 0x04002423 RID: 9251
		NoLongerRepairable,
		// Token: 0x04002424 RID: 9252
		DurabilityWouldBeReduced
	}
}
