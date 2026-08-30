using System;
using System.Collections.Generic;
using System.Text;
using Audio;
using UnityEngine;
using XMLData.Item;

// Token: 0x02000587 RID: 1415
public abstract class ItemAction : XMLData.Item.ItemActionData
{
	// Token: 0x06002E73 RID: 11891 RVA: 0x0012699A File Offset: 0x00124B9A
	public string GetSoundStart()
	{
		return this.soundStart;
	}

	// Token: 0x06002E74 RID: 11892 RVA: 0x001269A4 File Offset: 0x00124BA4
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

	// Token: 0x06002E75 RID: 11893 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEndDelayed()
	{
		return false;
	}

	// Token: 0x06002E76 RID: 11894 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHUD(global::ItemActionData _actionData, int _x, int _y)
	{
	}

	// Token: 0x06002E77 RID: 11895 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnScreenOverlay(global::ItemActionData _data)
	{
	}

	// Token: 0x06002E78 RID: 11896 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeScrollWheel(global::ItemActionData _actionData, float _scrollWheelInput, PlayerActionsLocal _playerInput)
	{
		return false;
	}

	// Token: 0x06002E79 RID: 11897 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ConsumeCameraFunction(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002E7A RID: 11898 RVA: 0x001269D9 File Offset: 0x00124BD9
	public virtual global::ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new global::ItemActionData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002E7B RID: 11899 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual RenderCubeType GetFocusType(global::ItemActionData _actionData)
	{
		return RenderCubeType.None;
	}

	// Token: 0x06002E7C RID: 11900 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsFocusBlockInside()
	{
		return true;
	}

	// Token: 0x06002E7D RID: 11901 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsHUDDisabled(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002E7E RID: 11902 RVA: 0x001269E2 File Offset: 0x00124BE2
	public virtual void StartHolding(global::ItemActionData _data)
	{
		this.OnModificationsChanged(_data);
	}

	// Token: 0x06002E7F RID: 11903 RVA: 0x001269EB File Offset: 0x00124BEB
	public virtual void StopHolding(global::ItemActionData _data)
	{
		_data.invData.holdingEntity.StopAnimatorAudio(Entity.StopAnimatorAudioType.StopOnStopHolding);
	}

	// Token: 0x06002E80 RID: 11904 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnHoldingUpdate(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002E81 RID: 11905 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ShowItem(global::ItemActionData _actionData, bool _show)
	{
	}

	// Token: 0x06002E82 RID: 11906 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void OnModificationsChanged(global::ItemActionData _data)
	{
	}

	// Token: 0x06002E83 RID: 11907 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool CanCancel(global::ItemActionData _data)
	{
		return false;
	}

