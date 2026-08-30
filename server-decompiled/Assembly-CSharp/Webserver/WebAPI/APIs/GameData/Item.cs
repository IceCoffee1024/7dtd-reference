using System;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001AEB RID: 6891
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Item : AbsRestApi
	{
		// Token: 0x0600CF5B RID: 53083 RVA: 0x004B7D68 File Offset: 0x004B5F68
		public Item(Web _parent)
			: base(null)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			int num = 0;
			for (int i = 0; i < ItemClass.list.Length; i++)
			{
				ItemClass itemClass = ItemClass.list[i];
				if (itemClass != null)
				{
					if (num > 0)
					{
						jsonWriter.WriteValueSeparator();
					}
					num++;
					string name = itemClass.Name;
					string localizedItemName = itemClass.GetLocalizedItemName();
					bool flag = itemClass.IsBlock();
					jsonWriter.WriteRaw(Item.jsonKeyName);
					jsonWriter.WriteString(name);
					jsonWriter.WriteRaw(Item.jsonKeyLocalizedName);
					jsonWriter.WriteString(localizedItemName);
					jsonWriter.WriteRaw(Item.jsonKeyIsBlock);
					jsonWriter.WriteBoolean(flag);
					jsonWriter.WriteEndObject();
				}
			}
			jsonWriter.WriteEndArray();
			this.allItemsSerialized = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600CF5C RID: 53084 RVA: 0x004B7E30 File Offset: 0x004B6030
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allItemsSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CF5D RID: 53085 RVA: 0x004B4F59 File Offset: 0x004B3159
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009D69 RID: 40297
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D6A RID: 40298
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyLocalizedName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("localizedName");

		// Token: 0x04009D6B RID: 40299
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsBlock = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isBlock");

		// Token: 0x04009D6C RID: 40300
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allItemsSerialized;
	}
}
