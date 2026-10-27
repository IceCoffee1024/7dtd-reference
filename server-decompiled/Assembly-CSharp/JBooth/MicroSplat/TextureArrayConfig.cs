using System;
using System.Collections.Generic;
using UnityEngine;

namespace JBooth.MicroSplat
{
	// Token: 0x02001D9E RID: 7582
	[CreateAssetMenu(menuName = "MicroSplat/Texture Array Config", order = 1)]
	[ExecuteInEditMode]
	public class TextureArrayConfig : ScriptableObject
	{
		// Token: 0x0600E033 RID: 57395 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsScatter()
		{
			return false;
		}

		// Token: 0x0600E034 RID: 57396 RVA: 0x00010E62 File Offset: 0x0000F062
		public bool IsDecal()
		{
			return false;
		}

		// Token: 0x0600E035 RID: 57397 RVA: 0x004FCEE8 File Offset: 0x004FB0E8
		[PublicizedFrom(EAccessModifier.Private)]
		public void Awake()
		{
			TextureArrayConfig.sAllConfigs.Add(this);
		}

		// Token: 0x0600E036 RID: 57398 RVA: 0x004FCEF5 File Offset: 0x004FB0F5
		[PublicizedFrom(EAccessModifier.Private)]
		public void OnDestroy()
		{
			TextureArrayConfig.sAllConfigs.Remove(this);
		}

		// Token: 0x0600E037 RID: 57399 RVA: 0x004FCF04 File Offset: 0x004FB104
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

		// Token: 0x0400A922 RID: 43298
		public bool diffuseIsLinear;

		// Token: 0x0400A923 RID: 43299
		[HideInInspector]
		public bool antiTileArray;

		// Token: 0x0400A924 RID: 43300
		[HideInInspector]
		public bool emisMetalArray;

		// Token: 0x0400A925 RID: 43301
		public bool traxArray;

		// Token: 0x0400A926 RID: 43302
		[HideInInspector]
		public TextureArrayConfig.TextureMode textureMode = TextureArrayConfig.TextureMode.PBR;

		// Token: 0x0400A927 RID: 43303
		[HideInInspector]
		public TextureArrayConfig.ClusterMode clusterMode;

		// Token: 0x0400A928 RID: 43304
		[HideInInspector]
		public TextureArrayConfig.PackingMode packingMode;

		// Token: 0x0400A929 RID: 43305
		[HideInInspector]
		public TextureArrayConfig.PBRWorkflow pbrWorkflow;

		// Token: 0x0400A92A RID: 43306
		[HideInInspector]
		public int hash;

		// Token: 0x0400A92B RID: 43307
		[PublicizedFrom(EAccessModifier.Private)]
		[NonSerialized]
		public static List<TextureArrayConfig> sAllConfigs = new List<TextureArrayConfig>();

		// Token: 0x0400A92C RID: 43308
		[HideInInspector]
		public Texture2DArray splatArray;

		// Token: 0x0400A92D RID: 43309
		[HideInInspector]
		public Texture2DArray diffuseArray;

		// Token: 0x0400A92E RID: 43310
		[HideInInspector]
		public Texture2DArray normalSAOArray;

		// Token: 0x0400A92F RID: 43311
		[HideInInspector]
		public Texture2DArray smoothAOArray;

		// Token: 0x0400A930 RID: 43312
		[HideInInspector]
		public Texture2DArray specularArray;

		// Token: 0x0400A931 RID: 43313
		[HideInInspector]
		public Texture2DArray diffuseArray2;

		// Token: 0x0400A932 RID: 43314
		[HideInInspector]
		public Texture2DArray normalSAOArray2;

		// Token: 0x0400A933 RID: 43315
		[HideInInspector]
		public Texture2DArray smoothAOArray2;

		// Token: 0x0400A934 RID: 43316
		[HideInInspector]
		public Texture2DArray specularArray2;

		// Token: 0x0400A935 RID: 43317
		[HideInInspector]
		public Texture2DArray diffuseArray3;

		// Token: 0x0400A936 RID: 43318
		[HideInInspector]
		public Texture2DArray normalSAOArray3;

		// Token: 0x0400A937 RID: 43319
		[HideInInspector]
		public Texture2DArray smoothAOArray3;

		// Token: 0x0400A938 RID: 43320
		[HideInInspector]
		public Texture2DArray specularArray3;

		// Token: 0x0400A939 RID: 43321
		[HideInInspector]
		public Texture2DArray emisArray;

		// Token: 0x0400A93A RID: 43322
		[HideInInspector]
		public Texture2DArray emisArray2;

		// Token: 0x0400A93B RID: 43323
		[HideInInspector]
		public Texture2DArray emisArray3;

		// Token: 0x0400A93C RID: 43324
		public TextureArrayConfig.TextureArrayGroup defaultTextureSettings = new TextureArrayConfig.TextureArrayGroup();

