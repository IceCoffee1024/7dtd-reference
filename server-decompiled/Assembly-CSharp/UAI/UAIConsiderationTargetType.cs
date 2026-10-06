using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017A7 RID: 6055
	[Preserve]
	public class UAIConsiderationTargetType : UAIConsiderationBase
	{
		// Token: 0x0600BBAE RID: 48046 RVA: 0x0045A924 File Offset: 0x00458B24
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

		// Token: 0x0600BBAF RID: 48047 RVA: 0x0045A988 File Offset: 0x00458B88
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

		// Token: 0x04008C7E RID: 35966
		public string[] type;
	}
}
