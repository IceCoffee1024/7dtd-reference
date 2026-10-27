using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017E8 RID: 6120
	[Preserve]
	public class UAIPackage
	{
		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x0600BD02 RID: 48386 RVA: 0x0045F263 File Offset: 0x0045D463
		// (set) Token: 0x0600BD03 RID: 48387 RVA: 0x0045F26B File Offset: 0x0045D46B
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x0600BD04 RID: 48388 RVA: 0x0045F274 File Offset: 0x0045D474
		// (set) Token: 0x0600BD05 RID: 48389 RVA: 0x0045F27C File Offset: 0x0045D47C
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600BD06 RID: 48390 RVA: 0x0045F285 File Offset: 0x0045D485
		public UAIPackage(string _name = "", float _weight = 1f)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.actionList = new List<UAIAction>();
		}

		// Token: 0x0600BD07 RID: 48391 RVA: 0x0045F2A8 File Offset: 0x0045D4A8
		public float DecideAction(Context _context, out UAIAction _chosenAction, out object _chosenTarget)
		{
			float num = 0f;
			_chosenAction = null;
			_chosenTarget = null;
			for (int i = 0; i < this.actionList.Count; i++)
			{
				int num2 = 0;
				int num3 = 0;
				while (num3 < _context.ConsiderationData.EntityTargets.Count && num2 <= UAIBase.MaxEntitiesToConsider)
				{
					float score = this.actionList[i].GetScore(_context, _context.ConsiderationData.EntityTargets[num3], 0f);
					if (score > num)
					{
						num = score;
						_chosenAction = this.actionList[i];
						_chosenTarget = _context.ConsiderationData.EntityTargets[num3];
					}
					num2++;
					num3++;
				}
				int num4 = 0;
				while (num4 < _context.ConsiderationData.WaypointTargets.Count && num4 <= UAIBase.MaxWaypointsToConsider)
				{
					float score2 = this.actionList[i].GetScore(_context, _context.ConsiderationData.WaypointTargets[num4], 0f);
					if (score2 > num)
					{
						num = score2;
						_chosenAction = this.actionList[i];
						_chosenTarget = _context.ConsiderationData.WaypointTargets[num4];
					}
					num4++;
				}
			}
			return num;
		}

		// Token: 0x0600BD08 RID: 48392 RVA: 0x0045F3E4 File Offset: 0x0045D5E4
		public List<UAIAction> GetActions()
		{
			return this.actionList;
		}

		// Token: 0x0600BD09 RID: 48393 RVA: 0x0045F3EC File Offset: 0x0045D5EC
		public void AddAction(UAIAction _action)
		{
			this.actionList.Add(_action);
		}

		// Token: 0x04008D8F RID: 36239
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIAction> actionList;
	}
}
