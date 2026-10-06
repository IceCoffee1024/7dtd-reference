using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine.Scripting;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI.APIs.Permissions
{
	// Token: 0x02001B13 RID: 6931
	[Preserve]
	public class WebModules : AbsRestApi
	{
		// Token: 0x0600D092 RID: 53394 RVA: 0x004BB6E0 File Offset: 0x004B98E0
		[PublicizedFrom(EAccessModifier.Private)]
		static WebModules()
		{
			for (int i = 0; i < WebModules.jsonMethodNameKeys.Length; i++)
			{
				ERequestMethod erequestMethod = (ERequestMethod)i;
				WebModules.jsonMethodNameKeys[i] = JsonWriter.GetEncodedPropertyName(erequestMethod.ToStringCached<ERequestMethod>());
			}
		}

		// Token: 0x17001A24 RID: 6692
		// (get) Token: 0x0600D093 RID: 53395 RVA: 0x004BB75A File Offset: 0x004B995A
		public static AdminWebModules ModulesInstance
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return AdminWebModules.Instance;
			}
		}

		// Token: 0x0600D094 RID: 53396 RVA: 0x004BB764 File Offset: 0x004B9964
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

		// Token: 0x0600D095 RID: 53397 RVA: 0x004BB81C File Offset: 0x004B9A1C
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

		// Token: 0x0600D096 RID: 53398 RVA: 0x004BB8F0 File Offset: 0x004B9AF0
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

		// Token: 0x0600D097 RID: 53399 RVA: 0x004BBAEC File Offset: 0x004B9CEC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestDelete(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			AbsRestApi.SendEmptyResponse(_context, WebModules.ModulesInstance.RemoveModule(requestPath) ? HttpStatusCode.NoContent : HttpStatusCode.NotFound, null, null, null);
		}

		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x0600D098 RID: 53400 RVA: 0x00020119 File Offset: 0x0001E319
		public override bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return true;
			}
		}

		// Token: 0x0600D099 RID: 53401 RVA: 0x004BAA05 File Offset: 0x004B8C05
		public override int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, -2147483647, int.MinValue };
		}

		// Token: 0x0600D09A RID: 53402 RVA: 0x004B3AA8 File Offset: 0x004B1CA8
		public WebModules()
			: base(null)
		{
		}

		// Token: 0x04009E12 RID: 40466
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyModule = "module";

		// Token: 0x04009E13 RID: 40467
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelGlobal = "permissionLevelGlobal";

		// Token: 0x04009E14 RID: 40468
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyPermissionLevelPerMethod = "permissionLevelPerMethod";

		// Token: 0x04009E15 RID: 40469
		[PublicizedFrom(EAccessModifier.Private)]
		public const string propertyIsDefault = "isDefault";

		// Token: 0x04009E16 RID: 40470
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyModule = JsonWriter.GetEncodedPropertyNameWithBeginObject("module");

		// Token: 0x04009E17 RID: 40471
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelGlobal = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelGlobal");

		// Token: 0x04009E18 RID: 40472
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyPermissionLevelPerMethod = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("permissionLevelPerMethod");

		// Token: 0x04009E19 RID: 40473
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[] jsonKeyIsDefault = JsonWriter.GetEncodedPropertyNameWithPrefixValueSeparator("isDefault");

		// Token: 0x04009E1A RID: 40474
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly byte[][] jsonMethodNameKeys = new byte[7][];
	}
}
