using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B1 RID: 1457
[Preserve]
public class ItemActionGainSkill : ItemAction
{
	// Token: 0x06002F9A RID: 12186 RVA: 0x001307B9 File Offset: 0x0012E9B9
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionGainSkill.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F9B RID: 12187 RVA: 0x001307C4 File Offset: 0x0012E9C4
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

	// Token: 0x06002F9C RID: 12188 RVA: 0x0013087F File Offset: 0x0012EA7F
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionGainSkill.MyInventoryData)_data).bReadingStarted = false;
	}

	// Token: 0x06002F9D RID: 12189 RVA: 0x00130894 File Offset: 0x0012EA94
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

	// Token: 0x06002F9E RID: 12190 RVA: 0x001308E4 File Offset: 0x0012EAE4
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionGainSkill.MyInventoryData myInventoryData = (ItemActionGainSkill.MyInventoryData)_actionData;
		return myInventoryData.bReadingStarted && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x0013093C File Offset: 0x0012EB3C
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

	// Token: 0x06002FA0 RID: 12192 RVA: 0x00130AE8 File Offset: 0x0012ECE8
	public override void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
		for (int i = 0; i < this.SkillsToGain.Length; i++)
		{
			_infoList.Add(ItemAction.StringFormatHandler(Localization.Get(_xui.playerUI.entityPlayer.Progression.GetProgressionValue(this.SkillsToGain[i]).ProgressionClass.NameKey, false, null), "+1"));
		}
	}

	// Token: 0x040024E9 RID: 9449
	public string[] SkillsToGain;

	// Token: 0x040024EA RID: 9450
	public new string Title;

	// Token: 0x040024EB RID: 9451
	public new string Description;

	// Token: 0x020005B2 RID: 1458
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FA2 RID: 12194 RVA: 0x0012A704 File Offset: 0x00128904
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024EC RID: 9452
		public bool bReadingStarted;
	}
}
