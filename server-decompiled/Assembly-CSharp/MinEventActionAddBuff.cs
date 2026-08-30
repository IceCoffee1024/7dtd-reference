using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D1 RID: 1745
[Preserve]
public class MinEventActionAddBuff : MinEventActionBuffModifierBase
{
	// Token: 0x0600361F RID: 13855 RVA: 0x00165AAC File Offset: 0x00163CAC
	public override void Execute(MinEventParams _params)
	{
		bool flag = !_params.Self.isEntityRemote | _params.IsLocal;
		int num = -1;
		if (_params.Buff != null)
		{
			num = _params.Buff.InstigatorId;
		}
		if (num == -1)
		{
			num = _params.Self.entityId;
		}
		for (int i = 0; i < this.targets.Count; i++)
		{
			string[] array = this.buffNames;
			if (this.buffOneOnly && this.buffWeights != null)
			{
				float randomFloat = this.targets[i].rand.RandomFloat;
				float num2 = 0f;
				for (int j = 0; j < this.buffWeights.Length; j++)
				{
					num2 += this.buffWeights[j];
					if (num2 >= randomFloat)
					{
						array = new string[] { this.buffNames[j] };
						break;
					}
				}
			}
			else if (this.buffWeights != null)
			{
				List<string> list = new List<string>();
				for (int k = 0; k < this.buffWeights.Length; k++)
				{
					float randomFloat2 = this.targets[i].rand.RandomFloat;
					if (this.buffWeights[k] >= randomFloat2)
					{
						list.Add(this.buffNames[k]);
					}
				}
				array = list.ToArray();
			}
			foreach (string text in array)
			{
				BuffClass buff = BuffManager.GetBuff(text);
				if (buff != null)
				{
					if (this.durationAltered && this.cvarRef)
					{
						if (this.targets[i].Buffs.HasCustomVar(this.refCvarName))
						{
							this.duration = this.targets[i].Buffs.GetCustomVar(this.refCvarName);
						}
						else
						{
							this.duration = buff.InitialDurationMax;
						}
					}
					if (this.durationAltered)
					{
						this.targets[i].Buffs.AddBuff(text, num, flag, false, this.duration);
					}
					else
					{
						this.targets[i].Buffs.AddBuff(text, num, flag, false, -1f);
					}
				}
			}
		}
	}

	// Token: 0x06003620 RID: 13856 RVA: 0x00165CC8 File Offset: 0x00163EC8
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "duration")
		{
			if (_attribute.Value.StartsWith("@"))
			{
				this.cvarRef = true;
				this.refCvarName = _attribute.Value.Substring(1);
			}
			else
			{
				this.duration = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
			}
			this.durationAltered = true;
			return true;
		}
		return flag;
	}

	// Token: 0x04002BB1 RID: 11185
	[PublicizedFrom(EAccessModifier.Private)]
	public float duration;

	// Token: 0x04002BB2 RID: 11186
	[PublicizedFrom(EAccessModifier.Private)]
	public bool durationAltered;

	// Token: 0x04002BB3 RID: 11187
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cvarRef;

	// Token: 0x04002BB4 RID: 11188
	[PublicizedFrom(EAccessModifier.Private)]
	public string refCvarName = string.Empty;
}
