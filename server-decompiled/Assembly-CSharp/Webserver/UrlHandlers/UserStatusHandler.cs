using System;
using System.Collections.Generic;
using System.Net;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AF9 RID: 6905
	public class UserStatusHandler : AbsHandler
	{
		// Token: 0x0600CFA9 RID: 53161 RVA: 0x004B9C19 File Offset: 0x004B7E19
		public UserStatusHandler(string _moduleName = null)
			: base(_moduleName, 0)
		{
		}

		// Token: 0x0600CFAA RID: 53162 RVA: 0x004B9C24 File Offset: 0x004B7E24
		[PublicizedFrom(EAccessModifier.Private)]
		static UserStatusHandler()
		{
			for (int i = 0; i < UserStatusHandler.jsonMethodNameKeys.Length; i++)
			{
				ERequestMethod erequestMethod = (ERequestMethod)i;
				UserStatusHandler.jsonMethodNameKeys[i] = JsonWriter.GetEncodedPropertyName(erequestMethod.ToStringCached<ERequestMethod>());
			}
		}

		// Token: 0x0600CFAB RID: 53163 RVA: 0x004B9CBC File Offset: 0x004B7EBC
		public override void HandleRequest(RequestContext _context)
		{
			JsonWriter jsonWriter;
			WebUtils.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(UserStatusHandler.jsonLoggedInKey);
			jsonWriter.WriteBoolean(_context.Connection != null);
			jsonWriter.WriteRaw(UserStatusHandler.jsonUsernameKey);
			jsonWriter.WriteString((_context.Connection != null) ? _context.Connection.Username : string.Empty);
			jsonWriter.WriteRaw(UserStatusHandler.jsonPermissionLevelKey);
			jsonWriter.WriteInt32(_context.PermissionLevel);
			jsonWriter.WriteRaw(UserStatusHandler.jsonPermissionsKey);
			jsonWriter.WriteBeginArray();
			List<AdminWebModules.WebModule> modules = AdminWebModules.Instance.GetModules();
			for (int i = 0; i < modules.Count; i++)
			{
				AdminWebModules.WebModule webModule = modules[i];
				if (i > 0)
				{
					jsonWriter.WriteValueSeparator();
				}
				jsonWriter.WriteRaw(UserStatusHandler.jsonModuleKey);
				jsonWriter.WriteString(webModule.Name);
				jsonWriter.WriteRaw(UserStatusHandler.jsonAllowedKey);
				jsonWriter.WriteBeginObject();
				if (webModule.LevelPerMethod == null)
				{
					jsonWriter.WriteRaw(UserStatusHandler.jsonMethodNameKeys[1]);
					jsonWriter.WriteBoolean(webModule.LevelGlobal >= _context.PermissionLevel);
				}
				else
				{
					bool flag = true;
					for (int j = 0; j < webModule.LevelPerMethod.Length; j++)
					{
						int num = webModule.LevelPerMethod[j];
						if (num != -2147483647)
						{
							if (num == -2147483648)
							{
								num = webModule.LevelGlobal;
							}
							if (!flag)
							{
								jsonWriter.WriteValueSeparator();
							}
							flag = false;
							jsonWriter.WriteRaw(UserStatusHandler.jsonMethodNameKeys[j]);
							jsonWriter.WriteBoolean(num >= _context.PermissionLevel);
						}
					}
				}
				jsonWriter.WriteEndObject();
				jsonWriter.WriteEndObject();
			}
			jsonWriter.WriteEndArray();
			jsonWriter.WriteEndObject();
			WebUtils.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
		}

		// Token: 0x04009DBA RID: 40378
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonLoggedInKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("loggedIn");

		// Token: 0x04009DBB RID: 40379
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonUsernameKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("username");

		// Token: 0x04009DBC RID: 40380
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPermissionLevelKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009DBD RID: 40381
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPermissionsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissions");

		// Token: 0x04009DBE RID: 40382
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonModuleKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("module");

		// Token: 0x04009DBF RID: 40383
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonAllowedKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("allowed");

		// Token: 0x04009DC0 RID: 40384
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[][] jsonMethodNameKeys = new byte[7][];
	}
}
