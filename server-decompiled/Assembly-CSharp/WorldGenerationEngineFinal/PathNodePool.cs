using System;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001772 RID: 6002
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNodePool
	{
		// Token: 0x0600BA78 RID: 47736 RVA: 0x0044CE18 File Offset: 0x0044B018
		public PathNodePool(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600BA79 RID: 47737 RVA: 0x0044CE54 File Offset: 0x0044B054
		public void Init(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600BA7A RID: 47738 RVA: 0x0044CE90 File Offset: 0x0044B090
		public int Alloc()
		{
			int length = this.pool.Length;
			if (this.free >= length)
			{
				this.pool.ResizeUninitialized(length + 10000);
			}
			int num = this.free;
			this.free = num + 1;
			return num;
		}

		// Token: 0x0600BA7B RID: 47739 RVA: 0x0044CED5 File Offset: 0x0044B0D5
		public void ReturnAll()
		{
			this.free = 0;
		}

		// Token: 0x0600BA7C RID: 47740 RVA: 0x0044CEDE File Offset: 0x0044B0DE
		public void Node(int _index, out PathNode _node)
		{
			_node = this.pool[_index];
		}

		// Token: 0x0600BA7D RID: 47741 RVA: 0x0044CEF2 File Offset: 0x0044B0F2
		public ref PathNode Node(int _index)
		{
			return this.pool.ElementAt(_index);
		}

		// Token: 0x0600BA7E RID: 47742 RVA: 0x0044CF00 File Offset: 0x0044B100
		public void Cleanup()
		{
			this.pool.Dispose();
		}

		// Token: 0x0600BA7F RID: 47743 RVA: 0x0044CF0D File Offset: 0x0044B10D
		public void LogStats()
		{
			Log.Out(string.Format("PathNodePool: Capacity={0}, Allocated={1}, Free={2}", this.pool.Capacity, this.pool.Length, this.free));
		}

		// Token: 0x04008B2F RID: 35631
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<PathNode> pool;

		// Token: 0x04008B30 RID: 35632
		[PublicizedFrom(EAccessModifier.Private)]
		public int free;
	}
}
