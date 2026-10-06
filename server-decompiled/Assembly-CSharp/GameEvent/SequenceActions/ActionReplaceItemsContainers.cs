using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1D RID: 6685
	[Preserve]
	public class ActionReplaceItemsContainers : ActionBaseContainersAction
	{
		// Token: 0x0600CBDA RID: 52186 RVA: 0x004A7C80 File Offset: 0x004A5E80
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

		// Token: 0x0600CBDB RID: 52187 RVA: 0x004A7D94 File Offset: 0x004A5F94
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

		// Token: 0x0600CBDC RID: 52188 RVA: 0x004A7FE4 File Offset: 0x004A61E4
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseBool(ActionReplaceItemsContainers.PropIncludeOutputs, ref this.includeOutputs);
			properties.ParseString(ActionReplaceItemsContainers.PropReplacedByItem, ref this.ReplacedByItem);
			properties.ParseString(ActionReplaceItemsContainers.PropItemTag, ref this.itemTags);
			this.fastItemTags = FastTags<TagGroup.Global>.Parse(this.itemTags);
		}

		// Token: 0x0600CBDD RID: 52189 RVA: 0x004A803C File Offset: 0x004A623C
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

		// Token: 0x04009A8A RID: 39562
		public string ReplacedByItem = "";

		// Token: 0x04009A8B RID: 39563
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool includeOutputs;

		// Token: 0x04009A8C RID: 39564
		[PublicizedFrom(EAccessModifier.Protected)]
		public string itemTags = "";

		// Token: 0x04009A8D RID: 39565
		public static string PropReplacedByItem = "replaced_by_item";

		// Token: 0x04009A8E RID: 39566
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIncludeOutputs = "include_outputs";

		// Token: 0x04009A8F RID: 39567
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropItemTag = "items_tags";

		// Token: 0x04009A90 RID: 39568
		[PublicizedFrom(EAccessModifier.Protected)]
		public FastTags<TagGroup.Global> fastItemTags = FastTags<TagGroup.Global>.none;
	}
}
