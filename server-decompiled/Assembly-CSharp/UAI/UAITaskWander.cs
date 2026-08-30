using System;
using System.Globalization;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001782 RID: 6018
	[Preserve]
	public class UAITaskWander : UAITaskBase
	{
		// Token: 0x0600BA75 RID: 47733 RVA: 0x00456C1C File Offset: 0x00454E1C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("max_distance"))
			{
				this.maxWanderDistance = StringParsers.ParseFloat(this.Parameters["max_distance"], 0, -1, NumberStyles.Any);
			}
		}

		// Token: 0x0600BA76 RID: 47734 RVA: 0x00456C58 File Offset: 0x00454E58
		public override void Start(Context _context)
		{
			base.Start(_context);
			int num = 10;
			_context.Self.FindPath(RandomPositionGenerator.CalcAround(_context.Self, num, num), _context.Self.GetMoveSpeed(), false, null);
		}

		// Token: 0x0600BA77 RID: 47735 RVA: 0x00456BFA File Offset: 0x00454DFA
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008BC3 RID: 35779
		public float maxWanderDistance;
	}
}
