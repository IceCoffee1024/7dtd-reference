using System;
using System.Collections.Generic;
using System.Globalization;
using Audio;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005B5 RID: 1461
[Preserve]
public class ItemActionOpenBundle : ItemAction
{
	// Token: 0x06002F9D RID: 12189 RVA: 0x001310C3 File Offset: 0x0012F2C3
	public override ItemActionData CreateModifierData(ItemInventoryData _invData, int _indexInEntityOfAction)
	{
		return new ItemActionOpenBundle.MyInventoryData(_invData, _indexInEntityOfAction);
	}

	// Token: 0x06002F9E RID: 12190 RVA: 0x001310CC File Offset: 0x0012F2CC
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (_props.Values.ContainsKey("Consume"))
		{
			this.Consume = StringParsers.ParseBool(_props.Values["Consume"], 0, -1, true);
		}
		else
		{
			this.Consume = true;
		}
		if (_props.Values.ContainsKey("Create_item"))
		{
			this.CreateItem = _props.Values["Create_item"].Replace(" ", "").Split(',', StringSplitOptions.None);
			if (_props.Values.ContainsKey("Create_item_count"))
			{
				this.CreateItemCount = _props.Values["Create_item_count"].Replace(" ", "").Split(',', StringSplitOptions.None);
			}
			else
			{
				this.CreateItemCount = new string[0];
			}
		}
		else
		{
			this.CreateItem = null;
			this.CreateItemCount = null;
		}
		if (_props.Values.ContainsKey("Random_item"))
		{
			this.RandomItem = _props.Values["Random_item"].Replace(" ", "").Split(',', StringSplitOptions.None);
			if (_props.Values.ContainsKey("Random_item_count"))
			{
				this.RandomItemCount = _props.Values["Random_item_count"].Replace(" ", "").Split(',', StringSplitOptions.None);
			}
			else
			{
				this.RandomItemCount = new string[0];
			}
		}
		else
		{
			this.RandomItem = null;
			this.RandomItemCount = null;
		}
		if (_props.Values.ContainsKey("Random_count"))
		{
			this.RandomCount = StringParsers.ParseSInt32(_props.Values["Random_count"], 0, -1, NumberStyles.Integer);
		}
		_props.ParseBool("Unique_random_only", ref this.UniqueRandomOnly);
		if (_props.Values.ContainsKey("Condition_raycast_block"))
		{
			string[] array = _props.Values["Condition_raycast_block"].Trim().Split(',', StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				int num = int.Parse(array[i].Trim());
				if (this.ConditionBlockTypes == null)
				{
					this.ConditionBlockTypes = new HashSet<int>();
				}
				this.ConditionBlockTypes.Add(num);
			}
			if (this.ConditionBlockTypes != null && this.ConditionBlockTypes.Count == 0)
			{
				this.ConditionBlockTypes = null;
			}
		}
		this.UseAnimation = false;
	}

	// Token: 0x06002F9F RID: 12191 RVA: 0x00131321 File Offset: 0x0012F521
	public override void StopHolding(ItemActionData _data)
	{
		base.StopHolding(_data);
		((ItemActionOpenBundle.MyInventoryData)_data).bEatingStarted = false;
	}

	// Token: 0x06002FA0 RID: 12192 RVA: 0x00131338 File Offset: 0x0012F538
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		ItemActionOpenBundle.MyInventoryData myInventoryData = (ItemActionOpenBundle.MyInventoryData)_actionData;
		if (!_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		if (this.IsActionRunning(_actionData))
		{
			return;
		}
		EntityAlive holdingEntity = myInventoryData.invData.holdingEntity;
		if (EffectManager.GetValue(PassiveEffects.DisableItem, holdingEntity.inventory.holdingItemItemValue, 0f, holdingEntity, null, _actionData.invData.item.ItemTags, true, true, true, true, true, 1, true, false) > 0f)
		{
			_actionData.lastUseTime = Time.time + 1f;
			Manager.PlayInsidePlayerHead("twitch_no_attack", -1, 0f, false, false);
			return;
		}
		BlockValue blockValue = BlockValue.Air;
		if (this.ConditionBlockTypes != null)
		{
			Ray ray = holdingEntity.GetLookRay();
			int modelLayer = holdingEntity.GetModelLayer();
			holdingEntity.SetModelLayer(2, false, null);
			Voxel.Raycast(myInventoryData.invData.world, ray, 2.5f, 131, (holdingEntity is EntityPlayer) ? 0.2f : 0.4f);
			holdingEntity.SetModelLayer(modelLayer, false, null);
			WorldRayHitInfo voxelRayHitInfo = Voxel.voxelRayHitInfo;
			if (!GameUtils.IsBlockOrTerrain(voxelRayHitInfo.tag))
			{
				return;
			}
			HitInfoDetails hit = voxelRayHitInfo.hit;
			blockValue = voxelRayHitInfo.hit.blockValue;
			if (blockValue.isair || !this.ConditionBlockTypes.Contains(blockValue.type))
			{
				ray = myInventoryData.invData.holdingEntity.GetLookRay();
				ray.origin += ray.direction.normalized * 0.5f;
				if (!Voxel.Raycast(myInventoryData.invData.world, ray, 2.5f, -538480653, 4095, 0f))
				{
					return;
				}
				HitInfoDetails hit2 = voxelRayHitInfo.hit;
				blockValue = voxelRayHitInfo.hit.blockValue;
				if (blockValue.isair || !this.ConditionBlockTypes.Contains(blockValue.type))
				{
					return;
				}
			}
		}
		_actionData.lastUseTime = Time.time;
		this.ExecuteInstantAction(myInventoryData.invData.holdingEntity, myInventoryData.invData.itemStack, true, null);
	}

	// Token: 0x06002FA1 RID: 12193 RVA: 0x0013154C File Offset: 0x0012F74C
	public override bool ExecuteInstantAction(EntityAlive ent, ItemStack stack, bool isHeldItem, XUiC_ItemStack stackController)
	{
		ent.MinEventContext.ItemValue = stack.itemValue;
		ent.MinEventContext.ItemValue.FireEvent(MinEventTypes.onSelfPrimaryActionStart, ent.MinEventContext);
		ent.FireEvent(MinEventTypes.onSelfPrimaryActionStart, false);
		if (this.soundStart != null)
		{
			ent.PlayOneShot(this.soundStart, false, false, false, null, 1f);
		}
		if (this.Consume)
		{
			if (stack.itemValue.MaxUseTimes > 0 && stack.itemValue.UseTimes + 1f < (float)stack.itemValue.MaxUseTimes)
			{
				stack.itemValue.UseTimes += EffectManager.GetValue(PassiveEffects.DegradationPerUse, stack.itemValue, 1f, ent, null, stack.itemValue.ItemClass.ItemTags, true, true, true, true, true, 1, true, false) * ItemAction.ItemDegradationModifier;
				return true;
			}
			if (isHeldItem)
			{
				ent.inventory.DecHoldingItem(1);
			}
			else
			{
				stack.count--;
			}
		}
		ent.MinEventContext.ItemValue = stack.itemValue;
		ent.MinEventContext.ItemValue.FireEvent(MinEventTypes.onSelfPrimaryActionEnd, ent.MinEventContext);
		ent.FireEvent(MinEventTypes.onSelfPrimaryActionEnd, false);
		if (this.CreateItem != null && this.CreateItemCount != null)
		{
			for (int i = 0; i < this.CreateItem.Length; i++)
			{
				string text = ((this.CreateItemCount != null && this.CreateItemCount.Length > i) ? this.CreateItemCount[i] : "1");
				ItemClass itemClass = ItemClass.GetItemClass(this.CreateItem[i], false);
				int num3;
				if (text.Contains("-"))
				{
					string[] array = text.Split('-', StringSplitOptions.None);
					int num = StringParsers.ParseSInt32(array[0], 0, -1, NumberStyles.Integer);
					int num2 = StringParsers.ParseSInt32(array[1], 0, -1, NumberStyles.Integer) + 1;
					num3 = ent.rand.RandomRange(num, num2);
				}
				else
				{
					num3 = StringParsers.ParseSInt32(text, 0, -1, NumberStyles.Integer);
				}
				ItemValue itemValue;
				if (itemClass.HasQuality)
				{
					itemValue = new ItemValue(itemClass.Id, num3, num3, false, null, 1f);
					num3 = 1;
				}
				else
				{
					itemValue = new ItemValue(itemClass.Id, false);
				}
				ItemStack itemStack = new ItemStack(itemValue, num3);
				XUiM_PlayerInventory playerInventory = LocalPlayerUI.GetUIForPlayer(ent as EntityPlayerLocal).xui.PlayerInventory;
				if (itemStack.itemValue.HasQuality)
				{
					itemStack.itemValue.MaxDurabilityModifier = 1f;
				}
				if (!playerInventory.AddItem(itemStack))
				{
					ent.world.gameManager.ItemDropServer(itemStack, ent.GetPosition(), Vector3.zero, -1, 60f, false);
				}
			}
		}
		if (this.RandomItem != null && this.RandomItemCount != null)
		{
			List<int> list = null;
			if (this.UniqueRandomOnly)
			{
				list = new List<int>();
				for (int j = 0; j < this.RandomItem.Length; j++)
				{
					list.Add(j);
				}
				for (int k = 0; k < list.Count * 3; k++)
				{
					int num4 = ent.rand.RandomRange(0, list.Count);
					int num5 = ent.rand.RandomRange(0, list.Count);
					if (num4 != num5)
					{
						int num6 = list[num4];
						list[num4] = list[num5];
						list[num5] = num6;
					}
				}
			}
			int num7 = -1;
			for (int l = 0; l < this.RandomCount; l++)
			{
				int num8;
				if (this.UniqueRandomOnly)
				{
					num7++;
					if (num7 >= this.RandomItem.Length)
					{
						num7 = 0;
					}
					num8 = list[num7];
				}
				else
				{
					num8 = ent.rand.RandomRange(0, this.RandomItem.Length);
				}
				string text2 = ((this.RandomItemCount != null && this.RandomItemCount.Length > num8) ? this.RandomItemCount[num8] : "1");
				ItemClass itemClass2;
				for (;;)
				{
					itemClass2 = ItemClass.GetItemClass(this.RandomItem[num8], false);
					if (itemClass2 != null)
					{
						break;
					}
					num8++;
					if (num8 > this.RandomItem.Length)
					{
						num8 = 0;
					}
				}
				int m;
				if (text2.Contains("-"))
				{
					string[] array2 = text2.Split('-', StringSplitOptions.None);
					int num9 = StringParsers.ParseSInt32(array2[0], 0, -1, NumberStyles.Integer);
					int num10 = StringParsers.ParseSInt32(array2[1], 0, -1, NumberStyles.Integer) + 1;
					m = ent.rand.RandomRange(num9, num10);
				}
				else
				{
					m = StringParsers.ParseSInt32(text2, 0, -1, NumberStyles.Integer);
				}
				ItemValue itemValue2;
				if (itemClass2.HasQuality)
				{
					itemValue2 = new ItemValue(itemClass2.Id, m, m, false, null, 1f);
					m = 1;
				}
				else
				{
					itemValue2 = new ItemValue(itemClass2.Id, false);
				}
				XUiM_PlayerInventory playerInventory2 = LocalPlayerUI.GetUIForPlayer(ent as EntityPlayerLocal).xui.PlayerInventory;
				while (m > 0)
				{
					ItemStack itemStack2 = new ItemStack(itemValue2, m);
					m -= itemStack2.count;
					if (!playerInventory2.AddItem(itemStack2))
					{
						ent.world.gameManager.ItemDropServer(itemStack2, ent.GetPosition(), Vector3.zero, -1, 60f, false);
					}
				}
			}
		}
		return true;
	}

	// Token: 0x040024CA RID: 9418
	public new string[] CreateItem;

	// Token: 0x040024CB RID: 9419
	public string[] CreateItemCount;

	// Token: 0x040024CC RID: 9420
	public new bool Consume;

	// Token: 0x040024CD RID: 9421
	public HashSet<int> ConditionBlockTypes;

	// Token: 0x040024CE RID: 9422
	public bool UniqueRandomOnly;

	// Token: 0x040024CF RID: 9423
	public string[] RandomItem;

	// Token: 0x040024D0 RID: 9424
	public string[] RandomItemCount;

	// Token: 0x040024D1 RID: 9425
	public int RandomCount = 1;

	// Token: 0x020005B6 RID: 1462
	[PublicizedFrom(EAccessModifier.Private)]
	public class MyInventoryData : ItemActionAttackData
	{
		// Token: 0x06002FA3 RID: 12195 RVA: 0x00129694 File Offset: 0x00127894
		public MyInventoryData(ItemInventoryData _invData, int _indexInEntityOfAction)
			: base(_invData, _indexInEntityOfAction)
		{
		}

		// Token: 0x040024D2 RID: 9426
		public bool bEatingStarted;
	}
}
