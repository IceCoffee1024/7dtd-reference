using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005AF RID: 1455
[Preserve]
public class ItemActionExchangeBlock : ItemAction
{
	// Token: 0x06002F90 RID: 12176 RVA: 0x0013023C File Offset: 0x0012E43C
	public override void ReadFrom(DynamicProperties _props)
	{
		base.ReadFrom(_props);
		if (!_props.Values.ContainsKey("Sourceblock"))
		{
			throw new Exception("Missing attribute 'sourceblock' in use_action 'ExchangeBlock'");
		}
		string text = _props.Values["Sourceblock"];
		this.sourceblock = ItemClass.GetItem(text, false).ToBlockValue(false);
		if (this.sourceblock.Equals(BlockValue.Air))
		{
			throw new Exception("Unknown block name '" + text + "' in use_action!");
		}
		if (!_props.Values.ContainsKey("Targetblock"))
		{
			throw new Exception("Missing attribute 'targetblock' in use_action 'ExchangeBlock'");
		}
		text = _props.Values["Targetblock"];
		this.targetBlock = ItemClass.GetItem(text, false).ToBlockValue(false);
	}

	// Token: 0x06002F91 RID: 12177 RVA: 0x001302FC File Offset: 0x0012E4FC
	public override void ExecuteAction(ItemActionData _actionData, bool _bReleased)
	{
		if (_bReleased)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime < this.Delay)
		{
			return;
		}
		if (Time.time - _actionData.lastUseTime > Constants.cBuildIntervall)
		{
			ItemInventoryData invData = _actionData.invData;
			WorldRayHitInfo hitInfo = ((EntityPlayerLocal)invData.holdingEntity).HitInfo;
			if (hitInfo.bHitValid && GameUtils.IsBlockOrTerrain(hitInfo.tag))
			{
				Vector3i blockPos = hitInfo.hit.blockPos;
				BlockValue block = invData.world.GetBlock(blockPos);
				if (block.Equals(this.sourceblock))
				{
					_actionData.lastUseTime = Time.time;
					if (block.type != this.targetBlock.type)
					{
						invData.world.SetBlockRPC(blockPos, this.targetBlock);
						invData.holdingEntity.PlayOneShot((this.soundStart != null) ? this.soundStart : "placeblock", false, false, false, null, 1f);
					}
				}
				invData.holdingEntity.RightArmAnimationAttack = true;
			}
		}
	}

	// Token: 0x040024E1 RID: 9441
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue sourceblock;

	// Token: 0x040024E2 RID: 9442
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue targetBlock;
}
