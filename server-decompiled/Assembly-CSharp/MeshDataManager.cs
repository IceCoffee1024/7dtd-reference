using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000B5A RID: 2906
public class MeshDataManager
{
	// Token: 0x06005825 RID: 22565 RVA: 0x0021A800 File Offset: 0x00218A00
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshDataManager()
	{
	}

	// Token: 0x06005826 RID: 22566 RVA: 0x0021A81E File Offset: 0x00218A1E
	public static void Init()
	{
		MeshDataManager.Enabled = true;
	}

	// Token: 0x06005827 RID: 22567 RVA: 0x0021A826 File Offset: 0x00218A26
	public void LateUpdate()
	{
		this.CheckUnstarted();
		this.CompleteBatches();
	}

	// Token: 0x06005828 RID: 22568 RVA: 0x0021A834 File Offset: 0x00218A34
	[PublicizedFrom(EAccessModifier.Private)]
	public void CheckUnstarted()
	{
		if (this.m_toStart.Count <= 0)
		{
			return;
		}
		Log.Error("[MeshDataManager] There were unstarted batches at the end of the frame.");
		this.StartBatches();
	}

	// Token: 0x06005829 RID: 22569 RVA: 0x0021A858 File Offset: 0x00218A58
	[PublicizedFrom(EAccessModifier.Private)]
	public void CompleteBatches()
	{
		if (this.m_running.Count <= 0)
		{
			return;
		}
		using (MeshDataManager.s_markerCompleteBatches.Auto())
		{
			LinkedListNode<MeshDataManager.JobBatch> linkedListNode = this.m_running.First;
			while (linkedListNode != null)
			{
				MeshDataManager.JobBatch value = linkedListNode.Value;
				bool flag = true;
				try
				{
					flag = value.TryApplyAndDispose();
				}
				catch (Exception ex)
				{
					Log.Error(string.Format("[MeshDataManager] Failed to apply and dispose job batch: {0}", ex));
				}
				finally
				{
					LinkedListNode<MeshDataManager.JobBatch> next = linkedListNode.Next;
					try
					{
						if (flag)
						{
							this.m_running.Remove(linkedListNode);
							value.Dispose();
						}
					}
					catch (Exception ex2)
					{
						Log.Error(string.Format("[MeshDataManager] Failed to remove or dispose job batch: {0}", ex2));
					}
					finally
					{
						linkedListNode = next;
					}
				}
			}
		}
	}

	// Token: 0x0600582A RID: 22570 RVA: 0x0021A948 File Offset: 0x00218B48
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static bool PreValidateJobData(Mesh mesh, ReadOnlySpan<Vector3> vertices, ReadOnlySpan<int> indices, ReadOnlySpan<Vector3> normals, ReadOnlySpan<Vector4> tangents, ReadOnlySpan<Color> colors, ReadOnlySpan<Vector2> texCoord0s, ReadOnlySpan<Vector2> texCoord1s)
	{
		if (!mesh)
		{
			Log.Error("Mesh is not valid.");
			return false;
		}
		int length = vertices.Length;
		if (length <= 0)
		{
			Log.Error(string.Format("Vertices.Length ({0}) <= 0", length));
			return false;
		}
		int length2 = indices.Length;
		if (length2 <= 0)
		{
			Log.Error(string.Format("Indices.Length ({0}) <= 0", length2));
			return false;
		}
		if (normals != null && normals.Length > 0 && normals.Length != length)
		{
			Log.Error(string.Format("Normals.Count ({0}) != Vertices.Length ({1})", normals.Length, length));
			return false;
		}
		if (tangents != null && tangents.Length > 0 && tangents.Length != length)
		{
			Log.Error(string.Format("Tangents.Length ({0}) != Vertices.Length ({1})", tangents.Length, length));
			return false;
		}
		if (colors != null && colors.Length > 0 && colors.Length != length)
		{
			Log.Error(string.Format("Colors.Length ({0}) != Vertices.Length ({1})", colors.Length, length));
			return false;
		}
		if (texCoord0s != null && texCoord0s.Length > 0 && texCoord0s.Length != length)
		{
			Log.Error(string.Format("TexCoord0s.Length ({0}) != Vertices.Length ({1})", texCoord0s.Length, length));
			return false;
		}
		if (texCoord1s != null && texCoord1s.Length > 0 && texCoord1s.Length != length)
		{
			Log.Error(string.Format("TexCoord1s.Length ({0}) != Vertices.Length ({1})", texCoord1s.Length, length));
			return false;
		}
		for (int i = 0; i < length2; i++)
		{
			if (*indices[i] < 0)
			{
				Log.Error(string.Format("Indices[{0}] ({1}) < 0", i, *indices[i]));
				return false;
			}
			if (*indices[i] > length)
			{
				Log.Error(string.Format("Indices[{0}] ({1}) > Vertices.Length ({2})", i, *indices[i], length));
				return false;
			}
		}
		return true;
	}

