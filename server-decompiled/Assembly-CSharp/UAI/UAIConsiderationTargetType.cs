using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x020017D8 RID: 6104
	[Preserve]
	public class UAIConsiderationTargetType : UAIConsiderationBase
	{
		// Token: 0x0600BCC2 RID: 48322 RVA: 0x0045E2D0 File Offset: 0x0045C4D0
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

		// Token: 0x0600BCC3 RID: 48323 RVA: 0x0045E334 File Offset: 0x0045C534
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

		// Token: 0x04008D67 RID: 36199
		public string[] type;
	}
}
