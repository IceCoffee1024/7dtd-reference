using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib.Tools;

namespace HarmonyLib
{
	// Token: 0x02000018 RID: 24
	internal class PatchSorter
	{
		// Token: 0x06000061 RID: 97 RVA: 0x000039F0 File Offset: 0x00001BF0
		internal PatchSorter(Patch[] patches, bool debug = false)
		{
			this.patches = patches.Select<Patch, PatchSorter.PatchSortingWrapper>((Patch x) => new PatchSorter.PatchSortingWrapper(x)).ToList<PatchSorter.PatchSortingWrapper>();
			this.debug = debug;
			using (List<PatchSorter.PatchSortingWrapper>.Enumerator enumerator = this.patches.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PatchSorter.PatchSortingWrapper node = enumerator.Current;
					node.AddBeforeDependency(this.patches.Where<PatchSorter.PatchSortingWrapper>((PatchSorter.PatchSortingWrapper x) => node.innerPatch.before.Contains(x.innerPatch.owner)));
					node.AddAfterDependency(this.patches.Where<PatchSorter.PatchSortingWrapper>((PatchSorter.PatchSortingWrapper x) => node.innerPatch.after.Contains(x.innerPatch.owner)));
				}
			}
			this.patches.Sort();
		}

		// Token: 0x06000062 RID: 98 RVA: 0x00003AD4 File Offset: 0x00001CD4
		internal List<MethodInfo> Sort(MethodBase original)
		{
			return (from x in this.SortAsPatches(original)
				select x.GetMethod(original)).ToList<MethodInfo>();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003B10 File Offset: 0x00001D10
		internal Patch[] SortAsPatches(MethodBase original)
		{
			if (this.sortedPatchArray != null)
			{
				return this.sortedPatchArray;
			}
			this.handledPatches = new HashSet<PatchSorter.PatchSortingWrapper>();
			this.waitingList = new List<PatchSorter.PatchSortingWrapper>();
			this.result = new List<PatchSorter.PatchSortingWrapper>(this.patches.Count);
			Queue<PatchSorter.PatchSortingWrapper> queue = new Queue<PatchSorter.PatchSortingWrapper>(this.patches);
			while (queue.Count != 0)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in queue)
				{
					if (patchSortingWrapper.after.All<PatchSorter.PatchSortingWrapper>((PatchSorter.PatchSortingWrapper x) => this.handledPatches.Contains(x)))
					{
						this.AddNodeToResult(patchSortingWrapper);
						if (patchSortingWrapper.before.Count != 0)
						{
							this.ProcessWaitingList();
						}
					}
					else
					{
						this.waitingList.Add(patchSortingWrapper);
					}
				}
				this.CullDependency();
				queue = new Queue<PatchSorter.PatchSortingWrapper>(this.waitingList);
				this.waitingList.Clear();
			}
			this.sortedPatchArray = this.result.Select<PatchSorter.PatchSortingWrapper, Patch>((PatchSorter.PatchSortingWrapper x) => x.innerPatch).ToArray<Patch>();
			this.handledPatches = null;
			this.waitingList = null;
			this.patches = null;
			return this.sortedPatchArray;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003C5C File Offset: 0x00001E5C
		internal bool ComparePatchLists(Patch[] patches)
		{
			if (this.sortedPatchArray == null)
			{
				this.Sort(null);
			}
			return patches != null && this.sortedPatchArray.Length == patches.Length && this.sortedPatchArray.All<Patch>((Patch x) => patches.Contains(x, new PatchSorter.PatchDetailedComparer()));
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003CBC File Offset: 0x00001EBC
		private void CullDependency()
		{
			int i;
			int j;
			for (i = this.waitingList.Count - 1; i >= 0; i = j - 1)
			{
				using (HashSet<PatchSorter.PatchSortingWrapper>.Enumerator enumerator = this.waitingList[i].after.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PatchSorter.PatchSortingWrapper afterNode = enumerator.Current;
						if (!this.handledPatches.Contains(afterNode))
						{
							this.waitingList[i].RemoveAfterDependency(afterNode);
							Logger.Log(Logger.LogChannel.Debug, delegate
							{
								string text = afterNode.innerPatch.PatchMethod.FullDescription();
								string text2 = this.waitingList[i].innerPatch.PatchMethod.FullDescription();
								return "Breaking dependence between " + text + " and " + text2;
							}, this.debug);
							return;
						}
					}
				}
				j = i;
			}
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003DBC File Offset: 0x00001FBC
		private void ProcessWaitingList()
		{
			int num = this.waitingList.Count;
			int i = 0;
			while (i < num)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = this.waitingList[i];
				if (patchSortingWrapper.after.All<PatchSorter.PatchSortingWrapper>(new Func<PatchSorter.PatchSortingWrapper, bool>(this.handledPatches.Contains)))
				{
					this.waitingList.Remove(patchSortingWrapper);
					this.AddNodeToResult(patchSortingWrapper);
					num--;
					i = 0;
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003E29 File Offset: 0x00002029
		private void AddNodeToResult(PatchSorter.PatchSortingWrapper node)
		{
			this.result.Add(node);
			this.handledPatches.Add(node);
		}

		// Token: 0x04000026 RID: 38
		private List<PatchSorter.PatchSortingWrapper> patches;

		// Token: 0x04000027 RID: 39
		private HashSet<PatchSorter.PatchSortingWrapper> handledPatches;

		// Token: 0x04000028 RID: 40
		private List<PatchSorter.PatchSortingWrapper> result;

		// Token: 0x04000029 RID: 41
		private List<PatchSorter.PatchSortingWrapper> waitingList;

		// Token: 0x0400002A RID: 42
		internal Patch[] sortedPatchArray;

		// Token: 0x0400002B RID: 43
		private readonly bool debug;

		// Token: 0x0200007A RID: 122
		private class PatchSortingWrapper : IComparable
		{
			// Token: 0x0600049C RID: 1180 RVA: 0x00016F73 File Offset: 0x00015173
			internal PatchSortingWrapper(Patch patch)
			{
				this.innerPatch = patch;
				this.before = new HashSet<PatchSorter.PatchSortingWrapper>();
				this.after = new HashSet<PatchSorter.PatchSortingWrapper>();
			}

			// Token: 0x0600049D RID: 1181 RVA: 0x00016F98 File Offset: 0x00015198
			public int CompareTo(object obj)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = obj as PatchSorter.PatchSortingWrapper;
				return PatchInfoSerialization.PriorityComparer((patchSortingWrapper != null) ? patchSortingWrapper.innerPatch : null, this.innerPatch.index, this.innerPatch.priority);
			}

			// Token: 0x0600049E RID: 1182 RVA: 0x00016FD4 File Offset: 0x000151D4
			public override bool Equals(object obj)
			{
				PatchSorter.PatchSortingWrapper patchSortingWrapper = obj as PatchSorter.PatchSortingWrapper;
				return patchSortingWrapper != null && this.innerPatch.PatchMethod == patchSortingWrapper.innerPatch.PatchMethod;
			}

			// Token: 0x0600049F RID: 1183 RVA: 0x00017008 File Offset: 0x00015208
			public override int GetHashCode()
			{
				return this.innerPatch.PatchMethod.GetHashCode();
			}

			// Token: 0x060004A0 RID: 1184 RVA: 0x0001701C File Offset: 0x0001521C
			internal void AddBeforeDependency(IEnumerable<PatchSorter.PatchSortingWrapper> dependencies)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in dependencies)
				{
					this.before.Add(patchSortingWrapper);
					patchSortingWrapper.after.Add(this);
				}
			}

			// Token: 0x060004A1 RID: 1185 RVA: 0x00017078 File Offset: 0x00015278
			internal void AddAfterDependency(IEnumerable<PatchSorter.PatchSortingWrapper> dependencies)
			{
				foreach (PatchSorter.PatchSortingWrapper patchSortingWrapper in dependencies)
				{
					this.after.Add(patchSortingWrapper);
					patchSortingWrapper.before.Add(this);
				}
			}

			// Token: 0x060004A2 RID: 1186 RVA: 0x000170D4 File Offset: 0x000152D4
			internal void RemoveAfterDependency(PatchSorter.PatchSortingWrapper afterNode)
			{
				this.after.Remove(afterNode);
				afterNode.before.Remove(this);
			}

			// Token: 0x060004A3 RID: 1187 RVA: 0x000170F0 File Offset: 0x000152F0
			internal void RemoveBeforeDependency(PatchSorter.PatchSortingWrapper beforeNode)
			{
				this.before.Remove(beforeNode);
				beforeNode.after.Remove(this);
			}

			// Token: 0x0400015C RID: 348
			internal readonly HashSet<PatchSorter.PatchSortingWrapper> after;

			// Token: 0x0400015D RID: 349
			internal readonly HashSet<PatchSorter.PatchSortingWrapper> before;

			// Token: 0x0400015E RID: 350
			internal readonly Patch innerPatch;
		}

		// Token: 0x0200007B RID: 123
		internal class PatchDetailedComparer : IEqualityComparer<Patch>
		{
			// Token: 0x060004A4 RID: 1188 RVA: 0x0001710C File Offset: 0x0001530C
			public bool Equals(Patch x, Patch y)
			{
				return y != null && x != null && x.owner == y.owner && x.PatchMethod == y.PatchMethod && x.index == y.index && x.priority == y.priority && x.before.Length == y.before.Length && x.after.Length == y.after.Length && x.before.All<string>(new Func<string, bool>(y.before.Contains<string>)) && x.after.All<string>(new Func<string, bool>(y.after.Contains<string>));
			}

			// Token: 0x060004A5 RID: 1189 RVA: 0x000171CA File Offset: 0x000153CA
			public int GetHashCode(Patch obj)
			{
				return obj.GetHashCode();
			}
		}
	}
}
