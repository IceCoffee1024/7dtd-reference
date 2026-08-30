using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;
using Webserver.WebAPI;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AC7 RID: 6855
	public class ApiHandler : AbsHandler
	{
		// Token: 0x0600CDE1 RID: 52705 RVA: 0x004B1D21 File Offset: 0x004AFF21
		public ApiHandler()
			: base(null, 0)
		{
		}

		// Token: 0x0600CDE2 RID: 52706 RVA: 0x004B1D38 File Offset: 0x004AFF38
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			ApiHandler.apiWithParentCtorArgs[0] = _parent;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsWebAPI), new Action<Type>(this.apiFoundCallback), false);
			this.addApi(new Null("viewallclaims"));
			this.addApi(new Null("viewallplayers"));
		}

		// Token: 0x0600CDE3 RID: 52707 RVA: 0x004B1D94 File Offset: 0x004AFF94
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

		// Token: 0x0600CDE4 RID: 52708 RVA: 0x004B1E04 File Offset: 0x004B0004
		[PublicizedFrom(EAccessModifier.Private)]
		public void addApi(AbsWebAPI _api)
		{
			this.apis.Add(_api.Name, _api);
			this.parent.OpenApiHelpers.LoadOpenApiSpec(_api);
		}

		// Token: 0x0600CDE5 RID: 52709 RVA: 0x004B1E2C File Offset: 0x004B002C
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

		// Token: 0x0600CDE6 RID: 52710 RVA: 0x004B1ED4 File Offset: 0x004B00D4
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

		// Token: 0x04009C40 RID: 40000
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsWebAPI> apis = new CaseInsensitiveStringDictionary<AbsWebAPI>();

		// Token: 0x04009C41 RID: 40001
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiWithParentCtorTypes = new Type[] { typeof(Web) };

		// Token: 0x04009C42 RID: 40002
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiWithParentCtorArgs = new object[1];

		// Token: 0x04009C43 RID: 40003
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiEmptyCtorTypes = new Type[0];

		// Token: 0x04009C44 RID: 40004
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiEmptyCtorArgs = new object[0];

		// Token: 0x04009C45 RID: 40005
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler apiHandlerSampler = CustomSampler.Create("API_Handler", false);
	}
}
