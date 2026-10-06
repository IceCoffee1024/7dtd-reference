using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001B16 RID: 6934
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class EntityClass : AbsRestApi
	{
		// Token: 0x0600D0AF RID: 53423 RVA: 0x004BC0A4 File Offset: 0x004BA2A4
		public EntityClass(Web _parent)
			: base(null)
		{
			JsonWriter jsonWriter = default(JsonWriter);
			jsonWriter.WriteBeginArray();
			int num = 0;
			int num2 = 0;
			foreach (KeyValuePair<int, EntityClass> keyValuePair in EntityClass.list.Dict)
			{
				int num3;
				EntityClass entityClass;
				keyValuePair.Deconstruct(out num3, out entityClass);
				int num4 = num3;
				EntityClass entityClass2 = entityClass;
				if (num > 0)
				{
					jsonWriter.WriteValueSeparator();
				}
				num++;
				string entityClassName = entityClass2.entityClassName;
				EntityClass.UserSpawnType userSpawnType = entityClass2.userSpawnType;
				jsonWriter.WriteRaw(EntityClass.jsonKeyName);
				jsonWriter.WriteString(entityClassName);
				jsonWriter.WriteRaw(EntityClass.jsonKeyId);
				jsonWriter.WriteInt32(num4);
				if (entityClass2.userSpawnType != EntityClass.UserSpawnType.None)
				{
					num2++;
					jsonWriter.WriteRaw(EntityClass.jsonKeyCommandIndex);
					jsonWriter.WriteInt32(num2);
				}
				jsonWriter.WriteRaw(EntityClass.jsonKeyManualSpawnType);
				jsonWriter.WriteString(userSpawnType.ToStringCached<EntityClass.UserSpawnType>());
				jsonWriter.WriteEndObject();
			}
			jsonWriter.WriteEndArray();
			this.allClassesSerialized = jsonWriter.ToUtf8ByteArray();
		}

		// Token: 0x0600D0B0 RID: 53424 RVA: 0x004BC1C4 File Offset: 0x004BA3C4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allClassesSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600D0B1 RID: 53425 RVA: 0x004B9425 File Offset: 0x004B7625
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009E2A RID: 40490
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009E2B RID: 40491
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("id");

		// Token: 0x04009E2C RID: 40492
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCommandIndex = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("commandId");

		// Token: 0x04009E2D RID: 40493
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyManualSpawnType = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("manualSpawnType");

		// Token: 0x04009E2E RID: 40494
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allClassesSerialized;
	}
}
