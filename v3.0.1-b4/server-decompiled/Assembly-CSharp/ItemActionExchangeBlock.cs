using System;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020005AA RID: 1450
[Preserve]
public class ItemActionExchangeBlock : ItemAction
{
	// Token: 0x06002F62 RID: 12130 RVA: 0x0012F1CC File Offset: 0x0012D3CC
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

	// Token: 0x06002F63 RID: 12131 RVA: 0x0012F28C File Offset: 0x0012D48C
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

	// Token: 0x040024AD RID: 9389
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue sourceblock;

	// Token: 0x040024AE RID: 9390
	[PublicizedFrom(EAccessModifier.Protected)]
	public BlockValue targetBlock;
}
