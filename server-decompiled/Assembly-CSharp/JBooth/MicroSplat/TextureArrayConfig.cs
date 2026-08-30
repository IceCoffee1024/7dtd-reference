using System;
using System.Collections.Generic;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D3B RID: 7483
	[CreateAssetMenu(menuName = "MicroSplat/Texture Array Config", order = 1)]
	[ExecuteInEditMode]
	public class TextureArrayConfig : ScriptableObject
	{
		// Token: 0x0600DDA4 RID: 56740 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsScatter()
		{
			return false;
		}

		// Token: 0x0600DDA5 RID: 56741 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsDecal()
		{
			return false;
		}

		// Token: 0x0600DDA6 RID: 56742 RVA: 0x004F4700 File Offset: 0x004F2900
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			TextureArrayConfig.sAllConfigs.Add(this);
		}

		// Token: 0x0600DDA7 RID: 56743 RVA: 0x004F470D File Offset: 0x004F290D
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDestroy()
		{
			TextureArrayConfig.sAllConfigs.Remove(this);
		}

		// Token: 0x0600DDA8 RID: 56744 RVA: 0x004F471C File Offset: 0x004F291C
		public static TextureArrayConfig FindConfig(Texture2DArray diffuse)
		{
			for (int i = 0; i < TextureArrayConfig.sAllConfigs.Count; i++)
			{
				if (TextureArrayConfig.sAllConfigs[i].diffuseArray == diffuse)
				{
					return TextureArrayConfig.sAllConfigs[i];
				}
			}
			return null;
		}

		// Token: 0x0400A762 RID: 42850
		public bool diffuseIsLinear;

		// Token: 0x0400A763 RID: 42851
		[HideInInspector]
		public bool antiTileArray;

		// Token: 0x0400A764 RID: 42852
		[HideInInspector]
		public bool emisMetalArray;

		// Token: 0x0400A765 RID: 42853
		public bool traxArray;

		// Token: 0x0400A766 RID: 42854
		[HideInInspector]
		public TextureArrayConfig.TextureMode textureMode = TextureArrayConfig.TextureMode.PBR;

		// Token: 0x0400A767 RID: 42855
		[HideInInspector]
		public TextureArrayConfig.ClusterMode clusterMode;

		// Token: 0x0400A768 RID: 42856
		[HideInInspector]
		public TextureArrayConfig.PackingMode packingMode;

		// Token: 0x0400A769 RID: 42857
		[HideInInspector]
		public TextureArrayConfig.PBRWorkflow pbrWorkflow;

		// Token: 0x0400A76A RID: 42858
		[HideInInspector]
		public int hash;

		// Token: 0x0400A76B RID: 42859
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static List<TextureArrayConfig> sAllConfigs = new List<TextureArrayConfig>();

		// Token: 0x0400A76C RID: 42860
		[HideInInspector]
		public Texture2DArray splatArray;

		// Token: 0x0400A76D RID: 42861
		[HideInInspector]
		public Texture2DArray diffuseArray;

		// Token: 0x0400A76E RID: 42862
		[HideInInspector]
		public Texture2DArray normalSAOArray;

		// Token: 0x0400A76F RID: 42863
		[HideInInspector]
		public Texture2DArray smoothAOArray;

		// Token: 0x0400A770 RID: 42864
		[HideInInspector]
		public Texture2DArray specularArray;

		// Token: 0x0400A771 RID: 42865
		[HideInInspector]
		public Texture2DArray diffuseArray2;

		// Token: 0x0400A772 RID: 42866
		[HideInInspector]
		public Texture2DArray normalSAOArray2;

		// Token: 0x0400A773 RID: 42867
		[HideInInspector]
		public Texture2DArray smoothAOArray2;

		// Token: 0x0400A774 RID: 42868
		[HideInInspector]
		public Texture2DArray specularArray2;

		// Token: 0x0400A775 RID: 42869
		[HideInInspector]
		public Texture2DArray diffuseArray3;

		// Token: 0x0400A776 RID: 42870
		[HideInInspector]
		public Texture2DArray normalSAOArray3;

		// Token: 0x0400A777 RID: 42871
		[HideInInspector]
		public Texture2DArray smoothAOArray3;

		// Token: 0x0400A778 RID: 42872
		[HideInInspector]
		public Texture2DArray specularArray3;

		// Token: 0x0400A779 RID: 42873
		[HideInInspector]
		public Texture2DArray emisArray;

		// Token: 0x0400A77A RID: 42874
		[HideInInspector]
		public Texture2DArray emisArray2;

		// Token: 0x0400A77B RID: 42875
		[HideInInspector]
		public Texture2DArray emisArray3;

		// Token: 0x0400A77C RID: 42876
		public TextureArrayConfig.TextureArrayGroup defaultTextureSettings = new TextureArrayConfig.TextureArrayGroup();

		// Token: 0x0400A77D RID: 42877
		public List<TextureArrayConfig.PlatformTextureOverride> platformOverrides = new List<TextureArrayConfig.PlatformTextureOverride>();

		// Token: 0x0400A77E RID: 42878
		public TextureArrayConfig.SourceTextureSize sourceTextureSize;

		// Token: 0x0400A77F RID: 42879
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelHeight = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A780 RID: 42880
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelSmoothness = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A781 RID: 42881
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelAO = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A782 RID: 42882
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A783 RID: 42883
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures2 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A784 RID: 42884
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures3 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x02001D3C RID: 7484
		public enum AllTextureChannel
		{
			// Token: 0x0400A786 RID: 42886
			R,
			// Token: 0x0400A787 RID: 42887
			G,
			// Token: 0x0400A788 RID: 42888
			B,
			// Token: 0x0400A789 RID: 42889
			A,
			// Token: 0x0400A78A RID: 42890
			Custom
		}

		// Token: 0x02001D3D RID: 7485
		public enum TextureChannel
		{
			// Token: 0x0400A78C RID: 42892
			R,
			// Token: 0x0400A78D RID: 42893
			G,
			// Token: 0x0400A78E RID: 42894
			B,
			// Token: 0x0400A78F RID: 42895
			A
		}

		// Token: 0x02001D3E RID: 7486
		public enum Compression
		{
			// Token: 0x0400A791 RID: 42897
			AutomaticCompressed,
			// Token: 0x0400A792 RID: 42898
			ForceDXT,
			// Token: 0x0400A793 RID: 42899
			ForcePVR,
			// Token: 0x0400A794 RID: 42900
			ForceETC2,
			// Token: 0x0400A795 RID: 42901
			ForceASTC,
			// Token: 0x0400A796 RID: 42902
			ForceCrunch,
			// Token: 0x0400A797 RID: 42903
			Uncompressed
		}

		// Token: 0x02001D3F RID: 7487
		public enum TextureSize
		{
			// Token: 0x0400A799 RID: 42905
			k4096 = 4096,
			// Token: 0x0400A79A RID: 42906
			k2048 = 2048,
			// Token: 0x0400A79B RID: 42907
			k1024 = 1024,
			// Token: 0x0400A79C RID: 42908
			k512 = 512,
			// Token: 0x0400A79D RID: 42909
			k256 = 256,
			// Token: 0x0400A79E RID: 42910
			k128 = 128,
			// Token: 0x0400A79F RID: 42911
			k64 = 64,
			// Token: 0x0400A7A0 RID: 42912
			k32 = 32
		}

		// Token: 0x02001D40 RID: 7488
		[Serializable]
		public class TextureArraySettings
		{
			// Token: 0x0600DDAB RID: 56747 RVA: 0x004F47D6 File Offset: 0x004F29D6
			public TextureArraySettings(TextureArrayConfig.TextureSize s, TextureArrayConfig.Compression c, FilterMode f, int a = 1)
			{
				this.textureSize = s;
				this.compression = c;
				this.filterMode = f;
				this.Aniso = a;
			}

			// Token: 0x0400A7A1 RID: 42913
			public TextureArrayConfig.TextureSize textureSize;

			// Token: 0x0400A7A2 RID: 42914
			public TextureArrayConfig.Compression compression;

			// Token: 0x0400A7A3 RID: 42915
			public FilterMode filterMode;

			// Token: 0x0400A7A4 RID: 42916
			[Range(0f, 16f)]
			public int Aniso = 1;
		}

		// Token: 0x02001D41 RID: 7489
		public enum PBRWorkflow
		{
			// Token: 0x0400A7A6 RID: 42918
			Metallic,
			// Token: 0x0400A7A7 RID: 42919
			Specular
		}

		// Token: 0x02001D42 RID: 7490
		public enum PackingMode
		{
			// Token: 0x0400A7A9 RID: 42921
			Fastest,
			// Token: 0x0400A7AA RID: 42922
			Quality
		}

		// Token: 0x02001D43 RID: 7491
		public enum SourceTextureSize
		{
			// Token: 0x0400A7AC RID: 42924
			Unchanged,
			// Token: 0x0400A7AD RID: 42925
			k32 = 32,
			// Token: 0x0400A7AE RID: 42926
			k256 = 256
		}

		// Token: 0x02001D44 RID: 7492
		public enum TextureMode
		{
			// Token: 0x0400A7B0 RID: 42928
			Basic,
			// Token: 0x0400A7B1 RID: 42929
			PBR
		}

		// Token: 0x02001D45 RID: 7493
		public enum ClusterMode
		{
			// Token: 0x0400A7B3 RID: 42931
			None,
			// Token: 0x0400A7B4 RID: 42932
			TwoVariations,
			// Token: 0x0400A7B5 RID: 42933
			ThreeVariations
		}

		// Token: 0x02001D46 RID: 7494
		[Serializable]
		public class TextureArrayGroup
		{
			// Token: 0x0400A7B6 RID: 42934
			public TextureArrayConfig.TextureArraySettings diffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A7B7 RID: 42935
			public TextureArrayConfig.TextureArraySettings normalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Trilinear, 1);

			// Token: 0x0400A7B8 RID: 42936
			public TextureArrayConfig.TextureArraySettings smoothSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A7B9 RID: 42937
			public TextureArrayConfig.TextureArraySettings antiTileSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A7BA RID: 42938
			public TextureArrayConfig.TextureArraySettings emissiveSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A7BB RID: 42939
			public TextureArrayConfig.TextureArraySettings specularSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A7BC RID: 42940
			public TextureArrayConfig.TextureArraySettings traxDiffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A7BD RID: 42941
			public TextureArrayConfig.TextureArraySettings traxNormalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A7BE RID: 42942
			public TextureArrayConfig.TextureArraySettings decalSplatSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);
		}

		// Token: 0x02001D47 RID: 7495
		[Serializable]
		public class PlatformTextureOverride
		{
			// Token: 0x0400A7BF RID: 42943
			public TextureArrayConfig.TextureArrayGroup settings = new TextureArrayConfig.TextureArrayGroup();
		}

		// Token: 0x02001D48 RID: 7496
		[Serializable]
		public class TextureEntry
		{
			// Token: 0x0600DDAE RID: 56750 RVA: 0x004F48D8 File Offset: 0x004F2AD8
			public void Reset()
			{
				this.diffuse = null;
				this.height = null;
				this.normal = null;
				this.smoothness = null;
				this.specular = null;
				this.ao = null;
				this.isRoughness = false;
				this.detailNoise = null;
				this.distanceNoise = null;
				this.metal = null;
				this.emis = null;
				this.heightChannel = TextureArrayConfig.TextureChannel.G;
				this.smoothnessChannel = TextureArrayConfig.TextureChannel.G;
				this.aoChannel = TextureArrayConfig.TextureChannel.G;
				this.distanceChannel = TextureArrayConfig.TextureChannel.G;
				this.detailChannel = TextureArrayConfig.TextureChannel.G;
				this.traxDiffuse = null;
				this.traxNormal = null;
				this.traxHeight = null;
				this.traxSmoothness = null;
				this.traxAO = null;
				this.traxHeightChannel = TextureArrayConfig.TextureChannel.G;
				this.traxSmoothnessChannel = TextureArrayConfig.TextureChannel.G;
				this.traxAOChannel = TextureArrayConfig.TextureChannel.G;
				this.splat = null;
			}

			// Token: 0x0600DDAF RID: 56751 RVA: 0x004F4994 File Offset: 0x004F2B94
			public bool HasTextures(TextureArrayConfig.PBRWorkflow wf)
			{
				if (wf == TextureArrayConfig.PBRWorkflow.Specular)
				{
					return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.specular != null || this.ao != null;
				}
				return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.metal != null || this.ao != null;
			}

			// Token: 0x0400A7C0 RID: 42944
			public Texture2D diffuse;

			// Token: 0x0400A7C1 RID: 42945
			public Texture2D height;

			// Token: 0x0400A7C2 RID: 42946
			public TextureArrayConfig.TextureChannel heightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7C3 RID: 42947
			public Texture2D normal;

			// Token: 0x0400A7C4 RID: 42948
			public Texture2D smoothness;

			// Token: 0x0400A7C5 RID: 42949
			public TextureArrayConfig.TextureChannel smoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7C6 RID: 42950
			public bool isRoughness;

			// Token: 0x0400A7C7 RID: 42951
			public Texture2D ao;

			// Token: 0x0400A7C8 RID: 42952
			public TextureArrayConfig.TextureChannel aoChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7C9 RID: 42953
			public Texture2D emis;

			// Token: 0x0400A7CA RID: 42954
			public Texture2D metal;

			// Token: 0x0400A7CB RID: 42955
			public TextureArrayConfig.TextureChannel metalChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7CC RID: 42956
			public Texture2D specular;

			// Token: 0x0400A7CD RID: 42957
			public Texture2D noiseNormal;

			// Token: 0x0400A7CE RID: 42958
			public Texture2D detailNoise;

			// Token: 0x0400A7CF RID: 42959
			public TextureArrayConfig.TextureChannel detailChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7D0 RID: 42960
			public Texture2D distanceNoise;

			// Token: 0x0400A7D1 RID: 42961
			public TextureArrayConfig.TextureChannel distanceChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7D2 RID: 42962
			public Texture2D traxDiffuse;

			// Token: 0x0400A7D3 RID: 42963
			public Texture2D traxHeight;

			// Token: 0x0400A7D4 RID: 42964
			public TextureArrayConfig.TextureChannel traxHeightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7D5 RID: 42965
			public Texture2D traxNormal;

			// Token: 0x0400A7D6 RID: 42966
			public Texture2D traxSmoothness;

			// Token: 0x0400A7D7 RID: 42967
			public TextureArrayConfig.TextureChannel traxSmoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7D8 RID: 42968
			public bool traxIsRoughness;

			// Token: 0x0400A7D9 RID: 42969
			public Texture2D traxAO;

			// Token: 0x0400A7DA RID: 42970
			public TextureArrayConfig.TextureChannel traxAOChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A7DB RID: 42971
			public Texture2D splat;
		}
	}
}
