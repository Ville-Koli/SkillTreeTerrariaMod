using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Runeforge.Content.Items.Accessories;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Runeforge.Content.Buffs
{
	public class PoisonImbuement : ModBuff
	{
		private const string BUFF_NAME = "Poison imbuement";
		private const string BUFF_TIP = "Imbues your current weapon with poison";
		public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
		{
			base.ModifyBuffText(ref buffName, ref tip, ref rare);
			buffName = BUFF_NAME;
			tip = BUFF_TIP;
		}
	}		
}