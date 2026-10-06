using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A3 RID: 6051
	[Preserve]
	public class UAIConsiderationSelfVisible : UAIConsiderationBase
	{
		// Token: 0x0600BBA4 RID: 48036 RVA: 0x0045A614 File Offset: 0x00458814
		public override float GetScore(Context _context, object target)
		{
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(target);
			if (entityAlive != null)
			{
				float num = _context.Self.GetSeeDistance();
				num *= num;
				float num2 = 1f - UAIUtils.DistanceSqr(_context.Self.getHeadPosition(), entityAlive.getHeadPosition()) / num;
				return (float)(entityAlive.CanEntityBeSeen(_context.Self, true) ? 1 : 0) * num2;
			}
			return 0f;
		}
	}
}
