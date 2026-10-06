using System;
using Webserver.Permissions;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001B19 RID: 6937
	public abstract class AbsHandler
	{
		// Token: 0x17001A2A RID: 6698
		// (get) Token: 0x0600D0BC RID: 53436 RVA: 0x004BC575 File Offset: 0x004BA775
		public string UrlBasePath
		{
			get
			{
				return this.urlBasePath;
			}
		}

		// Token: 0x0600D0BD RID: 53437 RVA: 0x004BC57D File Offset: 0x004BA77D
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsHandler(string _moduleName, int _defaultPermissionLevel = 0)
		{
			this.ModuleName = _moduleName;
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(_moduleName, _defaultPermissionLevel, true));
		}

		// Token: 0x0600D0BE RID: 53438
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600D0BF RID: 53439 RVA: 0x004BC59E File Offset: 0x004BA79E
		public virtual bool IsAuthorizedForHandler(RequestContext _context)
		{
			return this.ModuleName == null || AdminWebModules.Instance.ModuleAllowedWithLevel(this.ModuleName, _context.PermissionLevel);
		}

		// Token: 0x0600D0C0 RID: 53440 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Shutdown()
		{
		}

		// Token: 0x0600D0C1 RID: 53441 RVA: 0x004BC5C0 File Offset: 0x004BA7C0
		public virtual void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			this.parent = _parent;
			this.urlBasePath = _relativePath;
			this.parent.OpenApiHelpers.LoadOpenApiSpec(this);
		}

		// Token: 0x04009E34 RID: 40500
		public readonly string ModuleName;

		// Token: 0x04009E35 RID: 40501
		[PublicizedFrom(EAccessModifier.Protected)]
		public string urlBasePath;

		// Token: 0x04009E36 RID: 40502
		[PublicizedFrom(EAccessModifier.Protected)]
		public Web parent;
	}
}
