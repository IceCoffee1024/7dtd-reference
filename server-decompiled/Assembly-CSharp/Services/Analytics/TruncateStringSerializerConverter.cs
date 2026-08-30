using System;
using Newtonsoft.Json;

namespace Services.Analytics
{
	// Token: 0x02001682 RID: 5762
	public class TruncateStringSerializerConverter : JsonConverter<string>
	{
		// Token: 0x0600B48B RID: 46219 RVA: 0x004374A4 File Offset: 0x004356A4
		public TruncateStringSerializerConverter(int maxLength)
		{
			this._maxLength = maxLength;
		}

		// Token: 0x0600B48C RID: 46220 RVA: 0x004374B3 File Offset: 0x004356B3
		public TruncateStringSerializerConverter()
		{
			this._maxLength = 50;
		}

		// Token: 0x0600B48D RID: 46221 RVA: 0x004374C3 File Offset: 0x004356C3
		public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
		{
			if (string.IsNullOrEmpty(value))
			{
				writer.WriteNull();
				return;
			}
			writer.WriteValue((value.Length > this._maxLength) ? value.Substring(0, this._maxLength) : value);
		}

		// Token: 0x0600B48E RID: 46222 RVA: 0x0001FFE2 File Offset: 0x0001E1E2
		public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			return null;
		}

		// Token: 0x04008766 RID: 34662
		[PublicizedFrom(EAccessModifier.Private)]
		public const int DefaultMaxLength = 50;

		// Token: 0x04008767 RID: 34663
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly int _maxLength;
	}
}
