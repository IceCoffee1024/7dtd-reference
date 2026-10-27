using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000966 RID: 2406
public static class SimpleMeshFile
{
	// Token: 0x060045F9 RID: 17913 RVA: 0x001BDB48 File Offset: 0x001BBD48
	public static void WriteGameObject(PooledBinaryWriter _bw, GameObject _go, List<ImposterCanvas> _signs = null)
	{
		try
		{
			_bw.Write(1835365224);
			_bw.Write(7);
			MeshFilter[] componentsInChildren = _go.GetComponentsInChildren<MeshFilter>();
			_bw.Write((short)componentsInChildren.Length);
			int num = 0;
			foreach (MeshFilter meshFilter in componentsInChildren)
			{
				_bw.Write(meshFilter.transform.name);
				SimpleMeshFile.writeMesh(_bw, meshFilter.mesh, MeshDescription.meshes[0].textureAtlas.uvMapping);
				num += meshFilter.mesh.vertexCount;
			}
			Log.Out("Saved. Meshes: " + componentsInChildren.Length.ToString() + " Vertices: " + num.ToString());
			_bw.Write((_signs == null) ? 0 : ((ushort)_signs.Count));
			if (_signs != null)
			{
				foreach (ImposterCanvas imposterCanvas in _signs)
				{
					imposterCanvas.Write(_bw);
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045FA RID: 17914 RVA: 0x001BDC5C File Offset: 0x001BBE5C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void writeMesh(PooledBinaryWriter _bw, Mesh _mesh, UVRectTiling[] _uvMapping)
	{
		try
		{
			Vector3[] vertices = _mesh.vertices;
			_bw.Write((uint)vertices.Length);
			for (int i = 0; i < vertices.Length; i++)
			{
				_bw.Write(vertices[i].x);
				_bw.Write(vertices[i].y);
				_bw.Write(vertices[i].z);
			}
			int[] indices = _mesh.GetIndices(0);
			Vector2[] uv = _mesh.uv;
			Vector2[] uv2 = _mesh.uv2;
			_bw.Write((uint)uv.Length);
			for (int j = 0; j < uv.Length; j++)
			{
				int num = (int)uv2[j].x;
				int num2 = -1;
				for (int k = 0; k < _uvMapping.Length; k++)
				{
					if (_uvMapping[k].index == num || k + 1 >= _uvMapping.Length || (float)_uvMapping[k].index + _uvMapping[k].uv.width * _uvMapping[k].uv.height > (float)num)
					{
						num2 = k;
						break;
					}
				}
				if (num2 == -1)
				{
					num2 = 0;
				}
				_bw.Write((short)num2);
				_bw.Write((byte)(num - _uvMapping[num2].index));
				bool flag = (double)uv2[j].y > 0.5;
				_bw.Write(flag);
				_bw.Write((ushort)(uv[j].x * 10000f));
				_bw.Write((ushort)(uv[j].y * 10000f));
			}
			_bw.Write((uint)indices.Length);
			for (int l = 0; l < indices.Length; l++)
			{
				_bw.Write((ushort)indices[l]);
			}
		}
		finally
		{
		}
	}

	// Token: 0x060045FB RID: 17915 RVA: 0x001BDE40 File Offset: 0x001BC040
	public static GameObject ReadGameObject(PathAbstractions.AbstractedLocation _meshLocation, float _offsetY = 0f, Material _mat = null, bool _bTextureArray = true, bool _markMeshesNoLongerReadable = false, object _userCallbackData = null, SimpleMeshFile.GameObjectLoadedCallback _asyncCallback = null)
	{
		return SimpleMeshFile.ReadGameObject(_meshLocation.FullPath, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable, _userCallbackData, _asyncCallback);
	}

	// Token: 0x060045FC RID: 17916 RVA: 0x001BDE58 File Offset: 0x001BC058
	public static GameObject ReadGameObject(string _filename, float _offsetY = 0f, Material _mat = null, bool _bTextureArray = true, bool _markMeshesNoLongerReadable = false, object _userCallbackData = null, SimpleMeshFile.GameObjectLoadedCallback _asyncCallback = null)
	{
		SimpleMeshInfo simpleMeshInfo;
		return SimpleMeshFile.ReadGameObject(_filename, out simpleMeshInfo, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable, _userCallbackData, _asyncCallback);
	}

	// Token: 0x060045FD RID: 17917 RVA: 0x001BDE78 File Offset: 0x001BC078
	public static GameObject ReadGameObject(string _filename, out SimpleMeshInfo _meshInfo, float _offsetY = 0f, Material _mat = null, bool _bTextureArray = true, bool _markMeshesNoLongerReadable = false, object _userCallbackData = null, SimpleMeshFile.GameObjectLoadedCallback _asyncCallback = null)
	{
		_meshInfo = null;
		try
		{
			if (_asyncCallback == null)
			{
				_meshInfo = SimpleMeshFile.ReadGameObjectSync(_filename, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable);
				if (_meshInfo != null)
				{
					return SimpleMeshFile.CreateUnityObjects(_meshInfo);
				}
				return null;
			}
			else
			{
				Task.Run(() => SimpleMeshFile.ReadGameObjectAsync(_filename, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable, _userCallbackData, _asyncCallback));
			}
		}
		catch (Exception ex)
		{
			Log.Error("Reading mesh " + _filename + " failed:");
			Log.Exception(ex);
		}
		finally
		{
		}
		return null;
	}

	// Token: 0x060045FE RID: 17918 RVA: 0x001BDF58 File Offset: 0x001BC158
	[PublicizedFrom(EAccessModifier.Private)]
	public static SimpleMeshInfo ReadGameObjectSync(string _filename, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable)
	{
		try
		{
			using (Stream stream = SdFile.OpenRead(_filename))
			{
				return SimpleMeshFile.ReadGameObjectSync(stream, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable);
			}
		}
		catch (Exception ex)
		{
			Log.Error("Reading mesh " + _filename + " failed:");
			Log.Exception(ex);
		}
		return null;
	}

	// Token: 0x060045FF RID: 17919 RVA: 0x001BDFC0 File Offset: 0x001BC1C0
	[PublicizedFrom(EAccessModifier.Private)]
	public static async Task ReadGameObjectAsync(string _filename, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable, object _userCallbackData, SimpleMeshFile.GameObjectLoadedCallback _asyncCallback)
	{
		try
		{
			Stream fileStream = SdFile.OpenRead(_filename);
			object obj = null;
			try
			{
				await SimpleMeshFile.ReadGameObjectAsync(fileStream, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable, _userCallbackData, _asyncCallback);
			}
			catch (object obj)
			{
			}
			if (fileStream != null)
			{
				await ((IAsyncDisposable)fileStream).DisposeAsync();
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			obj = null;
			fileStream = null;
		}
		catch (Exception ex2)
		{
			Log.Error("Reading mesh " + _filename + " failed:");
			Log.Exception(ex2);
			ThreadManager.AddSingleTaskMainThread("SimpleMeshFile.ReadGameObjectAsyncCallback", delegate(object data)
			{
				_asyncCallback(null, data);
			}, _userCallbackData);
		}
	}

	// Token: 0x06004600 RID: 17920 RVA: 0x001BE038 File Offset: 0x001BC238
	[PublicizedFrom(EAccessModifier.Private)]
	public static SimpleMeshInfo ReadGameObjectSync(Stream _inputStream, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable)
	{
		SimpleMeshInfo simpleMeshInfo;
		using (SimpleMeshFile.SimpleMeshDataArray simpleMeshDataArray = new SimpleMeshFile.SimpleMeshDataArray(Mesh.AllocateWritableMeshData(SimpleMeshFile.readLengthFromHeaderAndReset(_inputStream))))
		{
			List<ImposterCanvas> list;
			SimpleMeshFile.readData(simpleMeshDataArray, _inputStream, _bTextureArray, out list);
			simpleMeshInfo = SimpleMeshFile.createMeshInfo(simpleMeshDataArray, _markMeshesNoLongerReadable, _offsetY, _mat, list);
		}
		return simpleMeshInfo;
	}

	// Token: 0x06004601 RID: 17921 RVA: 0x001BE08C File Offset: 0x001BC28C
	[PublicizedFrom(EAccessModifier.Private)]
	public static async Task ReadGameObjectAsync(Stream _inputStream, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable, object _userCallbackData, SimpleMeshFile.GameObjectLoadedCallback _asyncCallback)
	{
		int meshCnt = SimpleMeshFile.readLengthFromHeaderAndReset(_inputStream);
		Mesh.MeshDataArray meshDataArray = await ThreadManager.MainThreadFactory.StartNew<Mesh.MeshDataArray>(() => Mesh.AllocateWritableMeshData(meshCnt));
		using (SimpleMeshFile.SimpleMeshDataArray meshDatas = new SimpleMeshFile.SimpleMeshDataArray(meshDataArray))
		{
			List<ImposterCanvas> signs;
			SimpleMeshFile.readData(meshDatas, _inputStream, _bTextureArray, out signs);
			GameObject result = await ThreadManager.MainThreadFactory.StartNew<GameObject>(() => SimpleMeshFile.CreateUnityObjects(SimpleMeshFile.createMeshInfo(meshDatas, _markMeshesNoLongerReadable, _offsetY, _mat, signs)));
			ThreadManager.AddSingleTaskMainThread("SimpleMeshFile.ReadGameObjectAsyncCallback", delegate(object data)
			{
				_asyncCallback(result, data);
			}, _userCallbackData);
		}
	}

	// Token: 0x06004602 RID: 17922 RVA: 0x001BE104 File Offset: 0x001BC304
	[PublicizedFrom(EAccessModifier.Private)]
	public static int readLengthFromHeaderAndReset(Stream _inputStream)
	{
		long position = _inputStream.Position;
		int num2;
		using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
		{
			pooledBinaryReader.SetBaseStream(_inputStream);
			pooledBinaryReader.ReadInt32();
			pooledBinaryReader.ReadByte();
			int num = (int)pooledBinaryReader.ReadInt16();
			_inputStream.Seek(position, SeekOrigin.Begin);
			num2 = num;
		}
		return num2;
	}

	// Token: 0x06004603 RID: 17923 RVA: 0x001BE168 File Offset: 0x001BC368
	[PublicizedFrom(EAccessModifier.Private)]
	public static void readData(SimpleMeshFile.SimpleMeshDataArray _meshDatas, Stream _inputStream, bool _bTextureArray, out List<ImposterCanvas> _signs)
	{
		_signs = null;
		try
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(true))
			{
				pooledBinaryReader.SetBaseStream(_inputStream);
				pooledBinaryReader.ReadInt32();
				int num = (int)pooledBinaryReader.ReadByte();
				int num2 = (int)pooledBinaryReader.ReadInt16();
				UVRectTiling[] array = ((MeshDescription.meshes.Length != 0) ? MeshDescription.meshes[0].textureAtlas.uvMapping : new UVRectTiling[0]);
				for (int i = 0; i < num2; i++)
				{
					SimpleMeshFile.SimpleMeshDataArray.ReadFromReader(_meshDatas[i], num, pooledBinaryReader, array, _bTextureArray);
				}
				if (num >= 7)
				{
					int num3 = (int)pooledBinaryReader.ReadUInt16();
					if (num3 > 0)
					{
						_signs = new List<ImposterCanvas>(num3);
						for (int j = 0; j < num3; j++)
						{
							_signs.Add(ImposterCanvas.Read(pooledBinaryReader));
						}
					}
				}
			}
		}
		finally
		{
		}
	}

	// Token: 0x06004604 RID: 17924 RVA: 0x001BE244 File Offset: 0x001BC444
	public static GameObject CreateUnityObjects(SimpleMeshInfo _meshInfo)
	{
		GameObject gameObject3;
		try
		{
			Mesh[] meshes = _meshInfo.meshes;
			string[] meshNames = _meshInfo.meshNames;
			float offsetY = _meshInfo.offsetY;
			Material material = _meshInfo.mat;
			if (!material)
			{
				material = MeshDescription.GetOpaqueMaterial();
			}
			GameObject gameObject = new GameObject();
			Transform transform = gameObject.transform;
			Vector3 vector = new Vector3(0f, offsetY, 0f);
			for (int i = 0; i < meshes.Length; i++)
			{
				GameObject gameObject2 = new GameObject(meshNames[i]);
				gameObject2.AddComponent<MeshFilter>().mesh = meshes[i];
				gameObject2.AddComponent<MeshRenderer>().material = material;
				Transform transform2 = gameObject2.transform;
				transform2.SetParent(transform, false);
				transform2.localPosition = vector;
			}
			gameObject3 = gameObject;
		}
		finally
		{
		}
		return gameObject3;
	}

	// Token: 0x06004605 RID: 17925 RVA: 0x001BE304 File Offset: 0x001BC504
	public static Mesh[] ReadMesh(string _filename, float _offsetY = 0f, Material _mat = null, bool _bTextureArray = true, bool _markMeshesNoLongerReadable = false, object _userCallbackData = null, SimpleMeshFile.GameObjectMeshesReadCallback _asyncCallback = null)
	{
		try
		{
			if (_asyncCallback == null)
			{
				return SimpleMeshFile.ReadMeshSync(_filename, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable);
			}
			Task.Run(() => SimpleMeshFile.ReadMeshAsync(_filename, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable, _userCallbackData, _asyncCallback));
		}
		catch (Exception ex)
		{
			Log.Error("Reading mesh " + _filename + " failed:");
			Log.Exception(ex);
		}
		finally
		{
		}
		return null;
	}

	// Token: 0x06004606 RID: 17926 RVA: 0x001BE3D0 File Offset: 0x001BC5D0
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mesh[] ReadMeshSync(string _filename, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable)
	{
		Mesh[] array;
		using (Stream stream = SdFile.OpenRead(_filename))
		{
			array = SimpleMeshFile.ReadMeshSync(stream, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable);
		}
		return array;
	}

	// Token: 0x06004607 RID: 17927 RVA: 0x001BE410 File Offset: 0x001BC610
	[PublicizedFrom(EAccessModifier.Private)]
	public static async Task ReadMeshAsync(string _filename, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable, object _userCallbackData, SimpleMeshFile.GameObjectMeshesReadCallback _asyncCallback)
	{
		try
		{
			await SimpleMeshFile.ReadMeshAsyncInternal(_filename, _offsetY, _mat, _bTextureArray, _markMeshesNoLongerReadable, _userCallbackData, _asyncCallback);
		}
		catch (Exception ex)
		{
			Log.Error("Reading mesh " + _filename + " failed:");
			Log.Exception(ex);
			ThreadManager.AddSingleTaskMainThread("SimpleMeshFile.ReadMeshAsyncCallback", delegate(object data)
			{
				_asyncCallback(null, data);
			}, _userCallbackData);
		}
	}

	// Token: 0x06004608 RID: 17928 RVA: 0x001BE488 File Offset: 0x001BC688
	[PublicizedFrom(EAccessModifier.Private)]
	public static Mesh[] ReadMeshSync(Stream _inputStream, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable)
	{
		Mesh[] meshes;
		using (SimpleMeshFile.SimpleMeshDataArray simpleMeshDataArray = new SimpleMeshFile.SimpleMeshDataArray(Mesh.AllocateWritableMeshData(SimpleMeshFile.readLengthFromHeaderAndReset(_inputStream))))
		{
			List<ImposterCanvas> list;
			SimpleMeshFile.readData(simpleMeshDataArray, _inputStream, _bTextureArray, out list);
			meshes = SimpleMeshFile.createMeshInfo(simpleMeshDataArray, _markMeshesNoLongerReadable, _offsetY, _mat, list).meshes;
		}
		return meshes;
	}

	// Token: 0x06004609 RID: 17929 RVA: 0x001BE4E0 File Offset: 0x001BC6E0
	[PublicizedFrom(EAccessModifier.Private)]
	public static async Task ReadMeshAsyncInternal(string _filename, float _offsetY, Material _mat, bool _bTextureArray, bool _markMeshesNoLongerReadable, object _userCallbackData, SimpleMeshFile.GameObjectMeshesReadCallback _asyncCallback)
	{
		Stream stream = SdFile.OpenRead(_filename);
		int meshCnt = SimpleMeshFile.readLengthFromHeaderAndReset(stream);
		stream.Dispose();
		Mesh.MeshDataArray meshDataArray = await ThreadManager.MainThreadFactory.StartNew<Mesh.MeshDataArray>(() => Mesh.AllocateWritableMeshData(meshCnt));
		using (SimpleMeshFile.SimpleMeshDataArray meshDatas = new SimpleMeshFile.SimpleMeshDataArray(meshDataArray))
		{
			stream = SdFile.OpenRead(_filename);
			List<ImposterCanvas> signs;
			SimpleMeshFile.readData(meshDatas, stream, _bTextureArray, out signs);
			stream.Dispose();
			SimpleMeshInfo meshInfo = await ThreadManager.MainThreadFactory.StartNew<SimpleMeshInfo>(() => SimpleMeshFile.createMeshInfo(meshDatas, _markMeshesNoLongerReadable, _offsetY, _mat, signs));
			ThreadManager.AddSingleTaskMainThread("SimpleMeshFile.ReadMeshAsyncCallback", delegate(object data)
			{
				_asyncCallback(meshInfo, data);
			}, _userCallbackData);
		}
	}

	// Token: 0x0600460A RID: 17930 RVA: 0x001BE558 File Offset: 0x001BC758
	[PublicizedFrom(EAccessModifier.Private)]
	public static SimpleMeshInfo createMeshInfo(SimpleMeshFile.SimpleMeshDataArray _meshDatas, bool _markMeshesNoLongerReadable, float _offsetY, Material _mat, List<ImposterCanvas> _signs)
	{
		string[] array = new string[_meshDatas.Length];
		Mesh[] array2 = SimpleMeshFile.SimpleMeshDataArray.ToMeshes(_meshDatas, _markMeshesNoLongerReadable);
		for (int i = 0; i < _meshDatas.Length; i++)
		{
			array[i] = _meshDatas[i].Name;
		}
		return new SimpleMeshInfo(array, array2, _offsetY, _mat, _signs);
	}

	// Token: 0x0400387A RID: 14458
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cSaveFileVersion = 7;

	// Token: 0x0400387B RID: 14459
	[PublicizedFrom(EAccessModifier.Private)]
	public const int cHeader = 1835365224;

	// Token: 0x02000967 RID: 2407
	// (Invoke) Token: 0x0600460C RID: 17932
	public delegate void GameObjectLoadedCallback(GameObject _go, object _userCallbackData);

	// Token: 0x02000968 RID: 2408
	// (Invoke) Token: 0x06004610 RID: 17936
	public delegate void GameObjectMeshesReadCallback(SimpleMeshInfo _meshInfo, object _userCallbackData);

	// Token: 0x02000969 RID: 2409
	[PublicizedFrom(EAccessModifier.Private)]
	public sealed class SimpleMeshDataArray : IDisposable
	{
		// Token: 0x06004613 RID: 17939 RVA: 0x001BE5A8 File Offset: 0x001BC7A8
		public SimpleMeshDataArray(Mesh.MeshDataArray _array)
		{
			this.meshData = _array;
			this.names = new string[this.meshData.Length];
			this.wrappers = new SimpleMeshFile.SimpleMeshDataArray.SimpleMeshDataWrapper[this.meshData.Length];
			for (int i = 0; i < this.wrappers.Length; i++)
			{
				this.wrappers[i] = new SimpleMeshFile.SimpleMeshDataArray.SimpleMeshDataWrapper(this, i);
			}
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x001BE614 File Offset: 0x001BC814
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			this.DisposeMeshData();
			this.names = null;
			this.wrappers = null;
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x001BE63A File Offset: 0x001BC83A
		[PublicizedFrom(EAccessModifier.Private)]
		public void DisposeMeshData()
		{
			if (this.meshDataDisposed)
			{
				return;
			}
			this.meshDataDisposed = true;
			this.meshData.Dispose();
			this.meshData = default(Mesh.MeshDataArray);
		}

		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06004616 RID: 17942 RVA: 0x001BE663 File Offset: 0x001BC863
		public int Length
		{
			get
			{
				return this.meshData.Length;
			}
		}

		// Token: 0x1700078E RID: 1934
		public SimpleMeshFile.SimpleMeshDataArray.SimpleMeshDataWrapper this[int i]
		{
			get
			{
				return this.wrappers[i];
			}
		}

		// Token: 0x06004618 RID: 17944 RVA: 0x001BE67E File Offset: 0x001BC87E
		public void ApplyAndDisposeWritableMeshData(Mesh[] meshes, MeshUpdateFlags flags = MeshUpdateFlags.Default)
		{
			Mesh.ApplyAndDisposeWritableMeshData(this.meshData, meshes, flags);
			this.meshDataDisposed = true;
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x001BE694 File Offset: 0x001BC894
		public static void ReadFromReader(SimpleMeshFile.SimpleMeshDataArray.SimpleMeshDataWrapper _meshDataWrapper, int _version, BinaryReader _br, UVRectTiling[] _uvMapping, bool _bTextureArray)
		{
			try
			{
				Mesh.MeshData meshData = _meshDataWrapper.MeshData;
				_meshDataWrapper.Name = ((_version > 1) ? _br.ReadString() : "mesh");
				long position = _br.BaseStream.Position;
				int num = (int)_br.ReadUInt32();
				int num2;
				if (_version < 6)
				{
					num2 = 6;
				}
				else
				{
					num2 = 12;
				}
				_br.BaseStream.Seek((long)(num * num2), SeekOrigin.Current);
				int num3 = (int)_br.ReadUInt32();
				int num4 = 0;
				if (_version > 2)
				{
					num4 += 2;
				}
				if (_version > 4)
				{
					num4++;
				}
				if (_version > 3)
				{
					num4++;
				}
				num4 += 4;
				_br.BaseStream.Seek((long)(num3 * num4), SeekOrigin.Current);
				int num5 = (int)_br.ReadUInt32();
				int num6 = 2;
				_br.BaseStream.Seek((long)(num5 * num6), SeekOrigin.Current);
				_br.BaseStream.Seek(position, SeekOrigin.Begin);
				VertexAttributeDescriptor[] array = new VertexAttributeDescriptor[]
				{
					new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3, 0),
					new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3, 1),
					new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2, 2),
					new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4, 3)
				};
				meshData.SetVertexBufferParams(num, array);
				meshData.SetIndexBufferParams(num5, IndexFormat.UInt16);
				num = (int)_br.ReadUInt32();
				NativeArray<Vector3> vertexData = meshData.GetVertexData<Vector3>(0);
				for (int i = 0; i < num; i++)
				{
					if (_version < 6)
					{
						vertexData[i] = new Vector3((float)_br.ReadInt16() / 100f, (float)_br.ReadInt16() / 100f, (float)_br.ReadInt16() / 100f);
					}
					else
					{
						vertexData[i] = new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
					}
				}
				num3 = (int)_br.ReadUInt32();
				NativeArray<Vector2> vertexData2 = meshData.GetVertexData<Vector2>(2);
				NativeArray<Color> vertexData3 = meshData.GetVertexData<Color>(3);
				for (int j = 0; j < num3; j++)
				{
					int num7 = 0;
					int num8 = 0;
					if (_version > 2)
					{
						num7 = (int)_br.ReadInt16();
					}
					if (_version > 4)
					{
						num8 = (int)_br.ReadByte();
					}
					int num9 = ((num7 >= 0 && num7 < _uvMapping.Length) ? (_uvMapping[num7].index + num8) : 0);
					bool flag = false;
					if (_version > 3)
					{
						flag = _br.ReadBoolean();
					}
					vertexData2[j] = new Vector2((float)_br.ReadUInt16() / 10000f, (float)_br.ReadUInt16() / 10000f);
					if (!_bTextureArray && num9 >= 0 && num9 < _uvMapping.Length)
					{
						ref NativeArray<Vector2> ptr = ref vertexData2;
						int num10 = j;
						ptr[num10] += new Vector2(_uvMapping[num9].uv.x, _uvMapping[num9].uv.y);
					}
					vertexData3[j] = new Color(0f, (float)num9, 0f, (float)(flag ? 1 : 0));
				}
				num5 = (int)_br.ReadUInt32();
				NativeArray<ushort> indexData = meshData.GetIndexData<ushort>();
				for (int k = 0; k < num5; k++)
				{
					indexData[k] = _br.ReadUInt16();
				}
				meshData.subMeshCount = 1;
				meshData.SetSubMesh(0, new SubMeshDescriptor(0, num5, MeshTopology.Triangles), MeshUpdateFlags.Default);
			}
			finally
			{
			}
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x001BE9C0 File Offset: 0x001BCBC0
		public static Mesh[] ToMeshes(SimpleMeshFile.SimpleMeshDataArray _meshDataArray, bool _markMeshesNoLongerReadable)
		{
			Mesh[] array = new Mesh[_meshDataArray.Length];
			for (int i = 0; i < array.Length; i++)
			{
				Mesh mesh = new Mesh();
				array[i] = mesh;
				mesh.name = "Simple";
			}
			_meshDataArray.ApplyAndDisposeWritableMeshData(array, MeshUpdateFlags.Default);
			foreach (Mesh mesh2 in array)
			{
				mesh2.RecalculateNormals();
				mesh2.RecalculateBounds();
				GameUtils.SetMeshVertexAttributes(mesh2, true);
				mesh2.UploadMeshData(_markMeshesNoLongerReadable);
			}
			return array;
		}

		// Token: 0x0400387C RID: 14460
		[PublicizedFrom(EAccessModifier.Private)]
		public bool disposed;

		// Token: 0x0400387D RID: 14461
		[PublicizedFrom(EAccessModifier.Private)]
		public Mesh.MeshDataArray meshData;

		// Token: 0x0400387E RID: 14462
		[PublicizedFrom(EAccessModifier.Private)]
		public bool meshDataDisposed;

		// Token: 0x0400387F RID: 14463
		[PublicizedFrom(EAccessModifier.Private)]
		public string[] names;

		// Token: 0x04003880 RID: 14464
		[PublicizedFrom(EAccessModifier.Private)]
		public SimpleMeshFile.SimpleMeshDataArray.SimpleMeshDataWrapper[] wrappers;

		// Token: 0x0200096A RID: 2410
		public readonly struct SimpleMeshDataWrapper
		{
			// Token: 0x0600461B RID: 17947 RVA: 0x001BEA36 File Offset: 0x001BCC36
			public SimpleMeshDataWrapper(SimpleMeshFile.SimpleMeshDataArray _array, int _offset)
			{
				this.array = _array;
				this.offset = _offset;
			}

			// Token: 0x1700078F RID: 1935
			// (get) Token: 0x0600461C RID: 17948 RVA: 0x001BEA46 File Offset: 0x001BCC46
			public Mesh.MeshData MeshData
			{
				get
				{
					return this.array.meshData[this.offset];
				}
			}

			// Token: 0x17000790 RID: 1936
			// (get) Token: 0x0600461D RID: 17949 RVA: 0x001BEA5E File Offset: 0x001BCC5E
			// (set) Token: 0x0600461E RID: 17950 RVA: 0x001BEA72 File Offset: 0x001BCC72
			public string Name
			{
				get
				{
					return this.array.names[this.offset];
				}
				set
				{
					this.array.names[this.offset] = value;
				}
			}

			// Token: 0x04003881 RID: 14465
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly SimpleMeshFile.SimpleMeshDataArray array;

			// Token: 0x04003882 RID: 14466
			[PublicizedFrom(EAccessModifier.Private)]
			public readonly int offset;
		}
	}
}
