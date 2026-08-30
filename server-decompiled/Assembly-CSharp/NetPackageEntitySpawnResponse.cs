using System;
using UnityEngine.Scripting;

// Token: 0x020007EA RID: 2026
[Preserve]
public class NetPackageEntitySpawnResponse : NetPackage
{
	// Token: 0x06003BDD RID: 15325 RVA: 0x00180492 File Offset: 0x0017E692
	public NetPackageEntitySpawnResponse Setup(bool _success, ItemValue _itemValue)
	{
		this.success = _success;
		this.itemValue = _itemValue;
		return this;
	}

	// Token: 0x06003BDE RID: 15326 RVA: 0x001804A3 File Offset: 0x0017E6A3
	public override void read(PooledBinaryReader _reader)
	{
		this.success = _reader.ReadBoolean();
		this.itemValue = new ItemValue();
		this.itemValue.Read(_reader);
	}

	// Token: 0x06003BDF RID: 15327 RVA: 0x001804C8 File Offset: 0x0017E6C8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.success);
		this.itemValue.Write(_writer);
	}

	// Token: 0x06003BE0 RID: 15328 RVA: 0x001804EC File Offset: 0x0017E6EC
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

	// Token: 0x06003BE1 RID: 15329 RVA: 0x000B93F5 File Offset: 0x000B75F5
	public override int GetLength()
	{
		return 20;
	}

	// Token: 0x040030B6 RID: 12470
	public bool success;

	// Token: 0x040030B7 RID: 12471
	public ItemValue itemValue;
}
