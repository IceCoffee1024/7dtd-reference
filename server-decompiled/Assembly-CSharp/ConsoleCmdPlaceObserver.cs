using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000263 RID: 611
[Preserve]
public class ConsoleCmdPlaceObserver : ConsoleCmdAbstract
{
	// Token: 0x0600120A RID: 4618 RVA: 0x00070E8F File Offset: 0x0006F08F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Place a chunk observer on a given position.";
	}

	// Token: 0x0600120B RID: 4619 RVA: 0x00070E96 File Offset: 0x0006F096
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Usage:\n  1. chunkobserver add <x> <z> [size]\n  2. chunkobserver remove <x> <z>\n  3. chunkobserver list\n1. Place an observer on the chunk that contains the coordinate x/z.\n   Optionally specifying the box radius in chunks, defaulting to 1.\n2. Remove the observer from the chunk with the coordinate, if any.\n3. List all currently placed observers";
	}

	// Token: 0x0600120C RID: 4620 RVA: 0x00070E9D File Offset: 0x0006F09D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "chunkobserver", "co" };
	}

	// Token: 0x0600120D RID: 4621 RVA: 0x00070EB8 File Offset: 0x0006F0B8
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 1 && _params[0].EqualsCaseInsensitive("list"))
		{
			this.listObservers();
			return;
		}
		if ((!_params[0].EqualsCaseInsensitive("add") || (_params.Count != 3 && _params.Count != 4)) && (!_params[0].EqualsCaseInsensitive("remove") || _params.Count != 3))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Illegal arguments");
			return;
		}
		int num;
		if (!int.TryParse(_params[1], out num))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given x coordinate is not a valid integer");
			return;
		}
		int num2;
		if (!int.TryParse(_params[2], out num2))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given z coordinate is not a valid integer");
			return;
		}
		int num3;
		if (_params.Count == 4)
		{
			if (!int.TryParse(_params[3], out num3) || num3 < 1 || num3 > 15)
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("The given size is not a valid integer or exceeds the allowed range of 1-15");
				return;
			}
		}
		else
		{
			num3 = 1;
		}
		Vector2i vector2i = new Vector2i(num, num2);
		if (_params[0].EqualsCaseInsensitive("remove"))
		{
			if (this.removeObserver(vector2i))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Observer removed from " + vector2i.ToString());
				return;
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No observer on " + vector2i.ToString());
			return;
		}
		else
		{
			if (this.addObserver(vector2i, num3))
			{
				int num4 = 2 * (num3 - 1) + 1;
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
				{
					"Observer added to ",
					vector2i.ToString(),
					" with radius ",
					num3.ToString(),
					" (size ",
					num4.ToString(),
					"x",
					num4.ToString(),
					")"
				}));
				return;
			}
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Already an observer on " + vector2i.ToString());
			return;
		}
	}

	// Token: 0x0600120E RID: 4622 RVA: 0x000710C0 File Offset: 0x0006F2C0
	[PublicizedFrom(EAccessModifier.Private)]
	public bool addObserver(Vector2i _pos, int _radius)
	{
		if (this.observers.dict.ContainsKey(_pos))
		{
			return false;
		}
		Vector3 vector = new Vector3((float)_pos.x, 0f, (float)_pos.y);
		ChunkManager.ChunkObserver chunkObserver = GameManager.Instance.AddChunkObserver(vector, false, _radius, -1);
		this.observers.Add(_pos, chunkObserver);
		return true;
	}

	// Token: 0x0600120F RID: 4623 RVA: 0x0007111C File Offset: 0x0006F31C
	[PublicizedFrom(EAccessModifier.Private)]
	public bool removeObserver(Vector2i _pos)
	{
		if (!this.observers.dict.ContainsKey(_pos))
		{
			return false;
		}
		ChunkManager.ChunkObserver chunkObserver = this.observers.dict[_pos];
		GameManager.Instance.RemoveChunkObserver(chunkObserver);
		this.observers.Remove(_pos);
		return true;
	}

	// Token: 0x06001210 RID: 4624 RVA: 0x0007116C File Offset: 0x0006F36C
	[PublicizedFrom(EAccessModifier.Private)]
	public void listObservers()
	{
		int num = 0;
		foreach (KeyValuePair<Vector2i, ChunkManager.ChunkObserver> keyValuePair in this.observers.dict)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format(" {0,3}: {1}", ++num, keyValuePair.Key.ToString()));
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(num.ToString() + " observers");
	}

	// Token: 0x04000D03 RID: 3331
	[PublicizedFrom(EAccessModifier.Private)]
	public MapVisitor mapVisitor;

	// Token: 0x04000D04 RID: 3332
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly DictionaryList<Vector2i, ChunkManager.ChunkObserver> observers = new DictionaryList<Vector2i, ChunkManager.ChunkObserver>();
}
