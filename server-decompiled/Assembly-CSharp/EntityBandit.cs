using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200048C RID: 1164
[Preserve]
public class EntityBandit : EntityHuman
{
	// Token: 0x0600240C RID: 9228 RVA: 0x000DA732 File Offset: 0x000D8932
	public override void InitInventory()
	{
		if (this.inventory == null)
		{
			this.inventory = new Inventory(this, 8);
		}
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x000DA74C File Offset: 0x000D894C
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

	// Token: 0x0600240E RID: 9230 RVA: 0x000DA818 File Offset: 0x000D8A18
	public override void OnAddedToWorld()
	{
		base.OnAddedToWorld();
	}

	// Token: 0x0600240F RID: 9231 RVA: 0x000DA820 File Offset: 0x000D8A20
	public override void SetupHandItem()
	{
		this.ShowHoldingItem(true);
	}

	// Token: 0x06002410 RID: 9232 RVA: 0x000DA82C File Offset: 0x000D8A2C
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

	// Token: 0x06002411 RID: 9233 RVA: 0x000DA898 File Offset: 0x000D8A98
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

	// Token: 0x06002412 RID: 9234 RVA: 0x000DA8C2 File Offset: 0x000D8AC2
	public override bool GetAimTarget(out Vector3 aimTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out aimTarget) || base.GetAimTarget(out aimTarget);
	}

	// Token: 0x06002413 RID: 9235 RVA: 0x000DA8DC File Offset: 0x000D8ADC
	public override bool GetHeadLookTarget(out Vector3 lookTarget)
	{
		return AIFocusAim.GetActiveFocus(this, this.focusAim, out lookTarget) || base.GetHeadLookTarget(out lookTarget);
	}

	// Token: 0x06002414 RID: 9236 RVA: 0x000DA8F8 File Offset: 0x000D8AF8
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

	// Token: 0x0400198B RID: 6539
	public readonly AIFocus<AIFocusBody> focusBody = new AIFocus<AIFocusBody>(true);

	// Token: 0x0400198C RID: 6540
	public readonly AIFocus<AIFocusAim> focusAim = new AIFocus<AIFocusAim>(true);

	// Token: 0x0200048D RID: 1165
	[PublicizedFrom(EAccessModifier.Private)]
	public enum InvSlots
	{
		// Token: 0x0400198E RID: 6542
		Melee,
		// Token: 0x0400198F RID: 6543
		Ranged,
		// Token: 0x04001990 RID: 6544
		Thrown,
		// Token: 0x04001991 RID: 6545
		Heal
	}
}
