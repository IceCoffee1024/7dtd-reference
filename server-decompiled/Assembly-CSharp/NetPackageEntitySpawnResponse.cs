using System;
using UnityEngine.Scripting;

// Token: 0x020007F7 RID: 2039
[Preserve]
public class NetPackageEntitySpawnResponse : NetPackage
{
	// Token: 0x06003C09 RID: 15369 RVA: 0x00181ACE File Offset: 0x0017FCCE
	public NetPackageEntitySpawnResponse Setup(bool _success, ItemValue _itemValue)
	{
		this.success = _success;
		this.itemValue = _itemValue;
		return this;
	}

	// Token: 0x06003C0A RID: 15370 RVA: 0x00181ADF File Offset: 0x0017FCDF
	public override void read(PooledBinaryReader _reader)
	{
		this.success = _reader.ReadBoolean();
		this.itemValue = new ItemValue();
		this.itemValue.Read(_reader);
	}

	// Token: 0x06003C0B RID: 15371 RVA: 0x00181B04 File Offset: 0x0017FD04
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.success);
		this.itemValue.Write(_writer);
	}

	// Token: 0x06003C0C RID: 15372 RVA: 0x00181B28 File Offset: 0x0017FD28
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		EntityPlayerLocal primaryPlayer = _world.GetPrimaryPlayer();
		bool flag = this.itemValue.ItemClass.HasAnyTags(FastTags<TagGroup.Global>.Parse("vehicle"));
		bool flag2 = this.itemValue.ItemClass.HasAnyTags(FastTags<TagGroup.Global>.Parse("drone"));
		bool flag3 = this.itemValue.ItemClass.HasAnyTags(FastTags<TagGroup.Global>.Parse("turretRanged")) || this.itemValue.ItemClass.HasAnyTags(FastTags<TagGroup.Global>.Parse("turretMelee"));
		if (this.success)
		{
			if (flag)
			{
				if (primaryPlayer.inventory.holdingItem.Equals(this.itemValue.ItemClass))
				{
					ItemActionSpawnVehicle itemActionSpawnVehicle = primaryPlayer.inventory.holdingItem.Actions[1] as ItemActionSpawnVehicle;
					if (itemActionSpawnVehicle != null)
					{
						itemActionSpawnVehicle.ClearPreview(primaryPlayer.inventory.holdingItemData.actionData[1]);
					}
				}
				primaryPlayer.inventory.DecItem(this.itemValue, 1, false, null);
				primaryPlayer.PlayOneShot("placeblock", false, false, false, null, 1f);
				return;
			}
			if (primaryPlayer.inventory.holdingItem.Equals(this.itemValue.ItemClass))
			{
				ItemActionSpawnTurret itemActionSpawnTurret = primaryPlayer.inventory.holdingItem.Actions[1] as ItemActionSpawnTurret;
				if (itemActionSpawnTurret != null)
				{
					itemActionSpawnTurret.ClearPreview(primaryPlayer.inventory.holdingItemData.actionData[1]);
				}
			}
			primaryPlayer.inventory.DecItem(this.itemValue, 1, false, null);
			primaryPlayer.PlayOneShot("placeblock", false, false, false, null, 1f);
			return;
		}
		else
		{
			if (flag)
			{
				GameManager.ShowTooltip(primaryPlayer, "uiCannotAddVehicle", false, false, 0f);
				return;
			}
			if (flag2)
			{
				GameManager.ShowTooltip(primaryPlayer, "uiCannotAddDrone", false, false, 0f);
				return;
			}
			if (flag3)
			{
				GameManager.ShowTooltip(primaryPlayer, "uiCannotAddTurret", false, false, 0f);
			}
			return;
		}
	}

	// Token: 0x040030CF RID: 12495
	public bool success;

	// Token: 0x040030D0 RID: 12496
	public ItemValue itemValue;
}
