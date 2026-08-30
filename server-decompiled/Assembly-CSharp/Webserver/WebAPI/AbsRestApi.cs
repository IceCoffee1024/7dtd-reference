using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine.Profiling;
using Utf8Json;
using Webserver.Permissions;

namespace Webserver.WebAPI
{
	// Token: 0x02001AA4 RID: 6820
	public abstract class AbsRestApi : AbsWebAPI
	{
		// Token: 0x0600CD13 RID: 52499 RVA: 0x004AD51D File Offset: 0x004AB71D
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsRestApi(string _name = null)
			: this(null, _name)
		{
		}

		// Token: 0x0600CD14 RID: 52500 RVA: 0x004AD527 File Offset: 0x004AB727
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsRestApi(Web _parentWeb, string _name = null)
			: base(_parentWeb, _name)
		{
		}

		// Token: 0x0600CD15 RID: 52501 RVA: 0x004AD531 File Offset: 0x004AB731
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void RegisterPermissions()
		{
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(this.CachedApiModuleName, this.DefaultPermissionLevel(), this.DefaultMethodPermissionLevels(), true));
		}

		// Token: 0x0600CD16 RID: 52502 RVA: 0x004AD558 File Offset: 0x004AB758
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

		// Token: 0x0600CD17 RID: 52503 RVA: 0x004AD790 File Offset: 0x004AB990
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestGet(RequestContext _context)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, null, "Unsupported", null);
		}

		// Token: 0x0600CD18 RID: 52504 RVA: 0x004AD7A4 File Offset: 0x004AB9A4
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestPost(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, _jsonInputData, "Unsupported", null);
		}

		// Token: 0x0600CD19 RID: 52505 RVA: 0x004AD7A4 File Offset: 0x004AB9A4
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestPut(RequestContext _context, IDictionary<string, object> _jsonInput, byte[] _jsonInputData)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, _jsonInputData, "Unsupported", null);
		}

		// Token: 0x0600CD1A RID: 52506 RVA: 0x004AD790 File Offset: 0x004AB990
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void HandleRestDelete(RequestContext _context)
		{
			AbsRestApi.SendEmptyResponse(_context, HttpStatusCode.MethodNotAllowed, null, "Unsupported", null);
		}

		// Token: 0x0600CD1B RID: 52507 RVA: 0x004AD7B8 File Offset: 0x004AB9B8
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

		// Token: 0x17001907 RID: 6407
		// (get) Token: 0x0600CD1C RID: 52508 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual bool AllowPostWithId
		{
			[PublicizedFrom(EAccessModifier.Protected)]
			get
			{
				return false;
			}
		}

		// Token: 0x0600CD1D RID: 52509 RVA: 0x004AD82E File Offset: 0x004ABA2E
		public virtual int[] DefaultMethodPermissionLevels()
		{
			return new int[] { -2147483647, int.MinValue, int.MinValue, int.MinValue, int.MinValue };
		}

		// Token: 0x0600CD1E RID: 52510 RVA: 0x004AD841 File Offset: 0x004ABA41
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void PrepareEnvelopedResult(out JsonWriter _writer)
		{
			WebUtils.PrepareEnvelopedResult(out _writer);
		}

		// Token: 0x0600CD1F RID: 52511 RVA: 0x004AD849 File Offset: 0x004ABA49
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEnvelopedResult(RequestContext _context, ref JsonWriter _writer, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			WebUtils.SendEnvelopedResult(_context, ref _writer, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600CD20 RID: 52512 RVA: 0x004AD858 File Offset: 0x004ABA58
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode = HttpStatusCode.OK, byte[] _jsonInputData = null, string _errorCode = null, Exception _exception = null)
		{
			JsonWriter jsonWriter;
			AbsRestApi.PrepareEnvelopedResult(out jsonWriter);
			jsonWriter.WriteRaw(WebUtils.JsonEmptyData);
			AbsRestApi.SendEnvelopedResult(_context, ref jsonWriter, _statusCode, _jsonInputData, _errorCode, _exception);
		}

		// Token: 0x0600CD21 RID: 52513 RVA: 0x004AD885 File Offset: 0x004ABA85
		[PublicizedFrom(EAccessModifier.Protected)]
		public static void SendEmptyResponse(RequestContext _context, HttpStatusCode _statusCode, byte[] _jsonInputData, EApiErrorCode _errorCode, Exception _exception = null)
		{
			AbsRestApi.SendEmptyResponse(_context, _statusCode, _jsonInputData, _errorCode.ToStringCached<EApiErrorCode>(), _exception);
		}

		// Token: 0x04009B70 RID: 39792
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler jsonDeserializeSampler = CustomSampler.Create("JSON_Deserialize", false);
	}
}
