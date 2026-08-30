using System;
using System.Net;
using UnityEngine.Scripting;

namespace Webserver.WebAPI.APIs
{
	// Token: 0x02001AD4 RID: 6868
	[Preserve]
	public class OpenAPI : AbsRestApi
	{
		// Token: 0x0600CEDC RID: 52956 RVA: 0x004B4EEC File Offset: 0x004B30EC
		public OpenAPI(Web _parentWeb)
			: base(_parentWeb, null)
		{
		}

		// Token: 0x0600CEDD RID: 52957 RVA: 0x004B4EF8 File Offset: 0x004B30F8
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

		// Token: 0x0600CEDE RID: 52958 RVA: 0x004B4F59 File Offset: 0x004B3159
		public override int DefaultPermissionLevel()
		{
			return 2000;
		}
	}
}
