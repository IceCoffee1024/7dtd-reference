using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200177C RID: 6012
	[Preserve]
	public class UAIConsiderationTargetVisible : UAIConsiderationBase
	{
		// Token: 0x0600BA5A RID: 47706 RVA: 0x004564F8 File Offset: 0x004546F8
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
