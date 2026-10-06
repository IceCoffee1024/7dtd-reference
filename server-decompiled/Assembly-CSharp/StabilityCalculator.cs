using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000B07 RID: 2823
public class StabilityCalculator
{
	// Token: 0x060053AB RID: 21419 RVA: 0x00201F10 File Offset: 0x00200110
	public void Init(WorldBase _world)
	{
		this.bRunning = true;
		StabilityCalculator.world = _world;
		this.channelCalculator = new ChannelCalculator(StabilityCalculator.world);
		this.queueStabilityAvail = new Queue<Vector3i>();
		this.queueStabilityEmpty = new Queue<Vector3i>();
		this.ienumCoroutine = new StabilityCalculator.UpdatePhysics(this);
		ThreadManager.StartCoroutine(this.ienumCoroutine);
	}

	// Token: 0x060053AC RID: 21420 RVA: 0x00201F68 File Offset: 0x00200168
	public void Cleanup()
	{
		this.bRunning = false;
		if (this.ienumCoroutine != null)
		{
			ThreadManager.StopCoroutine(this.ienumCoroutine);
			this.ienumCoroutine = null;
		}
		this.channelCalculator = null;
	}

	// Token: 0x060053AD RID: 21421 RVA: 0x00201F94 File Offset: 0x00200194
	public void BlockRemovedAt(Vector3i _pos)
	{
		if (_pos.y >= 255)
		{
			return;
		}
		this.stab0Positions.Clear();
		this.channelCalculator.BlockRemovedAt(_pos, this.stab0Positions);
		if (StabilityCalculator.world.IsRemote())
		{
			return;
		}
		IChunk chunk = null;
		foreach (Vector3i vector3i in Vector3i.AllDirections)
		{
			Vector3i vector3i2 = _pos + vector3i;
			if (StabilityCalculator.world.GetChunkFromWorldPos(vector3i2, ref chunk))
			{
				int num = World.toBlockXZ(vector3i2.x);
				int num2 = World.toBlockY(vector3i2.y);
				int num3 = World.toBlockXZ(vector3i2.z);
				BlockValue blockNoDamage;
				BlockValue blockValue = (blockNoDamage = chunk.GetBlockNoDamage(num, num2, num3));
				if (!blockNoDamage.isair && !blockValue.Block.blockMaterial.IsLiquid)
				{
					int stability = (int)chunk.GetStability(num, num2, num3);
					if (stability == 0)
					{
						if (!this.stab0Positions.Contains(vector3i2))
						{
							this.queueStabilityEmpty.Enqueue(vector3i2);
						}
					}
					else if (stability < 15 && this.queueStabilityAvail.Count < 200)
					{
						this.queueStabilityAvail.Enqueue(vector3i2);
					}
				}
			}
		}
		foreach (Vector3i vector3i3 in this.stab0Positions)
		{
			this.queueStabilityEmpty.Enqueue(vector3i3);
		}
	}

	// Token: 0x060053AE RID: 21422 RVA: 0x00202114 File Offset: 0x00200314
	public void BlockPlacedAt(Vector3i _pos, bool _bForceFullStabe = false)
	{
		this.channelCalculator.BlockPlacedAt(_pos, _bForceFullStabe);
		if (StabilityCalculator.world.IsRemote())
		{
			return;
		}
		if (this.queueStabilityAvail.Count < 200)
		{
			this.queueStabilityAvail.Enqueue(_pos);
		}
	}

