using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HarmonyLib
{
	// Token: 0x02000041 RID: 65
	[Serializable]
	public class PatchInfo
	{
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000122 RID: 290 RVA: 0x00006228 File Offset: 0x00004428
		public bool Debugging
		{
			get
			{
				if (!this.prefixes.Any<Patch>((Patch p) => p.debug))
				{
					if (!this.postfixes.Any<Patch>((Patch p) => p.debug))
					{
						if (!this.transpilers.Any<Patch>((Patch p) => p.debug))
						{
							if (!this.finalizers.Any<Patch>((Patch p) => p.debug))
							{
								return this.ilmanipulators.Any<Patch>((Patch p) => p.debug);
							}
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000123 RID: 291 RVA: 0x00006318 File Offset: 0x00004518
		public string[] DebugEmitPaths
		{
			get
			{
				return (from p in this.prefixes.Concat<Patch>(this.postfixes).Concat<Patch>(this.transpilers).Concat<Patch>(this.finalizers)
						.Concat<Patch>(this.ilmanipulators)
					select p.debugEmitPath into p
					where p != null
					select p).ToArray<string>();
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000063A4 File Offset: 0x000045A4
		internal void AddPrefixes(string owner, params HarmonyMethod[] methods)
		{
			this.prefixes = PatchInfo.Add(owner, methods, this.prefixes);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x000063BC File Offset: 0x000045BC
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddPrefix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddPrefixes(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000063EB File Offset: 0x000045EB
		public void RemovePrefix(string owner)
		{
			this.prefixes = PatchInfo.Remove(owner, this.prefixes);
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000063FF File Offset: 0x000045FF
		internal void AddPostfixes(string owner, params HarmonyMethod[] methods)
		{
			this.postfixes = PatchInfo.Add(owner, methods, this.postfixes);
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006414 File Offset: 0x00004614
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddPostfix(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddPostfixes(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006443 File Offset: 0x00004643
		public void RemovePostfix(string owner)
		{
			this.postfixes = PatchInfo.Remove(owner, this.postfixes);
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00006457 File Offset: 0x00004657
		internal void AddTranspilers(string owner, params HarmonyMethod[] methods)
		{
			this.transpilers = PatchInfo.Add(owner, methods, this.transpilers);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000646C File Offset: 0x0000466C
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddTranspiler(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddTranspilers(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000649B File Offset: 0x0000469B
		public void RemoveTranspiler(string owner)
		{
			this.transpilers = PatchInfo.Remove(owner, this.transpilers);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000064AF File Offset: 0x000046AF
		internal void AddFinalizers(string owner, params HarmonyMethod[] methods)
		{
			this.finalizers = PatchInfo.Add(owner, methods, this.finalizers);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000064C4 File Offset: 0x000046C4
		[Obsolete("This method only exists for backwards compatibility since the class is public.")]
		public void AddFinalizer(MethodInfo patch, string owner, int priority, string[] before, string[] after, bool debug)
		{
			this.AddFinalizers(owner, new HarmonyMethod[]
			{
				new HarmonyMethod(patch, priority, before, after, new bool?(debug))
			});
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000064F3 File Offset: 0x000046F3
		public void RemoveFinalizer(string owner)
		{
			this.finalizers = PatchInfo.Remove(owner, this.finalizers);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006507 File Offset: 0x00004707
		internal void AddILManipulators(string owner, params HarmonyMethod[] methods)
		{
			this.ilmanipulators = PatchInfo.Add(owner, methods, this.ilmanipulators);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0000651C File Offset: 0x0000471C
		public void RemoveILManipulator(string owner)
		{
			this.ilmanipulators = PatchInfo.Remove(owner, this.ilmanipulators);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006530 File Offset: 0x00004730
		public void RemovePatch(MethodInfo patch)
		{
			this.prefixes = this.prefixes.Where<Patch>((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.postfixes = this.postfixes.Where<Patch>((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.transpilers = this.transpilers.Where<Patch>((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.finalizers = this.finalizers.Where<Patch>((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
			this.ilmanipulators = this.ilmanipulators.Where<Patch>((Patch p) => p.PatchMethod != patch).ToArray<Patch>();
		}

		// Token: 0x06000133 RID: 307 RVA: 0x000065F4 File Offset: 0x000047F4
		private static Patch[] Add(string owner, HarmonyMethod[] add, Patch[] current)
		{
			if (add.Length == 0)
			{
				return current;
			}
			int initialIndex = current.Length;
			List<Patch> list = new List<Patch>();
			list.AddRange(current);
			list.AddRange(add.Where<HarmonyMethod>((HarmonyMethod method) => method != null).Select<HarmonyMethod, Patch>((HarmonyMethod method, int i) => new Patch(method, i + initialIndex, owner)));
			return list.ToArray();
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000666C File Offset: 0x0000486C
		private static Patch[] Remove(string owner, Patch[] current)
		{
			if (!(owner == "*"))
			{
				return current.Where<Patch>((Patch patch) => patch.owner != owner).ToArray<Patch>();
			}
			return new Patch[0];
		}

		// Token: 0x04000074 RID: 116
		public Patch[] prefixes = new Patch[0];

		// Token: 0x04000075 RID: 117
		public Patch[] postfixes = new Patch[0];

		// Token: 0x04000076 RID: 118
		public Patch[] transpilers = new Patch[0];

		// Token: 0x04000077 RID: 119
		public Patch[] finalizers = new Patch[0];

		// Token: 0x04000078 RID: 120
		public Patch[] ilmanipulators = new Patch[0];
	}
}
