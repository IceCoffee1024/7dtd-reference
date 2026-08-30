using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001754 RID: 5972
	[Preserve]
	public class UAIConsiderationTargetHealth : UAIConsiderationBase
	{
		// Token: 0x0600B8D2 RID: 47314 RVA: 0x00450608 File Offset: 0x0044E808
		public override float GetScore(Context _context, object target)
		{
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
			if (entityAlive != null)
			{
				return (float)entityAlive.Health / (float)entityAlive.GetMaxHealth();
			}
			if (target.GetType() == typeof(Vector3))
			{
				BlockValue block = _context.Self.world.GetBlock(new Vector3i((Vector3)target));
				Block block2 = block.Block;
				return (float)(block2.MaxDamage - block.damage) / (float)block2.MaxDamage;
			}
			return 0f;
		}
	}
}
