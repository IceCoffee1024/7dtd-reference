using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B3F RID: 6975
	[Preserve]
	public class CommandPermissions : AbsRestApi
	{
		// Token: 0x17001A66 RID: 6758
		// (get) Token: 0x0600D184 RID: 53636 RVA: 0x004BE552 File Offset: 0x004BC752
		public static AdminCommands CommandsInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return GameManager.Instance.adminTools.Commands;
			}
		}

		// Token: 0x0600D185 RID: 53637 RVA: 0x004BE564 File Offset: 0x004BC764
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

		// Token: 0x0600D186 RID: 53638 RVA: 0x004BE668 File Offset: 0x004BC868
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

		// Token: 0x0600D187 RID: 53639 RVA: 0x004BE6BC File Offset: 0x004BC8BC
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

		// Token: 0x0600D188 RID: 53640 RVA: 0x004BE748 File Offset: 0x004BC948
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

		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x0600D189 RID: 53641 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D18A RID: 53642 RVA: 0x004BE501 File Offset: 0x004BC701
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D18B RID: 53643 RVA: 0x004B75A4 File Offset: 0x004B57A4
		public CommandPermissions()
			: base(null)
		{
		}

		// Token: 0x04009EDD RID: 40669
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyCommand = "command";

		// Token: 0x04009EDE RID: 40670
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevel = "permissionLevel";

		// Token: 0x04009EDF RID: 40671
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyIsDefault = "default";

		// Token: 0x04009EE0 RID: 40672
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyCommand = JsonWriter.GetEncodedPropertyNameWithBeginObject("command");

		// Token: 0x04009EE1 RID: 40673
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevel = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevel");

		// Token: 0x04009EE2 RID: 40674
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("default");
	}
}
