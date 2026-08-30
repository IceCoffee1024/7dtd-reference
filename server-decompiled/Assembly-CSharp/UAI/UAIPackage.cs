using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200178B RID: 6027
	[Preserve]
	public class UAIPackage
	{
		// Token: 0x170016A2 RID: 5794
		// (get) Token: 0x0600BA97 RID: 47767 RVA: 0x00457397 File Offset: 0x00455597
		// (set) Token: 0x0600BA98 RID: 47768 RVA: 0x0045739F File Offset: 0x0045559F
		public string Name
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x170016A3 RID: 5795
		// (get) Token: 0x0600BA99 RID: 47769 RVA: 0x004573A8 File Offset: 0x004555A8
		// (set) Token: 0x0600BA9A RID: 47770 RVA: 0x004573B0 File Offset: 0x004555B0
		public float Weight
		{
			get; [PublicizedFrom(EAccessModifier.Private)]
			set;
		}

		// Token: 0x0600BA9B RID: 47771 RVA: 0x004573B9 File Offset: 0x004555B9
		public UAIPackage(string _name = "", float _weight = 1f)
		{
			this.Name = _name;
			this.Weight = _weight;
			this.actionList = new List<UAIAction>();
		}

		// Token: 0x0600BA9C RID: 47772 RVA: 0x004573DC File Offset: 0x004555DC
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

		// Token: 0x0600BA9D RID: 47773 RVA: 0x00457518 File Offset: 0x00455718
		public List<UAIAction> GetActions()
		{
			return this.actionList;
		}

		// Token: 0x0600BA9E RID: 47774 RVA: 0x00457520 File Offset: 0x00455720
		public void AddAction(UAIAction _action)
		{
			this.actionList.Add(_action);
		}

		// Token: 0x04008BE1 RID: 35809
		[PublicizedFrom(EAccessModifier.Private)]
		public List<UAIAction> actionList;
	}
}
