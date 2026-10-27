using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017E5 RID: 6117
	[Preserve]
	public class Context
	{
		// Token: 0x1700178A RID: 6026
		// (get) Token: 0x0600BCFD RID: 48381 RVA: 0x0045F170 File Offset: 0x0045D370
		public List<string> AIPackages
		{
			get
			{
				return this.Self.AIPackages;
			}
		}

		// Token: 0x0600BCFE RID: 48382 RVA: 0x0045F17D File Offset: 0x0045D37D
		public Context(EntityAlive _self)
		{
			this.Self = _self;
			this.World = GameManager.Instance.World;
			this.ConsiderationData = new ConsiderationData();
			this.ActionData = default(ActionData);
		}

		// Token: 0x04008D7C RID: 36220
		public EntityAlive Self;

		// Token: 0x04008D7D RID: 36221
		public World World;

		// Token: 0x04008D7E RID: 36222
		public ConsiderationData ConsiderationData;

		// Token: 0x04008D7F RID: 36223
		public ActionData ActionData;

		// Token: 0x04008D80 RID: 36224
		public float updateTimer;
	}
}
