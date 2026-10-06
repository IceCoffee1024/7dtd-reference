using System;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001741 RID: 5953
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNodePool
	{
		// Token: 0x0600B964 RID: 47460 RVA: 0x0044946C File Offset: 0x0044766C
		public PathNodePool(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600B965 RID: 47461 RVA: 0x004494A8 File Offset: 0x004476A8
		public void Init(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600B966 RID: 47462 RVA: 0x004494E4 File Offset: 0x004476E4
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

		// Token: 0x0600B967 RID: 47463 RVA: 0x00449529 File Offset: 0x00447729
		public void ReturnAll()
		{
			this.free = 0;
		}

		// Token: 0x0600B968 RID: 47464 RVA: 0x00449532 File Offset: 0x00447732
		public void Node(int _index, out PathNode _node)
		{
			_node = this.pool[_index];
		}

		// Token: 0x0600B969 RID: 47465 RVA: 0x00449546 File Offset: 0x00447746
		public ref PathNode Node(int _index)
		{
			return this.pool.ElementAt(_index);
		}

		// Token: 0x0600B96A RID: 47466 RVA: 0x00449554 File Offset: 0x00447754
		public void Cleanup()
		{
			this.pool.Dispose();
		}

		// Token: 0x0600B96B RID: 47467 RVA: 0x00449561 File Offset: 0x00447761
		public void LogStats()
		{
			Log.Out(string.Format("PathNodePool: Capacity={0}, Allocated={1}, Free={2}", this.pool.Capacity, this.pool.Length, this.free));
		}

		// Token: 0x04008A46 RID: 35398
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<PathNode> pool;

		// Token: 0x04008A47 RID: 35399
		[PublicizedFrom(EAccessModifier.Private)]
		public int free;
	}
}
