using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Runeforge.Content.Buffs;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;

namespace Runeforge.Content.UI
{
	public class TextureManager
	{
		public Dictionary<ConnectionDirection, (Asset<Texture2D> active, Asset<Texture2D> inactive)> direction_textures = new();
		public Dictionary<NodeType, (Asset<Texture2D> active, Asset<Texture2D> inactive)> node_textures = new();

		public void AddNode(NodeType type, Asset<Texture2D> active, Asset<Texture2D> inactive)
		{
			if (node_textures.ContainsKey(type)) return;
			node_textures.Add(type, (active, inactive));
		}

		public void AddDirection(ConnectionDirection dir, Asset<Texture2D> active, Asset<Texture2D> inactive)
		{
			if (direction_textures.ContainsKey(dir)) return;
			direction_textures.Add(dir, (active, inactive));
		}

		public (Asset<Texture2D> active, Asset<Texture2D> inactive) GetNode(NodeType type)
		{
			if (node_textures.TryGetValue(type, out (Asset<Texture2D> active, Asset<Texture2D> inactive) tuple))
			{
				return tuple;
			}
			return (null, null);
		}
		public (Asset<Texture2D> active, Asset<Texture2D> inactive) GetDirection(ConnectionDirection dir)
		{
			if (direction_textures.TryGetValue(dir, out (Asset<Texture2D> active, Asset<Texture2D> inactive) tuple))
			{
				return tuple;
			}
			return (null, null);
		}
	}
}