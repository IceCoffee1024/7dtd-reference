using System;
using System.IO;
using System.Linq;

// Token: 0x020014CD RID: 5325
public class StockFileHashes
{
	// Token: 0x0600A6A9 RID: 42665 RVA: 0x003E06A4 File Offset: 0x003DE8A4
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

	// Token: 0x04007C1B RID: 31771
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
			58, 23, 157, 232, 155, 221, 154, 141, 129, 28,
			21, 138, 65, 73, 79, 193
		}),
		new StockFileHashes.HashDef("Data/Config/buffs.xml", new byte[]
		{
			64, 103, 120, 112, 87, 137, 100, 173, 19, 65,
			207, 31, 89, 102, 46, 131
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
			213, 203, 54, 234, 194, 214, 188, 149, 127, 34,
			81, 240, 76, 215, 49, 147
		}),
		new StockFileHashes.HashDef("Data/Config/entitygroups.xml", new byte[]
		{
			240, 188, 156, 135, 68, 154, 217, 111, 209, 126,
			54, 85, 160, 39, 154, 228
		}),
		new StockFileHashes.HashDef("Data/Config/events.xml", new byte[]
		{
			69, 102, 250, 190, 70, 178, 36, 89, 123, 60,
			95, 204, 74, 75, 181, 17
		}),
		new StockFileHashes.HashDef("Data/Config/gameevents.xml", new byte[]
		{
			93, 117, 27, 185, 193, 85, 77, 133, 106, 42,
			49, 146, 139, 216, 8, 198
		}),
		new StockFileHashes.HashDef("Data/Config/gamestages.xml", new byte[]
		{
			76, 83, 146, 37, 153, byte.MaxValue, 57, 173, 175, 213,
			208, 241, 75, 81, 85, 10
		}),
		new StockFileHashes.HashDef("Data/Config/items.xml", new byte[]
		{
			206, 59, 17, 219, 191, 163, 11, 124, 242, 157,
			79, 25, 87, 236, 10, 241
		}),
		new StockFileHashes.HashDef("Data/Config/item_modifiers.xml", new byte[]
		{
			55, 97, 51, 91, 199, 68, 133, 25, 51, 166,
			46, 109, 61, 45, 107, 151
		}),
		new StockFileHashes.HashDef("Data/Config/loadingscreen.xml", new byte[]
		{
			208, 45, 145, 77, 51, 50, 231, 175, 26, 247,
			106, 197, 251, 82, 209, 80
		}),
		new StockFileHashes.HashDef("Data/Config/loot.xml", new byte[]
		{
			246, 7, 23, 114, 93, 77, 73, byte.MaxValue, 119, 29,
			200, 155, 101, 60, 179, 209
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
			35, 166, 202, 202, 89, 51, 47, 108, 146, 110,
			122, 43, 170, 142, 226, 60
		}),
		new StockFileHashes.HashDef("Data/Config/qualityinfo.xml", new byte[]
		{
			97, 120, 145, 33, 103, 203, 2, 168, 192, 21,
			25, 195, 68, 215, 251, 211
		}),
		new StockFileHashes.HashDef("Data/Config/quests.xml", new byte[]
		{
			84, 105, 216, 120, 249, 78, 49, 224, 77, 234,
			59, 162, 85, 199, 118, 48
		}),
		new StockFileHashes.HashDef("Data/Config/recipes.xml", new byte[]
		{
			210, 19, 189, 244, 2, 147, 103, 138, 57, 193,
			64, 212, 223, 158, 245, 124
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
			142, 214, 176, 117, 118, 101, 6, 54, 251, 103,
			186, 159, 47, 22, 220, 199
		}),
		new StockFileHashes.HashDef("Data/Config/spawning.xml", new byte[]
		{
			211, 219, 214, 36, 20, 216, 165, 160, 52, 84,
			156, 218, 206, 73, 88, 167
		}),
		new StockFileHashes.HashDef("Data/Config/subtitles.xml", new byte[]
		{
			43, 226, 138, 234, 5, 182, 101, 56, 134, 12,
			29, 180, 158, 99, 28, 188
		}),
		new StockFileHashes.HashDef("Data/Config/toolbelt.xml", new byte[]
		{
			117, 41, 102, 69, 205, 41, 142, 197, 114, 148,
			224, 164, 66, 87, 159, 1
		}),
		new StockFileHashes.HashDef("Data/Config/traders.xml", new byte[]
		{
			134, 196, 123, 118, 109, 251, 22, 46, 1, 174,
			5, 145, 215, byte.MaxValue, 229, 192
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
			22, 167, 175, 11, 46, 230, 248, 229, 83, 159,
			64, 189, 156, 102, 116, 116
		}),
		new StockFileHashes.HashDef("Data/Config/utilityai.xml", new byte[]
		{
			214, 167, 177, 22, 147, 156, 88, 129, 207, 113,
			61, 241, 70, 38, 155, 75
		}),
		new StockFileHashes.HashDef("Data/Config/vehicles.xml", new byte[]
		{
			108, 137, 208, 51, 201, 85, 185, 9, 230, 94,
			52, 130, 105, 1, 82, 111
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
			125, 85, 245, 192, 147, 247, 209, 171, 128, 123,
			56, 172, 146, 175, 252, 33
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_Common/templates.xml", new byte[]
		{
			6, 76, 172, 58, 64, byte.MaxValue, 8, 229, 106, 247,
			116, 221, 21, 59, 160, 45
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/styles.xml", new byte[]
		{
			85, 175, 136, 53, 223, 110, 185, 11, 201, 158,
			162, 237, 134, 74, 144, 186
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/templates.xml", new byte[]
		{
			225, 73, 105, 228, 146, 83, 13, 131, 134, 247,
			233, 45, 217, 58, 183, 235
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/windows.xml", new byte[]
		{
			96, 229, 44, 205, 58, 104, 226, 226, 206, 127,
			80, 65, 14, 249, 190, 25
		}),
		new StockFileHashes.HashDef("Data/Config/XUi_InGame/xui.xml", new byte[]
		{
			236, 112, 225, 29, 48, 206, 188, 115, 126, 216,
			209, 232, 25, 50, 84, 129
		})
	};

	// Token: 0x020014CE RID: 5326
	[PublicizedFrom(EAccessModifier.Private)]
	public struct HashDef
	{
		// Token: 0x0600A6AC RID: 42668 RVA: 0x003E0E94 File Offset: 0x003DF094
		public HashDef(string _filename, byte[] _hash)
		{
			this.filename = _filename;
			this.hash = _hash;
		}

		// Token: 0x04007C1C RID: 31772
		public string filename;

		// Token: 0x04007C1D RID: 31773
		public byte[] hash;
	}
}
