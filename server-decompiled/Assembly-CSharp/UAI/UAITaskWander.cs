using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200175C RID: 5980
	[Preserve]
	public class UAITaskWander : UAITaskBase
	{
		// Token: 0x0600B8F2 RID: 47346 RVA: 0x00450EA4 File Offset: 0x0044F0A4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxWanderDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600B8F3 RID: 47347 RVA: 0x00450EE0 File Offset: 0x0044F0E0
		public override void Start(Context _context)
		{
			base.Start(_context);
			int num = 10;
			_context.Self.FindPath(RandomPositionGenerator.CalcAround(_context.Self, num, num), _context.Self.GetMoveSpeed(), false, null);
		}

		// Token: 0x0600B8F4 RID: 47348 RVA: 0x00450E82 File Offset: 0x0044F082
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008AA7 RID: 35495
		public float maxWanderDistance;
	}
}
