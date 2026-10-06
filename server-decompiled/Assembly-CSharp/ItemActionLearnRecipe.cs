using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B6 RID: 1462
[Preserve]
public class ItemActionLearnRecipe : ItemAction
{
	// Token: 0x06002FF6 RID: 12278 RVA: 0x00131B0B File Offset: 0x0012FD0B
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionLearnRecipe.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002FF7 RID: 12279 RVA: 0x00131B14 File Offset: 0x0012FD14
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

	// Token: 0x06002FF8 RID: 12280 RVA: 0x00131C00 File Offset: 0x0012FE00
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionLearnRecipe.MyInventoryData)_data).bReadingStarted = false;
	}

	// Token: 0x06002FF9 RID: 12281 RVA: 0x00131C18 File Offset: 0x0012FE18
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

	// Token: 0x06002FFA RID: 12282 RVA: 0x00131C68 File Offset: 0x0012FE68
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionLearnRecipe.MyInventoryData myInventoryData = (ItemActionLearnRecipe.MyInventoryData)_actionData;
		return myInventoryData.bReadingStarted && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06002FFB RID: 12283 RVA: 0x00131CC0 File Offset: 0x0012FEC0
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

	// Token: 0x06002FFC RID: 12284 RVA: 0x00131E20 File Offset: 0x00130020
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

	// Token: 0x040024EB RID: 9451
	public new string[] RecipesToLearn;

	// Token: 0x040024EC RID: 9452
	public string[] SkillsToGain;

	// Token: 0x040024ED RID: 9453
	public new string Title;

	// Token: 0x040024EE RID: 9454
	public new string Description;

	// Token: 0x020005B7 RID: 1463
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FFE RID: 12286 RVA: 0x0012B224 File Offset: 0x00129424
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024EF RID: 9455
		public bool bReadingStarted;
	}
}