	// Token: 0x060053AF RID: 21423 RVA: 0x00202150 File Offset: 0x00200350
	[PublicizedFrom(EAccessModifier.Private)]
	public void physicsIsolation(Vector3i _pos)
	{
		this.queueIsolation.Clear();
		this.queueIsolation.Enqueue(_pos);
		this.hashSetIsolation.Clear();
		this.hashSetIsolation.Add(_pos);
		this.hashSetProcessed.Clear();
		this.hashSetProcessed.Add(_pos);
		IChunk chunk = null;
		Vector3i vector3i = Vector3i.zero;
		while (this.queueIsolation.Count > 0)
		{
			Vector3i vector3i2 = this.queueIsolation.Dequeue();
			Vector3i[] allDirections = Vector3i.AllDirections;
			for (int i = 0; i < allDirections.Length; i++)
			{
				vector3i = vector3i2 + allDirections[i];
				if (StabilityCalculator.world.GetChunkFromWorldPos(vector3i, ref chunk))
				{
					Vector3i vector3i3 = World.toBlock(vector3i);
					BlockValue blockNoDamage = chunk.GetBlockNoDamage(vector3i3.x, vector3i3.y, vector3i3.z);
					if (!blockNoDamage.isair && !blockNoDamage.Block.blockMaterial.IsLiquid && !blockNoDamage.Block.StabilityIgnore && !this.hashSetProcessed.Contains(vector3i))
					{
						this.hashSetProcessed.Add(vector3i);
						if (chunk.GetStability(vector3i3.x, vector3i3.y, vector3i3.z) <= 0)
						{
							if (!blockNoDamage.ischild)
							{
								this.hashSetIsolation.Add(vector3i);
								if (this.hashSetIsolation.Count >= 1000)
								{
									return;
								}
							}
							this.queueIsolation.Enqueue(vector3i);
						}
					}
				}
			}
		}
	}

	// Token: 0x060053B0 RID: 21424 RVA: 0x002022CD File Offset: 0x002004CD
	public static float GetBlockStability(Vector3i _pos)
	{
		float blockStability = StabilityCalculator.GetBlockStability(_pos, BlockValue.Air);
		StabilityCalculator.posChecked.Clear();
		StabilityCalculator.posToCheck.Clear();
		StabilityCalculator.posToCheckNext.Clear();
		return blockStability;
	}

