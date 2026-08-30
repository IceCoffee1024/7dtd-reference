using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001ABE RID: 6846
	[Preserve]
	public class UserPermissions : AbsRestApi
	{
		// Token: 0x1700190C RID: 6412
		// (get) Token: 0x0600CD9E RID: 52638 RVA: 0x004B0827 File Offset: 0x004AEA27
		public static AdminUsers UsersInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Users;
			}
		}

		// Token: 0x0600CD9F RID: 52639 RVA: 0x004B0838 File Offset: 0x004AEA38
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

		// Token: 0x0600CDA0 RID: 52640 RVA: 0x004B0998 File Offset: 0x004AEB98
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

		// Token: 0x0600CDA1 RID: 52641 RVA: 0x004B09FC File Offset: 0x004AEBFC
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

		// Token: 0x0600CDA2 RID: 52642 RVA: 0x004B0A74 File Offset: 0x004AEC74
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

		// Token: 0x0600CDA3 RID: 52643 RVA: 0x004B0B38 File Offset: 0x004AED38
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

		// Token: 0x1700190D RID: 6413
		// (get) Token: 0x0600CDA4 RID: 52644 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CDA5 RID: 52645 RVA: 0x004B0145 File Offset: 0x004AE345
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CDA6 RID: 52646 RVA: 0x004A91E8 File Offset: 0x004A73E8
		public UserPermissions()
			: base(null)
		{
		}

		// Token: 0x04009C07 RID: 39943
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyName = "name";

		// Token: 0x04009C08 RID: 39944
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyUserId = "userId";

		// Token: 0x04009C09 RID: 39945
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009C0A RID: 39946
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyGroupId = "groupId";

		// Token: 0x04009C0B RID: 39947
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelMods = "permissionLevelMods";

		// Token: 0x04009C0C RID: 39948
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelNormal = "permissionLevelNormal";

		// Token: 0x04009C0D RID: 39949
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUsers = JsonWriter.GetEncodedPropertyNameWithBeginObject("users");

		// Token: 0x04009C0E RID: 39950
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroups = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groups");

		// Token: 0x04009C0F RID: 39951
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyName = JsonWriter.GetEncodedPropertyNameWithBeginObject("name");

		// Token: 0x04009C10 RID: 39952
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyUserId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("userId");

		// Token: 0x04009C11 RID: 39953
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009C12 RID: 39954
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyGroupId = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("groupId");

		// Token: 0x04009C13 RID: 39955
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelMods = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelMods");

		// Token: 0x04009C14 RID: 39956
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelNormal = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelNormal");
	}
}
