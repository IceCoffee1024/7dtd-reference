using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Scripting;

// Token: 0x02000BD6 RID: 3030
[Preserve]
public class TextureAtlasExternalModels : TextureAtlas
{
	// Token: 0x06005B0E RID: 23310 RVA: 0x00232F94 File Offset: 0x00231194
	public override bool LoadTextureAtlas(int _idx, MeshDescriptionCollection _tac, bool _bLoadTextures)
	{
		bool flag;
		try
		{
			Stream stream = new MemoryStream(_tac.meshes[_idx].MetaData.bytes);
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(stream);
				uint num = pooledBinaryReader.ReadUInt32();
				int num2 = 0;
				while ((long)num2 < (long)((ulong)num))
				{
					string text = pooledBinaryReader.ReadString();
					VoxelMeshExt3dModel voxelMeshExt3dModel = (VoxelMeshExt3dModel)VoxelMesh.Create(_idx, _tac.meshes[_idx].meshType, 500);
					voxelMeshExt3dModel.Read(pooledBinaryReader);
					this.Meshes[text] = voxelMeshExt3dModel;
					num2++;
				}
			}
			stream.Close();
			base.LoadTextureAtlas(_idx, _tac, _bLoadTextures);
			flag = true;
		}
		catch (Exception ex)
		{
			Log.Error("Loading model file. " + ex.Message);
			Log.Error(ex.StackTrace);
			flag = false;
		}
		return flag;
	}

	// Token: 0x040046EB RID: 18155
	public Dictionary<string, VoxelMeshExt3dModel> Meshes = new Dictionary<string, VoxelMeshExt3dModel>();
}
