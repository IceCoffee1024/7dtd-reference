using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.LiveData;

namespace Webserver.WebAPI.APIs.WorldState
{
	// Token: 0x02001AB0 RID: 6832
	[Preserve]
	[PublicizedFrom(EAccessModifier.Internal)]
	public class Hostile : AbsRestApi
	{
		// Token: 0x0600CD5C RID: 52572 RVA: 0x004AEE04 File Offset: 0x004AD004
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteBeginArray();
			List<EntityEnemy> list = this.entities;
			lock (list)
			{
				Hostiles.Instance.Get(this.entities);
				for (int i = 0; i < this.entities.Count; i++)
				{
					if (i > 0)
					{
						jsonWriter.WriteValueSeparator();
					}
					EntityAlive entityAlive = this.entities[i];
					Vector3i vector3i = new Vector3i(entityAlive.GetPosition());
					jsonWriter.WriteRaw(Hostile.jsonKeyId);
					jsonWriter.WriteInt32(entityAlive.entityId);
					jsonWriter.WriteRaw(Hostile.jsonKeyName);
					jsonWriter.WriteString((!string.IsNullOrEmpty(entityAlive.EntityName)) ? entityAlive.EntityName : string.Format("enemy class #{0}", entityAlive.entityClass));
					jsonWriter.WriteRaw(Hostile.jsonKeyPosition);
					JsonCommons.WriteVector3I(ref jsonWriter, vector3i);
					jsonWriter.WriteEndObject();
				}
			}
			jsonWriter.WriteEndArray();
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x0600CD5D RID: 52573 RVA: 0x004AEF2C File Offset: 0x004AD12C
		public Hostile()
			: base(null)
		{
		}

		// Token: 0x04009BBE RID: 39870
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<EntityEnemy> entities = new List<EntityEnemy>();

		// Token: 0x04009BBF RID: 39871
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyId = JsonWriter.GetEncodedPropertyNameWithBeginObject("id");

		// Token: 0x04009BC0 RID: 39872
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("name");

		// Token: 0x04009BC1 RID: 39873
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPosition = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("position");
	}
}
