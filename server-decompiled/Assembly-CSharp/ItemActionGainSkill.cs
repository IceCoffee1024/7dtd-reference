using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B2 RID: 1458
[Preserve]
public class ItemActionGainSkill : ItemAction
{
	// Token: 0x06002FDD RID: 12253 RVA: 0x00131257 File Offset: 0x0012F457
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionGainSkill.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FDE RID: 12254 RVA: 0x00131260 File Offset: 0x0012F460
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (!_props.Values.ContainsKey("Skills_to_gain"))
		{
			this.SkillsToGain = new string[0];
		}
		else
		{
			this.SkillsToGain = _props.Values["Skills_to_gain"].Split(',', StringSplitOptions.None);
		}
		if (!_props.Values.ContainsKey("Title"))
		{
			this.Title = "The title is impossible to read.";
		}
		else
		{
			this.Title = _props.Values["Title"];
		}
		if (!_props.Values.ContainsKey("Description"))
		{
			this.Description = "The description is impossible to read.";
			return;
		}
		this.Description = _props.Values["Description"];
	}

	// Token: 0x06002FDF RID: 12255 RVA: 0x0013131B File Offset: 0x0012F51B
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionGainSkill.MyInventoryData)_data).bReadingStarted = false;
	}

	// Token: 0x06002FE0 RID: 12256 RVA: 0x00131330 File Offset: 0x0012F530
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (!_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		_actionData.lastUseTime = Time.time;
		_actionData.invData.holdingEntity.RightArmAnimationUse = true;
		((ItemActionGainSkill.MyInventoryData)_actionData).bReadingStarted = true;
	}

	// Token: 0x06002FE1 RID: 12257 RVA: 0x00131380 File Offset: 0x0012F580
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionGainSkill.MyInventoryData myInventoryData = (ItemActionGainSkill.MyInventoryData)_actionData;
		return myInventoryData.bReadingStarted && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002FE2 RID: 12258 RVA: 0x001313D8 File Offset: 0x0012F5D8
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		ItemActionGainSkill.MyInventoryData myInventoryData = (ItemActionGainSkill.MyInventoryData)_actionData;
		if (!myInventoryData.bReadingStarted || Time.time - myInventoryData.lastUseTime < AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast)
		{
			return;
		}
		myInventoryData.bReadingStarted = false;
		bool flag = false;
		for (int i = 0; i < this.SkillsToGain.Length; i++)
		{
			if (myInventoryData.invData.holdingEntity is EntityPlayer)
			{
				EntityPlayerLocal entityPlayerLocal = myInventoryData.invData.holdingEntity as EntityPlayerLocal;
				ProgressionValue progressionValue = myInventoryData.invData.holdingEntity.Progression.GetProgressionValue(this.SkillsToGain[i]);
				if (progressionValue != null)
				{
					if (progressionValue.Level + 1 <= progressionValue.ProgressionClass.MaxLevel)
					{
						ProgressionValue progressionValue2 = progressionValue;
						int level = progressionValue2.Level;
						progressionValue2.Level = level + 1;
						entityPlayerLocal.MinEventContext.ProgressionValue = progressionValue;
						entityPlayerLocal.FireEvent(MinEventTypes.onPerkLevelChanged, true);
						string text = Localization.Get(progressionValue.ProgressionClass.NameKey, false, null);
						GameManager.ShowTooltip(entityPlayerLocal, string.Format(Localization.Get("ttSkillLevelUp", false, null), text, progressionValue.Level), false, false, 0f);
						(myInventoryData.invData.holdingEntity as EntityPlayer).bPlayerStatsChanged = true;
						flag = true;
					}
					else
					{
						GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("ttSkillMaxLevel", false, null), false, false, 0f);
					}
				}
			}
		}
		if (flag)
		{
			_actionData.invData.holdingEntity.inventory.DecHoldingItem(1);
			if (this.soundStart != null)
			{
				Manager.PlayInsidePlayerHead(this.soundStart, -1, 0f, false, false);
			}
		}
	}

	// Token: 0x06002FE3 RID: 12259 RVA: 0x00131584 File Offset: 0x0012F784
	public override void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
		for (int i = 0; i < this.SkillsToGain.Length; i++)
		{
			_infoList.Add(ItemAction.StringFormatHandler(Localization.Get(_xui.playerUI.entityPlayer.Progression.GetProgressionValue(this.SkillsToGain[i]).ProgressionClass.NameKey, false, null), "+1"));
		}
	}

	// Token: 0x040024E2 RID: 9442
	public string[] SkillsToGain;

	// Token: 0x040024E3 RID: 9443
	public new string Title;

	// Token: 0x040024E4 RID: 9444
	public new string Description;

	// Token: 0x020005B3 RID: 1459
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FE5 RID: 12261 RVA: 0x0012B224 File Offset: 0x00129424
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024E5 RID: 9445
		public bool bReadingStarted;
	}
}
