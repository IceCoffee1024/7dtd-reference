using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001ABB RID: 6843
	[Preserve]
	public class CommandPermissions : AbsRestApi
	{
		// Token: 0x1700190A RID: 6410
		// (get) Token: 0x0600CD8F RID: 52623 RVA: 0x004B0196 File Offset: 0x004AE396
		public static AdminCommands CommandsInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Commands;
			}
		}

		// Token: 0x0600CD90 RID: 52624 RVA: 0x004B01A8 File Offset: 0x004AE3A8
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

		// Token: 0x0600CD91 RID: 52625 RVA: 0x004B02AC File Offset: 0x004AE4AC
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

		// Token: 0x0600CD92 RID: 52626 RVA: 0x004B0300 File Offset: 0x004AE500
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

		// Token: 0x0600CD93 RID: 52627 RVA: 0x004B038C File Offset: 0x004AE58C
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

		// Token: 0x1700190B RID: 6411
		// (get) Token: 0x0600CD94 RID: 52628 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CD95 RID: 52629 RVA: 0x004B0145 File Offset: 0x004AE345
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CD96 RID: 52630 RVA: 0x004A91E8 File Offset: 0x004A73E8
		public CommandPermissions()
			: base(null)
		{
		}

		// Token: 0x04009BFD RID: 39933
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyCommand = "command";

		// Token: 0x04009BFE RID: 39934
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009BFF RID: 39935
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyIsDefault = "default";

		// Token: 0x04009C00 RID: 39936
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCommand = JsonWriter.GetEncodedPropertyNameWithBeginObject("command");

		// Token: 0x04009C01 RID: 39937
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009C02 RID: 39938
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("default");
	}
}
