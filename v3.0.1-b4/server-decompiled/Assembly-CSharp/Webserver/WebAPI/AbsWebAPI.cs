using System;
using Webserver.Permissions;

namespace Webserver.WebAPI
{
	// Token: 0x02001AA5 RID: 6821
	public abstract class AbsWebAPI
	{
		// Token: 0x0600CD23 RID: 52515 RVA: 0x004AD8A9 File Offset: 0x004ABAA9
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(string _name = null)
			: this(null, _name)
		{
		}

		// Token: 0x0600CD24 RID: 52516 RVA: 0x004AD8B4 File Offset: 0x004ABAB4
		[PublicizedFrom(EAccessModifier.Protected)]
		public AbsWebAPI(Web _parentWeb, string _name = null)
		{
			this.Name = _name ?? base.GetType().Name;
			this.ParentWeb = _parentWeb;
			this.CachedApiModuleName = "webapi." + this.Name;
			this.RegisterPermissions();
		}

		// Token: 0x0600CD25 RID: 52517 RVA: 0x004AD900 File Offset: 0x004ABB00
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual void RegisterPermissions()
		{
			AdminWebModules.Instance.AddKnownModule(new AdminWebModules.WebModule(this.CachedApiModuleName, this.DefaultPermissionLevel(), true));
		}

		// Token: 0x0600CD26 RID: 52518
		public abstract void HandleRequest(RequestContext _context);

		// Token: 0x0600CD27 RID: 52519 RVA: 0x004AD91E File Offset: 0x004ABB1E
		public virtual bool Authorized(RequestContext _context)
		{
			return AdminWebModules.Instance.GetModule(this.CachedApiModuleName).LevelGlobal >= _context.PermissionLevel;
		}

		// Token: 0x0600CD28 RID: 52520 RVA: 0x00010E62 File Offset: 0x0000F062
		public virtual int DefaultPermissionLevel()
		{
			return 0;
		}

		// Token: 0x04009B71 RID: 39793
		public readonly string Name;

		// Token: 0x04009B72 RID: 39794
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly Web ParentWeb;

		// Token: 0x04009B73 RID: 39795
		[PublicizedFrom(EAccessModifier.Protected)]
		public readonly string CachedApiModuleName;
	}
}
