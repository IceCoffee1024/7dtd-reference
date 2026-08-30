using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001781 RID: 6017
	[Preserve]
	public class UAITaskMoveToTarget : UAITaskBase
	{
		// Token: 0x0600BA71 RID: 47729 RVA: 0x00456A34 File Offset: 0x00454C34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void initializeParameters()
		{
			base.initializeParameters();
			if (this.Parameters.ContainsKey("distance"))
			{
				this.distance = StringParsers.ParseFloat(this.Parameters["distance"], 0, -1, NumberStyles.Any);
			}
			if (this.Parameters.ContainsKey("run"))
			{
				this.run = StringParsers.ParseBool(this.Parameters["run"], 0, -1, true);
			}
			if (this.Parameters.ContainsKey("break_walls"))
			{
				this.shouldBreakWalls = StringParsers.ParseBool(this.Parameters["break_walls"], 0, -1, true);
			}
		}

		// Token: 0x0600BA72 RID: 47730 RVA: 0x00456ADC File Offset: 0x00454CDC
		public override void Start(Context _context)
		{
			base.Start(_context);
			EntityAlive entityAlive = UAIUtils.ConvertToEntityAlive(_context.ActionData.Target);
			if (entityAlive != null)
			{
				_context.Self.FindPath(RandomPositionGenerator.CalcNear(_context.Self, entityAlive.position, (int)this.distance, (int)this.distance), this.run ? _context.Self.GetMoveSpeedPanic() : (_context.Self.IsAlert ? _context.Self.GetMoveSpeedAggro() : _context.Self.GetMoveSpeed()), this.shouldBreakWalls, null);
				return;
			}
			if (_context.ActionData.Target.GetType() == typeof(Vector3))
			{
				_context.Self.FindPath(RandomPositionGenerator.CalcNear(_context.Self, (Vector3)_context.ActionData.Target, (int)this.distance, (int)this.distance), this.run ? _context.Self.GetMoveSpeedPanic() : _context.Self.GetMoveSpeed(), this.shouldBreakWalls, null);
				return;
			}
			this.Stop(_context);
		}

		// Token: 0x0600BA73 RID: 47731 RVA: 0x00456BFA File Offset: 0x00454DFA
		public override void Update(Context _context)
		{
			base.Update(_context);
			if (_context.Self.getNavigator().noPathAndNotPlanningOne())
			{
				this.Stop(_context);
			}
		}

		// Token: 0x04008BC0 RID: 35776
		public float distance;

		// Token: 0x04008BC1 RID: 35777
		public bool run;

		// Token: 0x04008BC2 RID: 35778
		public bool shouldBreakWalls;
	}
}
