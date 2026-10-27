using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine.Profiling;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI
{
	// Token: 0x02001B28 RID: 6952
	public abstract class AbsRestApi : AbsWebAPI
	{
		// Token: 0x0600D108 RID: 53512 RVA: 0x004BB8D9 File Offset: 0x004B9AD9
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsRestApi(string _name = null)
			: this(null, _name)
		{
		}

		// Token: 0x0600D109 RID: 53513 RVA: 0x004BB8E3 File Offset: 0x004B9AE3
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsRestApi(Web _parentWeb, string _name = null)
			: base(_parentWeb, _name)
		{
		}

		// Token: 0x0600D10A RID: 53514 RVA: 0x004BB8ED File Offset: 0x004B9AED
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void RegisterPermissions()
		{
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(this.CachedApiModuleName, this.DefaultPermissionLevel(), this.DefaultMethodPermissionLevels(), true));
		}

		// Token: 0x0600D10B RID: 53515 RVA: 0x004BB914 File Offset: 0x004B9B14
		public sealed override void HandleRequest(RequestContext _context)
		{
			IDictionary<string, object> dictionary = null;
			byte[] array = null;
			if (_context.Request.HasEntityBody)
			{
				Stream inputStream = _context.Request.InputStream;
				array = new byte[_context.Request.ContentLength64];
				inputStream.Read(array, 0, (int)_context.Request.ContentLength64);
				try
				{
					dictionary = JsonSerializer.Deserialize<IDictionary<string, object>>(array);
				}
				catch (Exception ex)
				{
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, null, "INVALID_BODY", ex);
					return;
				}
			}
			try
			{
				switch (_context.Method)
				{
				case ERequestMethod.GET:
					if (dictionary != null)
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, array, "GET_WITH_BODY", null);
					}
					else
					{
						this.HandleRestGet(_context);
					}
					break;
				case ERequestMethod.POST:
					if (!this.AllowPostWithId && !string.IsNullOrEmpty(_context.RequestPath))
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, array, "POST_WITH_ID", null);
					}
					else if (dictionary == null)
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, null, "POST_WITHOUT_BODY", null);
					}
					else
					{
						this.HandleRestPost(_context, dictionary, array);
					}
					break;
				case ERequestMethod.PUT:
					if (string.IsNullOrEmpty(_context.RequestPath))
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, array, "PUT_WITHOUT_ID", null);
					}
					else if (dictionary == null)
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, null, "PUT_WITHOUT_BODY", null);
					}
					else
					{
						this.HandleRestPut(_context, dictionary, array);
					}
					break;
				case ERequestMethod.DELETE:
					if (string.IsNullOrEmpty(_context.RequestPath))
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, array, "DELETE_WITHOUT_ID", null);
					}
					else if (dictionary != null)
					{
						AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, null, "DELETE_WITH_BODY", null);
					}
					else
					{
						this.HandleRestDelete(_context);
					}
					break;
				default:
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.BadRequest, null, "INVALID_METHOD", null);
					break;
				}
			}
			catch (Exception ex2)
			{
				try
				{
					AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.InternalServerError, array, "ERROR_PROCESSING", ex2);
				}
				catch (Exception ex3)
				{
					Log.Error("[Web] In AbsRestApi.HandleRequest(): Handler " + this.Name + " threw an exception while trying to send a previous exception to the client:");
					Log.Exception(ex3);
				}
			}
		}

		// Token: 0x0600D10C RID: 53516 RVA: 0x004BBB4C File Offset: 0x004B9D4C
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestGet(RequestContext _context)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, null, "Unsupported", null);
		}

		// Token: 0x0600D10D RID: 53517 RVA: 0x004BBB60 File Offset: 0x004B9D60
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, _jsonInputData, "Unsupported", null);
		}

		// Token: 0x0600D10E RID: 53518 RVA: 0x004BBB60 File Offset: 0x004B9D60
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestPut(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, _jsonInputData, "Unsupported", null);
		}

		// Token: 0x0600D10F RID: 53519 RVA: 0x004BBB4C File Offset: 0x004B9D4C
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestDelete(RequestContext _context)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, null, "Unsupported", null);
		}

		// Token: 0x0600D110 RID: 53520 RVA: 0x004BBB74 File Offset: 0x004B9D74
		public override bool Authorized(RequestContext _context)
		{
			AdminWebModules.WebModule module = AdminWebModules.Instance.GetModule(this.CachedApiModuleName);
			if (module.LevelPerMethod == null)
			{
				return module.LevelGlobal >= _context.PermissionLevel;
			}
			int num = module.LevelPerMethod[(int)_context.Method];
			if (num == -2147483647)
			{
				return false;
			}
			if (num != -2147483648)
			{
				return num >= _context.PermissionLevel;
			}
			return module.LevelGlobal >= _context.PermissionLevel;
		}

		// Token: 0x17001A63 RID: 6755
		// (get) Token: 0x0600D111 RID: 53521 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600D112 RID: 53522 RVA: 0x004BBBEA File Offset: 0x004B9DEA
		public virtual int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, int.MinValue, int.MinValue };
		}

		// Token: 0x0600D113 RID: 53523 RVA: 0x004BBBFD File Offset: 0x004B9DFD
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void PrepareEnvelopedResult(out JsonWriter _writer)
		{
			WebUtils.PrepareEnvelopedResult(out _writer);
		}

		// Token: 0x0600D114 RID: 53524 RVA: 0x004BBC05 File Offset: 0x004B9E05
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEnvelopedResult(RequestContext _context, ref JsonWriter _writer, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			WebUtils.SendEnvelopedResult(_context, ref _writer, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600D115 RID: 53525 RVA: 0x004BBC14 File Offset: 0x004B9E14
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600D116 RID: 53526 RVA: 0x004BBC41 File Offset: 0x004B9E41
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode, byte[] _jsonInputData, EApiErrorCode _errorCode, Exception _exception = null)
		{
			AbsRestApi.SendEmptyResponse(_context, _statusCode, _jsonInputData, _errorCode.ToStringCached<EApiErrorCode>(), _exception);
		}

		// Token: 0x04009E50 RID: 40528
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler jsonDeserializeSampler = CustomSampler.Create("JSON_Deserialize", false);
	}
}
