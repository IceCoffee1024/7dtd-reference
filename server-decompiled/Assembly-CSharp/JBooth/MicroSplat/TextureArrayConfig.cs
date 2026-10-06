using System;
using System.Collections.Generic;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D6D RID: 7533
	[CreateAssetMenu(menuName = "MicroSplat/Texture Array Config", order = 1)]
	[ExecuteInEditMode]
	public class TextureArrayConfig : ScriptableObject
	{
		// Token: 0x0600DF1F RID: 57119 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsScatter()
		{
			return false;
		}

		// Token: 0x0600DF20 RID: 57120 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsDecal()
		{
			return false;
		}

		// Token: 0x0600DF21 RID: 57121 RVA: 0x004F9348 File Offset: 0x004F7548
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			TextureArrayConfig.sAllConfigs.Add(this);
		}

		// Token: 0x0600DF22 RID: 57122 RVA: 0x004F9355 File Offset: 0x004F7555
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDestroy()
		{
			TextureArrayConfig.sAllConfigs.Remove(this);
		}

		// Token: 0x0600DF23 RID: 57123 RVA: 0x004F9364 File Offset: 0x004F7564
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

		// Token: 0x0400A839 RID: 43065
		public bool diffuseIsLinear;

		// Token: 0x0400A83A RID: 43066
		[HideInInspector]
		public bool antiTileArray;

		// Token: 0x0400A83B RID: 43067
		[HideInInspector]
		public bool emisMetalArray;

		// Token: 0x0400A83C RID: 43068
		public bool traxArray;

		// Token: 0x0400A83D RID: 43069
		[HideInInspector]
		public TextureArrayConfig.TextureMode textureMode = TextureArrayConfig.TextureMode.PBR;

		// Token: 0x0400A83E RID: 43070
		[HideInInspector]
		public TextureArrayConfig.ClusterMode clusterMode;

		// Token: 0x0400A83F RID: 43071
		[HideInInspector]
		public TextureArrayConfig.PackingMode packingMode;

		// Token: 0x0400A840 RID: 43072
		[HideInInspector]
		public TextureArrayConfig.PBRWorkflow pbrWorkflow;

		// Token: 0x0400A841 RID: 43073
		[HideInInspector]
		public int hash;

		// Token: 0x0400A842 RID: 43074
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static List<TextureArrayConfig> sAllConfigs = new List<TextureArrayConfig>();

		// Token: 0x0400A843 RID: 43075
		[HideInInspector]
		public Texture2DArray splatArray;

		// Token: 0x0400A844 RID: 43076
		[HideInInspector]
		public Texture2DArray diffuseArray;

		// Token: 0x0400A845 RID: 43077
		[HideInInspector]
		public Texture2DArray normalSAOArray;

		// Token: 0x0400A846 RID: 43078
		[HideInInspector]
		public Texture2DArray smoothAOArray;

		// Token: 0x0400A847 RID: 43079
		[HideInInspector]
		public Texture2DArray specularArray;

		// Token: 0x0400A848 RID: 43080
		[HideInInspector]
		public Texture2DArray diffuseArray2;

		// Token: 0x0400A849 RID: 43081
		[HideInInspector]
		public Texture2DArray normalSAOArray2;

		// Token: 0x0400A84A RID: 43082
		[HideInInspector]
		public Texture2DArray smoothAOArray2;

		// Token: 0x0400A84B RID: 43083
		[HideInInspector]
		public Texture2DArray specularArray2;

		// Token: 0x0400A84C RID: 43084
		[HideInInspector]
		public Texture2DArray diffuseArray3;

		// Token: 0x0400A84D RID: 43085
		[HideInInspector]
		public Texture2DArray normalSAOArray3;

		// Token: 0x0400A84E RID: 43086
		[HideInInspector]
		public Texture2DArray smoothAOArray3;

		// Token: 0x0400A84F RID: 43087
		[HideInInspector]
		public Texture2DArray specularArray3;

		// Token: 0x0400A850 RID: 43088
		[HideInInspector]
		public Texture2DArray emisArray;

		// Token: 0x0400A851 RID: 43089
		[HideInInspector]
		public Texture2DArray emisArray2;

		// Token: 0x0400A852 RID: 43090
		[HideInInspector]
		public Texture2DArray emisArray3;

		// Token: 0x0400A853 RID: 43091
		public TextureArrayConfig.TextureArrayGroup defaultTextureSettings = new TextureArrayConfig.TextureArrayGroup();

		// Token: 0x0400A854 RID: 43092
		public List<TextureArrayConfig.PlatformTextureOverride> platformOverrides = new List<TextureArrayConfig.PlatformTextureOverride>();

		// Token: 0x0400A855 RID: 43093
		public TextureArrayConfig.SourceTextureSize sourceTextureSize;

		// Token: 0x0400A856 RID: 43094
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelHeight = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A857 RID: 43095
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelSmoothness = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A858 RID: 43096
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelAO = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A859 RID: 43097
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A85A RID: 43098
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures2 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A85B RID: 43099
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures3 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x02001D6E RID: 7534
		public enum AllTextureChannel
		{
			// Token: 0x0400A85D RID: 43101
			R,
			// Token: 0x0400A85E RID: 43102
			G,
			// Token: 0x0400A85F RID: 43103
			B,
			// Token: 0x0400A860 RID: 43104
			A,
			// Token: 0x0400A861 RID: 43105
			Custom
		}

		// Token: 0x02001D6F RID: 7535
		public enum TextureChannel
		{
			// Token: 0x0400A863 RID: 43107
			R,
			// Token: 0x0400A864 RID: 43108
			G,
			// Token: 0x0400A865 RID: 43109
			B,
			// Token: 0x0400A866 RID: 43110
			A
		}

		// Token: 0x02001D70 RID: 7536
		public enum Compression
		{
			// Token: 0x0400A868 RID: 43112
			AutomaticCompressed,
			// Token: 0x0400A869 RID: 43113
			ForceDXT,
			// Token: 0x0400A86A RID: 43114
			ForcePVR,
			// Token: 0x0400A86B RID: 43115
			ForceETC2,
			// Token: 0x0400A86C RID: 43116
			ForceASTC,
			// Token: 0x0400A86D RID: 43117
			ForceCrunch,
			// Token: 0x0400A86E RID: 43118
			Uncompressed
		}

		// Token: 0x02001D71 RID: 7537
		public enum TextureSize
		{
			// Token: 0x0400A870 RID: 43120
			k4096 = 4096,
			// Token: 0x0400A871 RID: 43121
			k2048 = 2048,
			// Token: 0x0400A872 RID: 43122
			k1024 = 1024,
			// Token: 0x0400A873 RID: 43123
			k512 = 512,
			// Token: 0x0400A874 RID: 43124
			k256 = 256,
			// Token: 0x0400A875 RID: 43125
			k128 = 128,
			// Token: 0x0400A876 RID: 43126
			k64 = 64,
			// Token: 0x0400A877 RID: 43127
			k32 = 32
		}

		// Token: 0x02001D72 RID: 7538
		[Serializable]
		public class TextureArraySettings
		{
			// Token: 0x0600DF26 RID: 57126 RVA: 0x004F941E File Offset: 0x004F761E
			public TextureArraySettings(TextureArrayConfig.TextureSize s, TextureArrayConfig.Compression c, FilterMode f, int a = 1)
			{
				this.textureSize = s;
				this.compression = c;
				this.filterMode = f;
				this.Aniso = a;
			}

			// Token: 0x0400A878 RID: 43128
			public TextureArrayConfig.TextureSize textureSize;

			// Token: 0x0400A879 RID: 43129
			public TextureArrayConfig.Compression compression;

			// Token: 0x0400A87A RID: 43130
			public FilterMode filterMode;

			// Token: 0x0400A87B RID: 43131
			[Range(0f, 16f)]
			public int Aniso = 1;
		}

		// Token: 0x02001D73 RID: 7539
		public enum PBRWorkflow
		{
			// Token: 0x0400A87D RID: 43133
			Metallic,
			// Token: 0x0400A87E RID: 43134
			Specular
		}

		// Token: 0x02001D74 RID: 7540
		public enum PackingMode
		{
			// Token: 0x0400A880 RID: 43136
			Fastest,
			// Token: 0x0400A881 RID: 43137
			Quality
		}

		// Token: 0x02001D75 RID: 7541
		public enum SourceTextureSize
		{
			// Token: 0x0400A883 RID: 43139
			Unchanged,
			// Token: 0x0400A884 RID: 43140
			k32 = 32,
			// Token: 0x0400A885 RID: 43141
			k256 = 256
		}

		// Token: 0x02001D76 RID: 7542
		public enum TextureMode
		{
			// Token: 0x0400A887 RID: 43143
			Basic,
			// Token: 0x0400A888 RID: 43144
			PBR
		}

		// Token: 0x02001D77 RID: 7543
		public enum ClusterMode
		{
			// Token: 0x0400A88A RID: 43146
			None,
			// Token: 0x0400A88B RID: 43147
			TwoVariations,
			// Token: 0x0400A88C RID: 43148
			ThreeVariations
		}

		// Token: 0x02001D78 RID: 7544
		[Serializable]
		public class TextureArrayGroup
		{
			// Token: 0x0400A88D RID: 43149
			public TextureArrayConfig.TextureArraySettings diffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A88E RID: 43150
			public TextureArrayConfig.TextureArraySettings normalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Trilinear, 1);

			// Token: 0x0400A88F RID: 43151
			public TextureArrayConfig.TextureArraySettings smoothSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A890 RID: 43152
			public TextureArrayConfig.TextureArraySettings antiTileSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A891 RID: 43153
			public TextureArrayConfig.TextureArraySettings emissiveSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A892 RID: 43154
			public TextureArrayConfig.TextureArraySettings specularSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A893 RID: 43155
			public TextureArrayConfig.TextureArraySettings traxDiffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A894 RID: 43156
			public TextureArrayConfig.TextureArraySettings traxNormalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A895 RID: 43157
			public TextureArrayConfig.TextureArraySettings decalSplatSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);
		}

		// Token: 0x02001D79 RID: 7545
		[Serializable]
		public class PlatformTextureOverride
		{
			// Token: 0x0400A896 RID: 43158
			public TextureArrayConfig.TextureArrayGroup settings = new TextureArrayConfig.TextureArrayGroup();
		}

		// Token: 0x02001D7A RID: 7546
		[Serializable]
		public class TextureEntry
		{
			// Token: 0x0600DF29 RID: 57129 RVA: 0x004F9520 File Offset: 0x004F7720
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

			// Token: 0x0600DF2A RID: 57130 RVA: 0x004F95DC File Offset: 0x004F77DC
			public bool HasTextures(TextureArrayConfig.PBRWorkflow wf)
			{
				if (wf == TextureArrayConfig.PBRWorkflow.Specular)
				{
					return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.specular != null || this.ao != null;
				}
				return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.metal != null || this.ao != null;
			}

			// Token: 0x0400A897 RID: 43159
			public Texture2D diffuse;

			// Token: 0x0400A898 RID: 43160
			public Texture2D height;

			// Token: 0x0400A899 RID: 43161
			public TextureArrayConfig.TextureChannel heightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A89A RID: 43162
			public Texture2D normal;

			// Token: 0x0400A89B RID: 43163
			public Texture2D smoothness;

			// Token: 0x0400A89C RID: 43164
			public TextureArrayConfig.TextureChannel smoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A89D RID: 43165
			public bool isRoughness;

			// Token: 0x0400A89E RID: 43166
			public Texture2D ao;

			// Token: 0x0400A89F RID: 43167
			public TextureArrayConfig.TextureChannel aoChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A8A0 RID: 43168
			public Texture2D emis;

			// Token: 0x0400A8A1 RID: 43169
			public Texture2D metal;

			// Token: 0x0400A8A2 RID: 43170
			public TextureArrayConfig.TextureChannel metalChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A8A3 RID: 43171
			public Texture2D specular;

			// Token: 0x0400A8A4 RID: 43172
			public Texture2D noiseNormal;

			// Token: 0x0400A8A5 RID: 43173
			public Texture2D detailNoise;

			// Token: 0x0400A8A6 RID: 43174
			public TextureArrayConfig.TextureChannel detailChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A8A7 RID: 43175
			public Texture2D distanceNoise;

			// Token: 0x0400A8A8 RID: 43176
			public TextureArrayConfig.TextureChannel distanceChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A8A9 RID: 43177
			public Texture2D traxDiffuse;

			// Token: 0x0400A8AA RID: 43178
			public Texture2D traxHeight;

			// Token: 0x0400A8AB RID: 43179
			public TextureArrayConfig.TextureChannel traxHeightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A8AC RID: 43180
			public Texture2D traxNormal;

			// Token: 0x0400A8AD RID: 43181
			public Texture2D traxSmoothness;

			// Token: 0x0400A8AE RID: 43182
			public TextureArrayConfig.TextureChannel traxSmoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A8AF RID: 43183
			public bool traxIsRoughness;

			// Token: 0x0400A8B0 RID: 43184
			public Texture2D traxAO;

			// Token: 0x0400A8B1 RID: 43185
			public TextureArrayConfig.TextureChannel traxAOChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A8B2 RID: 43186
			public Texture2D splat;
		}
	}
}
