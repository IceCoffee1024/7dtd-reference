using System;
using System.Globalization;
using System.Xml.Linq;
using UnityEngine.Scripting;

// Token: 0x02000710 RID: 1808
[Preserve]
public class MinEventActionSetItemMetaFloat : MinEventActionBase
{
	// Token: 0x06003734 RID: 14132 RVA: 0x0016B9BC File Offset: 0x00169BBC
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

	// Token: 0x06003735 RID: 14133 RVA: 0x0016BA58 File Offset: 0x00169C58
	public override bool CanExecute(MinEventTypes _eventType, MinEventParams _params)
	{
		return base.CanExecute(_eventType, _params) && !string.IsNullOrEmpty(this.metaKey);
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x0016BA78 File Offset: 0x00169C78
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

	// Token: 0x04002C5C RID: 11356
	[PublicizedFrom(EAccessModifier.Private)]
	public float change;

	// Token: 0x04002C5D RID: 11357
	[PublicizedFrom(EAccessModifier.Private)]
	public bool relative = true;

	// Token: 0x04002C5E RID: 11358
	[PublicizedFrom(EAccessModifier.Private)]
	public string metaKey;
}
