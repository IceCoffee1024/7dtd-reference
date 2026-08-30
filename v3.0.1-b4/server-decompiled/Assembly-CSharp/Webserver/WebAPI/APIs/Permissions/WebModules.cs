using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001AC0 RID: 6848
	[Preserve]
	public class WebModules : AbsRestApi
	{
		// Token: 0x0600CDB1 RID: 52657 RVA: 0x004B0E20 File Offset: 0x004AF020
		[PublicizedFrom(EAccessModifier.Private)]
		static WebModules()
		{
			for (int i = 0; i < WebModules.jsonMethodNameKeys.Length; i++)
			{
				ERequestMethod erequestMethod = (ERequestMethod)i;
				WebModules.jsonMethodNameKeys[i] = JsonWriter.GetEncodedPropertyName(erequestMethod.ToStringCached<ERequestMethod>());
			}
		}

		// Token: 0x17001910 RID: 6416
		// (get) Token: 0x0600CDB2 RID: 52658 RVA: 0x004B0E9A File Offset: 0x004AF09A
		public static AdminWebModules ModulesInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminWebModules.Instance;
			}
		}

		// Token: 0x0600CDB3 RID: 52659 RVA: 0x004B0EA4 File Offset: 0x004AF0A4
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
				foreach (AdminWebModules.WebModule webModule in WebModules.ModulesInstance.GetModules())
				{
					if (!flag)
					{
						jsonWriter.WriteValueSeparator();
					}
					flag = false;
					this.writeModuleJson(ref jsonWriter, webModule);
				}
				jsonWriter.WriteEndArray();
				AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.OK, null, null, null);
				return;
			}
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, HttpStatusCode.BadRequest, null, null, null);
		}

		// Token: 0x0600CDB4 RID: 52660 RVA: 0x004B0F5C File Offset: 0x004AF15C
		[PublicizedFrom(EAccessModifier.Private)]
		public void writeModuleJson(ref JsonWriter _writer, AdminWebModules.WebModule _module)
		{
			_writer.WriteRaw(WebModules.jsonKeyModule);
			_writer.WriteString(_module.Name);
			_writer.WriteRaw(WebModules.jsonKeyPermissionLevelGlobal);
			_writer.WriteInt32(_module.LevelGlobal);
			_writer.WriteRaw(WebModules.jsonKeyPermissionLevelPerMethod);
			_writer.WriteBeginObject();
			if (_module.LevelPerMethod != null)
			{
				bool flag = true;
				for (int i = 0; i < _module.LevelPerMethod.Length; i++)
				{
					int num = _module.LevelPerMethod[i];
					if (num != -2147483647)
					{
						if (!flag)
						{
							_writer.WriteValueSeparator();
						}
						flag = false;
						_writer.WriteRaw(WebModules.jsonMethodNameKeys[i]);
						if (num == -2147483648)
						{
							_writer.WriteString("inherit");
						}
						else
						{
							_writer.WriteInt32(num);
						}
					}
				}
			}
			_writer.WriteEndObject();
			_writer.WriteRaw(WebModules.jsonKeyIsDefault);
			_writer.WriteBoolean(_module.IsDefault);
			_writer.WriteEndObject();
		}

		// Token: 0x0600CDB5 RID: 52661 RVA: 0x004B1030 File Offset: 0x004AF230
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			string requestPath = _context.RequestPath;
			if (string.IsNullOrEmpty(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.NO_MODULE, null);
				return;
			}
			if (!AdminWebModules.Instance.IsKnownModule(requestPath))
			{
				AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_MODULE, null);
				return;
			}
			AdminWebModules.WebModule webModule = AdminWebModules.Instance.GetModule(requestPath);
			if (_jsonInput.ContainsKey("permissionLevelGlobal"))
			{
				int num;
				if (!JsonCommons.TryGetJsonField(_jsonInput, "permissionLevelGlobal", out num))
				{
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PERMISSION_LEVEL_GLOBAL, null);
					return;
				}
				webModule = webModule.SetLevelGlobal(num);
			}
			object obj;
			if (_jsonInput.TryGetValue("permissionLevelPerMethod", out obj))
			{
				IDictionary<string, object> dictionary = obj as IDictionary<string, object>;
				if (dictionary == null)
				{
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PERMISSION_LEVEL_PER_METHOD_PROPERTY, null);
					return;
				}
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					string text;
					object obj2;
					keyValuePair.Deconstruct(out text, out obj2);
					string text2 = text;
					object obj3 = obj2;
					ERequestMethod erequestMethod;
					if (!EnumUtils.TryParse<ERequestMethod>(text2, out erequestMethod, true))
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_METHOD_NAME, null);
						return;
					}
					if (webModule.LevelPerMethod == null || webModule.LevelPerMethod[(int)erequestMethod] == -2147483647)
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.UNSUPPORTED_METHOD, null);
						return;
					}
					string text3 = obj3 as string;
					int num3;
					if (text3 == null)
					{
						if (obj3 is double)
						{
							double num2 = (double)obj3;
							try
							{
								num3 = (int)num2;
								goto IL_0191;
							}
							catch (Exception)
							{
								AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PERMISSION_VALUE, null);
								return;
							}
						}
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PERMISSION_VALUE_TYPE, null);
						return;
					}
					if (!text3.EqualsCaseInsensitive("inherit"))
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, _jsonInputData, EApiErrorCode.INVALID_PERMISSION_STRING, null);
						return;
					}
					num3 = int.MinValue;
					IL_0191:
					webModule = webModule.SetLevelForMethod(erequestMethod, num3);
				}
			}
			WebModules.ModulesInstance.AddModule(webModule);
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.Created, null, null, null);
		}

		// Token: 0x0600CDB6 RID: 52662 RVA: 0x004B122C File Offset: 0x004AF42C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			AbsRestApi.SendEmptyResponse(_context, WebModules.ModulesInstance.RemoveModule(requestPath) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x17001911 RID: 6417
		// (get) Token: 0x0600CDB7 RID: 52663 RVA: 0x0001FF5D File Offset: 0x0001E15D
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600CDB8 RID: 52664 RVA: 0x004B0145 File Offset: 0x004AE345
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600CDB9 RID: 52665 RVA: 0x004A91E8 File Offset: 0x004A73E8
		public WebModules()
			: base(null)
		{
		}

		// Token: 0x04009C1B RID: 39963
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyModule = "module";

		// Token: 0x04009C1C RID: 39964
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelGlobal = "permissionLevelGlobal";

		// Token: 0x04009C1D RID: 39965
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelPerMethod = "permissionLevelPerMethod";

		// Token: 0x04009C1E RID: 39966
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyIsDefault = "isDefault";

		// Token: 0x04009C1F RID: 39967
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyModule = JsonWriter.GetEncodedPropertyNameWithBeginObject("module");

		// Token: 0x04009C20 RID: 39968
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelGlobal = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelGlobal");

		// Token: 0x04009C21 RID: 39969
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelPerMethod = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelPerMethod");

		// Token: 0x04009C22 RID: 39970
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isDefault");

		// Token: 0x04009C23 RID: 39971
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[][] jsonMethodNameKeys = new byte[7][];
	}
}
