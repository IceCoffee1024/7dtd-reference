using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PrefabVolumes;
using UniLinq;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x0200178D RID: 6029
	public class StreetTile
	{
		// Token: 0x0600BAFB RID: 47867 RVA: 0x004500CF File Offset: 0x0044E2CF
		public void SetTownship(Township _township)
		{
			this.Township = _township;
			this.GetData().IsCity = _township != null && !_township.IsWilderness();
		}

		// Token: 0x17001756 RID: 5974
		// (get) Token: 0x0600BAFC RID: 47868 RVA: 0x004500F2 File Offset: 0x0044E2F2
		public int GroupID
		{
			get
			{
				if (this.Township == null)
				{
					return -1;
				}
				return this.Township.ID;
			}
		}

		// Token: 0x17001757 RID: 5975
		// (get) Token: 0x0600BAFD RID: 47869 RVA: 0x00450109 File Offset: 0x0044E309
		public bool IsValidForStreetTile
		{
			get
			{
				return !this.GetData().OverlapsWater && (!this.OverlapsBiomes && !this.OverlapsRadiation && !this.HasSteepSlope) && this.TerrainType != TerrainType.mountains;
			}
		}

		// Token: 0x17001758 RID: 5976
		// (get) Token: 0x0600BAFE RID: 47870 RVA: 0x00450140 File Offset: 0x0044E340
		public bool IsValidForGateway
		{
			get
			{
				return !this.GetData().OverlapsWater && (!this.OverlapsBiomes && !this.OverlapsRadiation && !this.HasSteepSlope && this.TerrainType != TerrainType.mountains) && !this.HasPrefabs;
			}
		}

		// Token: 0x17001759 RID: 5977
		// (get) Token: 0x0600BAFF RID: 47871 RVA: 0x0045017D File Offset: 0x0044E37D
		public bool HasPrefabs
		{
			get
			{
				return this.StreetTilePrefabDatas != null && this.StreetTilePrefabDatas.Count > 0;
			}
		}

		// Token: 0x1700175A RID: 5978
		// (get) Token: 0x0600BB00 RID: 47872 RVA: 0x00450197 File Offset: 0x0044E397
		public bool HasStreetTilePrefab
		{
			get
			{
				return this.Township != null && this.District != null && this.HasPrefabs;
			}
		}

		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x0600BB01 RID: 47873 RVA: 0x004501B1 File Offset: 0x0044E3B1
		public Vector2i WorldPositionMax
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.WorldPosition + new Vector2i(150, 150);
			}
		}

		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x0600BB02 RID: 47874 RVA: 0x004501CD File Offset: 0x0044E3CD
		public BiomeType BiomeType
		{
			get
			{
				return this.worldBuilder.GetBiome(this.WorldPositionCenter);
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x0600BB03 RID: 47875 RVA: 0x004501E0 File Offset: 0x0044E3E0
		public float PositionHeight
		{
			get
			{
				return this.worldBuilder.GetHeight(this.WorldPositionCenter);
			}
		}

		// Token: 0x1700175E RID: 5982
		// (get) Token: 0x0600BB04 RID: 47876 RVA: 0x004501F3 File Offset: 0x0044E3F3
		public TerrainType TerrainType
		{
			get
			{
				return this.worldBuilder.GetTerrainType(this.WorldPositionCenter);
			}
		}

		// Token: 0x1700175F RID: 5983
		// (get) Token: 0x0600BB05 RID: 47877 RVA: 0x00450206 File Offset: 0x0044E406
		public int RoadExitCount
		{
			get
			{
				if (this.RoadShape < 0)
				{
					return 0;
				}
				return this.worldBuilder.StreetTileShared.RoadShapeExitCounts[this.RoadShape];
			}
		}

		// Token: 0x0600BB06 RID: 47878 RVA: 0x0045022E File Offset: 0x0044E42E
		public bool HasRoadExit(int _dir)
		{
			return this.RoadShape >= 0 && (this.worldBuilder.StreetTileShared.RoadShapeExitsPerRotation[this.RoadShape][this.Rotations] & (1 << _dir)) > 0;
		}

		// Token: 0x0600BB07 RID: 47879 RVA: 0x00450267 File Offset: 0x0044E467
		[PublicizedFrom(EAccessModifier.Private)]
		public int GetRoadExits(int _shape, int _rotations)
		{
			return this.worldBuilder.StreetTileShared.RoadShapeExitsPerRotation[_shape][_rotations];
		}

		// Token: 0x17001760 RID: 5984
		// (get) Token: 0x0600BB08 RID: 47880 RVA: 0x00450281 File Offset: 0x0044E481
		public string PrefabName
		{
			get
			{
				if (this.RoadShape < 0)
				{
					return "none";
				}
				return this.worldBuilder.StreetTileShared.RoadShapesDistrict[this.RoadShape];
			}
		}

		// Token: 0x17001761 RID: 5985
		// (get) Token: 0x0600BB09 RID: 47881 RVA: 0x004502A9 File Offset: 0x0044E4A9
		// (set) Token: 0x0600BB0A RID: 47882 RVA: 0x004502B1 File Offset: 0x0044E4B1
		public int Rotations
		{
			[PublicizedFrom(EAccessModifier.Private)]
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this.rotations;
			}
			[PublicizedFrom(EAccessModifier.Private)]
			set
			{
				if (value != this.rotations)
				{
					this.rotations = value;
				}
			}
		}

		// Token: 0x0600BB0B RID: 47883 RVA: 0x004502C4 File Offset: 0x0044E4C4
		public StreetTile(WorldBuilder _worldBuilder, Vector2i gridPosition)
		{
			this.worldBuilder = _worldBuilder;
			this.GridPosition = gridPosition;
			this.WorldPosition = this.GridPosition * 150;
			this.WorldPositionCenter = this.WorldPosition + new Vector2i(75, 75);
			this.Area = new Rect(this.WorldPosition.AsVector2(), Vector2.one * 150f);
			this.RoadShape = -1;
			this.Rotations = 0;
			if (this.GridPosition.x < 1 || this.GridPosition.x >= this.worldBuilder.StreetTileMapWidth - 1)
			{
				this.OverlapsRadiation = true;
			}
			if (this.GridPosition.y < 1 || this.GridPosition.y >= this.worldBuilder.StreetTileMapWidth - 1)
			{
				this.OverlapsRadiation = true;
			}
			this.HighwayExitInit();
			this.UpdateValidity();
		}

		// Token: 0x0600BB0C RID: 47884 RVA: 0x004503E0 File Offset: 0x0044E5E0
		public void UpdateValidity()
		{
			float positionHeight = this.PositionHeight;
			Vector2i worldPositionCenter = this.WorldPositionCenter;
			foreach (Vector2i vector2i in this.worldBuilder.StreetTileShared.dir9way)
			{
				Vector2i vector2i2 = worldPositionCenter + vector2i * 75;
				if (this.worldBuilder.GetRad(vector2i2.x, vector2i2.y) > 0)
				{
					this.OverlapsRadiation = true;
				}
				if (Utils.FastAbs(this.worldBuilder.GetHeight(vector2i2.x, vector2i2.y) - positionHeight) > 20f)
				{
					this.HasSteepSlope = true;
				}
			}
			ref StreetTileData data = ref this.GetData();
			BiomeType biomeType = this.BiomeType;
			int num = 0;
			int num2 = 0;
			Vector2i worldPositionMax = this.WorldPositionMax;
			for (int j = this.WorldPosition.y; j < worldPositionMax.y; j += 3)
			{
				for (int k = this.WorldPosition.x; k < worldPositionMax.x; k += 3)
				{
					num++;
					if (biomeType != this.worldBuilder.GetBiome(k, j))
					{
						this.OverlapsBiomes = true;
					}
					if (this.worldBuilder.data.GetWater(k, j) > 0)
					{
						num2++;
						data.OverlapsWater = true;
					}
				}
			}
			if ((float)num2 / (float)num > 0.9f)
			{
				this.AllIsWater = true;
			}
		}

		// Token: 0x0600BB0D RID: 47885 RVA: 0x00450540 File Offset: 0x0044E740
		[PublicizedFrom(EAccessModifier.Private)]
		public ref StreetTileData GetData()
		{
			return this.worldBuilder.data.GetStreetTileDataWorld(this.WorldPosition.x, this.WorldPosition.y);
		}

		// Token: 0x0600BB0E RID: 47886 RVA: 0x00450568 File Offset: 0x0044E768
		public Stamp GetStamp()
		{
			Color32 color = ((this.District.type == District.Type.Gateway) ? new Color32(byte.MaxValue, 0, 204, byte.MaxValue) : new Color32(byte.MaxValue, 0, 0, byte.MaxValue));
			int num = this.RoadShape;
			if (num < 0)
			{
				num = 2;
				color = Color.black;
			}
			TranslationData translationData = new TranslationData(this.WorldPositionCenter.x, this.WorldPositionCenter.y, 1f, this.rotations * -90);
			return new Stamp(this.worldBuilder, this.worldBuilder.StampManager.GetStamp(this.worldBuilder.StreetTileShared.RoadShapes[num], null), translationData, color, 0.1f, false, "");
		}

		// Token: 0x0600BB0F RID: 47887 RVA: 0x0045062C File Offset: 0x0044E82C
		public StreetTile[] GetNeighbors()
		{
			if (this.neighbors4Way == null)
			{
				this.neighbors4Way = new StreetTile[4];
				for (int i = 0; i < 4; i++)
				{
					this.neighbors4Way[i] = this.GetNeighbor(this.worldBuilder.StreetTileShared.dir4way[i]);
				}
			}
			return this.neighbors4Way;
		}

		// Token: 0x0600BB10 RID: 47888 RVA: 0x00450684 File Offset: 0x0044E884
		public int GetNeighborCount()
		{
			if (this.neighbors4Way == null)
			{
				this.GetNeighbors();
			}
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if (this.neighbors4Way[i] != null)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600BB11 RID: 47889 RVA: 0x004506C0 File Offset: 0x0044E8C0
		public StreetTile[] GetNeighborsDiagonal()
		{
			if (this.neighborsDiagonal == null)
			{
				this.neighborsDiagonal = new StreetTile[4];
				for (int i = 0; i < 4; i++)
				{
					this.neighborsDiagonal[i] = this.GetNeighbor(this.worldBuilder.StreetTileShared.dirDiagonal[i]);
				}
			}
			return this.neighborsDiagonal;
		}

		// Token: 0x0600BB12 RID: 47890 RVA: 0x00450718 File Offset: 0x0044E918
		public StreetTile[] GetNeighbors8Way()
		{
			if (this.neighbors8Way == null)
			{
				this.neighbors8Way = new StreetTile[8];
				for (int i = 0; i < 8; i++)
				{
					this.neighbors8Way[i] = this.GetNeighbor(this.worldBuilder.StreetTileShared.dir8way[i]);
				}
			}
			return this.neighbors8Way;
		}

		// Token: 0x0600BB13 RID: 47891 RVA: 0x0045076F File Offset: 0x0044E96F
		public StreetTile GetNeighbor(Vector2i direction)
		{
			return this.worldBuilder.GetStreetTileGrid(this.GridPosition + direction);
		}

		// Token: 0x0600BB14 RID: 47892 RVA: 0x00450788 File Offset: 0x0044E988
		public bool HasNeighbor(StreetTile otherTile)
		{
			StreetTile[] neighbors = this.GetNeighbors();
			for (int i = 0; i < neighbors.Length; i++)
			{
				if (neighbors[i] == otherTile)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BB15 RID: 47893 RVA: 0x004507B3 File Offset: 0x0044E9B3
		public StreetTile GetNeighbor(int _dir)
		{
			if (this.neighbors4Way == null)
			{
				this.GetNeighbors();
			}
			return this.neighbors4Way[_dir];
		}

		// Token: 0x0600BB16 RID: 47894 RVA: 0x004507CC File Offset: 0x0044E9CC
		public int GetNeighborDir(StreetTile otherTile)
		{
			if (this.neighbors4Way == null)
			{
				this.GetNeighbors();
			}
			for (int i = 0; i < 4; i++)
			{
				if (this.neighbors4Way[i] == otherTile)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600BB17 RID: 47895 RVA: 0x00450804 File Offset: 0x0044EA04
		public bool HasExitTo(StreetTile otherTile)
		{
			if (this.Township == null && this.District == null)
			{
				return false;
			}
			if (otherTile.Township == null && otherTile.District == null)
			{
				return false;
			}
			int neighborDir = this.GetNeighborDir(otherTile);
			return neighborDir >= 0 && this.HasRoadExit(neighborDir);
		}

		// Token: 0x0600BB18 RID: 47896 RVA: 0x0045084C File Offset: 0x0044EA4C
		public void SetPathingConstraintsForTile(bool allBlocked = false)
		{
			if (this.Township != null && this.District != null)
			{
				this.worldBuilder.PathingUtils.AddFullyBlockedArea(this.Area);
				this.isFullyBlocked = true;
				return;
			}
			if (allBlocked && !this.isFullyBlocked && !this.isPartBlocked)
			{
				this.worldBuilder.PathingUtils.AddFullyBlockedArea(this.Area);
				this.isFullyBlocked = true;
				return;
			}
			if (!allBlocked)
			{
				if (this.isFullyBlocked)
				{
					this.worldBuilder.PathingUtils.RemoveFullyBlockedArea(this.Area);
				}
				this.worldBuilder.PathingUtils.AddMoveLimitArea(this.Area);
				this.isPartBlocked = true;
			}
		}

		// Token: 0x0600BB19 RID: 47897 RVA: 0x004508F8 File Offset: 0x0044EAF8
		[PublicizedFrom(EAccessModifier.Private)]
		public void HighwayExitInit()
		{
			this.highwayExitPositions = new Vector2i[4];
			for (int i = 0; i < 4; i++)
			{
				this.highwayExitPositions[i] = this.HighwayExitFromDir(i);
			}
		}

		// Token: 0x0600BB1A RID: 47898 RVA: 0x00450930 File Offset: 0x0044EB30
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i HighwayExitFromDir(int _dir)
		{
			Vector2i vector2i;
			if (_dir == 0)
			{
				vector2i.x = this.WorldPositionCenter.x;
				vector2i.y = this.WorldPositionMax.y - 1;
				return vector2i;
			}
			if (_dir == 1)
			{
				vector2i.x = this.WorldPositionMax.x - 1;
				vector2i.y = this.WorldPositionCenter.y;
				return vector2i;
			}
			if (_dir == 2)
			{
				vector2i.x = this.WorldPositionCenter.x;
				vector2i.y = this.WorldPosition.y;
				return vector2i;
			}
			vector2i.x = this.WorldPosition.x;
			vector2i.y = this.WorldPositionCenter.y;
			return vector2i;
		}

		// Token: 0x0600BB1B RID: 47899 RVA: 0x004509E4 File Offset: 0x0044EBE4
		public int GetHighwayExitDir(Vector2i _worldPos)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.GetHighwayExitPos(i) == _worldPos)
				{
					return i;
				}
			}
			return 0;
		}

		// Token: 0x0600BB1C RID: 47900 RVA: 0x00450A0F File Offset: 0x0044EC0F
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Vector2i GetHighwayExitPos(int _dir)
		{
			return this.highwayExitPositions[_dir];
		}

		// Token: 0x0600BB1D RID: 47901 RVA: 0x00450A20 File Offset: 0x0044EC20
		public List<Vector2i> GetHighwayExits(bool _isGateway = false)
		{
			List<Vector2i> list = new List<Vector2i>();
			if (this.UsedExitList.Count == 1)
			{
				int num = -1;
				for (int i = 0; i < 4; i++)
				{
					if ((this.ConnectedExits & (1 << i)) > 0)
					{
						num = (i + 2) & 3;
						break;
					}
				}
				if (num != -1)
				{
					list.Add(this.GetHighwayExitPos(num));
				}
				else
				{
					Log.Error("Could not find opposite highway exit!");
				}
			}
			else
			{
				for (int j = 0; j < 4; j++)
				{
					if ((this.ConnectedExits & (1 << j)) <= 0)
					{
						list.Add(this.GetHighwayExitPos(j));
					}
				}
			}
			return list;
		}

		// Token: 0x0600BB1E RID: 47902 RVA: 0x00450AB4 File Offset: 0x0044ECB4
		[PublicizedFrom(EAccessModifier.Private)]
		public void SetRoadShape(int _exits)
		{
			int count = this.worldBuilder.StreetTileShared.RoadShapeExitCounts.Count;
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					if (_exits == this.GetRoadExits(i, j))
					{
						this.RoadShape = i;
						this.Rotations = j;
						return;
					}
				}
			}
			this.RoadShape = -1;
			this.Rotations = 0;
		}

		// Token: 0x0600BB1F RID: 47903 RVA: 0x00450B18 File Offset: 0x0044ED18
		public int CountAllDistrictExits()
		{
			int num = 0;
			this.CalcConnectedDistrictTiles(StreetTile.tempTiles);
			for (int i = 0; i < StreetTile.tempTiles.Count; i++)
			{
				num += StreetTile.tempTiles[i].CountDistrictExits();
			}
			StreetTile.tempTiles.Clear();
			return num;
		}

		// Token: 0x0600BB20 RID: 47904 RVA: 0x00450B68 File Offset: 0x0044ED68
		public int CountDistrictExitsBetween(StreetTile _neighbor)
		{
			int num = 0;
			this.CalcConnectedDistrictTiles(StreetTile.tempTiles);
			_neighbor.CalcConnectedDistrictTiles(StreetTile.tempTiles2);
			for (int i = 0; i < StreetTile.tempTiles.Count; i++)
			{
				StreetTile streetTile = StreetTile.tempTiles[i];
				for (int j = 0; j < StreetTile.tempTiles2.Count; j++)
				{
					StreetTile streetTile2 = StreetTile.tempTiles2[j];
					int neighborDir = streetTile.GetNeighborDir(streetTile2);
					if (neighborDir >= 0 && (streetTile.ConnectedExits & (1 << neighborDir)) > 0)
					{
						num++;
					}
				}
			}
			StreetTile.tempTiles.Clear();
			StreetTile.tempTiles2.Clear();
			return num;
		}

		// Token: 0x0600BB21 RID: 47905 RVA: 0x00450C09 File Offset: 0x0044EE09
		public void CalcConnectedDistrictTiles(List<StreetTile> _tiles)
		{
			_tiles.Add(this);
			this.CalcConnectedDistrictNeighbors(_tiles);
		}

		// Token: 0x0600BB22 RID: 47906 RVA: 0x00450C19 File Offset: 0x0044EE19
		public int CalcConnectedDistrictTilesCount(List<StreetTile> _tiles)
		{
			this.CalcConnectedDistrictNeighbors(_tiles);
			int num = 1 + _tiles.Count;
			_tiles.Clear();
			return num;
		}

		// Token: 0x0600BB23 RID: 47907 RVA: 0x00450C30 File Offset: 0x0044EE30
		[PublicizedFrom(EAccessModifier.Private)]
		public void CalcConnectedDistrictNeighbors(List<StreetTile> _tiles)
		{
			for (int i = 0; i < 4; i++)
			{
				if ((this.ConnectedExits & (1 << i)) > 0)
				{
					StreetTile neighbor = this.GetNeighbor(i);
					if (neighbor != null && neighbor.District == this.District && neighbor.Township == this.Township && !_tiles.Contains(neighbor))
					{
						_tiles.Add(neighbor);
						neighbor.CalcConnectedDistrictNeighbors(_tiles);
					}
				}
			}
		}

		// Token: 0x0600BB24 RID: 47908 RVA: 0x00450C98 File Offset: 0x0044EE98
		public int CountDistrictExits()
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				if ((this.ConnectedExits & (1 << i)) > 0)
				{
					StreetTile neighbor = this.GetNeighbor(i);
					if (neighbor != null && neighbor.District != this.District && neighbor.Township == this.Township)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x0600BB25 RID: 47909 RVA: 0x00450CF0 File Offset: 0x0044EEF0
		public bool ChangeLConnectionToCap(GameRandom _rand)
		{
			if (this.ConnectedExits != 3 && this.ConnectedExits != 6 && this.ConnectedExits != 12 && this.ConnectedExits != 9)
			{
				return false;
			}
			int num = this.CalcConnectedDistrictTilesCount(StreetTile.tempTiles);
			for (int i = 0; i < 4; i++)
			{
				if ((this.ConnectedExits & (1 << i)) > 0)
				{
					StreetTile neighbor = this.GetNeighbor(i);
					if (neighbor != null && neighbor.District == this.District && neighbor.Township == this.Township)
					{
						StreetTile.tempTiles.Add(neighbor);
					}
				}
			}
			bool flag = false;
			if (StreetTile.tempTiles.Count == 2)
			{
				int num2 = _rand.RandomRange(0, StreetTile.tempTiles.Count);
				int neighborDir = this.GetNeighborDir(StreetTile.tempTiles[num2]);
				this.ConnectedExits &= ~(1 << neighborDir);
				int num3 = this.CalcConnectedDistrictTilesCount(StreetTile.tempTiles2);
				if (num == num3)
				{
					this.SetExitUnUsedAndFromNeighbor(neighborDir);
					flag = true;
				}
				else
				{
					this.ConnectedExits |= 1 << neighborDir;
				}
			}
			StreetTile.tempTiles.Clear();
			return flag;
		}

		// Token: 0x0600BB26 RID: 47910 RVA: 0x00450E08 File Offset: 0x0044F008
		public void SetExitsToMyTownship()
		{
			int num = 0;
			for (int i = 0; i < 4; i++)
			{
				StreetTile neighbor = this.GetNeighbor(i);
				if (neighbor != null && neighbor.Township == this.Township)
				{
					num |= 1 << i;
				}
			}
			this.SetExits(num);
		}

		// Token: 0x0600BB27 RID: 47911 RVA: 0x00450E4C File Offset: 0x0044F04C
		public int GetExitUsedCount()
		{
			return this.UsedExitList.Count<Vector2i>();
		}

		// Token: 0x0600BB28 RID: 47912 RVA: 0x00450E5C File Offset: 0x0044F05C
		public void SetExits(int _exits)
		{
			this.ConnectedExits = _exits;
			this.SetRoadShape(_exits);
			this.UsedExitList.Clear();
			for (int i = 0; i < 4; i++)
			{
				if ((_exits & (1 << i)) > 0)
				{
					Vector2i highwayExitPos = this.GetHighwayExitPos(i);
					this.UsedExitList.Add(highwayExitPos);
				}
			}
		}

		// Token: 0x0600BB29 RID: 47913 RVA: 0x00450EAD File Offset: 0x0044F0AD
		public void SetExitUsed(int _dir)
		{
			this.ConnectedExits |= 1 << _dir;
			this.SetExits(this.ConnectedExits);
		}

		// Token: 0x0600BB2A RID: 47914 RVA: 0x00450ED0 File Offset: 0x0044F0D0
		public bool SetExitUsed(Vector2i exit)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.GetHighwayExitPos(i) == exit)
				{
					this.SetExitUsed(i);
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BB2B RID: 47915 RVA: 0x00450F02 File Offset: 0x0044F102
		public void SetExitUnUsed(int _dir)
		{
			this.ConnectedExits &= ~(1 << _dir);
			this.SetExits(this.ConnectedExits);
		}

		// Token: 0x0600BB2C RID: 47916 RVA: 0x00450F24 File Offset: 0x0044F124
		public void SetExitUnUsed(Vector2i _exit)
		{
			for (int i = 0; i < 4; i++)
			{
				if (this.GetHighwayExitPos(i) == _exit)
				{
					this.SetExitUnUsed(i);
					return;
				}
			}
		}

		// Token: 0x0600BB2D RID: 47917 RVA: 0x00450F54 File Offset: 0x0044F154
		public void SetExitUnUsedAndFromNeighbor(int _dir)
		{
			this.SetExitUnUsed(_dir);
			StreetTile neighbor = this.GetNeighbor(_dir);
			if (neighbor == null)
			{
				return;
			}
			neighbor.SetExitUnUsed((_dir + 2) & 3);
		}

		// Token: 0x17001762 RID: 5986
		// (get) Token: 0x0600BB2E RID: 47918 RVA: 0x00450F73 File Offset: 0x0044F173
		public bool ContainsHighway
		{
			get
			{
				return this.ConnectedHighways.Count > 0;
			}
		}

		// Token: 0x0600BB2F RID: 47919 RVA: 0x00450F83 File Offset: 0x0044F183
		public void AddHighway(Path _path)
		{
			if (!this.ConnectedHighways.Contains(_path))
			{
				this.ConnectedHighways.Add(_path);
				this.GetData().ConnectedHighwayCount = this.ConnectedHighways.Count;
			}
		}

		// Token: 0x0600BB30 RID: 47920 RVA: 0x00450FB5 File Offset: 0x0044F1B5
		public void RemoveHighway(Path _path)
		{
			if (this.ConnectedHighways.Remove(_path))
			{
				this.GetData().ConnectedHighwayCount = this.ConnectedHighways.Count;
			}
		}

		// Token: 0x0600BB31 RID: 47921 RVA: 0x00450FDC File Offset: 0x0044F1DC
		public bool SpawnPrefabs()
		{
			if (this.District == null || this.District.name == "wilderness")
			{
				if (!this.ContainsHighway)
				{
					this.District = DistrictPlannerStatic.Districts["wilderness"];
					if (this.spawnWildernessPrefab())
					{
						return true;
					}
				}
				this.District = null;
				return false;
			}
			string text = string.Format(this.PrefabName, this.District.prefabName);
			this.spawnStreetTile(this.WorldPosition, text, this.Rotations);
			return true;
		}

		// Token: 0x0600BB32 RID: 47922 RVA: 0x00451064 File Offset: 0x0044F264
		[PublicizedFrom(EAccessModifier.Private)]
		public bool spawnStreetTile(Vector2i tileMinPositionWorld, string streetPrefabName, int baseRotations)
		{
			bool flag = false;
			PrefabData prefabData = this.worldBuilder.PrefabManager.GetStreetTile(streetPrefabName, this.WorldPositionCenter, flag);
			if (prefabData == null && string.Format(this.PrefabName, "") != streetPrefabName)
			{
				prefabData = this.worldBuilder.PrefabManager.GetStreetTile(string.Format(this.PrefabName, ""), this.WorldPositionCenter, flag);
			}
			if (prefabData == null)
			{
				return false;
			}
			int num = this.Township.Height + prefabData.yOffset;
			if (num < 3)
			{
				return false;
			}
			int num2 = (baseRotations + (int)prefabData.rotationToFaceNorth) & 3;
			if (num2 == 1)
			{
				num2 = 3;
			}
			else if (num2 == 3)
			{
				num2 = 1;
			}
			Vector3i vector3i = new Vector3i(tileMinPositionWorld.x, num, tileMinPositionWorld.y) + this.worldBuilder.PrefabWorldOffset;
			int num3;
			if (this.worldBuilder.PrefabManager.StreetTilesUsed.TryGetValue(prefabData.Name, out num3))
			{
				this.worldBuilder.PrefabManager.StreetTilesUsed[prefabData.Name] = num3 + 1;
			}
			else
			{
				this.worldBuilder.PrefabManager.StreetTilesUsed.Add(prefabData.Name, 1);
			}
			float num4 = 62f;
			float num5;
			if (PrefabManagerStatic.TileMaxDensityScore.TryGetValue(prefabData.Name, out num5))
			{
				num4 = num5;
			}
			PrefabManager prefabManager = this.worldBuilder.PrefabManager;
			int prefabInstanceId = prefabManager.PrefabInstanceId;
			prefabManager.PrefabInstanceId = prefabInstanceId + 1;
			this.AddPrefab(new PrefabDataInstance(prefabInstanceId, vector3i, (byte)num2, prefabData));
			this.SpawnMarkerPartsAndPrefabs(prefabData, new Vector3i(this.WorldPosition.x, num, this.WorldPosition.y), num2, 0, num4);
			return true;
		}

		// Token: 0x0600BB33 RID: 47923 RVA: 0x004511FB File Offset: 0x0044F3FB
		public void SmoothWildernessTerrain()
		{
			this.SmoothTerrainRect(this.WildernessPOIPos, this.WildernessPOISize.x, this.WildernessPOISize.y, this.WildernessPOIHeight, 18);
		}

		// Token: 0x0600BB34 RID: 47924 RVA: 0x00451228 File Offset: 0x0044F428
		public void SmoothTownshipTerrain()
		{
			if (this.Township != null && this.District != null)
			{
				this.Township.CalcCenterStreetTile();
				int num = ((this.Township.Streets.Count <= 2) ? 50 : 110);
				this.SmoothTerrainRect(this.WorldPosition, 150, 150, this.Township.Height, num);
			}
		}

		// Token: 0x0600BB35 RID: 47925 RVA: 0x00451290 File Offset: 0x0044F490
		[PublicizedFrom(EAccessModifier.Private)]
		public void SmoothTerrainRect(Vector2i _startPos, int _sizeX, int _sizeY, int _height, int _fadeRange)
		{
			int num = _fadeRange + 1;
			float num2 = (float)_fadeRange;
			int x = _startPos.x;
			int num3 = _startPos.x + _sizeX;
			int y = _startPos.y;
			int num4 = _startPos.y + _sizeY;
			int num5 = Utils.FastMax(x - num, 1);
			int num6 = Utils.FastMax(y - num, 1);
			int num7 = Utils.FastMin(num3 + num, this.worldBuilder.WorldSize);
			int num8 = Utils.FastMin(num4 + num, this.worldBuilder.WorldSize);
			for (int i = num6; i < num8; i++)
			{
				bool flag = i >= y && i <= num4;
				int num9 = (flag ? i : ((i < _startPos.y) ? y : num4));
				for (int j = num5; j < num7; j++)
				{
					bool flag2 = j >= x && j <= num3;
					if (flag2 && flag)
					{
						this.worldBuilder.SetHeightTrusted(j, i, (float)_height);
					}
					else
					{
						int num10 = (flag2 ? j : ((j < _startPos.x) ? x : num3));
						float num11 = Mathf.Sqrt((float)this.distanceSqr(j, i, num10, num9)) / num2;
						if (num11 < 1f)
						{
							float height = this.worldBuilder.GetHeight(j, i);
							this.worldBuilder.SetHeightTrusted(j, i, StreetTile.SmoothStep((float)_height, height, (double)num11));
						}
					}
				}
			}
		}

		// Token: 0x0600BB36 RID: 47926 RVA: 0x004513F8 File Offset: 0x0044F5F8
		[PublicizedFrom(EAccessModifier.Private)]
		public void SmoothTerrainCircle(Vector2i _centerPos, int _size, int _height)
		{
			int num = _size / 2;
			float num2 = (float)num * 1.8f;
			int num3 = Mathf.CeilToInt(num2 * num2);
			int num4 = (int)((float)num * 3.2f);
			float num5 = (float)num4 - num2;
			int num6 = Utils.FastMax(_centerPos.x - num4, 1);
			int num7 = Utils.FastMax(_centerPos.y - num4, 1);
			int num8 = Utils.FastMin(_centerPos.x + num4, this.worldBuilder.WorldSize);
			int num9 = Utils.FastMin(_centerPos.y + num4, this.worldBuilder.WorldSize);
			for (int i = num7; i < num9; i++)
			{
				for (int j = num6; j < num8; j++)
				{
					int num10 = this.distanceSqr(j, i, _centerPos.x, _centerPos.y);
					if (num10 <= num3)
					{
						this.worldBuilder.SetHeightTrusted(j, i, (float)_height);
					}
					else
					{
						float num11 = (Mathf.Sqrt((float)num10) - num2) / num5;
						if (num11 < 1f)
						{
							float height = this.worldBuilder.GetHeight(j, i);
							this.worldBuilder.SetHeightTrusted(j, i, StreetTile.SmoothStep((float)_height, height, (double)num11));
						}
					}
				}
			}
		}

		// Token: 0x0600BB37 RID: 47927 RVA: 0x00451515 File Offset: 0x0044F715
		[PublicizedFrom(EAccessModifier.Private)]
		public static float SmoothStep(float from, float to, double t)
		{
			t = -2.0 * t * t * t + 3.0 * t * t;
			return (float)((double)to * t + (double)from * (1.0 - t));
		}

		// Token: 0x0600BB38 RID: 47928 RVA: 0x0045154C File Offset: 0x0044F74C
		[PublicizedFrom(EAccessModifier.Private)]
		public bool spawnWildernessPrefab()
		{
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(this.worldBuilder.Seed + 4096953 + this.GridPosition.x + this.GridPosition.y * 200);
			FastTags<TagGroup.Poi> fastTags = ((this.worldBuilder.Towns == WorldBuilder.GenerationSelections.None) ? FastTags<TagGroup.Poi>.none : this.worldBuilder.StreetTileShared.traderTag);
			PrefabManager prefabManager = this.worldBuilder.PrefabManager;
			FastTags<TagGroup.Poi> fastTags2 = fastTags;
			FastTags<TagGroup.Poi> none = FastTags<TagGroup.Poi>.none;
			Vector2i worldPositionCenter = this.WorldPositionCenter;
			PrefabData wildernessPrefab = prefabManager.GetWildernessPrefab(fastTags2, none, default(Vector2i), default(Vector2i), worldPositionCenter, false);
			for (int i = 0; i < 6; i++)
			{
				if (this.spawnWildernessPrefab(wildernessPrefab, gameRandom))
				{
					GameRandomManager.Instance.FreeGameRandom(gameRandom);
					return true;
				}
			}
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
			return false;
		}

		// Token: 0x0600BB39 RID: 47929 RVA: 0x00451620 File Offset: 0x0044F820
		[PublicizedFrom(EAccessModifier.Private)]
		public bool spawnWildernessPrefab(PrefabData prefab, GameRandom rndm)
		{
			int num = ((int)prefab.rotationToFaceNorth + rndm.RandomRange(0, 4)) & 3;
			int num2 = prefab.size.x;
			int num3 = prefab.size.z;
			if (num == 1 || num == 3)
			{
				num2 = prefab.size.z;
				num3 = prefab.size.x;
			}
			Vector2i vector2i;
			if (num2 >= 110 || num3 >= 110)
			{
				vector2i = this.WorldPositionCenter - new Vector2i(num2 / 2, num3 / 2);
				if (num2 > 150 || num3 > 150)
				{
					Log.Warning("RWG spawnWildernessPrefab {0}, overflows TileSize {1}", new object[] { prefab.Name, 150 });
				}
			}
			else
			{
				vector2i.x = this.WorldPosition.x + 20 + rndm.RandomRange(110 - num2);
				vector2i.y = this.WorldPosition.y + 20 + rndm.RandomRange(110 - num3);
			}
			if (vector2i.x < 0 || vector2i.x + num2 > this.worldBuilder.WorldSize)
			{
				return false;
			}
			if (vector2i.y < 0 || vector2i.y + num3 > this.worldBuilder.WorldSize)
			{
				return false;
			}
			Vector2i vector2i2;
			vector2i2.x = vector2i.x + num2 / 2;
			vector2i2.y = vector2i.y + num3 / 2;
			Vector2i vector2i3;
			vector2i3.x = vector2i.x + num2 - 1;
			vector2i3.y = vector2i.y + num3 - 1;
			BiomeType biome = this.worldBuilder.GetBiome(vector2i2.x, vector2i2.y);
			int num4 = Mathf.CeilToInt(this.worldBuilder.GetHeight(vector2i2.x, vector2i2.y));
			List<int> list = new List<int>();
			for (int i = vector2i.y; i < vector2i.y + num3; i++)
			{
				for (int j = vector2i.x; j < vector2i.x + num2; j++)
				{
					if (this.worldBuilder.data.GetWater(j, i) > 0)
					{
						return false;
					}
					if (biome != this.worldBuilder.GetBiome(j, i))
					{
						return false;
					}
					int num5 = Mathf.CeilToInt(this.worldBuilder.GetHeight(j, i));
					if (Utils.FastAbsInt(num5 - num4) > 11)
					{
						return false;
					}
					list.Add(num5);
				}
			}
			num4 = this.getMedianHeight(list);
			if (num4 + prefab.yOffset < 2)
			{
				return false;
			}
			int heightCeil = this.getHeightCeil(vector2i2.x, vector2i2.y);
			Vector3i vector3i = new Vector3i(this.subHalfWorld(vector2i.x), heightCeil, this.subHalfWorld(vector2i.y));
			PrefabManager prefabManager = this.worldBuilder.PrefabManager;
			int prefabInstanceId = prefabManager.PrefabInstanceId;
			prefabManager.PrefabInstanceId = prefabInstanceId + 1;
			int num6 = prefabInstanceId;
			rndm.SetSeed(vector2i.x + vector2i.x * vector2i.y + vector2i.y);
			if (prefab.POIMarkers != null)
			{
				List<Marker> list2 = prefab.RotatePOIMarkers(true, num);
				for (int k = list2.Count - 1; k >= 0; k--)
				{
					if (list2[k].MarkerType != Marker.MarkerTypes.RoadExit)
					{
						list2.RemoveAt(k);
					}
				}
				if (list2.Count > 0)
				{
					int num7 = rndm.RandomRange(0, list2.Count);
					float num8 = (float)Utils.FastMax(list2[num7].size.x, list2[num7].size.z) * 0.5f;
					Vector3i startPos = list2[num7].startPos;
					Vector2 vector = new Vector2((float)startPos.x + (float)list2[num7].size.x / 2f, (float)startPos.z + (float)list2[num7].size.z / 2f);
					Vector2 vector2 = new Vector2((float)vector2i.x + vector.x, (float)vector2i.y + vector.y);
					this.worldBuilder.WildernessPlanner.AddPathInfo(new Vector2i(vector2), num8);
				}
			}
			int num9 = num4 + prefab.yOffset;
			this.SpawnMarkerPartsAndPrefabsWilderness(prefab, new Vector3i(vector2i.x, num9, vector2i.y), (int)((byte)num));
			PrefabDataInstance prefabDataInstance = new PrefabDataInstance(num6, new Vector3i(vector3i.x, num9, vector3i.z), (byte)num, prefab);
			this.AddPrefab(prefabDataInstance);
			this.worldBuilder.WildernessPrefabCount++;
			this.WildernessPOIPos = vector2i;
			this.WildernessPOICenter = vector2i2;
			this.WildernessPOISize.x = num2;
			this.WildernessPOISize.y = num3;
			this.WildernessPOIHeight = num4;
			int num10 = vector2i.x / 10;
			int num11 = vector2i3.x / 10;
			int num12 = vector2i.y / 10;
			int num13 = vector2i3.y / 10;
			int num14 = num10 - 2;
			int num15 = num11 + 2;
			int num16 = num12 - 2;
			int num17 = num13 + 2;
			for (int l = num16; l <= num17; l++)
			{
				if (l >= 0 && l < this.worldBuilder.data.PathTileGridWidth)
				{
					for (int m = num14; m <= num15; m++)
					{
						if (m >= 0 && m < this.worldBuilder.data.PathTileGridWidth)
						{
							if (m >= num10 && m <= num11 && l >= num12 && l <= num13)
							{
								this.worldBuilder.PathingUtils.SetPathBlocked(m, l, true);
							}
							else if (m == num14 || m == num15 || l == num16 || l == num17)
							{
								this.worldBuilder.PathingUtils.SetPathBlocked(m, l, 2);
							}
							else
							{
								this.worldBuilder.PathingUtils.SetPathBlocked(m, l, 5);
							}
						}
					}
				}
			}
			num14 = vector2i.x;
			num15 = vector2i3.x;
			num16 = vector2i.y;
			num17 = vector2i3.y;
			for (int n = num16; n <= num17; n += 150)
			{
				for (int num18 = num14; num18 <= num15; num18 += 150)
				{
					StreetTile streetTileWorld = this.worldBuilder.GetStreetTileWorld(num18, n);
					if (streetTileWorld != null)
					{
						streetTileWorld.Used = true;
					}
				}
			}
			return true;
		}

		// Token: 0x0600BB3A RID: 47930 RVA: 0x00451C53 File Offset: 0x0044FE53
		[PublicizedFrom(EAccessModifier.Private)]
		public void AddPrefab(PrefabDataInstance pdi)
		{
			this.StreetTilePrefabDatas.Add(pdi);
			if (this.Township != null)
			{
				this.Township.AddPrefab(pdi);
				return;
			}
			this.worldBuilder.PrefabManager.AddUsedPrefabWorld(-1, pdi);
		}

		// Token: 0x17001763 RID: 5987
		// (get) Token: 0x0600BB3B RID: 47931 RVA: 0x00451C88 File Offset: 0x0044FE88
		public bool NeedsWildernessSmoothing
		{
			get
			{
				return this.WildernessPOISize.x > 0;
			}
		}

		// Token: 0x0600BB3C RID: 47932 RVA: 0x00451C98 File Offset: 0x0044FE98
		[PublicizedFrom(EAccessModifier.Private)]
		public int getMedianHeight(List<int> heights)
		{
			heights.Sort();
			int count = heights.Count;
			int num = count / 2;
			if (count % 2 == 0)
			{
				return (heights[num] + heights[num - 1]) / 2;
			}
			return heights[num];
		}

		// Token: 0x0600BB3D RID: 47933 RVA: 0x00451CD4 File Offset: 0x0044FED4
		[PublicizedFrom(EAccessModifier.Private)]
		public int getAverageHeight(List<int> heights)
		{
			int num = 0;
			foreach (int num2 in heights)
			{
				num += num2;
			}
			return num / heights.Count;
		}

		// Token: 0x0600BB3E RID: 47934 RVA: 0x00451D2C File Offset: 0x0044FF2C
		[PublicizedFrom(EAccessModifier.Private)]
		public int getHeightCeil(int x, int y)
		{
			return Mathf.CeilToInt(this.worldBuilder.GetHeight(x, y));
		}

		// Token: 0x0600BB3F RID: 47935 RVA: 0x00451D40 File Offset: 0x0044FF40
		[PublicizedFrom(EAccessModifier.Private)]
		public int subHalfWorld(int pos)
		{
			return pos - this.worldBuilder.WorldSize / 2;
		}

		// Token: 0x0600BB40 RID: 47936 RVA: 0x00451D54 File Offset: 0x0044FF54
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int distanceSqr(Vector2i v1, Vector2i v2)
		{
			int num = v1.x - v2.x;
			int num2 = v1.y - v2.y;
			return num * num + num2 * num2;
		}

		// Token: 0x0600BB41 RID: 47937 RVA: 0x00451D84 File Offset: 0x0044FF84
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int distanceSqr(int x1, int y1, int x2, int y2)
		{
			int num = x1 - x2;
			int num2 = y1 - y2;
			return num * num + num2 * num2;
		}

		// Token: 0x0600BB42 RID: 47938 RVA: 0x00451DA0 File Offset: 0x0044FFA0
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float distSqr(Vector2 v1, Vector2 v2)
		{
			float num = v1.x - v2.x;
			float num2 = v1.y - v2.y;
			return num * num + num2 * num2;
		}

		// Token: 0x0600BB43 RID: 47939 RVA: 0x00451DD0 File Offset: 0x0044FFD0
		[PublicizedFrom(EAccessModifier.Private)]
		public void SpawnMarkerPartsAndPrefabs(PrefabData _parentPrefab, Vector3i _parentPosition, int _parentRotations, int _depth, float totalDensityLeft)
		{
			List<Marker> list = _parentPrefab.RotatePOIMarkers(true, _parentRotations);
			if (list.Count == 0)
			{
				return;
			}
			FastTags<TagGroup.Poi> fastTags = FastTags<TagGroup.Poi>.Parse(this.District.name);
			this.worldBuilder.PathingUtils.AddFullyBlockedArea(this.Area);
			Vector3i size = _parentPrefab.size;
			if (_parentRotations % 2 == 1)
			{
				ref int ptr = ref size.z;
				int x = size.x;
				int num = size.z;
				ptr = x;
				size.x = num;
			}
			List<Marker> list2 = list.FindAll((Marker m) => m.MarkerType == Marker.MarkerTypes.POISpawn);
			if (_depth < 5 && list2.Count > 0)
			{
				list2.Sort((Marker m1, Marker m2) => (m2.size.x + m2.size.y + m2.size.z).CompareTo(m1.size.x + m1.size.y + m1.size.z));
				List<string> list3 = new List<string>();
				for (int i = 0; i < list2.Count; i++)
				{
					if (!list3.Contains(list2[i].GroupName))
					{
						list3.Add(list2[i].GroupName);
					}
				}
				this.Township.rand.SetSeed(this.Township.ID + (_parentPosition.x * _parentPosition.x + _parentPosition.y * _parentPosition.y));
				using (List<string>.Enumerator enumerator = list3.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string groupName2 = enumerator.Current;
						List<Marker> list4 = (from m in list2
							where m.GroupName == groupName2
							orderby this.Township.rand.RandomFloat descending
							select m).ToList<Marker>();
						int j = 0;
						while (j < list4.Count)
						{
							Marker marker = list4[j];
							Vector2i vector2i = new Vector2i(marker.size.x, marker.size.z);
							Vector2i vector2i2 = new Vector2i(marker.startPos.x, marker.startPos.z);
							vector2i2 + vector2i;
							Vector2i vector2i3 = vector2i2 + vector2i / 2;
							Vector2i vector2i4 = vector2i;
							if (this.District.spawnCustomSizePrefabs)
							{
								int num2;
								if (this.District.name != "gateway" && (num2 = Marker.MarkerSizes.IndexOf(new Vector3i(vector2i.x, 0, vector2i.y))) >= 0)
								{
									if (num2 > 0)
									{
										vector2i4 = new Vector2i(Marker.MarkerSizes[num2 - 1].x + 1, Marker.MarkerSizes[num2 - 1].z + 1);
									}
								}
								else
								{
									vector2i4 = vector2i / 2;
								}
							}
							Vector2i vector2i5 = new Vector2i(_parentPosition.x + vector2i3.x, _parentPosition.z + vector2i3.y);
							if (_depth == 0)
							{
								int halfWorldSize = this.worldBuilder.HalfWorldSize;
								Vector2 vector = new Vector2((float)(vector2i5.x - halfWorldSize), (float)(vector2i5.y - halfWorldSize));
								float num3 = 0f;
								List<PrefabDataInstance> prefabs = this.Township.Prefabs;
								for (int k = 0; k < prefabs.Count; k++)
								{
									PrefabDataInstance prefabDataInstance = prefabs[k];
									float densityScore = prefabDataInstance.prefab.DensityScore;
									if (densityScore > 6f)
									{
										Vector2 centerXZV = prefabDataInstance.CenterXZV2;
										if (Vector2.Distance(vector, centerXZV) < 190f)
										{
											if (densityScore >= 20f)
											{
												num3 += densityScore * 1.3f;
											}
											else
											{
												num3 += densityScore - 6f;
											}
										}
									}
								}
								if (num3 > 0f)
								{
									totalDensityLeft = Utils.FastMax(6f, totalDensityLeft - num3);
								}
							}
							PrefabData prefabData = this.worldBuilder.PrefabManager.GetPrefabWithDistrict(this.District, marker.Tags, vector2i4, vector2i, vector2i5, totalDensityLeft, 1f);
							if (prefabData != null)
							{
								goto IL_04D8;
							}
							prefabData = this.worldBuilder.PrefabManager.GetPrefabWithDistrict(this.District, marker.Tags, vector2i4, vector2i, vector2i5, totalDensityLeft + 8f, 0.3f);
							if (prefabData != null)
							{
								goto IL_04D8;
							}
							prefabData = this.worldBuilder.PrefabManager.GetPrefabWithDistrict(this.District, marker.Tags, vector2i4, vector2i, vector2i5, 18f, 0f);
							if (prefabData != null)
							{
								Log.Warning("SpawnMarkerPartsAndPrefabs retry2 {0}, tags {1}, size {2} {3}, totalDensityLeft {4}, picked {5}, density {6}", new object[]
								{
									this.District.name,
									marker.Tags,
									vector2i4,
									vector2i,
									totalDensityLeft,
									prefabData.Name,
									prefabData.DensityScore
								});
								goto IL_04D8;
							}
							Log.Warning("SpawnMarkerPartsAndPrefabs failed {0}, tags {1}, size {2} {3}, totalDensityLeft {4}", new object[]
							{
								this.District.name,
								marker.Tags,
								vector2i4,
								vector2i,
								totalDensityLeft
							});
							IL_08A1:
							j++;
							continue;
							IL_04D8:
							int num4 = _parentPosition.x + marker.startPos.x;
							int num5 = _parentPosition.z + marker.startPos.z;
							if (_parentPosition.y + marker.startPos.y + prefabData.yOffset < 3)
							{
								Log.Error("SpawnMarkerPartsAndPrefabs y low! {0}, pos {1} {2}", new object[] { prefabData.Name, num4, num5 });
								goto IL_08A1;
							}
							totalDensityLeft -= prefabData.DensityScore;
							if (prefabData.Tags.Test_AnySet(this.worldBuilder.StreetTileShared.traderTag) || prefabData.Name.Contains("trader"))
							{
								Vector2i vector2i6;
								vector2i6.x = num4 + marker.size.x / 2;
								vector2i6.y = num5 + marker.size.z / 2;
								this.worldBuilder.TraderCenterPositions.Add(vector2i6);
								if (this.BiomeType == BiomeType.forest)
								{
									this.worldBuilder.TraderForestCenterPositions.Add(vector2i6);
								}
								this.HasTrader = true;
								Log.Out("Trader {0}, {1}, {2}, at {3}", new object[]
								{
									prefabData.Name,
									this.BiomeType,
									this.District.name,
									vector2i6
								});
							}
							int num6 = (int)marker.Rotations;
							byte b = (byte)((_parentRotations + (int)prefabData.rotationToFaceNorth + num6) & 3);
							int num7 = prefabData.size.x;
							int num8 = prefabData.size.z;
							int num;
							if (b == 1 || b == 3)
							{
								int num9 = num7;
								num = num8;
								num8 = num9;
								num7 = num;
							}
							if (num6 == 2)
							{
								num4 += vector2i.x / 2 - num7 / 2;
							}
							else if (num6 == 3)
							{
								num5 += vector2i.y / 2 - num8 / 2;
								num4 += vector2i.x;
								num4 -= num7;
							}
							else if (num6 == 0)
							{
								num4 += vector2i.x / 2 - num7 / 2;
								num5 += vector2i.y;
								num5 -= num8;
							}
							else if (num6 == 1)
							{
								num5 += vector2i.y / 2 - num8 / 2;
							}
							Vector3i vector3i = new Vector3i(num4, _parentPosition.y + marker.startPos.y + prefabData.yOffset, num5) + this.worldBuilder.PrefabWorldOffset;
							PrefabManager prefabManager = this.worldBuilder.PrefabManager;
							num = prefabManager.PrefabInstanceId;
							prefabManager.PrefabInstanceId = num + 1;
							PrefabDataInstance prefabDataInstance2 = new PrefabDataInstance(num, vector3i, b, prefabData);
							Color preview_color = this.District.preview_color;
							if (prefabDataInstance2.prefab.Name.StartsWith("remnant_") || prefabDataInstance2.prefab.Name.StartsWith("abandoned_"))
							{
								preview_color.r *= 0.75f;
								preview_color.g *= 0.75f;
								preview_color.b *= 0.75f;
							}
							else if (prefabDataInstance2.prefab.DensityScore < 1f)
							{
								preview_color.r *= 0.4f;
								preview_color.g *= 0.4f;
								preview_color.b *= 0.4f;
							}
							else if (prefabDataInstance2.prefab.Name.StartsWith("trader_"))
							{
								preview_color = new Color(0.6f, 0.3f, 0.3f);
							}
							prefabDataInstance2.previewColor = preview_color;
							this.Township.AddPrefab(prefabDataInstance2);
							this.SpawnMarkerPartsAndPrefabs(prefabData, new Vector3i(num4, _parentPosition.y + marker.startPos.y + prefabData.yOffset, num5), (int)b, _depth + 1, totalDensityLeft);
							break;
						}
					}
				}
			}
			List<Marker> list5 = list.FindAll((Marker m) => m.MarkerType == Marker.MarkerTypes.PartSpawn);
			if (_depth < 20 && list5.Count > 0)
			{
				List<string> list6 = new List<string>();
				for (int l = 0; l < list5.Count; l++)
				{
					if (!list6.Contains(list5[l].GroupName))
					{
						list6.Add(list5[l].GroupName);
					}
				}
				this.Township.rand.SetSeed(this.Township.ID + (_parentPosition.x * _parentPosition.x + _parentPosition.y * _parentPosition.y) + 1);
				using (List<string>.Enumerator enumerator = list6.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string groupName = enumerator.Current;
						List<Marker> list7 = (from m in list5
							where m.GroupName == groupName
							orderby this.Township.rand.RandomFloat descending
							select m).ToList<Marker>();
						float num10 = 1f;
						if (list7.Count > 1)
						{
							num10 = 0f;
							foreach (Marker marker2 in list7)
							{
								num10 += marker2.PartChanceToSpawn;
							}
						}
						float num11 = 0f;
						using (List<Marker>.Enumerator enumerator2 = list7.GetEnumerator())
						{
							IL_0CB4:
							while (enumerator2.MoveNext())
							{
								Marker marker3 = enumerator2.Current;
								num11 += marker3.PartChanceToSpawn / num10;
								if (this.Township.rand.RandomRange(0f, 1f) <= num11)
								{
									if (!marker3.Tags.IsEmpty)
									{
										if (_depth == 0)
										{
											if (!this.District.tag.Test_AnySet(marker3.Tags))
											{
												continue;
											}
										}
										else if (!marker3.Tags.IsEmpty && !fastTags.Test_AnySet(marker3.Tags))
										{
											continue;
										}
									}
									PrefabData prefabByName = this.worldBuilder.PrefabManager.GetPrefabByName(marker3.PartToSpawn);
									if (prefabByName == null)
									{
										Log.Error("Part to spawn {0} not found!", new object[] { marker3.PartToSpawn });
									}
									else
									{
										Vector3i vector3i2 = new Vector3i(_parentPosition.x + marker3.startPos.x - this.worldBuilder.WorldSize / 2, _parentPosition.y + marker3.startPos.y, _parentPosition.z + marker3.startPos.z - this.worldBuilder.WorldSize / 2);
										if (vector3i2.y > 0)
										{
											byte b2 = marker3.Rotations;
											if (b2 == 1)
											{
												b2 = 3;
											}
											else if (b2 == 3)
											{
												b2 = 1;
											}
											byte b3 = (byte)((_parentRotations + (int)prefabByName.rotationToFaceNorth + (int)b2) % 4);
											Vector3i size2 = prefabByName.size;
											if (b3 == 1 || b3 == 3)
											{
												size2 = new Vector3i(size2.z, size2.y, size2.x);
											}
											Bounds bounds = new Bounds(vector3i2 + size2 * 0.5f, size2 - Vector3.one);
											foreach (Bounds bounds2 in this.partBounds)
											{
												if (bounds2.Intersects(bounds))
												{
													goto IL_0CB4;
												}
											}
											Township township = this.Township;
											PrefabManager prefabManager2 = this.worldBuilder.PrefabManager;
											int num = prefabManager2.PrefabInstanceId;
											prefabManager2.PrefabInstanceId = num + 1;
											township.AddPrefab(new PrefabDataInstance(num, vector3i2, b3, prefabByName));
											totalDensityLeft -= prefabByName.DensityScore;
											this.partBounds.Add(bounds);
											this.SpawnMarkerPartsAndPrefabs(prefabByName, _parentPosition + marker3.startPos, (int)b3, _depth + 1, totalDensityLeft);
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			if (this.District != null && this.District.name == "gateway")
			{
				list5 = list.FindAll((Marker m) => m.PartToSpawn.Contains("highway_transition"));
				if (list5.Count > 0)
				{
					foreach (Marker marker4 in list5)
					{
						Vector2 vector2 = new Vector2((float)marker4.startPos.x, (float)marker4.startPos.z) - new Vector2((float)(size.x / 2), (float)(size.z / 2));
						if (Mathf.Abs(vector2.x) > Mathf.Abs(vector2.y))
						{
							if (vector2.x > 0f)
							{
								if (!this.HasExitTo(this.GetNeighbor(Vector2i.right)))
								{
									continue;
								}
								if (this.GetNeighbor(Vector2i.right).Township != this.Township)
								{
									continue;
								}
							}
							else
							{
								if (!this.HasExitTo(this.GetNeighbor(Vector2i.left)))
								{
									continue;
								}
								if (this.GetNeighbor(Vector2i.left).Township != this.Township)
								{
									continue;
								}
							}
						}
						else if (vector2.y > 0f)
						{
							if (!this.HasExitTo(this.GetNeighbor(Vector2i.up)))
							{
								continue;
							}
							if (this.GetNeighbor(Vector2i.up).Township != this.Township)
							{
								continue;
							}
						}
						else if (!this.HasExitTo(this.GetNeighbor(Vector2i.down)) || this.GetNeighbor(Vector2i.down).Township != this.Township)
						{
							continue;
						}
						PrefabData prefabByName2 = this.worldBuilder.PrefabManager.GetPrefabByName(marker4.PartToSpawn);
						if (prefabByName2 != null)
						{
							Vector3i vector3i3 = new Vector3i(_parentPosition.x + marker4.startPos.x - this.worldBuilder.WorldSize / 2, _parentPosition.y + marker4.startPos.y, _parentPosition.z + marker4.startPos.z - this.worldBuilder.WorldSize / 2);
							if (vector3i3.y > 0)
							{
								byte b4 = marker4.Rotations;
								if (b4 == 1)
								{
									b4 = 3;
								}
								else if (b4 == 3)
								{
									b4 = 1;
								}
								byte b5 = (byte)((_parentRotations + (int)prefabByName2.rotationToFaceNorth + (int)b4) % 4);
								Vector3i size3 = prefabByName2.size;
								if (b5 == 1 || b5 == 3)
								{
									size3 = new Vector3i(size3.z, size3.y, size3.x);
								}
								Township township2 = this.Township;
								PrefabManager prefabManager3 = this.worldBuilder.PrefabManager;
								int num = prefabManager3.PrefabInstanceId;
								prefabManager3.PrefabInstanceId = num + 1;
								township2.AddPrefab(new PrefabDataInstance(num, vector3i3, b5, prefabByName2));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600BB44 RID: 47940 RVA: 0x00452E44 File Offset: 0x00451044
		[PublicizedFrom(EAccessModifier.Private)]
		public void SpawnMarkerPartsAndPrefabsWilderness(PrefabData _parentPrefab, Vector3i _parentPosition, int _parentRotations)
		{
			GameRandom gameRandom = GameRandomManager.Instance.CreateGameRandom(_parentPosition.ToString().GetHashCode());
			List<Marker> list = _parentPrefab.RotatePOIMarkers(true, _parentRotations);
			List<Marker> list2 = list.FindAll((Marker m) => m.MarkerType == Marker.MarkerTypes.POISpawn);
			if (list2.Count > 0)
			{
				for (int i = 0; i < list2.Count; i++)
				{
					Marker marker = list2[i];
					Vector2i vector2i = new Vector2i(marker.size.x, marker.size.z);
					Vector2i vector2i2 = new Vector2i(marker.startPos.x, marker.startPos.z) + vector2i / 2;
					Vector2i vector2i3 = vector2i;
					PrefabData wildernessPrefab = this.worldBuilder.PrefabManager.GetWildernessPrefab(this.worldBuilder.StreetTileShared.traderTag, marker.Tags, vector2i3, vector2i, new Vector2i(_parentPosition.x + vector2i2.x, _parentPosition.z + vector2i2.y), false);
					if (wildernessPrefab != null)
					{
						int num = _parentPosition.x + marker.startPos.x;
						int num2 = _parentPosition.z + marker.startPos.z;
						int num3 = (int)marker.Rotations;
						byte b = (byte)((_parentRotations + (int)wildernessPrefab.rotationToFaceNorth + num3) & 3);
						int num4 = wildernessPrefab.size.x;
						int num5 = wildernessPrefab.size.z;
						if (b == 1 || b == 3)
						{
							int num6 = num4;
							num4 = num5;
							num5 = num6;
						}
						if (num3 == 2)
						{
							num += vector2i.x / 2 - num4 / 2;
						}
						else if (num3 == 3)
						{
							num2 += vector2i.y / 2 - num5 / 2;
							num += vector2i.x;
							num -= num4;
						}
						else if (num3 == 0)
						{
							num += vector2i.x / 2 - num4 / 2;
							num2 += vector2i.y;
							num2 -= num5;
						}
						else if (num3 == 1)
						{
							num2 += vector2i.y / 2 - num5 / 2;
						}
						Vector3i vector3i = new Vector3i(num - this.worldBuilder.WorldSize / 2, _parentPosition.y + marker.startPos.y + wildernessPrefab.yOffset, num2 - this.worldBuilder.WorldSize / 2);
						PrefabManager prefabManager = this.worldBuilder.PrefabManager;
						int num7 = prefabManager.PrefabInstanceId;
						prefabManager.PrefabInstanceId = num7 + 1;
						PrefabDataInstance prefabDataInstance = new PrefabDataInstance(num7, vector3i, b, wildernessPrefab);
						this.AddPrefab(prefabDataInstance);
						this.worldBuilder.WildernessPrefabCount++;
						wildernessPrefab.RotatePOIMarkers(true, (int)b);
						this.SpawnMarkerPartsAndPrefabsWilderness(wildernessPrefab, new Vector3i(num, _parentPosition.y + marker.startPos.y + wildernessPrefab.yOffset, num2), (int)b);
					}
				}
			}
			List<Marker> list3 = list.FindAll((Marker m) => m.MarkerType == Marker.MarkerTypes.PartSpawn);
			if (list3.Count > 0)
			{
				List<string> list4 = new List<string>();
				for (int j = 0; j < list3.Count; j++)
				{
					if (!list4.Contains(list3[j].GroupName))
					{
						list4.Add(list3[j].GroupName);
					}
				}
				using (List<string>.Enumerator enumerator = list4.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string groupName = enumerator.Current;
						List<Marker> list5 = list3.FindAll((Marker m) => m.GroupName == groupName);
						float num8 = 1f;
						if (list5.Count > 1)
						{
							num8 = 0f;
							foreach (Marker marker2 in list5)
							{
								num8 += marker2.PartChanceToSpawn;
							}
						}
						float num9 = 0f;
						foreach (Marker marker3 in list5)
						{
							num9 += marker3.PartChanceToSpawn / num8;
							if (gameRandom.RandomRange(0f, 1f) <= num9 && (marker3.Tags.IsEmpty || this.worldBuilder.StreetTileShared.wildernessTag.Test_AnySet(marker3.Tags)))
							{
								PrefabData prefabByName = this.worldBuilder.PrefabManager.GetPrefabByName(marker3.PartToSpawn);
								if (prefabByName != null)
								{
									Vector3i vector3i2 = new Vector3i(_parentPosition.x + marker3.startPos.x - this.worldBuilder.WorldSize / 2, _parentPosition.y + marker3.startPos.y, _parentPosition.z + marker3.startPos.z - this.worldBuilder.WorldSize / 2);
									byte b2 = marker3.Rotations;
									if (b2 == 1)
									{
										b2 = 3;
									}
									else if (b2 == 3)
									{
										b2 = 1;
									}
									byte b3 = (byte)((_parentRotations + (int)prefabByName.rotationToFaceNorth + (int)b2) % 4);
									PrefabManager prefabManager2 = this.worldBuilder.PrefabManager;
									int num7 = prefabManager2.PrefabInstanceId;
									prefabManager2.PrefabInstanceId = num7 + 1;
									PrefabDataInstance prefabDataInstance2 = new PrefabDataInstance(num7, vector3i2, b3, prefabByName);
									this.AddPrefab(prefabDataInstance2);
									this.worldBuilder.WildernessPrefabCount++;
									this.SpawnMarkerPartsAndPrefabsWilderness(prefabByName, _parentPosition + marker3.startPos, (int)b3);
									break;
								}
								Log.Error("Part to spawn {0} not found!", new object[] { marker3.PartToSpawn });
							}
						}
					}
				}
			}
			GameRandomManager.Instance.FreeGameRandom(gameRandom);
		}

		// Token: 0x0600BB45 RID: 47941 RVA: 0x0045345C File Offset: 0x0045165C
		public int GetNumTownshipNeighbors()
		{
			int num = 0;
			StreetTile[] neighbors = this.GetNeighbors();
			for (int i = 0; i < neighbors.Length; i++)
			{
				if (neighbors[i].Township == this.Township)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x04008B9B RID: 35739
		public const int TileSize = 150;

		// Token: 0x04008B9C RID: 35740
		[PublicizedFrom(EAccessModifier.Private)]
		public const int TileSizeHalf = 75;

		// Token: 0x04008B9D RID: 35741
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cDensityRadius = 190f;

		// Token: 0x04008B9E RID: 35742
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cDensityBase = 6f;

		// Token: 0x04008B9F RID: 35743
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cDensityMid = 20f;

		// Token: 0x04008BA0 RID: 35744
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cDensityMidScale = 1.3f;

		// Token: 0x04008BA1 RID: 35745
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cDensityBudget = 62f;

		// Token: 0x04008BA2 RID: 35746
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cDensityRetry = 18f;

		// Token: 0x04008BA3 RID: 35747
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cRadiationEdgeSize = 1;

		// Token: 0x04008BA4 RID: 35748
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cHeightDiffMax = 20f;

		// Token: 0x04008BA5 RID: 35749
		[PublicizedFrom(EAccessModifier.Private)]
		public const int partDepthLimit = 20;

		// Token: 0x04008BA6 RID: 35750
		[PublicizedFrom(EAccessModifier.Private)]
		public const int poiDepthLimit = 5;

		// Token: 0x04008BA7 RID: 35751
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cTilePadInside = 20;

		// Token: 0x04008BA8 RID: 35752
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cTileSizeInside = 110;

		// Token: 0x04008BA9 RID: 35753
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cSmoothFullRadius = 1.8f;

		// Token: 0x04008BAA RID: 35754
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cSmoothFadeRadius = 3.2f;

		// Token: 0x04008BAB RID: 35755
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly WorldBuilder worldBuilder;

		// Token: 0x04008BAC RID: 35756
		public Township Township;

		// Token: 0x04008BAD RID: 35757
		public District District;

		// Token: 0x04008BAE RID: 35758
		public readonly List<PrefabDataInstance> StreetTilePrefabDatas = new List<PrefabDataInstance>();

		// Token: 0x04008BAF RID: 35759
		public readonly Vector2i GridPosition;

		// Token: 0x04008BB0 RID: 35760
		public readonly Vector2i WorldPosition;

		// Token: 0x04008BB1 RID: 35761
		public readonly Vector2i WorldPositionCenter;

		// Token: 0x04008BB2 RID: 35762
		public readonly Rect Area;

		// Token: 0x04008BB3 RID: 35763
		public bool OverlapsRadiation;

		// Token: 0x04008BB4 RID: 35764
		public bool OverlapsBiomes;

		// Token: 0x04008BB5 RID: 35765
		public bool HasSteepSlope;

		// Token: 0x04008BB6 RID: 35766
		public bool AllIsWater;

		// Token: 0x04008BB7 RID: 35767
		public bool HasTrader;

		// Token: 0x04008BB8 RID: 35768
		public bool HasFeature;

		// Token: 0x04008BB9 RID: 35769
		public Vector2i WildernessPOIPos;

		// Token: 0x04008BBA RID: 35770
		public Vector2i WildernessPOICenter;

		// Token: 0x04008BBB RID: 35771
		public Vector2i WildernessPOISize;

		// Token: 0x04008BBC RID: 35772
		public int WildernessPOIHeight;

		// Token: 0x04008BBD RID: 35773
		[PublicizedFrom(EAccessModifier.Private)]
		public Vector2i[] highwayExitPositions;

		// Token: 0x04008BBE RID: 35774
		[PublicizedFrom(EAccessModifier.Private)]
		public int RoadShape;

		// Token: 0x04008BBF RID: 35775
		public int ConnectedExits;

		// Token: 0x04008BC0 RID: 35776
		public readonly List<Vector2i> UsedExitList = new List<Vector2i>();

		// Token: 0x04008BC1 RID: 35777
		public readonly List<Path> ConnectedHighways = new List<Path>();

		// Token: 0x04008BC2 RID: 35778
		[PublicizedFrom(EAccessModifier.Private)]
		public List<Bounds> partBounds = new List<Bounds>();

		// Token: 0x04008BC3 RID: 35779
		[PublicizedFrom(EAccessModifier.Private)]
		public int rotations;

		// Token: 0x04008BC4 RID: 35780
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile[] neighbors4Way;

		// Token: 0x04008BC5 RID: 35781
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile[] neighborsDiagonal;

		// Token: 0x04008BC6 RID: 35782
		[PublicizedFrom(EAccessModifier.Private)]
		public StreetTile[] neighbors8Way;

		// Token: 0x04008BC7 RID: 35783
		public bool Used;

		// Token: 0x04008BC8 RID: 35784
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isFullyBlocked;

		// Token: 0x04008BC9 RID: 35785
		[PublicizedFrom(EAccessModifier.Private)]
		public bool isPartBlocked;

		// Token: 0x04008BCA RID: 35786
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<StreetTile> tempTiles = new List<StreetTile>();

		// Token: 0x04008BCB RID: 35787
		[PublicizedFrom(EAccessModifier.Private)]
		public static List<StreetTile> tempTiles2 = new List<StreetTile>();

		// Token: 0x0200178E RID: 6030
		[PublicizedFrom(EAccessModifier.Private)]
		public enum RoadShapeTypes
		{
			// Token: 0x04008BCD RID: 35789
			straight,
			// Token: 0x04008BCE RID: 35790
			t,
			// Token: 0x04008BCF RID: 35791
			intersection,
			// Token: 0x04008BD0 RID: 35792
			cap,
			// Token: 0x04008BD1 RID: 35793
			corner
		}
	}
}
