using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000705 RID: 1797
[Preserve]
public class MinEventActionSetPartActive : MinEventActionBase
{
	// Token: 0x06003711 RID: 14097 RVA: 0x0016A5DC File Offset: 0x001687DC
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

	// Token: 0x06003712 RID: 14098 RVA: 0x0016A633 File Offset: 0x00168833
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.partName != null;
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x0016A660 File Offset: 0x00168860
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

	// Token: 0x04002C36 RID: 11318
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;

	// Token: 0x04002C37 RID: 11319
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;

	// Token: 0x04002C38 RID: 11320
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvert;

	// Token: 0x04002C39 RID: 11321
	[PublicizedFrom(EAccessModifier.Private)]
	public string cVarName;
}