	// Token: 0x0600582B RID: 22571 RVA: 0x0021AB84 File Offset: 0x00218D84
	public void Add(Mesh mesh, ArrayListMP<Vector3> vertices, ArrayListMP<int> indices, ArrayListMP<Vector3> normals, ArrayListMP<Vector4> tangents, ArrayListMP<Color> colors, ArrayListMP<Vector2> texCoord0s, ArrayListMP<Vector2> texCoord1s, PinnedBuffer.Ownership ownership, bool generateNormals = false, bool generateTangents = false, bool recalculateUvDistributionMetrics = false, bool sameFrameUpload = false, Action onJobComplete = null)
	{
		using (MeshDataManager.s_markerAdd.Auto())
		{
			if (!MeshDataManager.Enabled)
			{
				Log.Warning("[MeshDataManager] MeshDataJob was added while disabled.");
			}
			if (MeshDataManager.PreValidateJobData(mesh, MeshDataManager.ToReadOnlySpan<Vector3>(vertices), MeshDataManager.ToReadOnlySpan<int>(indices), MeshDataManager.ToReadOnlySpan<Vector3>(normals), MeshDataManager.ToReadOnlySpan<Vector4>(tangents), MeshDataManager.ToReadOnlySpan<Color>(colors), MeshDataManager.ToReadOnlySpan<Vector2>(texCoord0s), MeshDataManager.ToReadOnlySpan<Vector2>(texCoord1s)))
			{
				MeshDataManager.JobState jobState = this.CreateJobState(mesh, vertices, indices, normals, tangents, colors, texCoord0s, texCoord1s, ownership, generateNormals, generateTangents, recalculateUvDistributionMetrics, sameFrameUpload, onJobComplete);
				this.m_toStart.Add(jobState);
			}
		}
	}

	// Token: 0x0600582C RID: 22572 RVA: 0x0021AC38 File Offset: 0x00218E38
	public void StartBatches()
	{
		using (MeshDataManager.s_markerStartBatches.Auto())
		{
			if (this.m_toStart.Count > 0)
			{
				try
				{
					this.StartBatch(this.m_toStart.Where<MeshDataManager.JobState>((MeshDataManager.JobState state) => state.SameFrameUpload));
					this.StartBatch(this.m_toStart.Where<MeshDataManager.JobState>((MeshDataManager.JobState state) => !state.SameFrameUpload));
				}
				finally
				{
					this.m_toStart.Clear();
				}
			}
		}
	}

	// Token: 0x0600582D RID: 22573 RVA: 0x0021ACFC File Offset: 0x00218EFC
	[PublicizedFrom(EAccessModifier.Private)]
	public void StartBatch(IEnumerable<MeshDataManager.JobState> stateSupplier)
	{
		bool flag = false;
		MeshDataManager.JobBatch jobBatch = null;
		try
		{
			jobBatch = new MeshDataManager.JobBatch(stateSupplier);
			if (jobBatch.States.Count > 0)
			{
				jobBatch.Start();
				this.m_running.AddLast(jobBatch);
				flag = true;
			}
		}
		catch (Exception ex)
		{
			Log.Exception(ex);
		}
		finally
		{
			if (!flag && jobBatch != null)
			{
				jobBatch.Dispose();
			}
		}
	}

