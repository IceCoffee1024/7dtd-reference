using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AE2 RID: 6882
	[Preserve]
	public class CommandPermissions : AbsRestApi
	{
		// Token: 0x1700197C RID: 6524
		// (get) Token: 0x0600CF18 RID: 53016 RVA: 0x004B658A File Offset: 0x004B478A
		public static AdminCommands CommandsInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Commands;
			}
		}

		// Token: 0x0600CF19 RID: 53017 RVA: 0x004B659C File Offset: 0x004B479C
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
				foreach (IConsoleCommand consoleCommand in SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommands())
				{
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					AdminCommands.CommandPermission adminToolsCommandPermission = CommandPermissions.CommandsInstance.GetAdminToolsCommandPermission(consoleCommand.GetCommands());
					bool flag2 = adminToolsCommandPermission.PermissionLevel == consoleCommand.DefaultPermissionLevel;
					if (adminToolsCommandPermission.Command == "")
					{
						adminToolsCommandPermission = new AdminCommands.CommandPermission(consoleCommand.GetCommands()[0], adminToolsCommandPermission.PermissionLevel);
					}
					this.writeCommandJson(ref jsonWriter, adminToolsCommandPermission, flag2);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600CF1A RID: 53018 RVA: 0x004B66A0 File Offset: 0x004B48A0
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeCommandJson(ref JsonWriter _writer, AdminCommands.CommandPermission _commandPermission, bool _isDefault)
		{
			_writer.WriteRaw(CommandPermissions.jsonKeyCommand);
			_writer.WriteString(_commandPermission.Command);
			_writer.WriteRaw(CommandPermissions.jsonKeyPermissionLevel);
			_writer.WriteInt32(_commandPermission.PermissionLevel);
			_writer.WriteRaw(CommandPermissions.jsonKeyIsDefault);
			_writer.WriteBoolean(_isDefault);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CF1B RID: 53019 RVA: 0x004B66F4 File Offset: 0x004B48F4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string requestPath = _context.RequestPath;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_COMMAND, null);
				return;
			}
			IConsoleCommand command = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(requestPath, false);
			if (command == null)
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.NotFound, _jsonInputData, EApiErrorCode.UNKNOWN_COMMAND, null);
				return;
			}
			int num;
			if (!JsonCommons.TryGetJsonField(_jsonInput, "permissionLevel", out num))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_OR_INVALID_PERMISSION_LEVEL, null);
				return;
			}
			CommandPermissions.CommandsInstance.AddCommand(command.GetCommands()[0], num, true);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600CF1C RID: 53020 RVA: 0x004B6780 File Offset: 0x004B4980
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			IConsoleCommand command = SingletonMonoBehaviour<SdtdConsole>.Instance.GetCommand(requestPath, false);
			if (command == null || !CommandPermissions.CommandsInstance.IsPermissionDefined(command.GetCommands()))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.NotFound, null, null, null);
				return;
			}
			AbsRestApi.SendEmptyResponse(_context, CommandPermissions.CommandsInstance.RemoveCommand(command.GetCommands()) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x1700197D RID: 6525
		// (get) Token: 0x0600CF1D RID: 53021 RVA: 0x00020021 File Offset: 0x0001E221
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CF1E RID: 53022 RVA: 0x004B6539 File Offset: 0x004B4739
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CF1F RID: 53023 RVA: 0x004AF5DC File Offset: 0x004AD7DC
		public CommandPermissions()
			: base(null)
		{
		}

		// Token: 0x04009D2E RID: 40238
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyCommand = "command";

		// Token: 0x04009D2F RID: 40239
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009D30 RID: 40240
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyIsDefault = "default";

		// Token: 0x04009D31 RID: 40241
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCommand = JsonWriter.GetEncodedPropertyNameWithBeginObject("command");

		// Token: 0x04009D32 RID: 40242
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009D33 RID: 40243
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("default");
	}
}
