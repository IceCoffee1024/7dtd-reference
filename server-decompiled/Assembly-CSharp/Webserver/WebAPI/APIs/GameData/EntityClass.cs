using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001AEA RID: 6890
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class EntityClass : AbsRestApi
	{
		// Token: 0x0600CF57 RID: 53079 RVA: 0x004B7BD8 File Offset: 0x004B5DD8
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

		// Token: 0x0600CF58 RID: 53080 RVA: 0x004B7CF8 File Offset: 0x004B5EF8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allClassesSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CF59 RID: 53081 RVA: 0x004B4F59 File Offset: 0x004B3159
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009D64 RID: 40292
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D65 RID: 40293
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("id");

		// Token: 0x04009D66 RID: 40294
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCommandIndex = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("commandId");

		// Token: 0x04009D67 RID: 40295
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyManualSpawnType = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("manualSpawnType");

		// Token: 0x04009D68 RID: 40296
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allClassesSerialized;
	}
}
