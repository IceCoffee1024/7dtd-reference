using System;
using System.IO;
using System.Linq;

// Token: 0x02001492 RID: 5266
public class StockFileHashes
{
	// Token: 0x0600A4FE RID: 42238 RVA: 0x003D669C File Offset: 0x003D489C
	public static bool HasStockXMLs()
	{
		string applicationPath = GameIO.GetApplicationPath();
		bool flag = true;
		foreach (StockFileHashes.HashDef hashDef in StockFileHashes.hashDefinitions)
		{
			if (File.Exists(applicationPath + "/" + hashDef.filename) && !IOUtils.CalcHashSync(applicationPath + "/" + hashDef.filename, "MD5").SequenceEqual<byte>(hashDef.hash))
			{
				Log.Out("Wrong hash on " + hashDef.filename);
				flag = false;
			}
		}
		return flag;
	}

	// Token: 0x04007AC7 RID: 31431
	[PublicizedFrom(EAccessModifier.Private)]
	public static StockFileHashes.HashDef[] hashDefinitions = new StockFileHashes.HashDef[]
	{
		new StockFileHashes.HashDef("Data/Config/archetypes.xml", new byte[]
		{
			129, 149, 214, 213, 160, 126, 29, 253, 240, 10,
			8, 204, 99, 252, 199, 182
		}),
		new StockFileHashes.HashDef("Data/Config/biomes.xml", new byte[]
		{
			194, 85, 165, 41, 188, 8, 172, 220, 66, 3,
			125, 67, 22, 213, 77, 66
		}),
		new StockFileHashes.HashDef("Data/Config/blockplaceholders.xml", new byte[]
		{
			139, 41, 168, 124, 142, 8, 20, 230, 200, 231,
			138, 138, 190, 208, 183, 207
		}),
		new StockFileHashes.HashDef("Data/Config/blocks.xml", new byte[]
		{
			195, 122, 243, 77, 206, 67, 176, 41, 178, 191,
			6, 64, 227, 129, 81, 70
		}),
		new StockFileHashes.HashDef("Data/Config/buffs.xml", new byte[]
		{
			190, 196, 28, 227, 229, 173, 206, 13, 85, 243,
			4, 44, 79, 197, 24, 70
		}),
		new StockFileHashes.HashDef("Data/Config/challenges.xml", new byte[]
		{
			164, 121, 16, 114, 79, 92, 158, 194, 62, 179,
			17, 221, 162, 77, 151, 49
		}),
		new StockFileHashes.HashDef("Data/Config/devicesgameprefs.xml", new byte[]
		{
			196, 158, 114, 144, 153, 61, 251, 22, 157, 95,
			34, 159, 164, 137, 247, 189
		}),
		new StockFileHashes.HashDef("Data/Config/dialogs.xml", new byte[]
		{
			235, 32, 125, 251, 192, 114, 65, 228, 1, 103,
			115, 141, 117, 159, 142, 168
		}),
		new StockFileHashes.HashDef("Data/Config/dmscontent.xml", new byte[]
		{
			146, 234, 236, 139, 101, 159, 66, 55, 76, 140,
			14, 56, 174, 18, 22, 93
		}),
		new StockFileHashes.HashDef("Data/Config/entityclasses.xml", new byte[]
		{
			179, 158, 225, 151, 71, 197, 178, 10, 87, 154,
			129, 22, 58, 148, 186, 231
		}),
		new StockFileHashes.HashDef("Data/Config/entitygroups.xml", new byte[]
		{
			228, 23, 205, 64, 89, 33, 72, 141, 140, 161,
			237, 50, 97, 84, 220, 83
		}),
		new StockFileHashes.HashDef("Data/Config/events.xml", new byte[]
		{
			69, 102, 250, 190, 70, 178, 36, 89, 123, 60,
			95, 204, 74, 75, 181, 17
		}),
		new StockFileHashes.HashDef("Data/Config/gameevents.xml", new byte[]
		{
			137, 149, 183, 187, 108, 34, 8, 114, 253, 47,
			81, 177, 200, 171, 164, 89
		}),
		new StockFileHashes.HashDef("Data/Config/gamestages.xml", new byte[]
		{
			76, 83, 146, 37, 153, byte.MaxValue, 57, 173, 175, 213,
			208, 241, 75, 81, 85, 10
		}),
		new StockFileHashes.HashDef("Data/Config/items.xml", new byte[]
		{
			78, 95, 208, 210, 190, 248, 85, 96, 165, 254,
			250, 214, 28, 42, 95, 162
		}),
		new StockFileHashes.HashDef("Data/Config/item_modifiers.xml", new byte[]
		{
			55, 97, 51, 91, 199, 68, 133, 25, 51, 166,
			46, 109, 61, 45, 107, 151
		}),
		new StockFileHashes.HashDef("Data/Config/loadingscreen.xml", new byte[]
		{
			118, 242, 214, 84, 52, 125, 37, 176, 246, 97,
			93, 35, 202, 234, 92, 228
		}),
		new StockFileHashes.HashDef("Data/Config/loot.xml", new byte[]
		{
			142, 175, 140, 139, 235, 208, 151, 69, 106, 189,
			135, 181, 79, 88, 178, 230
		}),
		new StockFileHashes.HashDef("Data/Config/materials.xml", new byte[]
		{
			128, 141, 183, 99, 200, 240, 79, 65, 81, 66,
			166, 176, 178, 225, 15, 178
		}),
		new StockFileHashes.HashDef("Data/Config/misc.xml", new byte[]
		{
			221, 223, 126, 194, 10, 67, 126, 120, 75, 188,
			233, 72, 150, 146, 41, 136
		}),
		new StockFileHashes.HashDef("Data/Config/music.xml", new byte[]
		{
			81, 85, 5, 120, 112, 228, 213, 50, 7, 73,
			121, 96, 243, 210, 139, 204
		}),
		new StockFileHashes.HashDef("Data/Config/nav_objects.xml", new byte[]
		{
			181, 226, 70, 45, 89, 206, 134, 249, 183, 2,
			53, 181, 134, 213, 105, 81
		}),
		new StockFileHashes.HashDef("Data/Config/npc.xml", new byte[]
		{
			39, 115, 86, 80, 152, 202, 155, 64, 250, 199,
			252, 24, 201, 139, 196, 27
		}),
		new StockFileHashes.HashDef("Data/Config/painting.xml", new byte[]
		{
			149, 174, 31, 175, 31, 238, 140, 122, 184, 224,
			212, 82, 77, 56, 99, 251
		}),
		new StockFileHashes.HashDef("Data/Config/physicsbodies.xml", new byte[]
		{
			123, 240, 13, 225, 25, 233, 87, 207, 179, 189,
			37, 15, 152, 11, 70, 169
		}),
		new StockFileHashes.HashDef("Data/Config/progression.xml", new byte[]
		{
			226, 35, 230, 130, 138, 82, 196, 167, 175, 89,
			43, 119, 196, 244, 133, 90
		}),
		new StockFileHashes.HashDef("Data/Config/qualityinfo.xml", new byte[]
		{
			97, 120, 145, 33, 103, 203, 2, 168, 192, 21,
			25, 195, 68, 215, 251, 211
		}),
		new StockFileHashes.HashDef("Data/Config/quests.xml", new byte[]
		{
			7, 49, 38, 15, 122, 237, 115, 58, 113, 170,
			31, byte.MaxValue, 126, 202, 47, 215
		}),
		new StockFileHashes.HashDef("Data/Config/recipes.xml", new byte[]
		{
			169, 141, 177, 67, 123, 39, 121, 5, 5, 173,
			8, 191, 123, 11, 196, 232
		}),
		new StockFileHashes.HashDef("Data/Config/rwgmixer.xml", new byte[]
		{
			163, 248, 8, 41, 103, 28, 207, 98, 108, 216,
			40, 12, 157, 113, 13, 145
		}),
		new StockFileHashes.HashDef("Data/Config/sandbox_overrides.xml", new byte[]
		{
			21, 211, 79, 241, 118, 82, 75, 217, 111, 77,
			136, 74, 233, 17, 164, 104
		}),
		new StockFileHashes.HashDef("Data/Config/shapes.xml", new byte[]
		{
			217, 222, 243, 93, 36, 150, 57, 2, 1, 225,
			157, 135, 116, 167, 185, 90
		}),
		new StockFileHashes.HashDef("Data/Config/signs.xml", new byte[]
		{
			241, 151, 215, 160, 187, 129, 79, 33, 145, 140,
			116, 42, 155, 195, 126, 16
		}),
		new StockFileHashes.HashDef("Data/Config/sounds.xml", new byte[]
		{
			204, 82, 121, 247, 205, 177, 195, 156, 43, 135,
			6, 47, 251, 186, 205, 87
		}),
		new StockFileHashes.HashDef("Data/Config/spawning.xml", new byte[]
		{
			215, 31, 45, 45, 178, 189, 31, 230, 82, 222,
			125, 217, 126, 39, 107, 11
		}),
		new StockFileHashes.HashDef("Data/Config/subtitles.xml", new byte[]
		{
			43, 226, 138, 234, 5, 182, 101, 56, 134, 12,
			29, 180, 158, 99, 28, 188
		}),
		new StockFileHashes.HashDef("Data/Config/traders.xml", new byte[]
		{
			125, 50, 197, 175, 193, 113, 39, 97, 165, 14,
			107, 174, 169, 112, 193, 160
		}),
		new StockFileHashes.HashDef("Data/Config/twitch.xml", new byte[]
		{
			37, 160, 24, 184, 126, 200, 188, 187, 142, 13,
			183, 134, 206, 174, 38, 110
		}),
		new StockFileHashes.HashDef("Data/Config/twitch_events.xml", new byte[]
		{
			2, 109, 178, 40, 65, 165, 27, 17, 153, 161,
			161, 105, 193, 80, 223, 163
		}),
		new StockFileHashes.HashDef("Data/Config/ui_display.xml", new byte[]
		{
			38, 125, 237, 77, 161, 159, 39, 134, 83, 21,
			78, 240, 100, 244, 50, 217
		}),
		new StockFileHashes.HashDef("Data/Config/utilityai.xml", new byte[]
		{
			214, 167, 177, 22, 147, 156, 88, 129, 207, 113,
			61, 241, 70, 38, 155, 75
		}),
		new StockFileHashes.HashDef("Data/Config/vehicles.xml", new byte[]
		{
			125, 77, 117, 176, 226, 193, 196, 149, 111, 49,
			151, 203, 244, 123, 54, 31
		}),
		new StockFileHashes.HashDef("Data/Config/videos.xml", new byte[]
		{
			111, 28, 117, 204, 29, 196, 6, 21, 182, 82,
			6, 72, 67, 229, 163, 234
		}),
		new StockFileHashes.HashDef("Data/Config/weathersurvival.xml", new byte[]
		{
			151, 245, 70, 52, 118, 190, 215, 193, 5, 166,
			162, 78, 110, 62, 129, 210
		}),
		new StockFileHashes.HashDef("Data/Config/worldglobal.xml", new byte[]
		{
			238, 79, 96, 194, 24, 77, 208, 166, 155, 226,
			190, 215, 20, 216, 134, 75
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_Common/styles.xml", new byte[]
		{
			151, 8, 249, 57, 235, 64, 135, 36, 36, 174,
			246, 19, 115, 202, 254, 239
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_Common/templates.xml", new byte[]
		{
			42, 237, 40, 14, 163, 232, 250, 190, 136, 141,
			12, 55, 179, 82, 143, 201
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/styles.xml", new byte[]
		{
			85, 175, 136, 53, 223, 110, 185, 11, 201, 158,
			162, 237, 134, 74, 144, 186
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/templates.xml", new byte[]
		{
			17, 34, 95, 108, 52, 241, 229, 61, 216, 3,
			64, 223, 233, 67, 124, 111
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/windows.xml", new byte[]
		{
			199, 120, 108, 184, 48, 16, 253, 113, 57, 67,
			159, 39, 132, 82, 108, 45
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/xui.xml", new byte[]
		{
			206, 120, 193, 22, 130, 164, 166, 7, 87, 101,
			119, 37, 207, 234, 248, 30
		})
	};

	// Token: 0x02001493 RID: 5267
	[PublicizedFrom(EAccessModifier.Private)]
	public struct HashDef
	{
		// Token: 0x0600A501 RID: 42241 RVA: 0x003D6E68 File Offset: 0x003D5068
		public HashDef(string _filename, byte[] _hash)
		{
			this.filename = _filename;
			this.hash = _hash;
		}

		// Token: 0x04007AC8 RID: 31432
		public string filename;

		// Token: 0x04007AC9 RID: 31433
		public byte[] hash;
	}
}
