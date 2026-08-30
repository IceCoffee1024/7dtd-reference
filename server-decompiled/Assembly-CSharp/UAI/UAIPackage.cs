using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001765 RID: 5989
	[Preserve]
	public class UAIPackage
	{
		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x0600B914 RID: 47380 RVA: 0x0045161F File Offset: 0x0044F81F
		// (set) Token: 0x0600B915 RID: 47381 RVA: 0x00451627 File Offset: 0x0044F827
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x0600B916 RID: 47382 RVA: 0x00451630 File Offset: 0x0044F830
		// (set) Token: 0x0600B917 RID: 47383 RVA: 0x00451638 File Offset: 0x0044F838
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600B918 RID: 47384 RVA: 0x00451641 File Offset: 0x0044F841
		public UAIPackage(string _name = "", float _weight = 1f)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.actionList = new List<UAIAction>();
		}

		// Token: 0x0600B919 RID: 47385 RVA: 0x00451664 File Offset: 0x0044F864
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

		// Token: 0x0600B91A RID: 47386 RVA: 0x004517A0 File Offset: 0x0044F9A0
		public List<UAIAction> GetActions()
		{
			return this.actionList;
		}

		// Token: 0x0600B91B RID: 47387 RVA: 0x004517A8 File Offset: 0x0044F9A8
		public void AddAction(UAIAction _action)
		{
			this.actionList.Add(_action);
		}

		// Token: 0x04008AC5 RID: 35525
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIAction> actionList;
	}
}
