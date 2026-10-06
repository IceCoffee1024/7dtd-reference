using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B14 RID: 6932
	[Preserve]
	public class WebUsers : AbsRestApi
	{
		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x0600D09B RID: 53403 RVA: 0x004BBB24 File Offset: 0x004B9D24
		public static AdminWebUsers WebUsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminWebUsers.Instance;
			}
		}

		// Token: 0x0600D09C RID: 53404 RVA: 0x004BBB2C File Offset: 0x004B9D2C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			if (string.IsNullOrEmpty(requestPath))
			{
				jsonWriter.WriteBeginArray();
				bool flag = true;
				foreach (KeyValuePair<string, AdminWebUsers.WebUser> keyValuePair in WebUsers.WebUsersInstance.GetUsers())
				{
					string text;
					AdminWebUsers.WebUser webUser;
					keyValuePair.Deconstruct(out text, out webUser);
					AdminWebUsers.WebUser webUser2 = webUser;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeUserJson(ref jsonWriter, webUser2);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600D09D RID: 53405 RVA: 0x004BBBF4 File Offset: 0x004B9DF4
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminWebUsers.WebUser _user)
		{
			_writer.WriteRaw(WebUsers.jsonKeyName);
			_writer.WriteString(_user.Name ?? "");
			_writer.WriteRaw(WebUsers.jsonKeyPlatformUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _user.PlatformUser);
			_writer.WriteRaw(WebUsers.jsonKeyCrossplatformUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _user.CrossPlatformUser);
			_writer.WriteEndObject();
		}

		// Token: 0x0600D09E RID: 53406 RVA: 0x004BBC58 File Offset: 0x004B9E58
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string text;
			if (!this.TryParseName(_context, _jsonInputData, out text))
			{
				return;
			}
			string text2;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "password", out text2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PASSWORD, null);
				return;
			}
			IDictionary<string, object> dictionary;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "platformUserId", out dictionary))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_PLATFORM_USER_ID, null);
				return;
			}
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			if (!JsonCommons.TryReadPlatformUserIdentifier(dictionary, out platformUserIdentifierAbs))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PLATFORM_USER_ID, null);
				return;
			}
			PlatformUserIdentifierAbs platformUserIdentifierAbs2 = null;
			if (JsonCommons.TryGetJsonField(_jsonInput, "crossplatformUserId", out dictionary) && !JsonCommons.TryReadPlatformUserIdentifier(dictionary, out platformUserIdentifierAbs2))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_CROSSPLATFORM_USER_ID, null);
				return;
			}
			WebUsers.WebUsersInstance.AddUser(text, text2, platformUserIdentifierAbs, platformUserIdentifierAbs2);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600D09F RID: 53407 RVA: 0x004BBD14 File Offset: 0x004B9F14
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string text;
			if (!this.TryParseName(_context, null, out text))
			{
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, WebUsers.WebUsersInstance.RemoveUser(text) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x0600D0A0 RID: 53408 RVA: 0x004BBD54 File Offset: 0x004B9F54
		[PublicizedFrom(EAccessModifier.Private)]
		public bool TryParseName(RequestContext _context, byte[] _jsonInputData, out string _userName)
		{
			string requestPath = _context.RequestPath;
			_userName = null;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_USERNAME, null);
				return false;
			}
			_userName = requestPath;
			return true;
		}

		// Token: 0x17001A27 RID: 6695
		// (get) Token: 0x0600D0A1 RID: 53409 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D0A2 RID: 53410 RVA: 0x004BAA05 File Offset: 0x004B8C05
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D0A3 RID: 53411 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public WebUsers()
			: base(null)
		{
		}

		// Token: 0x04009E1B RID: 40475
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009E1C RID: 40476
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPassword = "password";

		// Token: 0x04009E1D RID: 40477
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPlatformUserId = "platformUserId";

		// Token: 0x04009E1E RID: 40478
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyCrossplatformUserId = "crossplatformUserId";

		// Token: 0x04009E1F RID: 40479
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009E20 RID: 40480
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId");

		// Token: 0x04009E21 RID: 40481
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCrossplatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("crossplatformUserId");
	}
}
