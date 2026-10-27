using System;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005BD RID: 1469
[Preserve]
public class ItemActionLearnRecipe : ItemAction
{
	// Token: 0x06003004 RID: 12292 RVA: 0x001326FF File Offset: 0x001308FF
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionLearnRecipe.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06003005 RID: 12293 RVA: 0x00132708 File Offset: 0x00130908
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

	// Token: 0x06003006 RID: 12294 RVA: 0x001327F4 File Offset: 0x001309F4
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionLearnRecipe.MyInventoryData)_data).bReadingStarted = false;
	}

	// Token: 0x06003007 RID: 12295 RVA: 0x0013280C File Offset: 0x00130A0C
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

	// Token: 0x06003008 RID: 12296 RVA: 0x0013285C File Offset: 0x00130A5C
	public override bool IsActionRunning(ItemActionData _actionData)
	{
		ItemActionLearnRecipe.MyInventoryData myInventoryData = (ItemActionLearnRecipe.MyInventoryData)_actionData;
		return myInventoryData.bReadingStarted && Time.time - myInventoryData.lastUseTime < 2f * AnimationDelayData.AnimationDelay[myInventoryData.invData.item.HoldType.Value].RayCast;
	}

	// Token: 0x06003009 RID: 12297 RVA: 0x001328B4 File Offset: 0x00130AB4
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

	// Token: 0x0600300A RID: 12298 RVA: 0x00132A14 File Offset: 0x00130C14
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

	// Token: 0x04002506 RID: 9478
	public new string[] RecipesToLearn;

	// Token: 0x04002507 RID: 9479
	public string[] SkillsToGain;

	// Token: 0x04002508 RID: 9480
	public new string Title;

	// Token: 0x04002509 RID: 9481
	public new string Description;

	// Token: 0x020005BE RID: 1470
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x0600300C RID: 12300 RVA: 0x0012BE18 File Offset: 0x0012A018
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x0400250A RID: 9482
		public bool bReadingStarted;
	}
}
