using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AE5 RID: 6885
	[Preserve]
	public class UserPermissions : AbsRestApi
	{
		// Token: 0x1700197E RID: 6526
		// (get) Token: 0x0600CF27 RID: 53031 RVA: 0x004B6C1B File Offset: 0x004B4E1B
		public static AdminUsers UsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Users;
			}
		}

		// Token: 0x0600CF28 RID: 53032 RVA: 0x004B6C2C File Offset: 0x004B4E2C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			if (string.IsNullOrEmpty(requestPath))
			{
				jsonWriter.WriteRaw(UserPermissions.jsonKeyUsers);
				jsonWriter.WriteBeginArray();
				bool flag = true;
				foreach (KeyValuePair<PlatformUserIdentifierAbs, AdminUsers.UserPermission> keyValuePair in UserPermissions.UsersInstance.GetUsers())
				{
					PlatformUserIdentifierAbs platformUserIdentifierAbs;
					AdminUsers.UserPermission userPermission;
					keyValuePair.Deconstruct(out platformUserIdentifierAbs, out userPermission);
					AdminUsers.UserPermission userPermission2 = userPermission;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeUserJson(ref jsonWriter, userPermission2);
				}
				jsonWriter.WriteEndArray();
				jsonWriter.WriteRaw(UserPermissions.jsonKeyGroups);
				jsonWriter.WriteBeginArray();
				flag = true;
				foreach (KeyValuePair<string, AdminUsers.GroupPermission> keyValuePair2 in UserPermissions.UsersInstance.GetGroups())
				{
					string text;
					AdminUsers.GroupPermission groupPermission;
					keyValuePair2.Deconstruct(out text, out groupPermission);
					AdminUsers.GroupPermission groupPermission2 = groupPermission;
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeGroupJson(ref jsonWriter, groupPermission2);
				}
				jsonWriter.WriteEndArray();
				jsonWriter.WriteEndObject();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600CF29 RID: 53033 RVA: 0x004B6D8C File Offset: 0x004B4F8C
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeUserJson(ref JsonWriter _writer, AdminUsers.UserPermission _userPermission)
		{
			_writer.WriteRaw(UserPermissions.jsonKeyName);
			_writer.WriteString(_userPermission.Name ?? "");
			_writer.WriteRaw(UserPermissions.jsonKeyUserId);
			JsonCommons.WritePlatformUserIdentifier(ref _writer, _userPermission.UserIdentifier);
			_writer.WriteRaw(UserPermissions.jsonKeyPermissionLevel);
			_writer.WriteInt32(_userPermission.PermissionLevel);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CF2A RID: 53034 RVA: 0x004B6DF0 File Offset: 0x004B4FF0
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeGroupJson(ref JsonWriter _writer, AdminUsers.GroupPermission _groupPermission)
		{
			_writer.WriteRaw(UserPermissions.jsonKeyName);
			_writer.WriteString(_groupPermission.Name ?? "");
			_writer.WriteRaw(UserPermissions.jsonKeyGroupId);
			_writer.WriteString(_groupPermission.SteamIdGroup);
			_writer.WriteRaw(UserPermissions.jsonKeyPermissionLevelMods);
			_writer.WriteInt32(_groupPermission.PermissionLevelMods);
			_writer.WriteRaw(UserPermissions.jsonKeyPermissionLevelNormal);
			_writer.WriteInt32(_groupPermission.PermissionLevelNormal);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CF2B RID: 53035 RVA: 0x004B6E68 File Offset: 0x004B5068
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			string text;
			if (!PermissionsApiHelpers.TryParseId(_context, _jsonInputData, out platformUserIdentifierAbs, out text))
			{
				return;
			}
			if (platformUserIdentifierAbs != null)
			{
				int num;
				if (!JsonCommons.TryGetJsonField(_jsonInput, "permissionLevel", out num))
				{
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PERMISSION_LEVEL, null);
					return;
				}
				string text2;
				JsonCommons.TryGetJsonField(_jsonInput, "name", out text2);
				UserPermissions.UsersInstance.AddUser(text2, platformUserIdentifierAbs, num);
			}
			else
			{
				int num2;
				if (!JsonCommons.TryGetJsonField(_jsonInput, "permissionLevelMods", out num2))
				{
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PERMISSION_LEVEL_MODS, null);
					return;
				}
				int num3;
				if (!JsonCommons.TryGetJsonField(_jsonInput, "permissionLevelNormal", out num3))
				{
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PERMISSION_LEVEL_NORMAL, null);
					return;
				}
				string text3;
				JsonCommons.TryGetJsonField(_jsonInput, "name", out text3);
				UserPermissions.UsersInstance.AddGroup(text3, text, num3, num2);
			}
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600CF2C RID: 53036 RVA: 0x004B6F2C File Offset: 0x004B512C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			PlatformUserIdentifierAbs platformUserIdentifierAbs;
			string text;
			if (!PermissionsApiHelpers.TryParseId(_context, null, out platformUserIdentifierAbs, out text))
			{
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, ((platformUserIdentifierAbs != null) ? UserPermissions.UsersInstance.RemoveUser(platformUserIdentifierAbs, true) : UserPermissions.UsersInstance.RemoveGroup(text)) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x1700197F RID: 6527
		// (get) Token: 0x0600CF2D RID: 53037 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CF2E RID: 53038 RVA: 0x004B6539 File Offset: 0x004B4739
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CF2F RID: 53039 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public UserPermissions()
			: base(null)
		{
		}

		// Token: 0x04009D38 RID: 40248
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009D39 RID: 40249
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009D3A RID: 40250
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009D3B RID: 40251
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009D3C RID: 40252
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelMods = "permissionLevelMods";

		// Token: 0x04009D3D RID: 40253
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelNormal = "permissionLevelNormal";

		// Token: 0x04009D3E RID: 40254
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009D3F RID: 40255
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009D40 RID: 40256
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009D41 RID: 40257
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009D42 RID: 40258
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009D43 RID: 40259
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");

		// Token: 0x04009D44 RID: 40260
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelMods = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelMods");

		// Token: 0x04009D45 RID: 40261
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelNormal = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelNormal");
	}
}
