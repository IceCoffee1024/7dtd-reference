using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

// Token: 0x020006C3 RID: 1731
public class MinEventActionBuffModifierBase : MinEventActionTargetedBase
{
	// Token: 0x060035BE RID: 13758 RVA: 0x0016356C File Offset: 0x0016176C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "buffs" || localName == "buff")
			{
				if (_attribute.Value.Contains(","))
				{
					this.buffNames = _attribute.Value.Replace(" ", "").Split(',', StringSplitOptions.None);
				}
				else
				{
					this.buffNames = new string[] { _attribute.Value };
				}
				return true;
			}
			if (localName == "weights" || localName == "weight")
			{
				if (_attribute.Value.Contains(","))
				{
					string[] array = _attribute.Value.Replace(" ", "").Split(',', StringSplitOptions.None);
					this.buffWeights = new float[array.Length];
					for (int i = 0; i < array.Length; i++)
					{
						this.buffWeights[i] = StringParsers.ParseFloat(array[i], 0, -1, NumberStyles.Any);
					}
				}
				else
				{
					this.buffNames = new string[] { _attribute.Value };
					this.buffWeights = new float[] { 1f };
				}
				return true;
			}
			if (localName == "fireOneBuff")
			{
				this.buffOneOnly = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				return true;
			}
		}
		return flag;
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x001636D4 File Offset: 0x001618D4
	public override void ParseXMLPostProcess()
	{
		base.ParseXMLPostProcess();
		if (this.buffOneOnly && this.buffWeights != null)
		{
			float weightSum = this.buffWeights.Sum();
			this.buffWeights = this.buffWeights.Select<float, float>((float w) => w / weightSum).ToArray<float>();
		}
		if (!this.buffOneOnly && this.buffWeights != null)
		{
			if (this.buffWeights.Any<float>((float w) => w > 1f || w < 0f))
			{
				Log.Warning("Warning: Invalid \"Buffs.xml\" configuration. User has specified weights outside of range 0-1 and fireOneBuff=\"false\" or missing. When fireOneBuff=\"false\", the weights represent probabilities between 0-1 for the buffs to be added.");
			}
		}
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x00163778 File Offset: 0x00161978
	[PublicizedFrom(EAccessModifier.Protected)]
	public void Remove(MinEventParams _params)
	{
		bool flag = !_params.Self.isEntityRemote | _params.IsLocal;
		int num = ((_params.Self != null) ? _params.Self.entityId : (-1));
		for (int i = 0; i < this.buffNames.Length; i++)
		{
			string text = this.buffNames[i];
			if (BuffManager.GetBuff(text) != null)
			{
				for (int j = 0; j < this.targets.Count; j++)
				{
					this.targets[j].Buffs.RemoveBuff(text, num, flag);
				}
			}
		}
	}

	// Token: 0x04002B37 RID: 11063
	[PublicizedFrom(EAccessModifier.Protected)]
	public string[] buffNames;

	// Token: 0x04002B38 RID: 11064
	[PublicizedFrom(EAccessModifier.Protected)]
	public float[] buffWeights;

	// Token: 0x04002B39 RID: 11065
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool buffOneOnly;
}
