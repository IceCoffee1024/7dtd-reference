using System;
using System.Net;
using UnityEngine.Scripting;

namespace Webserver.WebAPI.APIs
{
	// Token: 0x02001B00 RID: 6912
	[Preserve]
	public class OpenAPI : AbsRestApi
	{
		// Token: 0x0600D034 RID: 53300 RVA: 0x004B93B8 File Offset: 0x004B75B8
		public OpenAPI(Web _parentWeb)
			: base(_parentWeb, null)
		{
		}

		// Token: 0x0600D035 RID: 53301 RVA: 0x004B93C4 File Offset: 0x004B75C4
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

		// Token: 0x0600D036 RID: 53302 RVA: 0x004B9425 File Offset: 0x004B7625
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}
	}
}