	// Token: 0x06002E84 RID: 11908 RVA: 0x00126A00 File Offset: 0x00124C00
	public virtual void CancelReload(global::ItemActionData _actionData, bool holsterWeapon)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		if (entityPlayerLocal != null && holsterWeapon)
		{
			entityPlayerLocal.HolsterWeapon(true);
		}
	}

	// Token: 0x06002E85 RID: 11909 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool AllowConcurrentActions()
	{
		return false;
	}

	// Token: 0x06002E86 RID: 11910 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Cleanup(global::ItemActionData _data)
	{
	}

	// Token: 0x06002E87 RID: 11911 RVA: 0x00126A34 File Offset: 0x00124C34
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

	// Token: 0x06002E88 RID: 11912 RVA: 0x00126BAD File Offset: 0x00124DAD
	public string GetDescription()
	{
		return this.Properties.GetString("Description");
	}

	// Token: 0x06002E89 RID: 11913 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual string CanInteract(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002E8A RID: 11914 RVA: 0x00126BC0 File Offset: 0x00124DC0
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

	// Token: 0x06002E8B RID: 11915 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsActionRunning(global::ItemActionData _actionData)
	{
		return false;
	}

	// Token: 0x06002E8C RID: 11916 RVA: 0x00126C74 File Offset: 0x00124E74
	public virtual bool CanExecute(global::ItemActionData _actionData)
	{
		if (this.ExecutionRequirements == null)
		{
			return true;
		}
		_actionData.invData.holdingEntity.MinEventContext.ItemValue = _actionData.invData.itemValue;
		return this.ExecutionRequirements.IsValid(_actionData.invData.holdingEntity.MinEventContext);
	}

	// Token: 0x06002E8D RID: 11917
	public abstract void ExecuteAction(global::ItemActionData _actionData, bool _bReleased);

	// Token: 0x06002E8E RID: 11918 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool ExecuteInstantAction(EntityAlive ent, ItemStack stack, bool isHeldItem, XUiC_ItemStack stackController)
	{
		return false;
	}

	// Token: 0x06002E8F RID: 11919 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void CancelAction(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002E90 RID: 11920 RVA: 0x0001FF1E File Offset: 0x0001E11E
	public virtual WorldRayHitInfo GetExecuteActionTarget(global::ItemActionData _actionData)
	{
		return null;
	}

	// Token: 0x06002E91 RID: 11921 RVA: 0x00126CCB File Offset: 0x00124ECB
	public virtual void GetIronSights(global::ItemActionData _actionData, out float _fov)
	{
		_fov = 0f;
	}

	// Token: 0x06002E92 RID: 11922 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual EnumCameraShake GetCameraShakeType(global::ItemActionData _actionData)
	{
		return EnumCameraShake.None;
	}

	// Token: 0x06002E93 RID: 11923 RVA: 0x00126CD4 File Offset: 0x00124ED4
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectPull()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002E94 RID: 11924 RVA: 0x00126CD4 File Offset: 0x00124ED4
	public virtual TriggerEffectManager.ControllerTriggerEffect GetControllerTriggerEffectShoot()
	{
		return TriggerEffectManager.NoneEffect;
	}

	// Token: 0x06002E95 RID: 11925 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool AllowItemLoopingSound(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002E96 RID: 11926 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public virtual bool IsAimingGunPossible(global::ItemActionData _actionData)
	{
		return true;
	}

	// Token: 0x06002E97 RID: 11927 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void AimingSet(global::ItemActionData _actionData, bool _isAiming, bool _wasAiming)
	{
	}

	// Token: 0x06002E98 RID: 11928 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void ItemActionEffects(GameManager _gameManager, global::ItemActionData _actionData, int _firingState, Vector3 _startPos, Vector3 _direction, int _userData = 0)
	{
	}

	// Token: 0x06002E99 RID: 11929 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void UpdateNozzleParticlesPosAndRot(global::ItemActionData _actionData)
	{
	}

	// Token: 0x06002E9A RID: 11930 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual int GetInitialMeta(ItemValue _itemValue)
	{
		return 0;
	}

	// Token: 0x06002E9B RID: 11931 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SwapAmmoType(EntityAlive _entity, int _ammoItemId = -1)
	{
	}

	// Token: 0x06002E9C RID: 11932 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool canShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002E9D RID: 11933 RVA: 0x00010E62 File Offset: 0x0000F062
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual bool isShowOverlay(global::ItemActionData actionData)
	{
		return false;
	}

	// Token: 0x06002E9E RID: 11934 RVA: 0x00126CDB File Offset: 0x00124EDB
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void getOverlayData(global::ItemActionData actionData, out float _perc, out string _text)
	{
		_perc = 0f;
		_text = "";
	}

	// Token: 0x06002E9F RID: 11935 RVA: 0x00126CEC File Offset: 0x00124EEC
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

	// Token: 0x06002EA0 RID: 11936 RVA: 0x00126DAC File Offset: 0x00124FAC
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual List<string> getBuffActions(global::ItemActionData _actionData)
	{
		if (this.BuffActions == null)
		{
			return new List<string>();
		}
		return this.BuffActions;
	}

	// Token: 0x06002EA1 RID: 11937 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
	}

	// Token: 0x06002EA2 RID: 11938 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsEditingTool()
	{
		return false;
	}

	// Token: 0x06002EA3 RID: 11939 RVA: 0x0004E124 File Offset: 0x0004C324
	public virtual string GetStat(global::ItemActionData _data)
	{
		return string.Empty;
	}

	// Token: 0x06002EA4 RID: 11940 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool IsStatChanged()
	{
		return false;
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x00010E62 File Offset: 0x0000F062
	public virtual bool HasRadial()
	{
		return false;
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void SetupRadial(XUiC_Radial _xuiRadialWindow, EntityPlayerLocal _epl)
	{
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x00126DC2 File Offset: 0x00124FC2
	[PublicizedFrom(EAccessModifier.Protected)]
	public static string StringFormatHandler(string title, object value)
	{
		return string.Format("{0}: [REPLACE_COLOR]{1}[-]\n", title, value);
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x00126DD0 File Offset: 0x00124FD0
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

	// Token: 0x06002EA9 RID: 11945 RVA: 0x00126E63 File Offset: 0x00125063
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

	// Token: 0x06002EAA RID: 11946 RVA: 0x00126EA0 File Offset: 0x001250A0
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

	// Token: 0x06002EAB RID: 11947 RVA: 0x00126EF0 File Offset: 0x001250F0
	[PublicizedFrom(EAccessModifier.Protected)]
	public void HandleItemBreak(global::ItemActionData _actionData)
	{
		if (_actionData.invData.itemValue.MaxUseTimes > 0 && _actionData.invData.itemValue.UseTimes >= (float)_actionData.invData.itemValue.MaxUseTimes)
		{
			Manager.BroadcastPlay(_actionData.invData.holdingEntity, "itembreak", false, 1f);
		}
	}

	// Token: 0x06002EAC RID: 11948 RVA: 0x00126F50 File Offset: 0x00125150
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

	// Token: 0x06002EAD RID: 11949 RVA: 0x00126FD0 File Offset: 0x001251D0
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

	// Token: 0x06002EAE RID: 11950 RVA: 0x00127084 File Offset: 0x00125284
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

	// Token: 0x06002EAF RID: 11951 RVA: 0x001270DE File Offset: 0x001252DE
	[PublicizedFrom(EAccessModifier.Protected)]
	public ItemAction()
	{
	}

	// Token: 0x040023D6 RID: 9174
	public static FastTags<TagGroup.Global> headTag = FastTags<TagGroup.Global>.Parse("head");

	// Token: 0x040023D7 RID: 9175
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> armTag = FastTags<TagGroup.Global>.Parse("arm");

	// Token: 0x040023D8 RID: 9176
	[PublicizedFrom(EAccessModifier.Private)]
	public static FastTags<TagGroup.Global> legTag = FastTags<TagGroup.Global>.Parse("leg");

	// Token: 0x040023D9 RID: 9177
	public static bool ShowDebugDisplayHit;

	// Token: 0x040023DA RID: 9178
	public static float DebugDisplayHitSize = 0.005f;

	// Token: 0x040023DB RID: 9179
	public static float DebugDisplayHitTime = 10f;

	// Token: 0x040023DC RID: 9180
	public static bool ShowDistanceDebugInfo = false;

	// Token: 0x040023DD RID: 9181
	public ItemClass item;

	// Token: 0x040023DE RID: 9182
	public List<string> BuffActions;

	// Token: 0x040023DF RID: 9183
	public new float Delay;

	// Token: 0x040023E0 RID: 9184
	public new float Range;

	// Token: 0x040023E1 RID: 9185
	public float SphereRadius;

	// Token: 0x040023E2 RID: 9186
	public DynamicProperties Properties = new DynamicProperties();

	// Token: 0x040023E3 RID: 9187
	public RequirementGroup ExecutionRequirements;

	// Token: 0x040023E4 RID: 9188
	public bool UseAnimation = true;

	// Token: 0x040023E5 RID: 9189
	public int ActionIndex;

	// Token: 0x040023E6 RID: 9190
	public static float ItemDegradationModifier = 1f;

	// Token: 0x040023E7 RID: 9191
	public static float ItemMaxDegrationAmount = 0f;

	// Token: 0x040023E8 RID: 9192
	public static ItemAction.RepairTypes RepairType = ItemAction.RepairTypes.Both;

	// Token: 0x040023E9 RID: 9193
	[PublicizedFrom(EAccessModifier.Protected)]
	public string soundStart;

	// Token: 0x040023EA RID: 9194
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool Sound_in_head;

	// Token: 0x040023EB RID: 9195
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool bUseParticleHarvesting;

	// Token: 0x040023EC RID: 9196
	[PublicizedFrom(EAccessModifier.Protected)]
	public string particleHarvestingCategory;

	// Token: 0x040023ED RID: 9197
	public int ActionExp;

	// Token: 0x040023EE RID: 9198
	public float ActionExpBonusMultiplier;

	// Token: 0x02000588 RID: 1416
	public enum RepairTypes
	{
		// Token: 0x040023F0 RID: 9200
		None,
		// Token: 0x040023F1 RID: 9201
		RepairOnly,
		// Token: 0x040023F2 RID: 9202
		CombineOnly,
		// Token: 0x040023F3 RID: 9203
		Both
	}

	// Token: 0x02000589 RID: 1417
	public enum CanRepairValues
	{
		// Token: 0x040023F5 RID: 9205
		Allowed,
		// Token: 0x040023F6 RID: 9206
		NoLongerRepairable,
		// Token: 0x040023F7 RID: 9207
		DurabilityWouldBeReduced
	}
}
