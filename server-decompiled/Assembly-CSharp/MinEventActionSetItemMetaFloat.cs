using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000706 RID: 1798
[Preserve]
public class MinEventActionSetItemMetaFloat : MinEventActionBase
{
	// Token: 0x060036DA RID: 14042 RVA: 0x0016A0C0 File Offset: 0x001682C0
	public override void Execute(MinEventParams _params)
	{
		ItemValue itemValue = _params.ItemValue;
		if (!itemValue.HasMetadata(this.metaKey, TypedMetadataValue.TypeTag.None))
		{
			itemValue.SetMetadata(this.metaKey, 0f);
		}
		object metadata = itemValue.GetMetadata(this.metaKey);
		if (metadata is float)
		{
			float num = (float)metadata;
			if (this.relative)
			{
				itemValue.SetMetadata(this.metaKey, num + this.change);
			}
			else
			{
				itemValue.SetMetadata(this.metaKey, this.change);
			}
			if (num < 0f)
			{
				itemValue.SetMetadata(this.metaKey, 0f);
			}
			return;
		}
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x0016A15C File Offset: 0x0016835C
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && !string.IsNullOrEmpty(this.metaKey);
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x0016A17C File Offset: 0x0016837C
	public override bool ParseXmlAttribute(XAttribute _attribute)
	{
		bool flag = base.ParseXmlAttribute(_attribute);
		if (!flag)
		{
			string localName = _attribute.Name.LocalName;
			if (localName == "change")
			{
				this.change = StringParsers.ParseFloat(_attribute.Value, 0, -1, NumberStyles.Any);
				return true;
			}
			if (localName == "relative")
			{
				this.relative = StringParsers.ParseBool(_attribute.Value, 0, -1, true);
				return true;
			}
			if (localName == "key")
			{
				this.metaKey = _attribute.Value;
				return true;
			}
		}
		return flag;
	}

	// Token: 0x04002C47 RID: 11335
	[PublicizedFrom(EAccessModifier.Private)]
	public float change;

	// Token: 0x04002C48 RID: 11336
	[PublicizedFrom(EAccessModifier.Private)]
	public bool relative = true;

	// Token: 0x04002C49 RID: 11337
	[PublicizedFrom(EAccessModifier.Private)]
	public string metaKey;
}
