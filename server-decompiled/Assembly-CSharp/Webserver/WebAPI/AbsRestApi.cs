using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine.Profiling;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI
{
	// Token: 0x02001AF7 RID: 6903
	public abstract class AbsRestApi : AbsWebAPI
	{
		// Token: 0x0600CFF4 RID: 53236 RVA: 0x004B7DDD File Offset: 0x004B5FDD
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsRestApi(string _name = null)
			: this(null, _name)
		{
		}

		// Token: 0x0600CFF5 RID: 53237 RVA: 0x004B7DE7 File Offset: 0x004B5FE7
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsRestApi(Web _parentWeb, string _name = null)
			: base(_parentWeb, _name)
		{
		}

		// Token: 0x0600CFF6 RID: 53238 RVA: 0x004B7DF1 File Offset: 0x004B5FF1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void RegisterPermissions()
		{
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(this.CachedApiModuleName, this.DefaultPermissionLevel(), this.DefaultMethodPermissionLevels(), true));
		}

		// Token: 0x0600CFF7 RID: 53239 RVA: 0x004B7E18 File Offset: 0x004B6018
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

		// Token: 0x0600CFF8 RID: 53240 RVA: 0x004B8050 File Offset: 0x004B6250
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestGet(RequestContext _context)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, null, "Unsupported", null);
		}

		// Token: 0x0600CFF9 RID: 53241 RVA: 0x004B8064 File Offset: 0x004B6264
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, _jsonInputData, "Unsupported", null);
		}

		// Token: 0x0600CFFA RID: 53242 RVA: 0x004B8064 File Offset: 0x004B6264
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestPut(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, _jsonInputData, "Unsupported", null);
		}

		// Token: 0x0600CFFB RID: 53243 RVA: 0x004B8050 File Offset: 0x004B6250
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestDelete(RequestContext _context)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, null, "Unsupported", null);
		}

		// Token: 0x0600CFFC RID: 53244 RVA: 0x004B8078 File Offset: 0x004B6278
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

		// Token: 0x17001A1B RID: 6683
		// (get) Token: 0x0600CFFD RID: 53245 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600CFFE RID: 53246 RVA: 0x004B80EE File Offset: 0x004B62EE
		public virtual int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, int.MinValue, int.MinValue };
		}

		// Token: 0x0600CFFF RID: 53247 RVA: 0x004B8101 File Offset: 0x004B6301
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void PrepareEnvelopedResult(out JsonWriter _writer)
		{
			WebUtils.PrepareEnvelopedResult(out _writer);
		}

		// Token: 0x0600D000 RID: 53248 RVA: 0x004B8109 File Offset: 0x004B6309
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEnvelopedResult(RequestContext _context, ref JsonWriter _writer, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			WebUtils.SendEnvelopedResult(_context, ref _writer, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600D001 RID: 53249 RVA: 0x004B8118 File Offset: 0x004B6318
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600D002 RID: 53250 RVA: 0x004B8145 File Offset: 0x004B6345
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode, byte[] _jsonInputData, EApiErrorCode _errorCode, Exception _exception = null)
		{
			AbsRestApi.SendEmptyResponse(_context, _statusCode, _jsonInputData, _errorCode.ToStringCached<EApiErrorCode>(), _exception);
		}

		// Token: 0x04009D67 RID: 40295
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler jsonDeserializeSampler = CustomSampler.Create("JSON_Deserialize", false);
	}
}