	// Token: 0x060053B1 RID: 21425 RVA: 0x002022F8 File Offset: 0x002004F8
	public static float GetBlockStabilityIfPlaced(Vector3i _pos, BlockValue _bv)
	{
		MicroStopwatch microStopwatch = new MicroStopwatch();
		Block block = _bv.Block;
		int num = (block.StabilitySupport ? 1 : 0);
		float num2;
		if (block.isMultiBlock)
		{
			int type = _bv.type;
			int rotation = (int)_bv.rotation;
			Vector3i vector3i = Vector3i.max;
			Vector3i vector3i2 = Vector3i.min;
			for (int i = block.multiBlockPos.Length - 1; i >= 0; i--)
			{
				Vector3i vector3i3 = block.multiBlockPos.Get(i, type, rotation) + _pos;
				if (!StabilityCalculator.posPlaced.ContainsKey(vector3i3))
				{
					StabilityCalculator.posPlaced.Add(vector3i3, num);
					vector3i = Vector3i.Min(vector3i, vector3i3);
					vector3i2 = Vector3i.Max(vector3i2, vector3i3);
				}
			}
			IChunk chunk = null;
			for (int j = vector3i.z; j <= vector3i2.z; j++)
			{
				Vector3i vector3i4;
				vector3i4.z = j;
				for (int k = vector3i.x; k <= vector3i2.x; k++)
				{
					vector3i4.x = k;
					StabilityCalculator.world.GetChunkFromWorldPos(k, j, ref chunk);
					if (chunk != null && chunk.GetStability(k & 15, vector3i.y - 1, j & 15) == 15)
					{
						for (int l = vector3i.y; l <= vector3i2.y; l++)
						{
							vector3i4.y = l;
							StabilityCalculator.posPlaced[vector3i4] = 15;
						}
					}
				}
			}
			num2 = 1f;
			using (Dictionary<Vector3i, int>.KeyCollection.Enumerator enumerator = StabilityCalculator.posPlaced.Keys.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Vector3i vector3i5 = enumerator.Current;
					if (vector3i5.x == vector3i.x || vector3i5.x == vector3i2.x || vector3i5.y == vector3i.y || vector3i5.y == vector3i2.y || vector3i5.z == vector3i.z || vector3i5.z == vector3i2.z)
					{
						float blockStability = StabilityCalculator.GetBlockStability(vector3i5, _bv);
						if (blockStability < num2)
						{
							num2 = blockStability;
							if (blockStability == 0f)
							{
								break;
							}
						}
						if (microStopwatch.ElapsedMicroseconds > 25000L)
						{
							break;
						}
					}
				}
				goto IL_0247;
			}
		}
		StabilityCalculator.posPlaced.Add(_pos, num);
		num2 = StabilityCalculator.GetBlockStability(_pos, _bv);
		IL_0247:
		StabilityCalculator.posPlaced.Clear();
		return num2;
	}

	// Token: 0x060053B2 RID: 21426 RVA: 0x00202568 File Offset: 0x00200768
	[PublicizedFrom(EAccessModifier.Private)]
	public static float GetBlockStability(Vector3i _pos, BlockValue _newBV)
	{
		StabilityCalculator.posChecked.Clear();
		StabilityCalculator.posToCheck.Clear();
		StabilityCalculator.posToCheckNext.Clear();
		if (!Block.BlocksLoaded)
		{
			return 1f;
		}
		if (!GameManager.bPhysicsActive)
		{
			return 1f;
		}
		BlockValue blockValue = (StabilityCalculator.posPlaced.ContainsKey(_pos) ? _newBV : StabilityCalculator.world.GetBlock(_pos));
		int num = 0;
		int num2 = 0;
		StabilityCalculator.posChecked.Add(_pos);
		StabilityCalculator.posToCheck.Add(_pos);
		int num3 = 0;
		int num4 = 0;
		float num5 = 0f;
		new Vector3(0f, -1f, 0f);
		IChunk chunk = null;
		while (num4 < 25 && StabilityCalculator.posToCheck.Count > 0)
		{
			num4++;
			int num6 = num;
			int i = 0;
			while (i < StabilityCalculator.posToCheck.Count)
			{
				Vector3i vector3i = StabilityCalculator.posToCheck[i];
				int num7;
				BlockValue blockValue2;
				if (StabilityCalculator.posPlaced.TryGetValue(vector3i, out num7))
				{
					blockValue2 = _newBV;
					goto IL_0110;
				}
				StabilityCalculator.world.GetChunkFromWorldPos(vector3i.x, vector3i.z, ref chunk);
				if (chunk != null)
				{
					blockValue2 = chunk.GetBlockNoDamage(vector3i.x & 15, vector3i.y, vector3i.z & 15);
					goto IL_0110;
				}
				IL_0286:
				i++;
				continue;
				IL_0110:
				num3 += 7;
				num2 += blockValue2.Block.blockMaterial.Mass.Value;
				Vector3i[] allDirectionsShuffled = Vector3i.AllDirectionsShuffled;
				for (int j = 0; j < allDirectionsShuffled.Length; j++)
				{
					Vector3i vector3i2 = vector3i + allDirectionsShuffled[j];
					if (vector3i2.y >= 0)
					{
						int stability;
						BlockValue blockValue3;
						if (StabilityCalculator.posPlaced.TryGetValue(vector3i2, out stability))
						{
							blockValue3 = _newBV;
						}
						else
						{
							StabilityCalculator.world.GetChunkFromWorldPos(vector3i2.x, vector3i2.z, ref chunk);
							if (chunk == null)
							{
								goto IL_0275;
							}
							int num8 = vector3i2.x & 15;
							int num9 = vector3i2.z & 15;
							blockValue3 = chunk.GetBlockNoDamage(num8, vector3i2.y, num9);
							stability = (int)chunk.GetStability(num8, vector3i2.y, num9);
						}
						if (stability > 0)
						{
							if (stability == 15)
							{
								vector3i;
								int forceToOtherBlock = blockValue2.GetForceToOtherBlock(blockValue3);
								if (allDirectionsShuffled[j].y == -1)
								{
									num6 = 100000;
								}
								else
								{
									num6 += forceToOtherBlock;
								}
								num += forceToOtherBlock;
							}
							else if ((stability > 1 || blockValue3.Block.StabilitySupport) && StabilityCalculator.posChecked.Add(vector3i2))
							{
								StabilityCalculator.posToCheckNext.Add(vector3i2);
								if (allDirectionsShuffled[j].y == -1)
								{
									num6 = 100000;
								}
								else
								{
									num6 += blockValue2.GetForceToOtherBlock(blockValue3);
								}
							}
						}
					}
					IL_0275:;
				}
				goto IL_0286;
			}
			if (num2 > num6)
			{
				StabilityViewer.GetBlocks += num3;
				StabilityViewer.TotalIterations += num4;
				return 0f;
			}
			if (num6 > 0)
			{
				num5 = Mathf.Max(num5, (float)num2 / ((float)num6 * 1.01f));
			}
			List<Vector3i> list = StabilityCalculator.posToCheck;
			StabilityCalculator.posToCheck = StabilityCalculator.posToCheckNext;
			StabilityCalculator.posToCheckNext = list;
			StabilityCalculator.posToCheckNext.Clear();
		}
		StabilityViewer.GetBlocks += num3;
		StabilityViewer.TotalIterations += num4;
		return 1f - num5;
	}

	// Token: 0x060053B3 RID: 21427 RVA: 0x002028A8 File Offset: 0x00200AA8
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> CalcPhysicsStabilityToFall(Vector3i _pos, int maxBlocksToCheck, out float calculatedStability)
	{
		List<Vector3i> list = null;
		calculatedStability = 0f;
		this.unstablePositions.Clear();
		this.unstablePositions.Add(_pos);
		this.positionsToCheck.Clear();
		this.positionsToCheck.Enqueue(_pos);
		this.uniqueUnstablePositions.Clear();
		int num = 0;
		int num2 = 0;
		IChunk chunk = null;
		int i = 0;
		while (i < maxBlocksToCheck)
		{
			int num3 = num;
			foreach (Vector3i vector3i in this.positionsToCheck)
			{
				StabilityCalculator.world.GetChunkFromWorldPos(vector3i, ref chunk);
				BlockValue blockValue = ((chunk != null) ? chunk.GetBlockNoDamage(World.toBlockXZ(vector3i.x), vector3i.y, World.toBlockXZ(vector3i.z)) : BlockValue.Air);
				Block block = blockValue.Block;
				num2 += block.blockMaterial.Mass.Value;
				foreach (Vector3i vector3i2 in Vector3i.AllDirectionsShuffled)
				{
					Vector3i vector3i3 = vector3i + vector3i2;
					if (chunk == null || chunk.X != World.toChunkXZ(vector3i3.x) || chunk.Z != World.toChunkXZ(vector3i3.z))
					{
						chunk = StabilityCalculator.world.GetChunkFromWorldPos(vector3i3);
					}
					int num4 = World.toBlockXZ(vector3i3.x);
					int num5 = World.toBlockXZ(vector3i3.z);
					BlockValue blockValue2 = ((chunk != null) ? chunk.GetBlockNoDamage(num4, vector3i3.y, num5) : BlockValue.Air);
					int num6 = (int)((!blockValue2.isair && chunk != null) ? chunk.GetStability(num4, vector3i3.y, num5) : 0);
					if (num6 == 15)
					{
						int forceToOtherBlock = blockValue.GetForceToOtherBlock(blockValue2);
						if (vector3i2.y == -1)
						{
							num3 = 100000;
						}
						else
						{
							num3 += forceToOtherBlock;
						}
						num += forceToOtherBlock;
					}
					else if (((num6 > 0 && blockValue2.Block.StabilitySupport) || num6 > 1) && this.unstablePositions.Add(vector3i3))
					{
						this.uniqueUnstablePositions.Enqueue(vector3i3);
						if (vector3i2.y == -1)
						{
							num3 = 100000;
						}
						else
						{
							num3 += blockValue.GetForceToOtherBlock(blockValue2);
						}
					}
				}
			}
			if (num3 > 0)
			{
				calculatedStability = 1f - (float)num2 / (float)num3;
			}
			if (num2 > num3)
			{
				list = this.unstablePositions.Except<Vector3i>(this.uniqueUnstablePositions).ToList<Vector3i>();
				if (list.Count == 0)
				{
					calculatedStability = 1f;
					break;
				}
				break;
			}
			else
			{
				if (this.uniqueUnstablePositions.Count == 0)
				{
					break;
				}
				this.positionsToCheck.Clear();
				Queue<Vector3i> queue = this.uniqueUnstablePositions;
				this.uniqueUnstablePositions = this.positionsToCheck;
				this.positionsToCheck = queue;
				this.uniqueUnstablePositions.Clear();
				i++;
			}
		}
		return list;
	}

	// Token: 0x060053B4 RID: 21428 RVA: 0x00202B98 File Offset: 0x00200D98
	[PublicizedFrom(EAccessModifier.Private)]
	public void addToFallingBlocks(IList<Vector3i> _list)
	{
		StabilityCalculator.world.AddFallingBlocks(_list);
	}

	// Token: 0x060053B5 RID: 21429 RVA: 0x00202BA8 File Offset: 0x00200DA8
	public void ClearChunkStabilityQueues(HashSetLong chunks)
	{
		this.resetTempPositions.Clear();
		while (this.queueStabilityEmpty.Count > 0)
		{
			Vector3i vector3i = this.queueStabilityEmpty.Dequeue();
			long num = WorldChunkCache.MakeChunkKey(World.toChunkXZ(vector3i.x), World.toChunkXZ(vector3i.z));
			if (!chunks.Contains(num))
			{
				this.resetTempPositions.Add(vector3i);
			}
		}
		this.queueStabilityEmpty = new Queue<Vector3i>(this.resetTempPositions);
		this.resetTempPositions.Clear();
		while (this.queueStabilityAvail.Count > 0)
		{
			Vector3i vector3i2 = this.queueStabilityAvail.Dequeue();
			long num2 = WorldChunkCache.MakeChunkKey(World.toChunkXZ(vector3i2.x), World.toChunkXZ(vector3i2.z));
			if (!chunks.Contains(num2))
			{
				this.resetTempPositions.Add(vector3i2);
			}
		}
		this.queueStabilityAvail = new Queue<Vector3i>(this.resetTempPositions);
		this.resetTempPositions.Clear();
	}

	// Token: 0x0400418A RID: 16778
	[PublicizedFrom(EAccessModifier.Private)]
	public static WorldBase world;

	// Token: 0x0400418B RID: 16779
	[PublicizedFrom(EAccessModifier.Private)]
	public ChannelCalculator channelCalculator;

	// Token: 0x0400418C RID: 16780
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bRunning;

	// Token: 0x0400418D RID: 16781
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator ienumCoroutine;

	// Token: 0x0400418E RID: 16782
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxIterations = 20;

	// Token: 0x0400418F RID: 16783
	[PublicizedFrom(EAccessModifier.Private)]
	public float updatePeriod = 0.1f;

	// Token: 0x04004190 RID: 16784
	[PublicizedFrom(EAccessModifier.Private)]
	public const int isolatedBlockLimit = 1000;

	// Token: 0x04004191 RID: 16785
	[PublicizedFrom(EAccessModifier.Private)]
	public const int stabilityQueueLimit = 200;

	// Token: 0x04004192 RID: 16786
	[PublicizedFrom(EAccessModifier.Private)]
	public int updateTimeLimit = 3000;

	// Token: 0x04004193 RID: 16787
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> queueStabilityAvail;

	// Token: 0x04004194 RID: 16788
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> queueStabilityEmpty;

	// Token: 0x04004195 RID: 16789
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> stab0Positions = new HashSet<Vector3i>();

	// Token: 0x04004196 RID: 16790
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> hashSetIsolation = new HashSet<Vector3i>();

	// Token: 0x04004197 RID: 16791
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> hashSetProcessed = new HashSet<Vector3i>();

	// Token: 0x04004198 RID: 16792
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> queueIsolation = new Queue<Vector3i>();

	// Token: 0x04004199 RID: 16793
	[PublicizedFrom(EAccessModifier.Private)]
	public static HashSet<Vector3i> posChecked = new HashSet<Vector3i>();

	// Token: 0x0400419A RID: 16794
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Vector3i> posToCheck = new List<Vector3i>(6);

	// Token: 0x0400419B RID: 16795
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<Vector3i> posToCheckNext = new List<Vector3i>(6);

	// Token: 0x0400419C RID: 16796
	[PublicizedFrom(EAccessModifier.Private)]
	public static Dictionary<Vector3i, int> posPlaced = new Dictionary<Vector3i, int>(256);

	// Token: 0x0400419D RID: 16797
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cInfiniteSupport = 100000;

	// Token: 0x0400419E RID: 16798
	[PublicizedFrom(EAccessModifier.Private)]
	public const float cSupportScale = 1.01f;

	// Token: 0x0400419F RID: 16799
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<Vector3i> unstablePositions = new HashSet<Vector3i>();

	// Token: 0x040041A0 RID: 16800
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> positionsToCheck = new Queue<Vector3i>();

	// Token: 0x040041A1 RID: 16801
	[PublicizedFrom(EAccessModifier.Private)]
	public Queue<Vector3i> uniqueUnstablePositions = new Queue<Vector3i>();

	// Token: 0x040041A2 RID: 16802
	[PublicizedFrom(EAccessModifier.Private)]
	public List<Vector3i> resetTempPositions = new List<Vector3i>();

	// Token: 0x02000B08 RID: 2824
	public class UpdatePhysics : IEnumerator<object>, IEnumerator, IDisposable
	{
		// Token: 0x060053B8 RID: 21432 RVA: 0x00202D46 File Offset: 0x00200F46
		public UpdatePhysics(StabilityCalculator _sp)
		{
			this.physicsCalculator = _sp;
			this.sw = MicroStopwatch.a();
		}

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060053B9 RID: 21433 RVA: 0x00202D60 File Offset: 0x00200F60
		public object System.Collections.Generic.IEnumerator<System.Object>.Current
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.myEnumerator;
			}
		}

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x060053BA RID: 21434 RVA: 0x00202D60 File Offset: 0x00200F60
		public object System.Collections.IEnumerator.Current
		{
			[PublicizedFrom(EAccessModifier.Private)]
			get
			{
				return this.myEnumerator;
			}
		}

		// Token: 0x060053BB RID: 21435 RVA: 0x00202D68 File Offset: 0x00200F68
		public bool MoveNext()
		{
			int num = this.state;
			this.state = -1;
			if (num > 1)
			{
				return false;
			}
			if (!GameStats.GetBool(EnumGameStats.ChunkStabilityEnabled))
			{
				this.myEnumerator = new WaitForSeconds(this.physicsCalculator.updatePeriod);
				this.state = 1;
				return true;
			}
			if (!this.physicsCalculator.bRunning)
			{
				this.state = -1;
				return false;
			}
			this.sw.ResetAndRestart();
			while (this.sw.ElapsedMicroseconds < (long)this.physicsCalculator.updateTimeLimit)
			{
				if (this.physicsCalculator.queueStabilityEmpty.Count <= 0)
				{
					break;
				}
				this.curPosition = this.physicsCalculator.queueStabilityEmpty.Dequeue();
				this.physicsCalculator.physicsIsolation(this.curPosition);
				foreach (Vector3i vector3i in this.physicsCalculator.hashSetIsolation)
				{
					((World)StabilityCalculator.world).AddFallingBlock(vector3i, false);
				}
			}
			while (this.sw.ElapsedMicroseconds < (long)this.physicsCalculator.updateTimeLimit && this.physicsCalculator.queueStabilityAvail.Count > 0)
			{
				Vector3i vector3i2 = this.physicsCalculator.queueStabilityAvail.Dequeue();
				float num2;
				List<Vector3i> list = this.physicsCalculator.CalcPhysicsStabilityToFall(vector3i2, 20, out num2);
				if (list != null)
				{
					this.physicsCalculator.addToFallingBlocks(list);
				}
			}
			this.sw.Stop();
			this.state = 1;
			return true;
		}

		// Token: 0x060053BC RID: 21436 RVA: 0x00202EF4 File Offset: 0x002010F4
		public void Dispose()
		{
			this.state = -1;
		}

		// Token: 0x060053BD RID: 21437 RVA: 0x00006566 File Offset: 0x00004766
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x040041A3 RID: 16803
		[PublicizedFrom(EAccessModifier.Private)]
		public MicroStopwatch sw;

		// Token: 0x040041A4 RID: 16804
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector3i curPosition;

		// Token: 0x040041A5 RID: 16805
		[PublicizedFrom(EAccessModifier.Private)]
		public int state;

		// Token: 0x040041A6 RID: 16806
		[PublicizedFrom(EAccessModifier.Private)]
		public object myEnumerator;

		// Token: 0x040041A7 RID: 16807
		[PublicizedFrom(EAccessModifier.Private)]
		public StabilityCalculator physicsCalculator;
	}
}
