using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;
using Webserver.WebAPI;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B4B RID: 6987
	public class ApiHandler : AbsHandler
	{
		// Token: 0x0600D1D6 RID: 53718 RVA: 0x004C00DD File Offset: 0x004BE2DD
		public ApiHandler()
			: base(null, 0)
		{
		}

		// Token: 0x0600D1D7 RID: 53719 RVA: 0x004C00F4 File Offset: 0x004BE2F4
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			ApiHandler.apiWithParentCtorArgs[0] = _parent;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsWebAPI), new Action<Type>(this.apiFoundCallback), false);
			this.addApi(new Null("viewallclaims"));
			this.addApi(new Null("viewallplayers"));
		}

		// Token: 0x0600D1D8 RID: 53720 RVA: 0x004C0150 File Offset: 0x004BE350
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

		// Token: 0x0600D1D9 RID: 53721 RVA: 0x004C01C0 File Offset: 0x004BE3C0
		[PublicizedFrom(EAccessModifier.Private)]
		public void addApi(AbsWebAPI _api)
		{
			this.apis.Add(_api.Name, _api);
			this.parent.OpenApiHelpers.LoadOpenApiSpec(_api);
		}

		// Token: 0x0600D1DA RID: 53722 RVA: 0x004C01E8 File Offset: 0x004BE3E8
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

		// Token: 0x0600D1DB RID: 53723 RVA: 0x004C0290 File Offset: 0x004BE490
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

		// Token: 0x04009F20 RID: 40736
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsWebAPI> apis = new CaseInsensitiveStringDictionary<AbsWebAPI>();

		// Token: 0x04009F21 RID: 40737
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiWithParentCtorTypes = new Type[] { typeof(Web) };

		// Token: 0x04009F22 RID: 40738
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiWithParentCtorArgs = new object[1];

		// Token: 0x04009F23 RID: 40739
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiEmptyCtorTypes = new Type[0];

		// Token: 0x04009F24 RID: 40740
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiEmptyCtorArgs = new object[0];

		// Token: 0x04009F25 RID: 40741
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler apiHandlerSampler = CustomSampler.Create("API_Handler", false);
	}
}
