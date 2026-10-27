using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200195B RID: 6491
	[Preserve]
	public class ChallengeObjectiveCraft : BaseChallengeObjective
	{
		// Token: 0x1700194C RID: 6476
		// (get) Token: 0x0600C7D1 RID: 51153 RVA: 0x000809D4 File Offset: 0x0007EBD4
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Craft;
			}
		}

		// Token: 0x1700194D RID: 6477
		// (get) Token: 0x0600C7D2 RID: 51154 RVA: 0x00492ED3 File Offset: 0x004910D3
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("lblContextActionCraft", false, null) + " " + Localization.Get(this.itemClassID, false, null) + ":";
			}
		}

		// Token: 0x1700194E RID: 6478
		// (get) Token: 0x0600C7D3 RID: 51155 RVA: 0x00492EFD File Offset: 0x004910FD
		public override ChallengeClass.UINavTypes NavType
		{
			get
			{
				if (this.itemRecipe == null)
				{
					return ChallengeClass.UINavTypes.None;
				}
				if (!(this.itemRecipe.craftingArea == ""))
				{
					return ChallengeClass.UINavTypes.None;
				}
				return ChallengeClass.UINavTypes.Crafting;
			}
		}

		// Token: 0x0600C7D4 RID: 51156 RVA: 0x00492F23 File Offset: 0x00491123
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
			this.itemRecipe = CraftingManager.GetRecipe(this.itemClassID);
		}

		// Token: 0x0600C7D5 RID: 51157 RVA: 0x00492F5A File Offset: 0x0049115A
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C7D6 RID: 51158 RVA: 0x00492F68 File Offset: 0x00491168
		[PublicizedFrom(EAccessModifier.Private)]
		public void CreateRequirements()
		{
			if (this.itemClassIDs.Length > 1)
			{
				return;
			}
			if (!this.ShowRequirements)
			{
				return;
			}
			this.Owner.SetRequirementGroup(new RequirementObjectiveGroupGatherIngredients(this.itemClassID)
			{
				CraftObj = this
			});
		}

		// Token: 0x0600C7D7 RID: 51159 RVA: 0x00492F9C File Offset: 0x0049119C
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
			QuestEventManager.Current.CraftItem += this.Current_CraftItem;
		}

		// Token: 0x0600C7D8 RID: 51160 RVA: 0x00492FCA File Offset: 0x004911CA
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
		}

		// Token: 0x0600C7D9 RID: 51161 RVA: 0x00492FE2 File Offset: 0x004911E2
		public override void HandleTrackingStarted()
		{
			base.HandleTrackingStarted();
		}

		// Token: 0x0600C7DA RID: 51162 RVA: 0x00492FEA File Offset: 0x004911EA
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
		}

		// Token: 0x0600C7DB RID: 51163 RVA: 0x00492FF4 File Offset: 0x004911F4
		[PublicizedFrom(EAccessModifier.Private)]
		public void Current_CraftItem(ItemStack stack)
		{
			if (this.CheckBaseRequirements())
			{
				return;
			}
			ItemClass itemClass = stack.itemValue.ItemClass;
			if (itemClass != null && this.itemClassIDs.ContainsCaseInsensitive(itemClass.Name))
			{
				base.Current += stack.count;
				this.CheckObjectiveComplete(true);
			}
		}

		// Token: 0x0600C7DC RID: 51164 RVA: 0x00493047 File Offset: 0x00491247
		public override bool CheckObjectiveComplete(bool handleComplete = true)
		{
			if (this.IsRequirement && this.CheckForNeededItem())
			{
				base.Complete = true;
				this.HandleRecipeListUpdate();
				return true;
			}
			base.Complete = false;
			this.HandleRecipeListUpdate();
			return base.CheckObjectiveComplete(handleComplete);
		}

		// Token: 0x0600C7DD RID: 51165 RVA: 0x0049307C File Offset: 0x0049127C
		public override Recipe GetRecipeItem()
		{
			return this.itemRecipe;
		}

		// Token: 0x0600C7DE RID: 51166 RVA: 0x00493084 File Offset: 0x00491284
		public override Recipe[] GetRecipeItems()
		{
			Recipe recipeFromRequirements = this.Owner.GetRecipeFromRequirements();
			if (recipeFromRequirements != null)
			{
				return new Recipe[] { recipeFromRequirements, this.itemRecipe };
			}
			return new Recipe[] { this.itemRecipe };
		}

		// Token: 0x0600C7DF RID: 51167 RVA: 0x004930C3 File Offset: 0x004912C3
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.SetupItem(e.GetAttribute("item"));
			}
		}

		// Token: 0x0600C7E0 RID: 51168 RVA: 0x004930F4 File Offset: 0x004912F4
		public void SetupItem(string itemID)
		{
			this.itemClassID = itemID;
			if (this.itemClassID.Contains(','))
			{
				this.itemClassIDs = this.itemClassID.Split(',', StringSplitOptions.None);
				this.itemClassID = this.itemClassIDs[0];
				return;
			}
			this.itemClassIDs = new string[1];
			this.itemClassIDs[0] = this.itemClassID;
		}

		// Token: 0x0600C7E1 RID: 51169 RVA: 0x00493154 File Offset: 0x00491354
		[PublicizedFrom(EAccessModifier.Private)]
		public bool CheckForNeededItem()
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			XUiM_PlayerInventory playerInventory = uiforPlayer.xui.PlayerInventory;
			ItemValue itemValue = new ItemValue(this.itemRecipe.itemValueType, false);
			int num = playerInventory.Backpack.GetItemCount(itemValue, -1, -1, true);
			num += playerInventory.Toolbelt.GetItemCount(itemValue, false, -1, -1, true);
			ItemStack currentStack = uiforPlayer.xui.DragAndDropWindow.CurrentStack;
			if (!currentStack.IsEmpty() && currentStack.itemValue.type == this.itemRecipe.itemValueType)
			{
				num += currentStack.count;
			}
			base.Current = num;
			return num >= this.MaxCount;
		}

		// Token: 0x0600C7E2 RID: 51170 RVA: 0x00493204 File Offset: 0x00491404
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleRecipeListUpdate()
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			if (uiforPlayer.xui.QuestTracker.TrackedChallenge == this.Owner)
			{
				uiforPlayer.xui.QuestTracker.HandleTrackedChallengeChanged();
			}
		}

		// Token: 0x0600C7E3 RID: 51171 RVA: 0x00493250 File Offset: 0x00491450
		public override BaseChallengeObjective Clone()
		{
			return new ChallengeObjectiveCraft
			{
				itemClassIDs = this.itemClassIDs,
				itemClassID = this.itemClassID,
				itemRecipe = this.itemRecipe,
				expectedItem = this.expectedItem,
				expectedItemClass = this.expectedItemClass
			};
		}

		// Token: 0x0600C7E4 RID: 51172 RVA: 0x00492267 File Offset: 0x00490467
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x040096E2 RID: 38626
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040096E3 RID: 38627
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x040096E4 RID: 38628
		public string[] itemClassIDs;

		// Token: 0x040096E5 RID: 38629
		public string itemClassID = "";

		// Token: 0x040096E6 RID: 38630
		public Recipe itemRecipe;
	}
}
