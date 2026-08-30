using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HarmonyLib
{
	// Token: 0x02000044 RID: 68
	public class Patches
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000152 RID: 338 RVA: 0x0000796C File Offset: 0x00005B6C
		public ReadOnlyCollection<string> Owners
		{
			get
			{
				HashSet<string> hashSet = new HashSet<string>();
				hashSet.UnionWith(this.Prefixes.Select<Patch, string>((Patch p) => p.owner));
				hashSet.UnionWith(this.Postfixes.Select<Patch, string>((Patch p) => p.owner));
				hashSet.UnionWith(this.Transpilers.Select<Patch, string>((Patch p) => p.owner));
				hashSet.UnionWith(this.Finalizers.Select<Patch, string>((Patch p) => p.owner));
				hashSet.UnionWith(this.ILManipulators.Select<Patch, string>((Patch p) => p.owner));
				return hashSet.ToList<string>().AsReadOnly();
			}
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007A7C File Offset: 0x00005C7C
		public Patches(Patch[] prefixes, Patch[] postfixes, Patch[] transpilers, Patch[] finalizers, Patch[] ilmanipulators)
		{
			if (prefixes == null)
			{
				prefixes = new Patch[0];
			}
			if (postfixes == null)
			{
				postfixes = new Patch[0];
			}
			if (transpilers == null)
			{
				transpilers = new Patch[0];
			}
			if (finalizers == null)
			{
				finalizers = new Patch[0];
			}
			if (ilmanipulators == null)
			{
				ilmanipulators = new Patch[0];
			}
			this.Prefixes = prefixes.ToList<Patch>().AsReadOnly();
			this.Postfixes = postfixes.ToList<Patch>().AsReadOnly();
			this.Transpilers = transpilers.ToList<Patch>().AsReadOnly();
			this.Finalizers = finalizers.ToList<Patch>().AsReadOnly();
			this.ILManipulators = ilmanipulators.ToList<Patch>().AsReadOnly();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007B1F File Offset: 0x00005D1F
		[Obsolete("Use newer constructor instead", true)]
		public Patches(Patch[] prefixes, Patch[] postfixes, Patch[] transpilers, Patch[] finalizers)
			: this(prefixes, postfixes, transpilers, finalizers, null)
		{
		}

		// Token: 0x0400008B RID: 139
		public readonly ReadOnlyCollection<Patch> Prefixes;

		// Token: 0x0400008C RID: 140
		public readonly ReadOnlyCollection<Patch> Postfixes;

		// Token: 0x0400008D RID: 141
		public readonly ReadOnlyCollection<Patch> Transpilers;

		// Token: 0x0400008E RID: 142
		public readonly ReadOnlyCollection<Patch> Finalizers;

		// Token: 0x0400008F RID: 143
		public readonly ReadOnlyCollection<Patch> ILManipulators;
	}
}
