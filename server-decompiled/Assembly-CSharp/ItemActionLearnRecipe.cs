using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B5 RID: 1461
[Preserve]
public class ItemActionLearnRecipe : ItemAction
{
	// Token: 0x06002FB3 RID: 12211 RVA: 0x00131073 File Offset: 0x0012F273
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionLearnRecipe.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FB4 RID: 12212 RVA: 0x0013107C File Offset: 0x0012F27C
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (!_props.Values.ContainsKey("Recipes_to_learn"))
		{
			this.RecipesToLearn = new string[0];
		}
		else
		{
			this.RecipesToLearn = _props.Values["Recipes_to_learn"].Replace(" ", "").Split(',', StringSplitOptions.None);
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
		}
		else
		{
			this.Description = _props.Values["Description"];
		}
		for (int i = 0; i < this.RecipesToLearn.Length; i++)
		{
			CraftingManager.LockRecipe(this.RecipesToLearn[i], CraftingManager.RecipeLockTypes.Item);
		}
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x00131168 File Offset: 0x0012F368
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionLearnRecipe.MyInventoryData)_data).bReadingStarted = false;
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x00131180 File Offset: 0x0012F380
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
		((ItemActionLearnRecipe.MyInventoryData)_actionData).bReadingStarted = true;
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x001311D0 File Offset: 0x0012F3D0
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionLearnRecipe.MyInventoryData myInventoryData = (ItemActionLearnRecipe.MyInventoryData)_actionData;
		return myInventoryData.bReadingStarted && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x00131228 File Offset: 0x0012F428
	public override void OnHoldingUpdate(ItemActionData _actionData)
	{
		EntityPlayerLocal entityPlayerLocal = _actionData.invData.holdingEntity as EntityPlayerLocal;
		ItemActionLearnRecipe.MyInventoryData myInventoryData = (ItemActionLearnRecipe.MyInventoryData)_actionData;
		if (!myInventoryData.bReadingStarted || Time.time - myInventoryData.lastUseTime < AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast)
		{
			return;
		}
		myInventoryData.bReadingStarted = false;
		bool flag = false;
		for (int i = 0; i < this.RecipesToLearn.Length; i++)
		{
			if (CraftingManager.GetRecipe(this.RecipesToLearn[i]).tags.Equals(FastTags<TagGroup.Global>.Parse("learnable")) && myInventoryData.invData.holdingEntity.GetCVar(this.RecipesToLearn[i]) == 0f)
			{
				flag = true;
				myInventoryData.invData.holdingEntity.SetCVar(this.RecipesToLearn[i], 1f);
				GameManager.ShowTooltip(entityPlayerLocal, string.Format(Localization.Get("ttRecipeUnlocked", false, null), Localization.Get(this.RecipesToLearn[i], false, null)), false, false, 0f);
			}
		}
		if (flag)
		{
			_actionData.invData.holdingEntity.inventory.DecHoldingItem(1);
			if (this.soundStart != null)
			{
				Manager.PlayInsidePlayerHead(this.soundStart, -1, 0f, false, false);
				return;
			}
		}
		else
		{
			GameManager.ShowTooltip(entityPlayerLocal, Localization.Get("alreadyKnown", false, null), false, false, 0f);
		}
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x00131388 File Offset: 0x0012F588
	public override void GetItemValueActionInfo(ref List<string> _infoList, ItemValue _itemValue, XUi _xui, int _actionIndex = 0)
	{
		for (int i = 0; i < this.RecipesToLearn.Length; i++)
		{
			if (!XUiM_Recipes.GetRecipeIsUnlocked(_xui, this.RecipesToLearn[i]))
			{
				_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblAttributeRecipe", false, null), Localization.Get(this.RecipesToLearn[i], false, null)));
			}
			else
			{
				_infoList.Add(ItemAction.StringFormatHandler(Localization.Get("lblAttributeRecipe", false, null), Localization.Get("lblKnown", false, null)));
			}
		}
	}

	// Token: 0x040024F2 RID: 9458
	public new string[] RecipesToLearn;

	// Token: 0x040024F3 RID: 9459
	public string[] SkillsToGain;

	// Token: 0x040024F4 RID: 9460
	public new string Title;

	// Token: 0x040024F5 RID: 9461
	public new string Description;

	// Token: 0x020005B6 RID: 1462
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FBB RID: 12219 RVA: 0x0012A704 File Offset: 0x00128904
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024F6 RID: 9462
		public bool bReadingStarted;
	}
}
