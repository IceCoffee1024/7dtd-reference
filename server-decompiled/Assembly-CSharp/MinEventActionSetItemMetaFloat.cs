using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000708 RID: 1800
[Preserve]
public class MinEventActionSetItemMetaFloat : MinEventActionBase
{
	// Token: 0x0600371D RID: 14109 RVA: 0x0016AA24 File Offset: 0x00168C24
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

	// Token: 0x0600371E RID: 14110 RVA: 0x0016AAC0 File Offset: 0x00168CC0
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && !string.IsNullOrEmpty(this.metaKey);
	}

	// Token: 0x0600371F RID: 14111 RVA: 0x0016AAE0 File Offset: 0x00168CE0
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

	// Token: 0x04002C40 RID: 11328
	[PublicizedFrom(EAccessModifier.Private)]
	public float change;

	// Token: 0x04002C41 RID: 11329
	[PublicizedFrom(EAccessModifier.Private)]
	public bool relative = true;

	// Token: 0x04002C42 RID: 11330
	[PublicizedFrom(EAccessModifier.Private)]
	public string metaKey;
}
