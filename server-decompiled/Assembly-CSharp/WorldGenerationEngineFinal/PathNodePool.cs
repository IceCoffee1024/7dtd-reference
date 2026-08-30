using System;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001715 RID: 5909
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNodePool
	{
		// Token: 0x0600B80D RID: 47117 RVA: 0x00444F4C File Offset: 0x0044314C
		public PathNodePool(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600B80E RID: 47118 RVA: 0x00444F88 File Offset: 0x00443188
		public void Init(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600B80F RID: 47119 RVA: 0x00444FC4 File Offset: 0x004431C4
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

		// Token: 0x0600B810 RID: 47120 RVA: 0x00445009 File Offset: 0x00443209
		public void ReturnAll()
		{
			this.free = 0;
		}

		// Token: 0x0600B811 RID: 47121 RVA: 0x00445012 File Offset: 0x00443212
		public void Node(int _index, out PathNode _node)
		{
			_node = this.pool[_index];
		}

		// Token: 0x0600B812 RID: 47122 RVA: 0x00445026 File Offset: 0x00443226
		public ref PathNode Node(int _index)
		{
			return this.pool.ElementAt(_index);
		}

		// Token: 0x0600B813 RID: 47123 RVA: 0x00445034 File Offset: 0x00443234
		public void Cleanup()
		{
			this.pool.Dispose();
		}

		// Token: 0x0600B814 RID: 47124 RVA: 0x00445041 File Offset: 0x00443241
		public void LogStats()
		{
			Log.Out(string.Format("PathNodePool: Capacity={0}, Allocated={1}, Free={2}", this.pool.Capacity, this.pool.Length, this.free));
		}

		// Token: 0x04008981 RID: 35201
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<PathNode> pool;

		// Token: 0x04008982 RID: 35202
		[PublicizedFrom(EAccessModifier.Private)]
		public int free;
	}
}
