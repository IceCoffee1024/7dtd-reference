using System;
using System.Collections.Generic;
using System.Net;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B25 RID: 6949
	public class UserStatusHandler : AbsHandler
	{
		// Token: 0x0600D101 RID: 53505 RVA: 0x004BE0E5 File Offset: 0x004BC2E5
		public UserStatusHandler(string _moduleName = null)
			: base(_moduleName, 0)
		{
		}

		// Token: 0x0600D102 RID: 53506 RVA: 0x004BE0F0 File Offset: 0x004BC2F0
		[PublicizedFrom(EAccessModifier.Private)]
		static UserStatusHandler()
		{
			for (int i = 0; i < UserStatusHandler.jsonMethodNameKeys.Length; i++)
			{
				ERequestMethod erequestMethod = (ERequestMethod)i;
				UserStatusHandler.jsonMethodNameKeys[i] = JsonWriter.GetEncodedPropertyName(erequestMethod.ToStringCached<ERequestMethod>());
			}
		}

		// Token: 0x0600D103 RID: 53507 RVA: 0x004BE188 File Offset: 0x004BC388
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

		// Token: 0x04009E80 RID: 40576
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonLoggedInKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("loggedIn");

		// Token: 0x04009E81 RID: 40577
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonUsernameKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("username");

		// Token: 0x04009E82 RID: 40578
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPermissionLevelKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009E83 RID: 40579
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonPermissionsKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissions");

		// Token: 0x04009E84 RID: 40580
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonModuleKey = JsonWriter.GetEncodedPropertyNameWithBeginObject("module");

		// Token: 0x04009E85 RID: 40581
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonAllowedKey = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("allowed");

		// Token: 0x04009E86 RID: 40582
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[][] jsonMethodNameKeys = new byte[7][];
	}
}
