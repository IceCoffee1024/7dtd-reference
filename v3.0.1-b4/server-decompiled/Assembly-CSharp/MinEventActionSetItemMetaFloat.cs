using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x020006FC RID: 1788
[Preserve]
public class MinEventActionSetItemMetaFloat : MinEventActionBase
{
	// Token: 0x06003684 RID: 13956 RVA: 0x00167EE8 File Offset: 0x001660E8
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

	// Token: 0x06003685 RID: 13957 RVA: 0x00167F84 File Offset: 0x00166184
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && !string.IsNullOrEmpty(this.metaKey);
	}

	// Token: 0x06003686 RID: 13958 RVA: 0x00167FA4 File Offset: 0x001661A4
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

	// Token: 0x04002BD3 RID: 11219
	[PublicizedFrom(EAccessModifier.Private)]
	public float change;

	// Token: 0x04002BD4 RID: 11220
	[PublicizedFrom(EAccessModifier.Private)]
	public bool relative = true;

	// Token: 0x04002BD5 RID: 11221
	[PublicizedFrom(EAccessModifier.Private)]
	public string metaKey;
}
