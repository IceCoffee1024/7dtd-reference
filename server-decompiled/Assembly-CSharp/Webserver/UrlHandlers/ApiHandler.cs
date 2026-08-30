using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;
using Webserver.WebAPI;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AEE RID: 6894
	public class ApiHandler : AbsHandler
	{
		// Token: 0x0600CF6A RID: 53098 RVA: 0x004B8115 File Offset: 0x004B6315
		public ApiHandler()
			: base(null, 0)
		{
		}

		// Token: 0x0600CF6B RID: 53099 RVA: 0x004B812C File Offset: 0x004B632C
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			ApiHandler.apiWithParentCtorArgs[0] = _parent;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsWebAPI), new Action<Type>(this.apiFoundCallback), false);
			this.addApi(new Null("viewallclaims"));
			this.addApi(new Null("viewallplayers"));
		}

		// Token: 0x0600CF6C RID: 53100 RVA: 0x004B8188 File Offset: 0x004B6388
		[PublicizedFrom(EAccessModifier.Private)]
		public void apiFoundCallback(Type _type)
		{
			ConstructorInfo constructorInfo = _type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ApiHandler.apiWithParentCtorTypes, null);
			if (constructorInfo != null)
			{
				AbsWebAPI absWebAPI = (AbsWebAPI)constructorInfo.Invoke(ApiHandler.apiWithParentCtorArgs);
				this.addApi(absWebAPI);
				return;
			}
			constructorInfo = _type.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, ApiHandler.apiEmptyCtorTypes, null);
			if (constructorInfo != null)
			{
				AbsWebAPI absWebAPI2 = (AbsWebAPI)constructorInfo.Invoke(ApiHandler.apiEmptyCtorArgs);
				this.addApi(absWebAPI2);
			}
		}

		// Token: 0x0600CF6D RID: 53101 RVA: 0x004B81F8 File Offset: 0x004B63F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void addApi(AbsWebAPI _api)
		{
			this.apis.Add(_api.Name, _api);
			this.parent.OpenApiHelpers.LoadOpenApiSpec(_api);
		}

		// Token: 0x0600CF6E RID: 53102 RVA: 0x004B8220 File Offset: 0x004B6420
		[PublicizedFrom(EAccessModifier.Private)]
		public bool HandleCors(RequestContext _context)
		{
			string text;
			_context.Request.Headers.TryGetValue("Origin", out text);
			_context.Response.AddHeader("Access-Control-Allow-Origin", text ?? "*");
			if (_context.Method != ERequestMethod.OPTIONS)
			{
				return false;
			}
			string text2;
			if (!_context.Request.Headers.TryGetValue("Access-Control-Request-Method", out text2))
			{
				return false;
			}
			_context.Response.AddHeader("Access-Control-Allow-Methods", "GET, PUT, POST, DELETE, OPTIONS, HEAD");
			_context.Response.AddHeader("Access-Control-Allow-Headers", "X-SDTD-API-TOKENNAME, X-SDTD-API-SECRET");
			_context.Response.AddHeader("Access-Control-Allow-Credentials", "true");
			return true;
		}

		// Token: 0x0600CF6F RID: 53103 RVA: 0x004B82C8 File Offset: 0x004B64C8
		public override void HandleRequest(RequestContext _context)
		{
			string text = null;
			int num = _context.RequestPath.IndexOf('/', this.urlBasePath.Length);
			string text2;
			if (num >= 0)
			{
				text2 = _context.RequestPath.Substring(this.urlBasePath.Length, num - this.urlBasePath.Length);
				text = _context.RequestPath.Substring(num + 1);
			}
			else
			{
				text2 = _context.RequestPath.Substring(this.urlBasePath.Length);
			}
			AbsWebAPI absWebAPI;
			if (!this.apis.TryGetValue(text2, out absWebAPI))
			{
				Log.Warning("[Web] In ApiHandler.HandleRequest(): No handler found for API \"" + text2 + "\"");
				_context.Response.StatusCode = 404;
				return;
			}
			if (this.HandleCors(_context))
			{
				return;
			}
			_context.RequestPath = text;
			if (!absWebAPI.Authorized(_context))
			{
				_context.Response.StatusCode = 403;
				WebConnection connection = _context.Connection;
				return;
			}
			try
			{
				absWebAPI.HandleRequest(_context);
			}
			catch (Exception ex)
			{
				Log.Error("[Web] In ApiHandler.HandleRequest(): Handler " + absWebAPI.Name + " threw an exception:");
				Log.Exception(ex);
				_context.Response.StatusCode = 500;
			}
		}

		// Token: 0x04009D71 RID: 40305
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsWebAPI> apis = new CaseInsensitiveStringDictionary<AbsWebAPI>();

		// Token: 0x04009D72 RID: 40306
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiWithParentCtorTypes = new Type[] { typeof(Web) };

		// Token: 0x04009D73 RID: 40307
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiWithParentCtorArgs = new object[1];

		// Token: 0x04009D74 RID: 40308
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiEmptyCtorTypes = new Type[0];

		// Token: 0x04009D75 RID: 40309
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiEmptyCtorArgs = new object[0];

		// Token: 0x04009D76 RID: 40310
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler apiHandlerSampler = CustomSampler.Create("API_Handler", false);
	}
}
