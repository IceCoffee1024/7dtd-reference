using System;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B20 RID: 6944
	public class RewriteHandler : AbsHandler
	{
		// Token: 0x0600D0E9 RID: 53481 RVA: 0x004BD590 File Offset: 0x004BB790
		public RewriteHandler(string _target, bool _fixedTarget = false)
			: base(null, 0)
		{
			this.target = _target;
			this.fixedTarget = _fixedTarget;
		}

		// Token: 0x0600D0EA RID: 53482 RVA: 0x004BD5A8 File Offset: 0x004BB7A8
		public override void HandleRequest(RequestContext _context)
		{
			_context.RequestPath = (this.fixedTarget ? this.target : (this.target + _context.RequestPath.Remove(0, this.urlBasePath.Length)));
			this.parent.ApplyPathHandler(_context);
		}

		// Token: 0x04009E6B RID: 40555
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly string target;

		// Token: 0x04009E6C RID: 40556
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool fixedTarget;
	}
}
