using System;
using System.Net;
using UnityEngine.Scripting;

namespace Webserver.WebAPI.APIs
{
	// Token: 0x02001AAD RID: 6829
	[Preserve]
	public class OpenAPI : AbsRestApi
	{
		// Token: 0x0600CD53 RID: 52563 RVA: 0x004AEAF8 File Offset: 0x004ACCF8
		public OpenAPI(Web _parentWeb)
			: base(_parentWeb, null)
		{
		}

		// Token: 0x0600CD54 RID: 52564 RVA: 0x004AEB04 File Offset: 0x004ACD04
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void HandleRestGet(RequestContext _context)
		{
			string requestPath = _context.RequestPath;
			string text;
			if (!this.ParentWeb.OpenApiHelpers.TryGetOpenApiSpec(requestPath, out text))
			{
				WebUtils.WriteText(_context.Response, "Spec for " + requestPath + " not found", HttpStatusCode.NotFound, null);
				return;
			}
			WebUtils.WriteText(_context.Response, text, HttpStatusCode.OK, "text/x-yaml");
		}

		// Token: 0x0600CD55 RID: 52565 RVA: 0x004AEB65 File Offset: 0x004ACD65
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}
	}
}
