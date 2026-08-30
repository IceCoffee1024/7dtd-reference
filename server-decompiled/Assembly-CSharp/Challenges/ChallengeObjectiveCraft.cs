using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

namespace Challenges
{
	// Token: 0x020018FE RID: 6398
	[Preserve]
	public class ChallengeObjectiveCraft : BaseChallengeObjective
	{
		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x0600C565 RID: 50533 RVA: 0x00080550 File Offset: 0x0007E750
		public override ChallengeObjectiveType ObjectiveType
		{
			get
			{
				return ChallengeObjectiveType.Craft;
			}
		}

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x0600C566 RID: 50534 RVA: 0x0048AFEF File Offset: 0x004891EF
		public override string DescriptionText
		{
			get
			{
				return Localization.Get("lblContextActionCraft", false, null) + " " + Localization.Get(this.itemClassID, false, null) + ":";
			}
		}

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x0600C567 RID: 50535 RVA: 0x0048B019 File Offset: 0x00489219
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

		// Token: 0x0600C568 RID: 50536 RVA: 0x0048B03F File Offset: 0x0048923F
		public override void Init()
		{
			this.expectedItem = ItemClass.GetItem(this.itemClassID, false);
			this.expectedItemClass = ItemClass.GetItemClass(this.itemClassID, false);
			this.itemRecipe = CraftingManager.GetRecipe(this.itemClassID);
		}

		// Token: 0x0600C569 RID: 50537 RVA: 0x0048B076 File Offset: 0x00489276
		public override void HandleOnCreated()
		{
			base.HandleOnCreated();
			this.CreateRequirements();
		}

		// Token: 0x0600C56A RID: 50538 RVA: 0x0048B084 File Offset: 0x00489284
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

		// Token: 0x0600C56B RID: 50539 RVA: 0x0048B0B8 File Offset: 0x004892B8
		public override void HandleAddHooks()
		{
			QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
			QuestEventManager.Current.CraftItem += this.Current_CraftItem;
		}

		// Token: 0x0600C56C RID: 50540 RVA: 0x0048B0E6 File Offset: 0x004892E6
		public override void HandleRemoveHooks()
		{
			QuestEventManager.Current.CraftItem -= this.Current_CraftItem;
		}

		// Token: 0x0600C56D RID: 50541 RVA: 0x0048B0FE File Offset: 0x004892FE
		public override void HandleTrackingStarted()
		{
			base.HandleTrackingStarted();
		}

		// Token: 0x0600C56E RID: 50542 RVA: 0x0048B106 File Offset: 0x00489306
		public override void HandleTrackingEnded()
		{
			base.HandleTrackingEnded();
		}

		// Token: 0x0600C56F RID: 50543 RVA: 0x0048B110 File Offset: 0x00489310
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

		// Token: 0x0600C570 RID: 50544 RVA: 0x0048B163 File Offset: 0x00489363
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

		// Token: 0x0600C571 RID: 50545 RVA: 0x0048B198 File Offset: 0x00489398
		public override Recipe GetRecipeItem()
		{
			return this.itemRecipe;
		}

		// Token: 0x0600C572 RID: 50546 RVA: 0x0048B1A0 File Offset: 0x004893A0
		public override Recipe[] GetRecipeItems()
		{
			Recipe recipeFromRequirements = this.Owner.GetRecipeFromRequirements();
			if (recipeFromRequirements != null)
			{
				return new Recipe[] { recipeFromRequirements, this.itemRecipe };
			}
			return new Recipe[] { this.itemRecipe };
		}

		// Token: 0x0600C573 RID: 50547 RVA: 0x0048B1DF File Offset: 0x004893DF
		public override void ParseElement(XElement e)
		{
			base.ParseElement(e);
			if (e.HasAttribute("item"))
			{
				this.SetupItem(e.GetAttribute("item"));
			}
		}

		// Token: 0x0600C574 RID: 50548 RVA: 0x0048B210 File Offset: 0x00489410
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

		// Token: 0x0600C575 RID: 50549 RVA: 0x0048B270 File Offset: 0x00489470
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

		// Token: 0x0600C576 RID: 50550 RVA: 0x0048B320 File Offset: 0x00489520
		[PublicizedFrom(EAccessModifier.Private)]
		public void HandleRecipeListUpdate()
		{
			LocalPlayerUI uiforPlayer = LocalPlayerUI.GetUIForPlayer(this.Owner.Owner.Player);
			if (uiforPlayer.xui.QuestTracker.TrackedChallenge == this.Owner)
			{
				uiforPlayer.xui.QuestTracker.HandleTrackedChallengeChanged();
			}
		}

		// Token: 0x0600C577 RID: 50551 RVA: 0x0048B36C File Offset: 0x0048956C
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

		// Token: 0x0600C578 RID: 50552 RVA: 0x0048A383 File Offset: 0x00488583
		public override void CompleteObjective(bool handleComplete = true)
		{
			base.Current = this.MaxCount;
			base.Complete = true;
			if (handleComplete)
			{
				this.Owner.HandleComplete(true, false);
			}
		}

		// Token: 0x04009533 RID: 38195
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemValue expectedItem = ItemValue.None;

		// Token: 0x04009534 RID: 38196
		[PublicizedFrom(EAccessModifier.Private)]
		public ItemClass expectedItemClass;

		// Token: 0x04009535 RID: 38197
		public string[] itemClassIDs;

		// Token: 0x04009536 RID: 38198
		public string itemClassID = "";

		// Token: 0x04009537 RID: 38199
		public Recipe itemRecipe;
	}
}
