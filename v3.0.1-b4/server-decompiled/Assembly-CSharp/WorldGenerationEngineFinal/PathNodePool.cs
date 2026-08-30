using System;
using Unity.Burst;
using Unity.Collections;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016EF RID: 5871
	[BurstCompile(CompileSynchronously = true)]
	public struct PathNodePool
	{
		// Token: 0x0600B68A RID: 46730 RVA: 0x0043F1D4 File Offset: 0x0043D3D4
		public PathNodePool(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600B68B RID: 46731 RVA: 0x0043F210 File Offset: 0x0043D410
		public void Init(int _size)
		{
			this.pool = new NativeList<PathNode>(_size, Allocator.Persistent);
			PathNode pathNode = default(PathNode);
			this.pool.AddReplicate(in pathNode, _size);
			this.free = 0;
		}

		// Token: 0x0600B68C RID: 46732 RVA: 0x0043F24C File Offset: 0x0043D44C
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

		// Token: 0x0600B68D RID: 46733 RVA: 0x0043F291 File Offset: 0x0043D491
		public void ReturnAll()
		{
			this.free = 0;
		}

		// Token: 0x0600B68E RID: 46734 RVA: 0x0043F29A File Offset: 0x0043D49A
		public void Node(int _index, out PathNode _node)
		{
			_node = this.pool[_index];
		}

		// Token: 0x0600B68F RID: 46735 RVA: 0x0043F2AE File Offset: 0x0043D4AE
		public ref PathNode Node(int _index)
		{
			return this.pool.ElementAt(_index);
		}

		// Token: 0x0600B690 RID: 46736 RVA: 0x0043F2BC File Offset: 0x0043D4BC
		public void Cleanup()
		{
			this.pool.Dispose();
		}

		// Token: 0x0600B691 RID: 46737 RVA: 0x0043F2C9 File Offset: 0x0043D4C9
		public void LogStats()
		{
			Log.Out(string.Format("PathNodePool: Capacity={0}, Allocated={1}, Free={2}", this.pool.Capacity, this.pool.Length, this.free));
		}

		// Token: 0x04008865 RID: 34917
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<PathNode> pool;

		// Token: 0x04008866 RID: 34918
		[PublicizedFrom(EAccessModifier.Private)]
		public int free;
	}
}
