using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017DF RID: 6111
	[Preserve]
	public class UAITaskWander : UAITaskBase
	{
		// Token: 0x0600BCE0 RID: 48352 RVA: 0x0045EAE8 File Offset: 0x0045CCE8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxWanderDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600BCE1 RID: 48353 RVA: 0x0045EB24 File Offset: 0x0045CD24
		public override void Start(Context _context)
		{
			base.Start(_context);
			int num = 10;
			_context.Self.FindPath(RandomPositionGenerator.CalcAround(_context.Self, num, num), _context.Self.GetMoveSpeed(), false, null);
		}

		// Token: 0x0600BCE2 RID: 48354 RVA: 0x0045EAC6 File Offset: 0x0045CCC6
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008D71 RID: 36209
		public float maxWanderDistance;
	}
}
