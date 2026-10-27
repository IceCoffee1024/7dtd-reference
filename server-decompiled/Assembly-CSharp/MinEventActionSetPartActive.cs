using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x0200070D RID: 1805
[Preserve]
public class MinEventActionSetPartActive : MinEventActionBase
{
	// Token: 0x06003728 RID: 14120 RVA: 0x0016B574 File Offset: 0x00169774
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

	// Token: 0x06003729 RID: 14121 RVA: 0x0016B5CB File Offset: 0x001697CB
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.partName != null;
	}

	// Token: 0x0600372A RID: 14122 RVA: 0x0016B5F8 File Offset: 0x001697F8
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

	// Token: 0x04002C52 RID: 11346
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;

	// Token: 0x04002C53 RID: 11347
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;

	// Token: 0x04002C54 RID: 11348
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvert;

	// Token: 0x04002C55 RID: 11349
	[PublicizedFrom(EAccessModifier.Private)]
	public string cVarName;
}