		// Token: 0x0400A93D RID: 43325
		public List<TextureArrayConfig.PlatformTextureOverride> platformOverrides = new List<TextureArrayConfig.PlatformTextureOverride>();

		// Token: 0x0400A93E RID: 43326
		public TextureArrayConfig.SourceTextureSize sourceTextureSize;

		// Token: 0x0400A93F RID: 43327
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelHeight = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A940 RID: 43328
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelSmoothness = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A941 RID: 43329
		[HideInInspector]
		public TextureArrayConfig.AllTextureChannel allTextureChannelAO = TextureArrayConfig.AllTextureChannel.G;

		// Token: 0x0400A942 RID: 43330
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A943 RID: 43331
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures2 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x0400A944 RID: 43332
		[HideInInspector]
		public List<TextureArrayConfig.TextureEntry> sourceTextures3 = new List<TextureArrayConfig.TextureEntry>();

		// Token: 0x02001D9F RID: 7583
		public enum AllTextureChannel
		{
			// Token: 0x0400A946 RID: 43334
			R,
			// Token: 0x0400A947 RID: 43335
			G,
			// Token: 0x0400A948 RID: 43336
			B,
			// Token: 0x0400A949 RID: 43337
			A,
			// Token: 0x0400A94A RID: 43338
			Custom
		}

		// Token: 0x02001DA0 RID: 7584
		public enum TextureChannel
		{
			// Token: 0x0400A94C RID: 43340
			R,
			// Token: 0x0400A94D RID: 43341
			G,
			// Token: 0x0400A94E RID: 43342
			B,
			// Token: 0x0400A94F RID: 43343
			A
		}

		// Token: 0x02001DA1 RID: 7585
		public enum Compression
		{
			// Token: 0x0400A951 RID: 43345
			AutomaticCompressed,
			// Token: 0x0400A952 RID: 43346
			ForceDXT,
			// Token: 0x0400A953 RID: 43347
			ForcePVR,
			// Token: 0x0400A954 RID: 43348
			ForceETC2,
			// Token: 0x0400A955 RID: 43349
			ForceASTC,
			// Token: 0x0400A956 RID: 43350
			ForceCrunch,
			// Token: 0x0400A957 RID: 43351
			Uncompressed
		}

		// Token: 0x02001DA2 RID: 7586
		public enum TextureSize
		{
			// Token: 0x0400A959 RID: 43353
			k4096 = 4096,
			// Token: 0x0400A95A RID: 43354
			k2048 = 2048,
			// Token: 0x0400A95B RID: 43355
			k1024 = 1024,
			// Token: 0x0400A95C RID: 43356
			k512 = 512,
			// Token: 0x0400A95D RID: 43357
			k256 = 256,
			// Token: 0x0400A95E RID: 43358
			k128 = 128,
			// Token: 0x0400A95F RID: 43359
			k64 = 64,
			// Token: 0x0400A960 RID: 43360
			k32 = 32
		}

		// Token: 0x02001DA3 RID: 7587
		[Serializable]
		public class TextureArraySettings
		{
			// Token: 0x0600E03A RID: 57402 RVA: 0x004FCFBE File Offset: 0x004FB1BE
			public TextureArraySettings(TextureArrayConfig.TextureSize s, TextureArrayConfig.Compression c, FilterMode f, int a = 1)
			{
				this.textureSize = s;
				this.compression = c;
				this.filterMode = f;
				this.Aniso = a;
			}

			// Token: 0x0400A961 RID: 43361
			public TextureArrayConfig.TextureSize textureSize;

			// Token: 0x0400A962 RID: 43362
			public TextureArrayConfig.Compression compression;

			// Token: 0x0400A963 RID: 43363
			public FilterMode filterMode;

			// Token: 0x0400A964 RID: 43364
			[Range(0f, 16f)]
			public int Aniso = 1;
		}

		// Token: 0x02001DA4 RID: 7588
		public enum PBRWorkflow
		{
			// Token: 0x0400A966 RID: 43366
			Metallic,
			// Token: 0x0400A967 RID: 43367
			Specular
		}

		// Token: 0x02001DA5 RID: 7589
		public enum PackingMode
		{
			// Token: 0x0400A969 RID: 43369
			Fastest,
			// Token: 0x0400A96A RID: 43370
			Quality
		}

		// Token: 0x02001DA6 RID: 7590
		public enum SourceTextureSize
		{
			// Token: 0x0400A96C RID: 43372
			Unchanged,
			// Token: 0x0400A96D RID: 43373
			k32 = 32,
			// Token: 0x0400A96E RID: 43374
			k256 = 256
		}

		// Token: 0x02001DA7 RID: 7591
		public enum TextureMode
		{
			// Token: 0x0400A970 RID: 43376
			Basic,
			// Token: 0x0400A971 RID: 43377
			PBR
		}

