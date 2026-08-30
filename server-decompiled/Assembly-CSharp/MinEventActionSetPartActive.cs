using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000703 RID: 1795
[Preserve]
public class MinEventActionSetPartActive : MinEventActionBase
{
	// Token: 0x060036CE RID: 14030 RVA: 0x00169C78 File Offset: 0x00167E78
	public override void Execute(MinEventParams _params)
	{
		bool flag = this.isActive;
		if (this.cVarName != null)
		{
			flag = _params.Self.GetCVar(this.cVarName) != 0f;
			if (this.isInvert)
			{
				flag = !flag;
			}
		}
		_params.Self.SetPartActive(this.partName, flag);
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x00169CCF File Offset: 0x00167ECF
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.partName != null;
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x00169CFC File Offset: 0x00167EFC
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "active")
			{
				if (_attribute.Value.Length >= 2 && _attribute.Value[0] == '@')
				{
					int num = 1;
					if (_attribute.Value[1] == '!')
					{
						num++;
						this.isInvert = true;
					}
					this.cVarName = _attribute.Value.Substring(num);
				}
				else
				{
					this.isActive = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				}
				return true;
			}
			if (localName == "part")
			{
				this.partName = _attribute.Value;
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002C3D RID: 11325
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;

	// Token: 0x04002C3E RID: 11326
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;

	// Token: 0x04002C3F RID: 11327
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvert;

	// Token: 0x04002C40 RID: 11328
	[PublicizedFrom(EAccessModifier.Private)]
	public string cVarName;
}
