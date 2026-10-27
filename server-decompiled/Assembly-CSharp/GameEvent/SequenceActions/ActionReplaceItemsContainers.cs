using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A4E RID: 6734
	[Preserve]
	public class ActionReplaceItemsContainers : ActionBaseContainersAction
	{
		// Token: 0x0600CCEE RID: 52462 RVA: 0x004AB77C File Offset: 0x004A997C
		public override bool CheckValidTileEntity(TileEntity te, out bool isEmpty)
		{
			isEmpty = true;
			TileEntityType tileEntityType = te.GetTileEntityType();
			if (tileEntityType != TileEntityType.Workstation)
			{
				TEFeatureStorage tefeatureStorage;
				if (tileEntityType == TileEntityType.Composite && te.TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage))
				{
					for (int i = 0; i < tefeatureStorage.ItemGrid.Length; i++)
					{
						ItemStack itemStack = tefeatureStorage.ItemGrid[i];
						if (!itemStack.IsEmpty() && itemStack.itemValue.ItemClass.HasAnyTags(this.fastItemTags) && itemStack.itemValue.ItemClass.GetItemName() != this.ReplacedByItem)
						{
							isEmpty = false;
						}
					}
					return true;
				}
			}
			else if (this.includeOutputs)
			{
				TileEntityWorkstation tileEntityWorkstation = te as TileEntityWorkstation;
				if (tileEntityWorkstation != null)
				{
					foreach (ItemStack itemStack2 in tileEntityWorkstation.Output)
					{
						if (!itemStack2.IsEmpty() && itemStack2.itemValue.ItemClass.HasAnyTags(this.fastItemTags) && itemStack2.itemValue.ItemClass.GetItemName() != this.ReplacedByItem)
						{
							isEmpty = false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600CCEF RID: 52463 RVA: 0x004AB890 File Offset: 0x004A9A90
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool HandleContainerAction(List<TileEntity> tileEntityList)
		{
			new List<ItemStack>();
			List<TileEntity> list = new List<TileEntity>();
			bool flag = false;
			for (int i = 0; i < tileEntityList.Count; i++)
			{
				bool flag2 = false;
				TileEntityType tileEntityType = tileEntityList[i].GetTileEntityType();
				if (tileEntityType != TileEntityType.Workstation)
				{
					TEFeatureStorage tefeatureStorage;
					if (tileEntityType == TileEntityType.Composite && tileEntityList[i].TryGetSelfOrFeature<TEFeatureStorage>(out tefeatureStorage))
					{
						for (int j = 0; j < tefeatureStorage.ItemGrid.Length; j++)
						{
							ItemStack itemStack = tefeatureStorage.ItemGrid[j];
							if (!itemStack.IsEmpty() && itemStack.itemValue.ItemClass.HasAnyTags(this.fastItemTags) && itemStack.itemValue.ItemClass.GetItemName() != this.ReplacedByItem)
							{
								tefeatureStorage.ItemGrid[j] = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), itemStack.count);
								flag = true;
								flag2 = true;
							}
						}
					}
				}
				else if (this.includeOutputs)
				{
					TileEntityWorkstation tileEntityWorkstation = tileEntityList[i] as TileEntityWorkstation;
					if (tileEntityWorkstation != null)
					{
						ItemStack[] output = tileEntityWorkstation.Output;
						for (int k = 0; k < output.Length; k++)
						{
							ItemStack itemStack2 = output[k];
							if (!itemStack2.IsEmpty() && itemStack2.itemValue.ItemClass.HasAnyTags(this.fastItemTags) && itemStack2.itemValue.ItemClass.GetItemName() != this.ReplacedByItem)
							{
								output[k] = new ItemStack(ItemClass.GetItem(this.ReplacedByItem, false), itemStack2.count);
								flag = true;
								flag2 = true;
							}
						}
						if (flag2)
						{
							tileEntityWorkstation.Output = output;
							list.Add(tileEntityWorkstation);
						}
					}
				}
				if (flag2)
				{
					tileEntityList[i].SetModified();
				}
			}
			if (flag && this.changeName && base.Owner.Target != null)
			{
				PersistentPlayerData playerDataFromEntityID = GameManager.Instance.persistentPlayers.GetPlayerDataFromEntityID(base.Owner.Target.entityId);
				for (int l = 0; l < tileEntityList.Count; l++)
				{
					ITileEntitySignable tileEntitySignable;
					if (tileEntityList[l].TryGetSelfOrFeature<ITileEntitySignable>(out tileEntitySignable))
					{
						tileEntitySignable.SetText(base.ModifiedName, true, (playerDataFromEntityID != null) ? playerDataFromEntityID.PrimaryId : null);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600CCF0 RID: 52464 RVA: 0x004ABAE0 File Offset: 0x004A9CE0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionReplaceItemsContainers.PropIncludeOutputs, ref this.includeOutputs);
			properties.ParseString(ActionReplaceItemsContainers.PropReplacedByItem, ref this.ReplacedByItem);
			properties.ParseString(ActionReplaceItemsContainers.PropItemTag, ref this.itemTags);
			this.fastItemTags = FastTags<TagGroup.Global>.Parse(this.itemTags);
		}

		// Token: 0x0600CCF1 RID: 52465 RVA: 0x004ABB38 File Offset: 0x004A9D38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionReplaceItemsContainers
			{
				TargetingType = this.TargetingType,
				maxDistance = this.maxDistance,
				newName = this.newName,
				changeName = this.changeName,
				includeOutputs = this.includeOutputs,
				tileEntityList = this.tileEntityList,
				fastItemTags = this.fastItemTags,
				ReplacedByItem = this.ReplacedByItem
			};
		}

		// Token: 0x04009B73 RID: 39795
		public string ReplacedByItem = "";

		// Token: 0x04009B74 RID: 39796
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeOutputs;

		// Token: 0x04009B75 RID: 39797
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemTags = "";

		// Token: 0x04009B76 RID: 39798
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009B77 RID: 39799
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeOutputs = "include_outputs";

		// Token: 0x04009B78 RID: 39800
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemTag = "items_tags";

		// Token: 0x04009B79 RID: 39801
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> fastItemTags = FastTags<TagGroup.Global>.none;
	}
}