		// Token: 0x02001DA8 RID: 7592
		public enum ClusterMode
		{
			// Token: 0x0400A973 RID: 43379
			None,
			// Token: 0x0400A974 RID: 43380
			TwoVariations,
			// Token: 0x0400A975 RID: 43381
			ThreeVariations
		}

		// Token: 0x02001DA9 RID: 7593
		[Serializable]
		public class TextureArrayGroup
		{
			// Token: 0x0400A976 RID: 43382
			public TextureArrayConfig.TextureArraySettings diffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A977 RID: 43383
			public TextureArrayConfig.TextureArraySettings normalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Trilinear, 1);

			// Token: 0x0400A978 RID: 43384
			public TextureArrayConfig.TextureArraySettings smoothSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A979 RID: 43385
			public TextureArrayConfig.TextureArraySettings antiTileSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A97A RID: 43386
			public TextureArrayConfig.TextureArraySettings emissiveSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A97B RID: 43387
			public TextureArrayConfig.TextureArraySettings specularSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A97C RID: 43388
			public TextureArrayConfig.TextureArraySettings traxDiffuseSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A97D RID: 43389
			public TextureArrayConfig.TextureArraySettings traxNormalSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);

			// Token: 0x0400A97E RID: 43390
			public TextureArrayConfig.TextureArraySettings decalSplatSettings = new TextureArrayConfig.TextureArraySettings(TextureArrayConfig.TextureSize.k1024, TextureArrayConfig.Compression.AutomaticCompressed, FilterMode.Bilinear, 1);
		}

		// Token: 0x02001DAA RID: 7594
		[Serializable]
		public class PlatformTextureOverride
		{
			// Token: 0x0400A97F RID: 43391
			public TextureArrayConfig.TextureArrayGroup settings = new TextureArrayConfig.TextureArrayGroup();
		}

		// Token: 0x02001DAB RID: 7595
		[Serializable]
		public class TextureEntry
		{
			// Token: 0x0600E03D RID: 57405 RVA: 0x004FD0C0 File Offset: 0x004FB2C0
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

			// Token: 0x0600E03E RID: 57406 RVA: 0x004FD17C File Offset: 0x004FB37C
			public bool HasTextures(TextureArrayConfig.PBRWorkflow wf)
			{
				if (wf == TextureArrayConfig.PBRWorkflow.Specular)
				{
					return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.specular != null || this.ao != null;
				}
				return this.diffuse != null || this.height != null || this.normal != null || this.smoothness != null || this.metal != null || this.ao != null;
			}

			// Token: 0x0400A980 RID: 43392
			public Texture2D diffuse;

			// Token: 0x0400A981 RID: 43393
			public Texture2D height;

			// Token: 0x0400A982 RID: 43394
			public TextureArrayConfig.TextureChannel heightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A983 RID: 43395
			public Texture2D normal;

			// Token: 0x0400A984 RID: 43396
			public Texture2D smoothness;

			// Token: 0x0400A985 RID: 43397
			public TextureArrayConfig.TextureChannel smoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A986 RID: 43398
			public bool isRoughness;

			// Token: 0x0400A987 RID: 43399
			public Texture2D ao;

			// Token: 0x0400A988 RID: 43400
			public TextureArrayConfig.TextureChannel aoChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A989 RID: 43401
			public Texture2D emis;

			// Token: 0x0400A98A RID: 43402
			public Texture2D metal;

			// Token: 0x0400A98B RID: 43403
			public TextureArrayConfig.TextureChannel metalChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A98C RID: 43404
			public Texture2D specular;

			// Token: 0x0400A98D RID: 43405
			public Texture2D noiseNormal;

			// Token: 0x0400A98E RID: 43406
			public Texture2D detailNoise;

			// Token: 0x0400A98F RID: 43407
			public TextureArrayConfig.TextureChannel detailChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A990 RID: 43408
			public Texture2D distanceNoise;

			// Token: 0x0400A991 RID: 43409
			public TextureArrayConfig.TextureChannel distanceChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A992 RID: 43410
			public Texture2D traxDiffuse;

			// Token: 0x0400A993 RID: 43411
			public Texture2D traxHeight;

			// Token: 0x0400A994 RID: 43412
			public TextureArrayConfig.TextureChannel traxHeightChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A995 RID: 43413
			public Texture2D traxNormal;

			// Token: 0x0400A996 RID: 43414
			public Texture2D traxSmoothness;

			// Token: 0x0400A997 RID: 43415
			public TextureArrayConfig.TextureChannel traxSmoothnessChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A998 RID: 43416
			public bool traxIsRoughness;

			// Token: 0x0400A999 RID: 43417
			public Texture2D traxAO;

			// Token: 0x0400A99A RID: 43418
			public TextureArrayConfig.TextureChannel traxAOChannel = TextureArrayConfig.TextureChannel.G;

			// Token: 0x0400A99B RID: 43419
			public Texture2D splat;
		}
	}
}
