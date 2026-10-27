using System;
using System.Net;
using UnityEngine.Scripting;

namespace Webserver.WebAPI.APIs
{
	// Token: 0x02001B31 RID: 6961
	[Preserve]
	public class OpenAPI : AbsRestApi
	{
		// Token: 0x0600D148 RID: 53576 RVA: 0x004BCEB4 File Offset: 0x004BB0B4
		public OpenAPI(Web _parentWeb)
			: base(_parentWeb, null)
		{
		}

		// Token: 0x0600D149 RID: 53577 RVA: 0x004BCEC0 File Offset: 0x004BB0C0
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

		// Token: 0x0600D14A RID: 53578 RVA: 0x004BCF21 File Offset: 0x004BB121
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}
	}
}
