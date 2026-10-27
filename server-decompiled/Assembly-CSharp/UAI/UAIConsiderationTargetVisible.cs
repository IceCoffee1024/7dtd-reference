using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017D9 RID: 6105
	[Preserve]
	public class UAIConsiderationTargetVisible : UAIConsiderationBase
	{
		// Token: 0x0600BCC5 RID: 48325 RVA: 0x0045E3C4 File Offset: 0x0045C5C4
		public override float GetScore(Context _context, object target)
		{
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
			if (entityAlive != null)
			{
				return (float)(_context.Self.CanEntityBeSeen(entityAlive, true) ? 1 : 0);
			}
			if (target.GetType() == typeof(Vector3))
			{
				return (float)(_context.Self.CanSee((Vector3)target) ? 1 : 0);
			}
			return 0f;
		}
	}
}
