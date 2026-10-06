using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B11 RID: 6929
	[Preserve]
	public class UserPermissions : AbsRestApi
	{
		// Token: 0x17001A20 RID: 6688
		// (get) Token: 0x0600D07F RID: 53375 RVA: 0x004BB0E7 File Offset: 0x004B92E7
		public static AdminUsers UsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Users;
			}
		}

		// Token: 0x0600D080 RID: 53376 RVA: 0x004BB0F8 File Offset: 0x004B92F8
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

		// Token: 0x0600D081 RID: 53377 RVA: 0x004BB258 File Offset: 0x004B9458
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

		// Token: 0x0600D082 RID: 53378 RVA: 0x004BB2BC File Offset: 0x004B94BC
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

		// Token: 0x0600D083 RID: 53379 RVA: 0x004BB334 File Offset: 0x004B9534
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

		// Token: 0x0600D084 RID: 53380 RVA: 0x004BB3F8 File Offset: 0x004B95F8
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

		// Token: 0x17001A21 RID: 6689
		// (get) Token: 0x0600D085 RID: 53381 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D086 RID: 53382 RVA: 0x004BAA05 File Offset: 0x004B8C05
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D087 RID: 53383 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public UserPermissions()
			: base(null)
		{
		}

		// Token: 0x04009DFE RID: 40446
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009DFF RID: 40447
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009E00 RID: 40448
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009E01 RID: 40449
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009E02 RID: 40450
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelMods = "permissionLevelMods";

		// Token: 0x04009E03 RID: 40451
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelNormal = "permissionLevelNormal";

		// Token: 0x04009E04 RID: 40452
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009E05 RID: 40453
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009E06 RID: 40454
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009E07 RID: 40455
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009E08 RID: 40456
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009E09 RID: 40457
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");

		// Token: 0x04009E0A RID: 40458
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelMods = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelMods");

		// Token: 0x04009E0B RID: 40459
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelNormal = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelNormal");
	}
}
