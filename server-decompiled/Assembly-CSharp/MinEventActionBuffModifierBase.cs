using System;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

// Token: 0x020006CD RID: 1741
public class MinEventActionBuffModifierBase : MinEventActionTargetedBase
{
	// Token: 0x06003614 RID: 13844 RVA: 0x00165744 File Offset: 0x00163944
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

	// Token: 0x06003615 RID: 13845 RVA: 0x001658AC File Offset: 0x00163AAC
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

	// Token: 0x06003616 RID: 13846 RVA: 0x00165950 File Offset: 0x00163B50
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

	// Token: 0x04002BAB RID: 11179
	[PublicizedFrom(EAccessModifier.Protected)]
	public string[] buffNames;

	// Token: 0x04002BAC RID: 11180
	[PublicizedFrom(EAccessModifier.Protected)]
	public float[] buffWeights;

	// Token: 0x04002BAD RID: 11181
	[PublicizedFrom(EAccessModifier.Protected)]
	public bool buffOneOnly;
}
