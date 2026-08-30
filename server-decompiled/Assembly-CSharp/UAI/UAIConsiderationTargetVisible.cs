using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001756 RID: 5974
	[Preserve]
	public class UAIConsiderationTargetVisible : UAIConsiderationBase
	{
		// Token: 0x0600B8D7 RID: 47319 RVA: 0x00450780 File Offset: 0x0044E980
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
