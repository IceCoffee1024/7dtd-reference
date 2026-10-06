using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A8 RID: 6056
	[Preserve]
	public class UAIConsiderationTargetVisible : UAIConsiderationBase
	{
		// Token: 0x0600BBB1 RID: 48049 RVA: 0x0045AA18 File Offset: 0x00458C18
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
