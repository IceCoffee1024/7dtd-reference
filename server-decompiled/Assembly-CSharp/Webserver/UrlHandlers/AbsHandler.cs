using System;
using Webserver.Permissions;

namespace Webserver.UrlHandlers
{
	// Token: 0x02001AC6 RID: 6854
	public abstract class AbsHandler
	{
		// Token: 0x17001916 RID: 6422
		// (get) Token: 0x0600CDDB RID: 52699 RVA: 0x004B1CB5 File Offset: 0x004AFEB5
		public string UrlBasePath
		{
			get
			{
				return this.urlBasePath;
			}
		}

		// Token: 0x0600CDDC RID: 52700 RVA: 0x004B1CBD File Offset: 0x004AFEBD
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsHandler(string _moduleName, int _defaultPermissionLevel = 0)
		{
			this.ModuleName = _moduleName;
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(_moduleName, _defaultPermissionLevel, true));
		}

		// Token: 0x0600CDDD RID: 52701
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600CDDE RID: 52702 RVA: 0x004B1CDE File Offset: 0x004AFEDE
		public virtual bool IsAuthorizedForHandler(RequestContext _context)
		{
			return this.ModuleName == null || AdminWebModules.Instance.ModuleAllowedWithLevel(this.ModuleName, _context.PermissionLevel);
		}

		// Token: 0x0600CDDF RID: 52703 RVA: 0x000027FC File Offset: 0x000009FC
		public virtual void Shutdown()
		{
		}

		// Token: 0x0600CDE0 RID: 52704 RVA: 0x004B1D00 File Offset: 0x004AFF00
		public virtual void SetBasePathAndParent(Web _parent, string _relativePath)
		{
			this.parent = _parent;
			this.urlBasePath = _relativePath;
			this.parent.OpenApiHelpers.LoadOpenApiSpec(this);
		}

		// Token: 0x04009C3D RID: 39997
		public readonly string ModuleName;

		// Token: 0x04009C3E RID: 39998
		[PublicizedFrom(EAccessModifier.Protected)]
		public string urlBasePath;

		// Token: 0x04009C3F RID: 39999
		[PublicizedFrom(EAccessModifier.Protected)]
		public Web parent;
	}
}
