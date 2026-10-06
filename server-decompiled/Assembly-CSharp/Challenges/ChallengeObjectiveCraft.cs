using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x0200192A RID: 6442
	[Preserve]
	public class ChallengeObjectiveCraft : BaseChallengeObjective
	{
		// Token: 0x17001904 RID: 6404
		// (get) Token: 0x0600C6BD RID: 50877 RVA: 0x00080804 File Offset: 0x0007EA04
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Craft;
			}
		}

		// Token: 0x17001905 RID: 6405
		// (get) Token: 0x0600C6BE RID: 50878 RVA: 0x0048F4DF File Offset: 0x0048D6DF
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("lblContextActionCraft", false, null) + " " + Localization.Get(this.itemClassID, false, null) + ":";
			}
		}

		// Token: 0x17001906 RID: 6406
		// (get) Token: 0x0600C6BF RID: 50879 RVA: 0x0048F509 File Offset: 0x0048D709
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

		// Token: 0x0600C6C0 RID: 50880 RVA: 0x0048F52F File Offset: 0x0048D72F
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
			this.itemRecipe = CraftingManager.GetRecipe(this.itemClassID);
		}

		// Token: 0x0600C6C1 RID: 50881 RVA: 0x0048F566 File Offset: 0x0048D766
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C6C2 RID: 50882 RVA: 0x0048F574 File Offset: 0x0048D774
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

		// Token: 0x0600C6C3 RID: 50883 RVA: 0x0048F5A8 File Offset: 0x0048D7A8
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
			QuestEventManager.Current.CraftItem += this.Current_CraftItem;
		}

		// Token: 0x0600C6C4 RID: 50884 RVA: 0x0048F5D6 File Offset: 0x0048D7D6
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
		}

		// Token: 0x0600C6C5 RID: 50885 RVA: 0x0048F5EE File Offset: 0x0048D7EE
		public override void HandleTrackingStarted()
		{
			base.HandleTrackingStarted();
		}

		// Token: 0x0600C6C6 RID: 50886 RVA: 0x0048F5F6 File Offset: 0x0048D7F6
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
		}

		// Token: 0x0600C6C7 RID: 50887 RVA: 0x0048F600 File Offset: 0x0048D800
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

		// Token: 0x0600C6C8 RID: 50888 RVA: 0x0048F653 File Offset: 0x0048D853
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

		// Token: 0x0600C6C9 RID: 50889 RVA: 0x0048F688 File Offset: 0x0048D888
		public override Recipe GetRecipeItem()
		{
			return this.itemRecipe;
		}

		// Token: 0x0600C6CA RID: 50890 RVA: 0x0048F690 File Offset: 0x0048D890
		public override Recipe[] GetRecipeItems()
		{
			Recipe recipeFromRequirements = this.Owner.GetRecipeFromRequirements();
			if (recipeFromRequirements != null)
			{
				return new Recipe[] { recipeFromRequirements, this.itemRecipe };
			}
			return new Recipe[] { this.itemRecipe };
		}

		// Token: 0x0600C6CB RID: 50891 RVA: 0x0048F6CF File Offset: 0x0048D8CF
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.SetupItem(e.GetAttribute("item"));
			}
		}

		// Token: 0x0600C6CC RID: 50892 RVA: 0x0048F700 File Offset: 0x0048D900
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

		// Token: 0x0600C6CD RID: 50893 RVA: 0x0048F760 File Offset: 0x0048D960
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

		// Token: 0x0600C6CE RID: 50894 RVA: 0x0048F810 File Offset: 0x0048DA10
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleRecipeListUpdate()
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			if (uiforPlayer.xui.QuestTracker.TrackedChallenge == this.Owner)
			{
				uiforPlayer.xui.QuestTracker.HandleTrackedChallengeChanged();
			}
		}

		// Token: 0x0600C6CF RID: 50895 RVA: 0x0048F85C File Offset: 0x0048DA5C
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

		// Token: 0x0600C6D0 RID: 50896 RVA: 0x0048E873 File Offset: 0x0048CA73
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x040095F9 RID: 38393
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x040095FA RID: 38394
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x040095FB RID: 38395
		public string[] itemClassIDs;

		// Token: 0x040095FC RID: 38396
		public string itemClassID = "";

		// Token: 0x040095FD RID: 38397
		public Recipe itemRecipe;
	}
}
