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
	class AutoConnector
	{
		public TextureManager textureManager;
		public ConnectionUI AutoConnect(SkillTreePanel panel, NodeUI a, NodeUI b, ConnectionDirection dir)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset = textureManager.GetDirection(dir);
			if (dir_asset.inactive != null && dir_asset.active != null)
			{
				Vector2 nodeALocation = a.GetLocation();
				Vector2 dir_dimensions = dir_asset.inactive.Size();
				Vector2 a_dimensions = a.inactive_node_image.Size();
				Vector2 b_dimensions = b.inactive_node_image.Size();
				ModContent.GetInstance<Runeforge>().Logger.Info("DATA: " + nodeALocation + " " + dir_dimensions + " " + a_dimensions + " " + b_dimensions + " " + (a.inactive_node_image != null) + " " + (b.inactive_node_image != null));
				Vector2 resulting_location_for_b;
				Vector2 resulting_location_for_dir;
				// assume that A is already placed to their correct spot
				switch (dir)
				{
					case ConnectionDirection.UP:
						resulting_location_for_dir = nodeALocation + new Vector2(MathF.Abs(a_dimensions.X / 2 - dir_dimensions.X / 2), -dir_dimensions.Y);
						resulting_location_for_b = nodeALocation + new Vector2(MathF.Abs(a_dimensions.X / 2 - b_dimensions.X / 2), -dir_dimensions.Y - b_dimensions.Y);
						b.SetLocation(resulting_location_for_b);
						ConnectionUI conn = new ConnectionUI(panel, dir_asset.inactive, dir_asset.active, a, b, dir, resulting_location_for_dir);
						a.AddConnection(conn);
						b.AddConnection(conn);
						return conn;
					case ConnectionDirection.DOWN:
						resulting_location_for_dir = nodeALocation + new Vector2(MathF.Abs(a_dimensions.X / 2 - dir_dimensions.X / 2), a_dimensions.Y);
						resulting_location_for_b = nodeALocation + new Vector2(MathF.Abs(a_dimensions.X / 2 - b_dimensions.X / 2), a_dimensions.Y + dir_dimensions.Y);
						b.SetLocation(resulting_location_for_b);
						ConnectionUI conn2 = new ConnectionUI(panel, dir_asset.inactive, dir_asset.active, a, b, dir, resulting_location_for_dir);
						a.AddConnection(conn2);
						b.AddConnection(conn2);
						return conn2;
					case ConnectionDirection.RIGHT:
						resulting_location_for_dir = nodeALocation + new Vector2(a_dimensions.X, MathF.Abs(a_dimensions.Y / 2 - dir_dimensions.Y / 2));
						resulting_location_for_b = nodeALocation + new Vector2(a_dimensions.X + dir_dimensions.X, MathF.Abs(a_dimensions.Y / 2 - b_dimensions.Y / 2));
						b.SetLocation(resulting_location_for_b);
						ConnectionUI conn3 = new ConnectionUI(panel, dir_asset.inactive, dir_asset.active, a, b, dir, resulting_location_for_dir);
						a.AddConnection(conn3);
						b.AddConnection(conn3);
						return conn3;
				}
			}
			return null;
		}

		public AutoConnector(TextureManager textureManager)
		{
			this.textureManager = textureManager;
		}
	}
}