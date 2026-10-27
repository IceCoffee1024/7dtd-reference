using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017D7 RID: 6103
	[Preserve]
	public class UAIConsiderationTargetHealth : UAIConsiderationBase
	{
		// Token: 0x0600BCC0 RID: 48320 RVA: 0x0045E24C File Offset: 0x0045C44C
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
