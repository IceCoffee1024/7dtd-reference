using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020002AD RID: 685
	[NativeType("Runtime/Graphics/SpriteFrame.h")]
	[ExcludeFromPreset]
	[NativeHeader("Runtime/Graphics/SpriteUtility.h")]
	[NativeHeader("Runtime/2D/Common/SpriteDataAccess.h")]
	[NativeHeader("Runtime/2D/Common/ScriptBindings/SpritesMarshalling.h")]
	public sealed class Sprite : Object
	{
		// Token: 0x06001D23 RID: 7459 RVA: 0x000111F6 File Offset: 0x0000F3F6
		[RequiredByNativeCode]
		private Sprite()
		{
		}

		// Token: 0x06001D24 RID: 7460
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetPackingMode();

		// Token: 0x06001D25 RID: 7461
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetPackingRotation();

		// Token: 0x06001D26 RID: 7462
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern int GetPacked();

		// Token: 0x06001D27 RID: 7463 RVA: 0x00030018 File Offset: 0x0002E218
		internal Rect GetTextureRect()
		{
			Rect rect;
			this.GetTextureRect_Injected(out rect);
			return rect;
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x00030030 File Offset: 0x0002E230
		internal Vector2 GetTextureRectOffset()
		{
			Vector2 vector;
			this.GetTextureRectOffset_Injected(out vector);
			return vector;
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x00030048 File Offset: 0x0002E248
		internal Vector4 GetInnerUVs()
		{
			Vector4 vector;
			this.GetInnerUVs_Injected(out vector);
			return vector;
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x00030060 File Offset: 0x0002E260
		internal Vector4 GetOuterUVs()
		{
			Vector4 vector;
			this.GetOuterUVs_Injected(out vector);
			return vector;
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x00030078 File Offset: 0x0002E278
		internal Vector4 GetPadding()
		{
			Vector4 vector;
			this.GetPadding_Injected(out vector);
			return vector;
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x0003008E File Offset: 0x0002E28E
		[FreeFunction("SpritesBindings::CreateSpriteWithoutTextureScripting")]
		internal static Sprite CreateSpriteWithoutTextureScripting(Rect rect, Vector2 pivot, float pixelsToUnits, Texture2D texture)
		{
			return Sprite.CreateSpriteWithoutTextureScripting_Injected(ref rect, ref pivot, pixelsToUnits, texture);
		}

		// Token: 0x06001D2D RID: 7469 RVA: 0x0003009C File Offset: 0x0002E29C
		[FreeFunction("SpritesBindings::CreateSprite", ThrowsException = true)]
		internal static Sprite CreateSprite(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, Vector4 border, bool generateFallbackPhysicsShape, [Unmarshalled] SecondarySpriteTexture[] secondaryTexture)
		{
			return Sprite.CreateSprite_Injected(texture, ref rect, ref pivot, pixelsPerUnit, extrude, meshType, ref border, generateFallbackPhysicsShape, secondaryTexture);
		}

		// Token: 0x170005BF RID: 1471
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x000300C0 File Offset: 0x0002E2C0
		public Bounds bounds
		{
			get
			{
				Bounds bounds;
				this.get_bounds_Injected(out bounds);
				return bounds;
			}
		}

		// Token: 0x170005C0 RID: 1472
		// (get) Token: 0x06001D2F RID: 7471 RVA: 0x000300D8 File Offset: 0x0002E2D8
		public Rect rect
		{
			get
			{
				Rect rect;
				this.get_rect_Injected(out rect);
				return rect;
			}
		}

		// Token: 0x170005C1 RID: 1473
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x000300F0 File Offset: 0x0002E2F0
		public Vector4 border
		{
			get
			{
				Vector4 vector;
				this.get_border_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x170005C2 RID: 1474
		// (get) Token: 0x06001D31 RID: 7473
		public extern Texture2D texture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001D32 RID: 7474
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern Texture2D GetSecondaryTexture(int index);

		// Token: 0x06001D33 RID: 7475
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetSecondaryTextureCount();

		// Token: 0x06001D34 RID: 7476
		[FreeFunction("SpritesBindings::GetSecondaryTextures", ThrowsException = true, HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetSecondaryTextures([Unmarshalled] [NotNull("ArgumentNullException")] SecondarySpriteTexture[] secondaryTexture);

		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001D35 RID: 7477
		public extern float pixelsPerUnit
		{
			[NativeMethod("GetPixelsToUnits")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001D36 RID: 7478
		public extern float spriteAtlasTextureScale
		{
			[NativeMethod("GetSpriteAtlasTextureScale")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001D37 RID: 7479
		public extern Texture2D associatedAlphaSplitTexture
		{
			[NativeMethod("GetAlphaTexture")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x00030108 File Offset: 0x0002E308
		public Vector2 pivot
		{
			[NativeMethod("GetPivotInPixels")]
			get
			{
				Vector2 vector;
				this.get_pivot_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001D39 RID: 7481 RVA: 0x00030120 File Offset: 0x0002E320
		public bool packed
		{
			get
			{
				return this.GetPacked() == 1;
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x0003013C File Offset: 0x0002E33C
		public SpritePackingMode packingMode
		{
			get
			{
				return (SpritePackingMode)this.GetPackingMode();
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001D3B RID: 7483 RVA: 0x00030154 File Offset: 0x0002E354
		public SpritePackingRotation packingRotation
		{
			get
			{
				return (SpritePackingRotation)this.GetPackingRotation();
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x0003016C File Offset: 0x0002E36C
		public Rect textureRect
		{
			get
			{
				return this.GetTextureRect();
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001D3D RID: 7485 RVA: 0x00030184 File Offset: 0x0002E384
		public Vector2 textureRectOffset
		{
			get
			{
				return this.GetTextureRectOffset();
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001D3E RID: 7486
		public extern Vector2[] vertices
		{
			[FreeFunction("SpriteAccessLegacy::GetSpriteVertices", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005CD RID: 1485
		// (get) Token: 0x06001D3F RID: 7487
		public extern ushort[] triangles
		{
			[FreeFunction("SpriteAccessLegacy::GetSpriteIndices", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170005CE RID: 1486
		// (get) Token: 0x06001D40 RID: 7488
		public extern Vector2[] uv
		{
			[FreeFunction("SpriteAccessLegacy::GetSpriteUVs", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06001D41 RID: 7489
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetPhysicsShapeCount();

		// Token: 0x06001D42 RID: 7490 RVA: 0x0003019C File Offset: 0x0002E39C
		public int GetPhysicsShapePointCount(int shapeIdx)
		{
			int physicsShapeCount = this.GetPhysicsShapeCount();
			bool flag = shapeIdx < 0 || shapeIdx >= physicsShapeCount;
			if (flag)
			{
				throw new IndexOutOfRangeException(string.Format("Index({0}) is out of bounds(0 - {1})", shapeIdx, physicsShapeCount - 1));
			}
			return this.Internal_GetPhysicsShapePointCount(shapeIdx);
		}

		// Token: 0x06001D43 RID: 7491
		[NativeMethod("GetPhysicsShapePointCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int Internal_GetPhysicsShapePointCount(int shapeIdx);

		// Token: 0x06001D44 RID: 7492 RVA: 0x000301EC File Offset: 0x0002E3EC
		public int GetPhysicsShape(int shapeIdx, List<Vector2> physicsShape)
		{
			int physicsShapeCount = this.GetPhysicsShapeCount();
			bool flag = shapeIdx < 0 || shapeIdx >= physicsShapeCount;
			if (flag)
			{
				throw new IndexOutOfRangeException(string.Format("Index({0}) is out of bounds(0 - {1})", shapeIdx, physicsShapeCount - 1));
			}
			Sprite.GetPhysicsShapeImpl(this, shapeIdx, physicsShape);
			return physicsShape.Count;
		}

		// Token: 0x06001D45 RID: 7493
		[FreeFunction("SpritesBindings::GetPhysicsShape", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetPhysicsShapeImpl(Sprite sprite, int shapeIdx, [NotNull("ArgumentNullException")] List<Vector2> physicsShape);

		// Token: 0x06001D46 RID: 7494 RVA: 0x00030244 File Offset: 0x0002E444
		public void OverridePhysicsShape(IList<Vector2[]> physicsShapes)
		{
			bool flag = physicsShapes == null;
			if (flag)
			{
				throw new ArgumentNullException("physicsShapes");
			}
			for (int i = 0; i < physicsShapes.Count; i++)
			{
				Vector2[] array = physicsShapes[i];
				bool flag2 = array == null;
				if (flag2)
				{
					throw new ArgumentNullException("physicsShape", string.Format("Physics Shape at {0} is null.", i));
				}
				bool flag3 = array.Length < 3;
				if (flag3)
				{
					throw new ArgumentException(string.Format("Physics Shape at {0} has less than 3 vertices ({1}).", i, array.Length));
				}
			}
			Sprite.OverridePhysicsShapeCount(this, physicsShapes.Count);
			for (int j = 0; j < physicsShapes.Count; j++)
			{
				Sprite.OverridePhysicsShape(this, physicsShapes[j], j);
			}
		}

		// Token: 0x06001D47 RID: 7495
		[FreeFunction("SpritesBindings::OverridePhysicsShapeCount")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OverridePhysicsShapeCount(Sprite sprite, int physicsShapeCount);

		// Token: 0x06001D48 RID: 7496
		[FreeFunction("SpritesBindings::OverridePhysicsShape", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void OverridePhysicsShape(Sprite sprite, [Unmarshalled] Vector2[] physicsShape, int idx);

		// Token: 0x06001D49 RID: 7497
		[FreeFunction("SpritesBindings::OverrideGeometry", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void OverrideGeometry([NotNull("ArgumentNullException")] [Unmarshalled] Vector2[] vertices, [NotNull("ArgumentNullException")] [Unmarshalled] ushort[] triangles);

		// Token: 0x06001D4A RID: 7498 RVA: 0x00030310 File Offset: 0x0002E510
		internal static Sprite Create(Rect rect, Vector2 pivot, float pixelsToUnits, Texture2D texture)
		{
			return Sprite.CreateSpriteWithoutTextureScripting(rect, pivot, pixelsToUnits, texture);
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x0003032C File Offset: 0x0002E52C
		internal static Sprite Create(Rect rect, Vector2 pivot, float pixelsToUnits)
		{
			return Sprite.CreateSpriteWithoutTextureScripting(rect, pivot, pixelsToUnits, null);
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x00030348 File Offset: 0x0002E548
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, Vector4 border, bool generateFallbackPhysicsShape)
		{
			return Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, generateFallbackPhysicsShape, null);
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x0003036C File Offset: 0x0002E56C
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, Vector4 border, bool generateFallbackPhysicsShape, SecondarySpriteTexture[] secondaryTextures)
		{
			bool flag = texture == null;
			Sprite sprite;
			if (flag)
			{
				sprite = null;
			}
			else
			{
				bool flag2 = rect.xMax > (float)texture.width || rect.yMax > (float)texture.height;
				if (flag2)
				{
					throw new ArgumentException(string.Format("Could not create sprite ({0}, {1}, {2}, {3}) from a {4}x{5} texture.", new object[] { rect.x, rect.y, rect.width, rect.height, texture.width, texture.height }));
				}
				bool flag3 = pixelsPerUnit <= 0f;
				if (flag3)
				{
					throw new ArgumentException("pixelsPerUnit must be set to a positive non-zero value.");
				}
				bool flag4 = secondaryTextures != null;
				if (flag4)
				{
					foreach (SecondarySpriteTexture secondarySpriteTexture in secondaryTextures)
					{
						bool flag5 = secondarySpriteTexture.texture == texture;
						if (flag5)
						{
							throw new ArgumentException(string.Format("{0} is using source Texture as Secondary Texture.", secondarySpriteTexture.name));
						}
					}
				}
				sprite = Sprite.CreateSprite(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, generateFallbackPhysicsShape, secondaryTextures);
			}
			return sprite;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000304B0 File Offset: 0x0002E6B0
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, Vector4 border)
		{
			return Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, border, false);
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000304D4 File Offset: 0x0002E6D4
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType)
		{
			return Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, meshType, Vector4.zero);
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x000304F8 File Offset: 0x0002E6F8
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit, uint extrude)
		{
			return Sprite.Create(texture, rect, pivot, pixelsPerUnit, extrude, SpriteMeshType.Tight);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x00030518 File Offset: 0x0002E718
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot, float pixelsPerUnit)
		{
			return Sprite.Create(texture, rect, pivot, pixelsPerUnit, 0U);
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x00030534 File Offset: 0x0002E734
		public static Sprite Create(Texture2D texture, Rect rect, Vector2 pivot)
		{
			return Sprite.Create(texture, rect, pivot, 100f);
		}

		// Token: 0x06001D53 RID: 7507
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetTextureRect_Injected(out Rect ret);

		// Token: 0x06001D54 RID: 7508
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetTextureRectOffset_Injected(out Vector2 ret);

		// Token: 0x06001D55 RID: 7509
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetInnerUVs_Injected(out Vector4 ret);

		// Token: 0x06001D56 RID: 7510
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetOuterUVs_Injected(out Vector4 ret);

		// Token: 0x06001D57 RID: 7511
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPadding_Injected(out Vector4 ret);

		// Token: 0x06001D58 RID: 7512
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Sprite CreateSpriteWithoutTextureScripting_Injected(ref Rect rect, ref Vector2 pivot, float pixelsToUnits, Texture2D texture);

		// Token: 0x06001D59 RID: 7513
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Sprite CreateSprite_Injected(Texture2D texture, ref Rect rect, ref Vector2 pivot, float pixelsPerUnit, uint extrude, SpriteMeshType meshType, ref Vector4 border, bool generateFallbackPhysicsShape, SecondarySpriteTexture[] secondaryTexture);

		// Token: 0x06001D5A RID: 7514
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		// Token: 0x06001D5B RID: 7515
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_rect_Injected(out Rect ret);

		// Token: 0x06001D5C RID: 7516
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_border_Injected(out Vector4 ret);

		// Token: 0x06001D5D RID: 7517
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_pivot_Injected(out Vector2 ret);
	}
}
