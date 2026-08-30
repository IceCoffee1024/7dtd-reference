using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AE8 RID: 6888
	[Preserve]
	public class WebUsers : AbsRestApi
	{
		// Token: 0x17001984 RID: 6532
		// (get) Token: 0x0600CF43 RID: 53059 RVA: 0x004B7658 File Offset: 0x004B5858
		public static AdminWebUsers WebUsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminWebUsers.Instance;
			}
		}

		// Token: 0x0600CF44 RID: 53060 RVA: 0x004B7660 File Offset: 0x004B5860
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

		// Token: 0x0600CF45 RID: 53061 RVA: 0x004B7728 File Offset: 0x004B5928
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

		// Token: 0x0600CF46 RID: 53062 RVA: 0x004B778C File Offset: 0x004B598C
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

		// Token: 0x0600CF47 RID: 53063 RVA: 0x004B7848 File Offset: 0x004B5A48
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

		// Token: 0x0600CF48 RID: 53064 RVA: 0x004B7888 File Offset: 0x004B5A88
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

		// Token: 0x17001985 RID: 6533
		// (get) Token: 0x0600CF49 RID: 53065 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CF4A RID: 53066 RVA: 0x004B6539 File Offset: 0x004B4739
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CF4B RID: 53067 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public WebUsers()
			: base(null)
		{
		}

		// Token: 0x04009D55 RID: 40277
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009D56 RID: 40278
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPassword = "password";

		// Token: 0x04009D57 RID: 40279
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPlatformUserId = "platformUserId";

		// Token: 0x04009D58 RID: 40280
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyCrossplatformUserId = "crossplatformUserId";

		// Token: 0x04009D59 RID: 40281
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D5A RID: 40282
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPlatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("platformUserId");

		// Token: 0x04009D5B RID: 40283
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCrossplatformUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("crossplatformUserId");
	}
}
