using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.Profiling;
using Webserver.WebAPI;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B1A RID: 6938
	public class ApiHandler : AbsHandler
	{
		// Token: 0x0600D0C2 RID: 53442 RVA: 0x004BC5E1 File Offset: 0x004BA7E1
		public ApiHandler()
			: base(null, 0)
		{
		}

		// Token: 0x0600D0C3 RID: 53443 RVA: 0x004BC5F8 File Offset: 0x004BA7F8
		public override void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			base.SetBasePathAndParent(_parent, _relativePath);
			ApiHandler.apiWithParentCtorArgs[0] = _parent;
			ReflectionHelpers.FindTypesImplementingBase(typeof(AbsWebAPI), new Action<Type>(this.apiFoundCallback), false);
			this.addApi(new Null("viewallclaims"));
			this.addApi(new Null("viewallplayers"));
		}

		// Token: 0x0600D0C4 RID: 53444 RVA: 0x004BC654 File Offset: 0x004BA854
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

		// Token: 0x0600D0C5 RID: 53445 RVA: 0x004BC6C4 File Offset: 0x004BA8C4
		[PublicizedFrom(EAccessModifier.Private)]
		public void addApi(AbsWebAPI _api)
		{
			this.apis.Add(_api.Name, _api);
			this.parent.OpenApiHelpers.LoadOpenApiSpec(_api);
		}

		// Token: 0x0600D0C6 RID: 53446 RVA: 0x004BC6EC File Offset: 0x004BA8EC
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

		// Token: 0x0600D0C7 RID: 53447 RVA: 0x004BC794 File Offset: 0x004BA994
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

		// Token: 0x04009E37 RID: 40503
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly Dictionary<string, AbsWebAPI> apis = new CaseInsensitiveStringDictionary<AbsWebAPI>();

		// Token: 0x04009E38 RID: 40504
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiWithParentCtorTypes = new Type[] { typeof(Web) };

		// Token: 0x04009E39 RID: 40505
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiWithParentCtorArgs = new object[1];

		// Token: 0x04009E3A RID: 40506
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Type[] apiEmptyCtorTypes = new Type[0];

		// Token: 0x04009E3B RID: 40507
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly object[] apiEmptyCtorArgs = new object[0];

		// Token: 0x04009E3C RID: 40508
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly CustomSampler apiHandlerSampler = CustomSampler.Create("API_Handler", false);
	}
}
