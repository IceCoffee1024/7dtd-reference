using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x0200177B RID: 6011
	[Preserve]
	public class UAIConsiderationTargetType : UAIConsiderationBase
	{
		// Token: 0x0600BA57 RID: 47703 RVA: 0x00456404 File Offset: 0x00454604
		public override void Init(Dictionary<string, string> parameters)
		{
			base.Init(parameters);
			if (parameters.ContainsKey("type"))
			{
				this.type = parameters["type"].Split(',', StringSplitOptions.None);
				for (int i = 0; i < this.type.Length; i++)
				{
					this.type[i] = this.type[i].Trim();
				}
			}
		}

		// Token: 0x0600BA58 RID: 47704 RVA: 0x00456468 File Offset: 0x00454668
		public override float GetScore(Context _context, object target)
		{
			for (int i = 0; i < this.type.Length; i++)
			{
				Type type = Type.GetType(this.type[i]);
				if (type.IsAssignableFrom(target.GetType()))
				{
					return 1f;
				}
				if (target.GetType() == typeof(Vector3) && type.IsAssignableFrom(_context.World.GetBlock(new Vector3i((Vector3)target)).Block.GetType()))
				{
					return 1f;
				}
			}
			return 0f;
		}

		// Token: 0x04008BB9 RID: 35769
		public string[] type;
	}
}
