using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace UAI
{
	// Token: 0x02001755 RID: 5973
	[Preserve]
	public class UAIConsiderationTargetType : UAIConsiderationBase
	{
		// Token: 0x0600B8D4 RID: 47316 RVA: 0x0045068C File Offset: 0x0044E88C
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

		// Token: 0x0600B8D5 RID: 47317 RVA: 0x004506F0 File Offset: 0x0044E8F0
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

		// Token: 0x04008A9D RID: 35485
		public string[] type;
	}
}
