using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200135C RID: 4956
public class GameGraphManager
{
	// Token: 0x06009C32 RID: 39986 RVA: 0x003AF98E File Offset: 0x003ADB8E
	public static GameGraphManager Create(EntityPlayerLocal player)
	{
		GameGraphManager gameGraphManager = new GameGraphManager();
		gameGraphManager.player = player;
		gameGraphManager.Init();
		return gameGraphManager;
	}

	// Token: 0x06009C33 RID: 39987 RVA: 0x003AF9A2 File Offset: 0x003ADBA2
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		if (!GameGraphManager.whiteTex)
		{
			GameGraphManager.whiteTex = new Texture2D(1, 1);
			GameGraphManager.whiteTex.FillTexture(Color.white, true, false);
		}
	}

	// Token: 0x06009C34 RID: 39988 RVA: 0x003AF9CD File Offset: 0x003ADBCD
	public void Destroy()
	{
		if (GameGraphManager.whiteTex)
		{
			global::UnityEngine.Object.Destroy(GameGraphManager.whiteTex);
		}
	}

	// Token: 0x06009C35 RID: 39989 RVA: 0x003AF9E8 File Offset: 0x003ADBE8
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

	// Token: 0x06009C36 RID: 39990 RVA: 0x003AFA34 File Offset: 0x003ADC34
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

	// Token: 0x06009C37 RID: 39991 RVA: 0x003AFA80 File Offset: 0x003ADC80
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

	// Token: 0x06009C38 RID: 39992 RVA: 0x003AFACC File Offset: 0x003ADCCC
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

	// Token: 0x06009C39 RID: 39993 RVA: 0x003AFB1A File Offset: 0x003ADD1A
	public void RemoveAll()
	{
		this.graphs.Clear();
	}

	// Token: 0x06009C3A RID: 39994 RVA: 0x003AFB28 File Offset: 0x003ADD28
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

	// Token: 0x06009C3B RID: 39995 RVA: 0x003AFB6C File Offset: 0x003ADD6C
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

	// Token: 0x06009C3C RID: 39996 RVA: 0x003AFBE7 File Offset: 0x003ADDE7
	public void SetHeight(int _height)
	{
		this.graphHeight = _height;
		this.graphHeight = Mathf.Clamp(this.graphHeight, 1, 2100);
	}

	// Token: 0x040075BB RID: 30139
	[PublicizedFrom(EAccessModifier.Private)]
	public static Texture2D whiteTex;

	// Token: 0x040075BC RID: 30140
	[PublicizedFrom(EAccessModifier.Private)]
	public EntityPlayerLocal player;

	// Token: 0x040075BD RID: 30141
	[PublicizedFrom(EAccessModifier.Private)]
	public List<GameGraphManager.Graph> graphs = new List<GameGraphManager.Graph>();

	// Token: 0x040075BE RID: 30142
	[PublicizedFrom(EAccessModifier.Private)]
	public int graphHeight = 100;

	// Token: 0x0200135D RID: 4957
	public class Graph
	{
		// Token: 0x06009C3E RID: 39998 RVA: 0x003AFC24 File Offset: 0x003ADE24
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

		// Token: 0x06009C3F RID: 39999 RVA: 0x003AFC84 File Offset: 0x003ADE84
		public void AddValue(float value)
		{
			this.index = (this.index + 1) % this.count;
			this.values[this.index] = value;
		}

		// Token: 0x06009C40 RID: 40000 RVA: 0x003AFCAC File Offset: 0x003ADEAC
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

		// Token: 0x06009C41 RID: 40001 RVA: 0x003AFEF8 File Offset: 0x003AE0F8
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

		// Token: 0x040075BF RID: 30143
		public string name;

		// Token: 0x040075C0 RID: 30144
		public GameGraphManager.Graph.Callback callback;

		// Token: 0x040075C1 RID: 30145
		public string cvarName;

		// Token: 0x040075C2 RID: 30146
		public PassiveEffects passiveEffect;

		// Token: 0x040075C3 RID: 30147
		public string statName;

		// Token: 0x040075C4 RID: 30148
		public Vector2 pos;

		// Token: 0x040075C5 RID: 30149
		[PublicizedFrom(EAccessModifier.Private)]
		public GameGraphManager manager;

		// Token: 0x040075C6 RID: 30150
		[PublicizedFrom(EAccessModifier.Private)]
		public int count;

		// Token: 0x040075C7 RID: 30151
		[PublicizedFrom(EAccessModifier.Private)]
		public float maxValue;

		// Token: 0x040075C8 RID: 30152
		[PublicizedFrom(EAccessModifier.Private)]
		public float markerValue;

		// Token: 0x040075C9 RID: 30153
		[PublicizedFrom(EAccessModifier.Private)]
		public float[] values;

		// Token: 0x040075CA RID: 30154
		[PublicizedFrom(EAccessModifier.Private)]
		public int index;

		// Token: 0x0200135E RID: 4958
		// (Invoke) Token: 0x06009C43 RID: 40003
		public delegate bool Callback(ref float value);
	}
}
