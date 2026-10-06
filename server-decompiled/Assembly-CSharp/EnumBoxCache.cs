using System;
using System.Collections.Generic;

// Token: 0x02001119 RID: 4377
[PublicizedFrom(EAccessModifier.Internal)]
public sealed class EnumBoxCache<TEnum>
{
	// Token: 0x06008AA2 RID: 35490 RVA: 0x0034EBFB File Offset: 0x0034CDFB
	[PublicizedFrom(EAccessModifier.Private)]
	public EnumBoxCache()
	{
	}

	// Token: 0x06008AA3 RID: 35491 RVA: 0x0034EC10 File Offset: 0x0034CE10
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

	// Token: 0x04006708 RID: 26376
	public static readonly EnumBoxCache<TEnum> Instance = new EnumBoxCache<TEnum>();

	// Token: 0x04006709 RID: 26377
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Dictionary<TEnum, object> boxes = new Dictionary<TEnum, object>();
}
