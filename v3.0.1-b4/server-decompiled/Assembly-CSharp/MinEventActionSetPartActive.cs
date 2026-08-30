using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006F9 RID: 1785
[Preserve]
public class MinEventActionSetPartActive : MinEventActionBase
{
	// Token: 0x06003678 RID: 13944 RVA: 0x00167AA0 File Offset: 0x00165CA0
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

	// Token: 0x06003679 RID: 13945 RVA: 0x00167AF7 File Offset: 0x00165CF7
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && _params.ItemValue != null && this.partName != null;
	}

	// Token: 0x0600367A RID: 13946 RVA: 0x00167B24 File Offset: 0x00165D24
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

	// Token: 0x04002BC9 RID: 11209
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;

	// Token: 0x04002BCA RID: 11210
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isActive;

	// Token: 0x04002BCB RID: 11211
	[PublicizedFrom(EAccessModifier.Private)]
	public bool isInvert;

	// Token: 0x04002BCC RID: 11212
	[PublicizedFrom(EAccessModifier.Private)]
	public string cVarName;
}