	// Token: 0x0600582E RID: 22574 RVA: 0x0021AD6C File Offset: 0x00218F6C
	[PublicizedFrom(EAccessModifier.Private)]
	public static ReadOnlySpan<T> ToReadOnlySpan<T>(ArrayListMP<T> list) where T : new()
	{
		if (list != null)
		{
			return list.Items.AsSpan<T>(0, list.Count);
		}
		return ReadOnlySpan<T>.Empty;
	}

	// Token: 0x0600582F RID: 22575 RVA: 0x0021AD90 File Offset: 0x00218F90
	[PublicizedFrom(EAccessModifier.Private)]
	public MeshDataManager.JobState CreateJobState(Mesh mesh, ArrayListMP<Vector3> vertices, ArrayListMP<int> indices, ArrayListMP<Vector3> normals, ArrayListMP<Vector4> tangents, ArrayListMP<Color> colors, ArrayListMP<Vector2> texCoord0s, ArrayListMP<Vector2> texCoord1s, PinnedBuffer.Ownership ownership, bool generateNormals, bool generateTangents, bool recalculateUvDistributionMetrics, bool sameFrameUpload, Action onJobComplete)
	{
		bool flag = false;
		List<PinnedBuffer> list = new List<PinnedBuffer>();
		MeshDataManager.JobState jobState2;
		try
		{
			PinnedBuffer<Vector3> pinnedBuffer = PinnedBuffer.Create<Vector3>(vertices, ownership);
			list.Add(pinnedBuffer);
			PinnedBuffer<int> pinnedBuffer2 = PinnedBuffer.Create<int>(indices, ownership);
			list.Add(pinnedBuffer2);
			PinnedBuffer<Vector3> pinnedBuffer3;
			if (generateNormals && (normals == null || normals.Count <= 0))
			{
				pinnedBuffer3 = new PinnedBuffer<Vector3>(MemoryPools.poolVector3, pinnedBuffer.Length);
			}
			else
			{
				pinnedBuffer3 = PinnedBuffer.Create<Vector3>(normals, ownership);
				generateNormals = false;
			}
			list.Add(pinnedBuffer3);
			PinnedBuffer<Vector4> pinnedBuffer4;
			if (generateTangents && (tangents == null || tangents.Count <= 0) && pinnedBuffer3.Length > 0)
			{
				pinnedBuffer4 = new PinnedBuffer<Vector4>(MemoryPools.poolVector4, pinnedBuffer.Length);
			}
			else
			{
				pinnedBuffer4 = PinnedBuffer.Create<Vector4>(tangents, ownership);
				generateTangents = false;
			}
			list.Add(pinnedBuffer4);
			PinnedBuffer<Color> pinnedBuffer5 = PinnedBuffer.Create<Color>(colors, ownership);
			list.Add(pinnedBuffer5);
			PinnedBuffer<Vector2> pinnedBuffer6 = PinnedBuffer.Create<Vector2>(texCoord0s, ownership);
			list.Add(pinnedBuffer6);
			PinnedBuffer<Vector2> pinnedBuffer7 = PinnedBuffer.Create<Vector2>(texCoord1s, ownership);
			list.Add(pinnedBuffer7);
			MeshDataManager.JobState jobState = new MeshDataManager.JobState(mesh, pinnedBuffer, pinnedBuffer2, pinnedBuffer3, pinnedBuffer4, pinnedBuffer5, pinnedBuffer6, pinnedBuffer7, generateNormals, generateTangents, recalculateUvDistributionMetrics, sameFrameUpload, onJobComplete);
			flag = true;
			jobState2 = jobState;
		}
		finally
		{
			if (!flag)
			{
				foreach (PinnedBuffer pinnedBuffer8 in list)
				{
					pinnedBuffer8.Dispose();
				}
			}
		}
		return jobState2;
	}

