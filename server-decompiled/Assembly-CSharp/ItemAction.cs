using System;
using System.Collections.Generic;
using System.Text;
using Audio;
using UnityEngine;
using XMLData.Item;

// Token: 0x0200058C RID: 1420
public abstract class ItemAction : XMLData.Item.ItemActionData
{
	// Token: 0x06002EA1 RID: 11937 RVA: 0x00127A8A File Offset: 0x00125C8A
	public string GetSoundStart()
	{
		return this.soundStart;
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x00127A94 File Offset: 0x00125C94
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

	// Token: 0x06002EA3 RID: 11939 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEndDelayed()
	{
		return false;
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHUD(global::ItemActionData _actionData, int _x, int _y)
	{
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnScreenOverlay(global::ItemActionData _data)
	{
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeScrollWheel(global::ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeCameraFunction(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x00127AC9 File Offset: 0x00125CC9
	public virtual global::ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new global::ItemActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002EA9 RID: 11945 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual RenderCubeType GetFocusType(global::ItemActionData _actionData)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06002EAA RID: 11946 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool IsFocusBlockInside()
	{
		return true;
	}

	// Token: 0x06002EAB RID: 11947 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsHUDDisabled(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002EAC RID: 11948 RVA: 0x00127AD2 File Offset: 0x00125CD2
	public virtual void StartHolding(global::ItemActionData _data)
	{
		this.OnModificationsChanged(_data);
	}

	// Token: 0x06002EAD RID: 11949 RVA: 0x00127ADB File Offset: 0x00125CDB
	public virtual void StopHolding(global::ItemActionData _data)
	{
		_data.invData.holdingEntity.StopAnimatorAudio(Entity.StopAnimatorAudioType.StopOnStopHolding);
	}

	// Token: 0x06002EAE RID: 11950 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingUpdate(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002EAF RID: 11951 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ShowItem(global::ItemActionData _actionData, bool _show)
	{
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnModificationsChanged(global::ItemActionData _data)
	{
	}

	// Token: 0x06002EB1 RID: 11953 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CanCancel(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002EB2 RID: 11954 RVA: 0x00127AF0 File Offset: 0x00125CF0
	public virtual void CancelReload(global::ItemActionData _actionData, bool holsterWeapon)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && holsterWeapon)
		{
			entityPlayerLocal.HolsterWeapon(true);
		}
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup(global::ItemActionData _data)
	{
	}

	// Token: 0x06002EB5 RID: 11957 RVA: 0x00127B24 File Offset: 0x00125D24
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

	// Token: 0x06002EB6 RID: 11958 RVA: 0x00127C9D File Offset: 0x00125E9D
	public string GetDescription()
	{
		return this.Properties.GetString("Description");
	}

	// Token: 0x06002EB7 RID: 11959 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public virtual string CanInteract(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002EB8 RID: 11960 RVA: 0x00127CB0 File Offset: 0x00125EB0
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

	// Token: 0x06002EB9 RID: 11961 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsActionRunning(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002EBA RID: 11962 RVA: 0x00127D64 File Offset: 0x00125F64
	public virtual bool CanExecute(global::ItemActionData _actionData)
	{
		if (this.ExecutionRequirements == null)
		{
			return true;
		}
		_actionData.invData.holdingEntity.MinEventContext.ItemValue = _actionData.invData.itemValue;
		return this.ExecutionRequirements.IsValid(_actionData.invData.holdingEntity.MinEventContext);
	}

	// Token: 0x06002EBB RID: 11963
	public abstract void ExecuteAction(global::ItemActionData _actionData, bool _bReleased);

	// Token: 0x06002EBC RID: 11964 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ExecuteInstantAction(EntityAlive ent, ItemStack stack, bool isHeldItem, XUiC_ItemStack stackController)
	{
		return false;
	}

	// Token: 0x06002EBD RID: 11965 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CancelAction(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002EBE RID: 11966 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
	public virtual WorldRayHitInfo GetExecuteActionTarget(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002EBF RID: 11967 RVA: 0x00127DBB File Offset: 0x00125FBB
	public virtual void GetIronSights(global::ItemActionData _actionData, out float _fov)
	{
		_fov = 0f;
	}

	// Token: 0x06002EC0 RID: 11968 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual EnumCameraShake GetCameraShakeType(global::ItemActionData _actionData)
	{
		return EnumCameraShake.None;
	}

	// Token: 0x06002EC1 RID: 11969 RVA: 0x00127DC4 File Offset: 0x00125FC4
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002EC2 RID: 11970 RVA: 0x00127DC4 File Offset: 0x00125FC4
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002EC3 RID: 11971 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool AllowItemLoopingSound(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002EC4 RID: 11972 RVA: 0x00020021 File Offset: 0x0001E221
	public virtual bool IsAimingGunPossible(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002EC5 RID: 11973 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AimingSet(global::ItemActionData _actionData, bool _isAiming, bool _wasAiming)
	{
	}

	// Token: 0x06002EC6 RID: 11974 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ItemActionEffects(GameManager _gameManager, global::ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
	}

	// Token: 0x06002EC7 RID: 11975 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateNozzleParticlesPosAndRot(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002EC8 RID: 11976 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetInitialMeta(ItemValue _itemValue)
	{
		return 0;
	}

	// Token: 0x06002EC9 RID: 11977 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SwapAmmoType(EntityAlive _entity, int _ammoItemId = -1)
	{
	}

	// Token: 0x06002ECA RID: 11978 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool canShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002ECB RID: 11979 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool isShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002ECC RID: 11980 RVA: 0x00127DCB File Offset: 0x00125FCB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void getOverlayData(global::ItemActionData actionData, out float _perc, out string _text)
	{
		_perc = 0f;
		_text = "";
	}

	// Token: 0x06002ECD RID: 11981 RVA: 0x00127DDC File Offset: 0x00125FDC
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

	// Token: 0x06002ECE RID: 11982 RVA: 0x00127E9C File Offset: 0x0012609C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual List<string> getBuffActions(global::ItemActionData _actionData)
	{
		if (this.BuffActions == null)
		{
			return new List<string>();
		}
		return this.BuffActions;
	}

	// Token: 0x06002ECF RID: 11983 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
	}

	// Token: 0x06002ED0 RID: 11984 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEditingTool()
	{
		return false;
	}

	// Token: 0x06002ED1 RID: 11985 RVA: 0x0004E53C File Offset: 0x0004C73C
	public virtual string GetStat(global::ItemActionData _data)
	{
		return string.Empty;
	}

	// Token: 0x06002ED2 RID: 11986 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsStatChanged()
	{
		return false;
	}

	// Token: 0x06002ED3 RID: 11987 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool HasRadial()
	{
		return false;
	}

	// Token: 0x06002ED4 RID: 11988 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x00127EB2 File Offset: 0x001260B2
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string StringFormatHandler(string title, object value)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1}[-]\n", title, value);
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x00127EC0 File Offset: 0x001260C0
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

	// Token: 0x06002ED7 RID: 11991 RVA: 0x00127F53 File Offset: 0x00126153
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

	// Token: 0x06002ED8 RID: 11992 RVA: 0x00127F90 File Offset: 0x00126190
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

	// Token: 0x06002ED9 RID: 11993 RVA: 0x00127FE0 File Offset: 0x001261E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleItemBreak(global::ItemActionData _actionData)
	{
		if (_actionData.invData.itemValue.MaxUseTimes > 0 && _actionData.invData.itemValue.UseTimes >= (float)_actionData.invData.itemValue.MaxUseTimes)
		{
			Manager.BroadcastPlay(_actionData.invData.holdingEntity, "itembreak", false, 1f);
		}
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x00128040 File Offset: 0x00126240
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

	// Token: 0x06002EDB RID: 11995 RVA: 0x001280C0 File Offset: 0x001262C0
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

	// Token: 0x06002EDC RID: 11996 RVA: 0x00128174 File Offset: 0x00126374
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

	// Token: 0x06002EDD RID: 11997 RVA: 0x001281CE File Offset: 0x001263CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemAction()
	{
	}

	// Token: 0x0400240A RID: 9226
	public static FastTags<TagGroup.Global> headTag = FastTags<TagGroup.Global>.Parse("head");

	// Token: 0x0400240B RID: 9227
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> armTag = FastTags<TagGroup.Global>.Parse("arm");

	// Token: 0x0400240C RID: 9228
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> legTag = FastTags<TagGroup.Global>.Parse("leg");

	// Token: 0x0400240D RID: 9229
	public static bool ShowDebugDisplayHit;

	// Token: 0x0400240E RID: 9230
	public static float DebugDisplayHitSize = 0.005f;

	// Token: 0x0400240F RID: 9231
	public static float DebugDisplayHitTime = 10f;

	// Token: 0x04002410 RID: 9232
	public static bool ShowDistanceDebugInfo = false;

	// Token: 0x04002411 RID: 9233
	public ItemClass item;

	// Token: 0x04002412 RID: 9234
	public List<string> BuffActions;

	// Token: 0x04002413 RID: 9235
	public new float Delay;

	// Token: 0x04002414 RID: 9236
	public new float Range;

	// Token: 0x04002415 RID: 9237
	public float SphereRadius;

	// Token: 0x04002416 RID: 9238
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x04002417 RID: 9239
	public RequirementGroup ExecutionRequirements;

	// Token: 0x04002418 RID: 9240
	public bool UseAnimation = true;

	// Token: 0x04002419 RID: 9241
	public int ActionIndex;

	// Token: 0x0400241A RID: 9242
	public static float ItemDegradationModifier = 1f;

	// Token: 0x0400241B RID: 9243
	public static float ItemMaxDegrationAmount = 0f;

	// Token: 0x0400241C RID: 9244
	public static ItemAction.RepairTypes RepairType = ItemAction.RepairTypes.Both;

	// Token: 0x0400241D RID: 9245
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundStart;

	// Token: 0x0400241E RID: 9246
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Sound_in_head;

	// Token: 0x0400241F RID: 9247
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseParticleHarvesting;

	// Token: 0x04002420 RID: 9248
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particleHarvestingCategory;

	// Token: 0x04002421 RID: 9249
	public int ActionExp;

	// Token: 0x04002422 RID: 9250
	public float ActionExpBonusMultiplier;

	// Token: 0x0200058D RID: 1421
	public enum RepairTypes
	{
		// Token: 0x04002424 RID: 9252
		None,
		// Token: 0x04002425 RID: 9253
		RepairOnly,
		// Token: 0x04002426 RID: 9254
		CombineOnly,
		// Token: 0x04002427 RID: 9255
		Both
	}

	// Token: 0x0200058E RID: 1422
	public enum CanRepairValues
	{
		// Token: 0x04002429 RID: 9257
		Allowed,
		// Token: 0x0400242A RID: 9258
		NoLongerRepairable,
		// Token: 0x0400242B RID: 9259
		DurabilityWouldBeReduced
	}
}
