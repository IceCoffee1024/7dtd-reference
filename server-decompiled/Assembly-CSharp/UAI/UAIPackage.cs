using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017B7 RID: 6071
	[Preserve]
	public class UAIPackage
	{
		// Token: 0x17001744 RID: 5956
		// (get) Token: 0x0600BBEE RID: 48110 RVA: 0x0045B8B7 File Offset: 0x00459AB7
		// (set) Token: 0x0600BBEF RID: 48111 RVA: 0x0045B8BF File Offset: 0x00459ABF
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001745 RID: 5957
		// (get) Token: 0x0600BBF0 RID: 48112 RVA: 0x0045B8C8 File Offset: 0x00459AC8
		// (set) Token: 0x0600BBF1 RID: 48113 RVA: 0x0045B8D0 File Offset: 0x00459AD0
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600BBF2 RID: 48114 RVA: 0x0045B8D9 File Offset: 0x00459AD9
		public UAIPackage(string _name = "", float _weight = 1f)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.actionList = new List<UAIAction>();
		}

		// Token: 0x0600BBF3 RID: 48115 RVA: 0x0045B8FC File Offset: 0x00459AFC
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

		// Token: 0x0600BBF4 RID: 48116 RVA: 0x0045BA38 File Offset: 0x00459C38
		public List<UAIAction> GetActions()
		{
			return this.actionList;
		}

		// Token: 0x0600BBF5 RID: 48117 RVA: 0x0045BA40 File Offset: 0x00459C40
		public void AddAction(UAIAction _action)
		{
			this.actionList.Add(_action);
		}

		// Token: 0x04008CA6 RID: 36006
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIAction> actionList;
	}
}
