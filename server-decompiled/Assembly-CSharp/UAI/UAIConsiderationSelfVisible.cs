using System;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001777 RID: 6007
	[Preserve]
	public class UAIConsiderationSelfVisible : UAIConsiderationBase
	{
		// Token: 0x0600BA4D RID: 47693 RVA: 0x004560F4 File Offset: 0x004542F4
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
