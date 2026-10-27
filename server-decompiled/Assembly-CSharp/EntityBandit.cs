using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200048E RID: 1166
[Preserve]
public class EntityBandit : EntityHuman
{
	// Token: 0x06002412 RID: 9234 RVA: 0x000DAAB6 File Offset: 0x000D8CB6
	public override void InitInventory()
	{
		if (this.inventory == null)
		{
			this.inventory = new Inventory(this, 8);
		}
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000DAAD0 File Offset: 0x000D8CD0
	public override void PostInit()
	{
		string @string = EntityClass.list[this.entityClass].Properties.GetString(EntityClass.PropHandItem);
		if (@string.Length > 0)
		{
			base.SetInventorySlots(@string);
		}
		if (this.inventory.GetItem(0).IsEmpty())
		{
			ItemValue bareHandItemValue = base.Hand.BareHandItemValue;
			bareHandItemValue.Quality = (ushort)this.rand.RandomRange(1, 3);
			bareHandItemValue.UseTimes = (float)bareHandItemValue.MaxUseTimes * 0.7f - 1f;
			this.inventory.SetItem(0, bareHandItemValue, 1);
		}
		int num = 1;
		if (!this.inventory.GetItem(num).IsEmpty())
		{
			this.inventory.SetHoldingItemIdx(num);
		}
		if (this.moveHelper != null)
		{
			this.moveHelper.CanOpenDoors = true;
		}
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000DAB9C File Offset: 0x000D8D9C
	public override void OnAddedToWorld()
	{
		base.OnAddedToWorld();
	}

	// Token: 0x06002415 RID: 9237 RVA: 0x000DABA4 File Offset: 0x000D8DA4
	public override void SetupHandItem()
	{
		this.ShowHoldingItem(true);
	}

	// Token: 0x06002416 RID: 9238 RVA: 0x000DABB0 File Offset: 0x000D8DB0
	public override bool UseHoldingItem(int _actionIndex, bool _isReleased)
	{
		if (!_isReleased)
		{
			ItemActionAttackData itemActionAttackData = this.inventory.holdingItemData.actionData[0] as ItemActionAttackData;
			if (itemActionAttackData != null)
			{
				ItemValue itemValue = itemActionAttackData.invData.itemValue;
				itemValue.UseTimes = (float)itemValue.MaxUseTimes * 0.8f - 1f;
				if (itemActionAttackData is ItemActionRanged.ItemActionDataRanged)
				{
					itemValue.Meta = 2;
				}
			}
		}
		return base.UseHoldingItem(_actionIndex, _isReleased);
	}

	// Token: 0x06002417 RID: 9239 RVA: 0x000DAC1C File Offset: 0x000D8E1C
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void updateTasks()
	{
		base.updateTasks();
		Vector3 vector;
		if (AIFocusAim.GetActiveFocus(this, this.focusAim, out vector))
		{
			base.SetLookPosition(vector);
		}
	}

	// Token: 0x06002418 RID: 9240 RVA: 0x000DAC46 File Offset: 0x000D8E46
	public override bool GetAimTarget(out Vector3 aimTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out aimTarget) || base.GetAimTarget(out aimTarget);
	}

	// Token: 0x06002419 RID: 9241 RVA: 0x000DAC60 File Offset: 0x000D8E60
	public override bool GetHeadLookTarget(out Vector3 lookTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out lookTarget) || base.GetHeadLookTarget(out lookTarget);
	}

	// Token: 0x0600241A RID: 9242 RVA: 0x000DAC7C File Offset: 0x000D8E7C
	public override bool CalcStrafeYawOffset(float _moveX, float _moveZ, ref float _desiredyaw, ref float _yawOffset)
	{
		float num;
		if (AIFocusBody.GetActiveFocus(this, this.focusBody, out num))
		{
			float num2 = Mathf.Atan2(_moveX, _moveZ) * 57.29578f;
			_desiredyaw = num;
			_yawOffset = MathUtils.NormalizeAxis(num2 - _desiredyaw);
			return true;
		}
		return false;
	}

	// Token: 0x0400199B RID: 6555
	public readonly AIFocus<AIFocusBody> focusBody = new AIFocus<AIFocusBody>(true);

	// Token: 0x0400199C RID: 6556
	public readonly AIFocus<AIFocusAim> focusAim = new AIFocus<AIFocusAim>(true);

	// Token: 0x0200048F RID: 1167
	[PublicizedFrom(EAccessModifier.Private)]
	public enum InvSlots
	{
		// Token: 0x0400199E RID: 6558
		Melee,
		// Token: 0x0400199F RID: 6559
		Ranged,
		// Token: 0x040019A0 RID: 6560
		Thrown,
		// Token: 0x040019A1 RID: 6561
		Heal
	}
}