	// Token: 0x04004427 RID: 17447
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_markerCompleteBatches = new ProfilerMarker("MeshDataManager.CompleteBatches");

	// Token: 0x04004428 RID: 17448
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_markerAdd = new ProfilerMarker("MeshDataManager.Add");

	// Token: 0x04004429 RID: 17449
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly ProfilerMarker s_markerStartBatches = new ProfilerMarker("MeshDataManager.StartBatches");

	// Token: 0x0400442A RID: 17450
	public static readonly MeshDataManager Instance = new MeshDataManager();

	// Token: 0x0400442B RID: 17451
	public static bool Enabled;

	// Token: 0x0400442C RID: 17452
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<MeshDataManager.JobState> m_toStart = new List<MeshDataManager.JobState>();

	// Token: 0x0400442D RID: 17453
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<MeshDataManager.JobBatch> m_running = new LinkedList<MeshDataManager.JobBatch>();

	// Token: 0x02000B5B RID: 2907
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class JobBatch : IDisposable
	{
		// Token: 0x06005831 RID: 22577 RVA: 0x0021AF30 File Offset: 0x00219130
		public JobBatch(IEnumerable<MeshDataManager.JobState> stateSupplier)
		{
			this.m_states = stateSupplier.ToArray<MeshDataManager.JobState>();
			if (this.m_states.Length != 0)
			{
				this.m_meshDataArray = Mesh.AllocateWritableMeshData(this.m_states.Length);
				this.m_sameFrameUpload = this.m_states.Any<MeshDataManager.JobState>((MeshDataManager.JobState state) => state.SameFrameUpload);
				return;
			}
			this.m_disposed = true;
			this.m_disposedMeshData = true;
		}

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x06005832 RID: 22578 RVA: 0x0021AFAA File Offset: 0x002191AA
		public IReadOnlyList<MeshDataManager.JobState> States
		{
			get
			{
				return this.m_states;
			}
		}

		// Token: 0x06005833 RID: 22579 RVA: 0x0021AFB4 File Offset: 0x002191B4
		public void Start()
		{
			Task[] array = new Task[this.m_states.Length];
			for (int i = 0; i < this.m_states.Length; i++)
			{
				array[i] = this.m_states[i].Start(this.m_meshDataArray[i]);
			}
			this.m_statesAllComplete = Task.WhenAll(array);
		}

		// Token: 0x06005834 RID: 22580 RVA: 0x0021B00C File Offset: 0x0021920C
		public bool TryApplyAndDispose()
		{
			if (this.m_disposedMeshData || this.m_disposed)
			{
				return true;
			}
			if (!this.m_sameFrameUpload && !this.m_statesAllComplete.IsCompleted)
			{
				return false;
			}
			try
			{
				this.m_statesAllComplete.Wait();
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
			Mesh[] array = this.m_states.Select<MeshDataManager.JobState, Mesh>((MeshDataManager.JobState s) => s.Mesh).ToArray<Mesh>();
			this.m_disposedMeshData = true;
			Mesh.ApplyAndDisposeWritableMeshData(this.m_meshDataArray, array, MeshUpdateFlags.DontRecalculateBounds);
			this.m_meshDataArray = default(Mesh.MeshDataArray);
			foreach (MeshDataManager.JobState jobState in this.m_states)
			{
				try
				{
					jobState.PostApply();
				}
				catch (Exception ex2)
				{
					Log.Exception(ex2);
				}
			}
			this.Dispose();
			return true;
		}

		// Token: 0x06005835 RID: 22581 RVA: 0x0021B0F4 File Offset: 0x002192F4
		public void Dispose()
		{
			if (this.m_disposed)
			{
				return;
			}
			this.m_disposed = true;
			foreach (MeshDataManager.JobState jobState in this.m_states)
			{
				try
				{
					if (jobState != null)
					{
						jobState.Dispose();
					}
				}
				catch (Exception ex)
				{
					Log.Exception(ex);
				}
			}
			if (!this.m_disposedMeshData)
			{
				this.m_meshDataArray.Dispose();
				this.m_meshDataArray = default(Mesh.MeshDataArray);
				this.m_disposedMeshData = true;
			}
		}

		// Token: 0x0400442E RID: 17454
		[PublicizedFrom(EAccessModifier.Private)]
		public Mesh.MeshDataArray m_meshDataArray;

		// Token: 0x0400442F RID: 17455
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly MeshDataManager.JobState[] m_states;

		// Token: 0x04004430 RID: 17456
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool m_sameFrameUpload;

		// Token: 0x04004431 RID: 17457
		[PublicizedFrom(EAccessModifier.Private)]
		public Task m_statesAllComplete;

		// Token: 0x04004432 RID: 17458
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposed;

		// Token: 0x04004433 RID: 17459
		[PublicizedFrom(EAccessModifier.Private)]
		public bool m_disposedMeshData;
	}

	// Token: 0x02000B5D RID: 2909
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class JobState : IDisposable
	{
		// Token: 0x0600583A RID: 22586 RVA: 0x0021B190 File Offset: 0x00219390
		public JobState(Mesh mesh, PinnedBuffer<Vector3> vertices, PinnedBuffer<int> indices, PinnedBuffer<Vector3> normals, PinnedBuffer<Vector4> tangents, PinnedBuffer<Color> colors, PinnedBuffer<Vector2> texCoord0s, PinnedBuffer<Vector2> texCoord1s, bool generateNormals, bool generateTangents, bool recalculateUvDistributionMetrics, bool sameFrameUpload, Action onJobComplete)
		{
			this.Mesh = mesh;
			this.Vertices = vertices;
			this.Indices = indices;
			this.Normals = normals;
			this.Tangents = tangents;
			this.Colors = colors;
			this.TexCoord0s = texCoord0s;
			this.TexCoord1s = texCoord1s;
			this.GenerateNormals = generateNormals;
			this.GenerateTangents = generateTangents;
			this.RecalculateUvDistributionMetrics = recalculateUvDistributionMetrics;
			this.SameFrameUpload = sameFrameUpload;
			this.OnJobComplete = onJobComplete;
		}

		// Token: 0x0600583B RID: 22587 RVA: 0x0021B208 File Offset: 0x00219408
		public void Dispose()
		{
			CancellationTokenSource copyTaskCancellation = this.CopyTaskCancellation;
			if (copyTaskCancellation != null)
			{
				copyTaskCancellation.Cancel();
			}
			Task copyTask = this.CopyTask;
			if (copyTask != null)
			{
				copyTask.Wait();
			}
			Task copyTask2 = this.CopyTask;
			if (copyTask2 != null)
			{
				copyTask2.Dispose();
			}
			this.CopyTask = null;
			CancellationTokenSource copyTaskCancellation2 = this.CopyTaskCancellation;
			if (copyTaskCancellation2 != null)
			{
				copyTaskCancellation2.Dispose();
			}
			this.CopyTaskCancellation = null;
			PinnedBuffer<VertexAttributeDescriptor> vertexAttributes = this.VertexAttributes;
			if (vertexAttributes != null)
			{
				vertexAttributes.Dispose();
			}
			this.VertexAttributes = null;
			PinnedBuffer<Vector3> vertices = this.Vertices;
			if (vertices != null)
			{
				vertices.Dispose();
			}
			this.Vertices = null;
			PinnedBuffer<int> indices = this.Indices;
			if (indices != null)
			{
				indices.Dispose();
			}
			this.Indices = null;
			PinnedBuffer<Vector2> texCoord0s = this.TexCoord0s;
			if (texCoord0s != null)
			{
				texCoord0s.Dispose();
			}
			this.TexCoord0s = null;
			PinnedBuffer<Vector2> texCoord1s = this.TexCoord1s;
			if (texCoord1s != null)
			{
				texCoord1s.Dispose();
			}
			this.TexCoord1s = null;
			PinnedBuffer<Vector3> normals = this.Normals;
			if (normals != null)
			{
				normals.Dispose();
			}
			this.Normals = null;
			PinnedBuffer<Vector4> tangents = this.Tangents;
			if (tangents != null)
			{
				tangents.Dispose();
			}
			this.Tangents = null;
			PinnedBuffer<Color> colors = this.Colors;
			if (colors != null)
			{
				colors.Dispose();
			}
			this.Colors = null;
			try
			{
				Action onJobComplete = this.OnJobComplete;
				if (onJobComplete != null)
				{
					onJobComplete();
				}
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
			}
			finally
			{
				this.OnJobComplete = null;
			}
		}

		// Token: 0x0600583C RID: 22588 RVA: 0x0021B368 File Offset: 0x00219568
		public Task Start(Mesh.MeshData meshData)
		{
			this.CopyTaskCancellation = new CancellationTokenSource();
			return this.CopyTask = Task.Run(() => this.CopyToMeshData(meshData, this.CopyTaskCancellation.Token), this.CopyTaskCancellation.Token);
		}

		// Token: 0x0600583D RID: 22589 RVA: 0x0021B3BC File Offset: 0x002195BC
		[PublicizedFrom(EAccessModifier.Private)]
		public async Task CopyToMeshData(Mesh.MeshData meshData, CancellationToken cancellationToken)
		{
			MeshDataManager.JobState.<>c__DisplayClass20_0 CS$<>8__locals1 = new MeshDataManager.JobState.<>c__DisplayClass20_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.meshData = meshData;
			using (NativeList<VertexAttributeDescriptor> nativeList = new NativeList<VertexAttributeDescriptor>(Allocator.Persistent))
			{
				CS$<>8__locals1.layout = MeshDataUtils.SetAttributes(CS$<>8__locals1.meshData, this.Vertices, this.Indices, this.Normals, this.Tangents, this.Colors, this.TexCoord0s, this.TexCoord1s, nativeList);
				this.VertexAttributes = new PinnedBuffer<VertexAttributeDescriptor>(nativeList.Length);
				nativeList.AsArray().AsReadOnlySpan().CopyTo(this.VertexAttributes);
			}
			List<Task> list = new List<Task>(3);
			CS$<>8__locals1.recalculateBoundsTask = Task.Run(new Action(CS$<>8__locals1.<CopyToMeshData>g__RecalculateBounds|0), cancellationToken);
			if (this.GenerateNormals)
			{
				CS$<>8__locals1.generateNormalsTask = Task.Run(new Action(CS$<>8__locals1.<CopyToMeshData>g__GenerateNormals|1), cancellationToken);
			}
			else
			{
				CS$<>8__locals1.generateNormalsTask = Task.CompletedTask;
			}
			if (this.GenerateTangents)
			{
				CS$<>8__locals1.generateTangentsTask = Task.Run(new Func<Task>(CS$<>8__locals1.<CopyToMeshData>g__GenerateTangents|2), cancellationToken);
			}
			else
			{
				CS$<>8__locals1.generateTangentsTask = Task.CompletedTask;
			}
			CS$<>8__locals1.copyIndicesTask = Task.Run(new Action(CS$<>8__locals1.<CopyToMeshData>g__CopyIndices|3), cancellationToken);
			Task task = CS$<>8__locals1.<CopyToMeshData>g__SetSubmesh|4();
			list.Add(task);
			CS$<>8__locals1.vertexCopyTasks = new List<Task>(6);
			CS$<>8__locals1.vertexData = CS$<>8__locals1.meshData.GetVertexData<byte>(0);
			CS$<>8__locals1.vertexCopyTasks.Add(Task.Run(new Action(CS$<>8__locals1.<CopyToMeshData>g__CopyPosition|5), cancellationToken));
			if (this.Normals.Length > 0)
			{
				CS$<>8__locals1.vertexCopyTasks.Add(Task.Run(new Func<Task>(CS$<>8__locals1.<CopyToMeshData>g__CopyNormals|6), cancellationToken));
			}
			if (this.Tangents.Length > 0)
			{
				CS$<>8__locals1.vertexCopyTasks.Add(Task.Run(new Func<Task>(CS$<>8__locals1.<CopyToMeshData>g__CopyTangents|7), cancellationToken));
			}
			if (this.Colors.Length > 0)
			{
				CS$<>8__locals1.vertexCopyTasks.Add(Task.Run(new Action(CS$<>8__locals1.<CopyToMeshData>g__CopyColors|8), cancellationToken));
			}
			if (this.TexCoord0s.Length > 0)
			{
				CS$<>8__locals1.vertexCopyTasks.Add(Task.Run(new Action(CS$<>8__locals1.<CopyToMeshData>g__CopyTexCoord0s|9), cancellationToken));
			}
			if (this.TexCoord1s.Length > 0)
			{
				CS$<>8__locals1.vertexCopyTasks.Add(Task.Run(new Action(CS$<>8__locals1.<CopyToMeshData>g__CopyTexCoord1s|10), cancellationToken));
			}
			list.Add(Task.Run(new Func<Task>(CS$<>8__locals1.<CopyToMeshData>g__CompressMesh|11), cancellationToken));
			await Task.WhenAll(list);
		}

		// Token: 0x0600583E RID: 22590 RVA: 0x0021B40F File Offset: 0x0021960F
		public void PostApply()
		{
			this.Mesh.bounds = this.Bounds;
			if (this.RecalculateUvDistributionMetrics)
			{
				this.Mesh.RecalculateUVDistributionMetrics(1E-09f);
			}
		}

		// Token: 0x04004437 RID: 17463
		public readonly Mesh Mesh;

		// Token: 0x04004438 RID: 17464
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<Vector3> Vertices;

		// Token: 0x04004439 RID: 17465
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<int> Indices;

		// Token: 0x0400443A RID: 17466
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<Vector3> Normals;

		// Token: 0x0400443B RID: 17467
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<Vector4> Tangents;

		// Token: 0x0400443C RID: 17468
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<Color> Colors;

		// Token: 0x0400443D RID: 17469
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<Vector2> TexCoord0s;

		// Token: 0x0400443E RID: 17470
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<Vector2> TexCoord1s;

		// Token: 0x0400443F RID: 17471
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool GenerateNormals;

		// Token: 0x04004440 RID: 17472
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool GenerateTangents;

		// Token: 0x04004441 RID: 17473
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly bool RecalculateUvDistributionMetrics;

		// Token: 0x04004442 RID: 17474
		public readonly bool SameFrameUpload;

		// Token: 0x04004443 RID: 17475
		[PublicizedFrom(EAccessModifier.Private)]
		public Action OnJobComplete;

		// Token: 0x04004444 RID: 17476
		[PublicizedFrom(EAccessModifier.Private)]
		public CancellationTokenSource CopyTaskCancellation;

		// Token: 0x04004445 RID: 17477
		[PublicizedFrom(EAccessModifier.Private)]
		public Task CopyTask;

		// Token: 0x04004446 RID: 17478
		[PublicizedFrom(EAccessModifier.Private)]
		public PinnedBuffer<VertexAttributeDescriptor> VertexAttributes;

		// Token: 0x04004447 RID: 17479
		[PublicizedFrom(EAccessModifier.Private)]
		public Bounds Bounds;
	}
}
