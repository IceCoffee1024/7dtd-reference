using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017AE RID: 6062
	[Preserve]
	public class UAITaskWander : UAITaskBase
	{
		// Token: 0x0600BBCC RID: 48076 RVA: 0x0045B13C File Offset: 0x0045933C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxWanderDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600BBCD RID: 48077 RVA: 0x0045B178 File Offset: 0x00459378
		public override void Start(Context _context)
		{
			base.Start(_context);
			int num = 10;
			_context.Self.FindPath(RandomPositionGenerator.CalcAround(_context.Self, num, num), _context.Self.GetMoveSpeed(), false, null);
		}

		// Token: 0x0600BBCE RID: 48078 RVA: 0x0045B11A File Offset: 0x0045931A
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008C88 RID: 35976
		public float maxWanderDistance;
	}
}
