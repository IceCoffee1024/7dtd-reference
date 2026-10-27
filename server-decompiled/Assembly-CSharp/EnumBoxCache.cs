using System;
using System.Collections.Generic;

// Token: 0x02001129 RID: 4393
[PublicizedFrom(EAccessModifier.Internal)]
public sealed class EnumBoxCache<TEnum>
{
	// Token: 0x06008AF6 RID: 35574 RVA: 0x00350CAB File Offset: 0x0034EEAB
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBoxCache()
	{
	}

	// Token: 0x06008AF7 RID: 35575 RVA: 0x00350CC0 File Offset: 0x0034EEC0
	public object GetBoxed(TEnum _value)
	{
		object obj;
		if (!this.boxes.TryGetValue(_value, out obj))
		{
			obj = _value;
			this.boxes[_value] = obj;
		}
		return obj;
	}

	// Token: 0x04006748 RID: 26440
	public static readonly EnumBoxCache<TEnum> Instance = new EnumBoxCache<TEnum>();

	// Token: 0x04006749 RID: 26441
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TEnum, object> boxes = new Dictionary<TEnum, object>();
}
