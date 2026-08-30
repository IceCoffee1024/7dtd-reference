using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.GameData
{
	// Token: 0x02001AC3 RID: 6851
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class EntityClass : AbsRestApi
	{
		// Token: 0x0600CDCE RID: 52686 RVA: 0x004B17E4 File Offset: 0x004AF9E4
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

		// Token: 0x0600CDCF RID: 52687 RVA: 0x004B1904 File Offset: 0x004AFB04
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(this.allClassesSerialized);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CDD0 RID: 52688 RVA: 0x004AEB65 File Offset: 0x004ACD65
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}

		// Token: 0x04009C33 RID: 39987
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009C34 RID: 39988
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("id");

		// Token: 0x04009C35 RID: 39989
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCommandIndex = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("commandId");

		// Token: 0x04009C36 RID: 39990
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyManualSpawnType = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("manualSpawnType");

		// Token: 0x04009C37 RID: 39991
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly byte[] allClassesSerialized;
	}
}
