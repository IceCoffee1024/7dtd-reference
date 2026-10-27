using System;
using Newtonsoft.Json;

namespace Services.Analytics
{
	// Token: 0x020016B4 RID: 5812
	public class TruncateStringSerializerConverter : JsonConverter<string>
	{
		// Token: 0x0600B580 RID: 46464 RVA: 0x0043D66C File Offset: 0x0043B86C
		public TruncateStringSerializerConverter(int maxLength)
		{
			this._maxLength = maxLength;
		}

		// Token: 0x0600B581 RID: 46465 RVA: 0x0043D67B File Offset: 0x0043B87B
		public TruncateStringSerializerConverter()
		{
			this._maxLength = 50;
		}

		// Token: 0x0600B582 RID: 46466 RVA: 0x0043D68B File Offset: 0x0043B88B
		public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
		{
			if (string.IsNullOrEmpty(value))
			{
				writer.WriteNull();
				return;
			}
			writer.WriteValue((value.Length > this._maxLength) ? value.Substring(0, this._maxLength) : value);
		}

		// Token: 0x0600B583 RID: 46467 RVA: 0x000200DA File Offset: 0x0001E2DA
		public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}

		// Token: 0x04008800 RID: 34816
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultMaxLength = 50;

		// Token: 0x04008801 RID: 34817
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int _maxLength;
	}
}
