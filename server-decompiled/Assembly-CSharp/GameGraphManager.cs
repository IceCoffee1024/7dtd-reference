using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200136E RID: 4974
public class GameGraphManager
{
	// Token: 0x06009C8A RID: 40074 RVA: 0x003B1D92 File Offset: 0x003AFF92
	public static GameGraphManager Create(EntityPlayerLocal player)
	{
		GameGraphManager gameGraphManager = new GameGraphManager();
		gameGraphManager.player = player;
		gameGraphManager.Init();
		return gameGraphManager;
	}

	// Token: 0x06009C8B RID: 40075 RVA: 0x003B1DA6 File Offset: 0x003AFFA6
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		if (!GameGraphManager.whiteTex)
		{
			GameGraphManager.whiteTex = new Texture2D(1, 1);
			GameGraphManager.whiteTex.FillTexture(Color.white, true, false);
		}
	}

	// Token: 0x06009C8C RID: 40076 RVA: 0x003B1DD1 File Offset: 0x003AFFD1
	public void Destroy()
	{
		if (GameGraphManager.whiteTex)
		{
			global::UnityEngine.Object.Destroy(GameGraphManager.whiteTex);
		}
	}

	// Token: 0x06009C8D RID: 40077 RVA: 0x003B1DEC File Offset: 0x003AFFEC
	public void Add(string name, GameGraphManager.Graph.Callback callback, int sampleCount, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (sampleCount > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, sampleCount, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.callback = callback;
		}
	}

	// Token: 0x06009C8E RID: 40078 RVA: 0x003B1E38 File Offset: 0x003B0038
	public void AddCVar(string name, int count, string cvarName, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.cvarName = cvarName;
		}
	}

	// Token: 0x06009C8F RID: 40079 RVA: 0x003B1E84 File Offset: 0x003B0084
	public void AddPassiveEffect(string name, int count, PassiveEffects passiveEffect, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.passiveEffect = passiveEffect;
		}
	}

	// Token: 0x06009C90 RID: 40080 RVA: 0x003B1ED0 File Offset: 0x003B00D0
	public void AddStat(string name, int count, string statName, float maxValue, float markerValue = 0f)
	{
		GameGraphManager.Graph graph = this.FindGraph(name);
		if (graph != null)
		{
			this.graphs.Remove(graph);
		}
		if (count > 0)
		{
			GameGraphManager.Graph graph2 = new GameGraphManager.Graph(this, name, count, maxValue, markerValue);
			this.graphs.Add(graph2);
			graph2.statName = statName.ToLower();
		}
	}

	// Token: 0x06009C91 RID: 40081 RVA: 0x003B1F1E File Offset: 0x003B011E
	public void RemoveAll()
	{
		this.graphs.Clear();
	}

	// Token: 0x06009C92 RID: 40082 RVA: 0x003B1F2C File Offset: 0x003B012C
	public GameGraphManager.Graph FindGraph(string name)
	{
		for (int i = 0; i < this.graphs.Count; i++)
		{
			GameGraphManager.Graph graph = this.graphs[i];
			if (graph.name == name)
			{
				return graph;
			}
		}
		return null;
	}

	// Token: 0x06009C93 RID: 40083 RVA: 0x003B1F70 File Offset: 0x003B0170
	public void Draw()
	{
		bool flag = Event.current.type == EventType.Repaint;
		float num = 1f;
		for (int i = 0; i < this.graphs.Count; i++)
		{
			GameGraphManager.Graph graph = this.graphs[i];
			if (flag)
			{
				graph.UpdateValues();
			}
			graph.pos.x = 2f;
			graph.pos.y = num;
			graph.Draw();
			num += (float)(this.graphHeight + 2);
		}
	}

	// Token: 0x06009C94 RID: 40084 RVA: 0x003B1FEB File Offset: 0x003B01EB
	public void SetHeight(int _height)
	{
		this.graphHeight = _height;
		this.graphHeight = Mathf.Clamp(this.graphHeight, 1, 2100);
	}

	// Token: 0x040075FC RID: 30204
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D whiteTex;

	// Token: 0x040075FD RID: 30205
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040075FE RID: 30206
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameGraphManager.Graph> graphs = new List<GameGraphManager.Graph>();

	// Token: 0x040075FF RID: 30207
	[PublicizedFrom(EAccessModifier.Private)]
	public int graphHeight = 100;

	// Token: 0x0200136F RID: 4975
	public class Graph
	{
		// Token: 0x06009C96 RID: 40086 RVA: 0x003B2028 File Offset: 0x003B0228
		public Graph(GameGraphManager _manager, string _name, int _count, float _maxValue, float _markerValue)
		{
			this.manager = _manager;
			this.name = _name;
			this.count = _count;
			this.count = Mathf.Clamp(this.count, 1, 4096);
			this.maxValue = _maxValue;
			this.markerValue = _markerValue;
			this.values = new float[this.count];
		}

		// Token: 0x06009C97 RID: 40087 RVA: 0x003B2088 File Offset: 0x003B0288
		public void AddValue(float value)
		{
			this.index = (this.index + 1) % this.count;
			this.values[this.index] = value;
		}

		// Token: 0x06009C98 RID: 40088 RVA: 0x003B20B0 File Offset: 0x003B02B0
		public void Draw()
		{
			Texture whiteTex = GameGraphManager.whiteTex;
			float num = (float)this.count * 2f + 2f;
			int graphHeight = this.manager.graphHeight;
			GUI.color = Color.white;
			GUI.DrawTexture(new Rect(this.pos.x, this.pos.y, num, (float)(graphHeight + 2)), whiteTex, ScaleMode.StretchToFill, false, 0f, new Color(0f, 0f, 0f, 0.9f), 0f, 0f);
			int num2 = this.index + 1;
			for (int i = 0; i < this.count; i++)
			{
				float num3 = this.values[num2 % this.count];
				num3 /= this.maxValue;
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				float num4 = (float)graphHeight * num3;
				num4 = (float)((int)(num4 + 0.5f));
				Color color = new Color(1f, num3, num3 * 0.6f + 0.4f);
				GUI.DrawTexture(new Rect(this.pos.x + 1f + (float)i * 2f, this.pos.y + 1f + (float)graphHeight - num4, 2f, num4), whiteTex, ScaleMode.StretchToFill, false, 0f, color, 0f, 0f);
				num2++;
			}
			if (this.markerValue > 0f)
			{
				GUI.DrawTexture(new Rect(this.pos.x, this.pos.y + (float)graphHeight - this.markerValue / this.maxValue * (float)graphHeight, num, 1f), whiteTex, ScaleMode.StretchToFill, true, 0f, new Color(1f, 1f, 0f, 0.6f), 0f, 0f);
			}
			GUI.color = new Color(0.6f, 0.6f, 1f);
			GUI.Label(new Rect(this.pos.x + 1f, this.pos.y + 1f, 256f, 256f), string.Format("{0} {1}", this.name, this.values[this.index]));
		}

		// Token: 0x06009C99 RID: 40089 RVA: 0x003B22FC File Offset: 0x003B04FC
		public void UpdateValues()
		{
			if (GameManager.Instance.World == null)
			{
				return;
			}
			EntityPlayerLocal player = this.manager.player;
			if (this.callback != null)
			{
				float num = this.values[this.index];
				if (this.callback(ref num))
				{
					this.AddValue(num);
					return;
				}
			}
			else if (!string.IsNullOrEmpty(this.cvarName))
			{
				float cvar = player.GetCVar(this.cvarName);
				if (cvar != this.values[this.index])
				{
					this.AddValue(cvar);
					return;
				}
			}
			else if (this.passiveEffect != PassiveEffects.None)
			{
				float value = EffectManager.GetValue(this.passiveEffect, null, 0f, player, null, default(FastTags<TagGroup.Global>), true, true, true, true, true, 1, true, false);
				if (value != this.values[this.index])
				{
					this.AddValue(value);
					return;
				}
			}
			else if (!string.IsNullOrEmpty(this.statName))
			{
				float num2 = 0f;
				string text = this.statName;
				if (!(text == "health"))
				{
					if (!(text == "stamina"))
					{
						if (!(text == "coretemp"))
						{
							if (text == "water")
							{
								num2 = player.PlayerStats.Water.Value;
							}
						}
						else
						{
							num2 = player.PlayerStats.CoreTemp;
						}
					}
					else
					{
						num2 = player.PlayerStats.Stamina.Value;
					}
				}
				else
				{
					num2 = player.PlayerStats.Health.Value;
				}
				if (num2 != this.values[this.index])
				{
					this.AddValue(num2);
				}
			}
		}

		// Token: 0x04007600 RID: 30208
		public string name;

		// Token: 0x04007601 RID: 30209
		public GameGraphManager.Graph.Callback callback;

		// Token: 0x04007602 RID: 30210
		public string cvarName;

		// Token: 0x04007603 RID: 30211
		public PassiveEffects passiveEffect;

		// Token: 0x04007604 RID: 30212
		public string statName;

		// Token: 0x04007605 RID: 30213
		public Vector2 pos;

		// Token: 0x04007606 RID: 30214
		[PublicizedFrom(EAccessModifier.Private)]
		public GameGraphManager manager;

		// Token: 0x04007607 RID: 30215
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x04007608 RID: 30216
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxValue;

		// Token: 0x04007609 RID: 30217
		[PublicizedFrom(EAccessModifier.Private)]
		public float markerValue;

		// Token: 0x0400760A RID: 30218
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] values;

		// Token: 0x0400760B RID: 30219
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x02001370 RID: 4976
		// (Invoke) Token: 0x06009C9B RID: 40091
		public delegate bool Callback(ref float value);
	}
}
