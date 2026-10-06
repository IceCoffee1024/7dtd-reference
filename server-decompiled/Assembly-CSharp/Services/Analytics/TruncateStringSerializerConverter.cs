using System;
using Newtonsoft.Json;

namespace Services.Analytics
{
	// Token: 0x020016A2 RID: 5794
	public class TruncateStringSerializerConverter : JsonConverter<string>
	{
		// Token: 0x0600B525 RID: 46373 RVA: 0x0043B1B8 File Offset: 0x004393B8
		public TruncateStringSerializerConverter(int maxLength)
		{
			this._maxLength = maxLength;
		}

		// Token: 0x0600B526 RID: 46374 RVA: 0x0043B1C7 File Offset: 0x004393C7
		public TruncateStringSerializerConverter()
		{
			this._maxLength = 50;
		}

		// Token: 0x0600B527 RID: 46375 RVA: 0x0043B1D7 File Offset: 0x004393D7
		public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
		{
			if (string.IsNullOrEmpty(value))
			{
				writer.WriteNull();
				return;
			}
			writer.WriteValue((value.Length > this._maxLength) ? value.Substring(0, this._maxLength) : value);
		}

		// Token: 0x0600B528 RID: 46376 RVA: 0x000200DA File Offset: 0x0001E2DA
		public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}

		// Token: 0x040087BD RID: 34749
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultMaxLength = 50;

		// Token: 0x040087BE RID: 34750
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int _maxLength;
	}
}
