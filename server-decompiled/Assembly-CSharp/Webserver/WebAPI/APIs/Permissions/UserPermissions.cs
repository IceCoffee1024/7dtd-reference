using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B42 RID: 6978
	[Preserve]
	public class UserPermissions : AbsRestApi
	{
		// Token: 0x17001A68 RID: 6760
		// (get) Token: 0x0600D193 RID: 53651 RVA: 0x004BEBE3 File Offset: 0x004BCDE3
		public static AdminUsers UsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Users;
			}
		}

		// Token: 0x0600D194 RID: 53652 RVA: 0x004BEBF4 File Offset: 0x004BCDF4
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

		// Token: 0x0600D195 RID: 53653 RVA: 0x004BED54 File Offset: 0x004BCF54
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

		// Token: 0x0600D196 RID: 53654 RVA: 0x004BEDB8 File Offset: 0x004BCFB8
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

		// Token: 0x0600D197 RID: 53655 RVA: 0x004BEE30 File Offset: 0x004BD030
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

		// Token: 0x0600D198 RID: 53656 RVA: 0x004BEEF4 File Offset: 0x004BD0F4
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

		// Token: 0x17001A69 RID: 6761
		// (get) Token: 0x0600D199 RID: 53657 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D19A RID: 53658 RVA: 0x004BE501 File Offset: 0x004BC701
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D19B RID: 53659 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public UserPermissions()
			: base(null)
		{
		}

		// Token: 0x04009EE7 RID: 40679
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009EE8 RID: 40680
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009EE9 RID: 40681
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009EEA RID: 40682
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009EEB RID: 40683
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelMods = "permissionLevelMods";

		// Token: 0x04009EEC RID: 40684
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelNormal = "permissionLevelNormal";

		// Token: 0x04009EED RID: 40685
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009EEE RID: 40686
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009EEF RID: 40687
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009EF0 RID: 40688
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009EF1 RID: 40689
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009EF2 RID: 40690
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");

		// Token: 0x04009EF3 RID: 40691
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelMods = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelMods");

		// Token: 0x04009EF4 RID: 40692
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelNormal = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelNormal");
	}
}
