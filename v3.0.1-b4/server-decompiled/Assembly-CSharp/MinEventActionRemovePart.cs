using System;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006D8 RID: 1752
[Preserve]
public class MinEventActionRemovePart : MinEventActionTargetedBase
{
	// Token: 0x060035FA RID: 13818 RVA: 0x001651C6 File Offset: 0x001633C6
	public override void Execute(MinEventParams _params)
	{
		if (_params.Self == null)
		{
			return;
		}
		_params.Self.RemovePart(this.partName);
	}

	// Token: 0x060035FB RID: 13819 RVA: 0x001651E8 File Offset: 0x001633E8
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && _params.Self != null && this.partName != null;
	}

	// Token: 0x060035FC RID: 13820 RVA: 0x00165210 File Offset: 0x00163410
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag && _attribute.Name.LocalName == "part")
		{
			this.partName = _attribute.Value;
			return true;
		}
		return flag;
	}

	// Token: 0x04002B71 RID: 11121
	[PublicizedFrom(EAccessModifier.Private)]
	public string partName;
}
