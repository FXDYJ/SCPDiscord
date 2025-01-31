using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandSystem;

namespace SCPDiscord.Commands
{
	[CommandHandler(typeof (GameConsoleCommandHandler))]
	public class RemoveReservedSlotCommand : ICommand
	{
		public string Command => "scpdiscord_removereservedslot";
		public string[] Aliases => new[] { "scpd_removereservedslot", "scpd_rrs" };
		public string Description => "Removes a player from the reserved slots list.";
		public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
		{
			/*
			if (sender is Player player)
			{
				if (!player.HasPermission("scpdiscord.removereservedslot"))
				{
					return new[] { "You don't have permission to use that command." };
				}
			}
			*/

			if (arguments.Count < 1 || arguments.At(0).Length < 10)
			{
				response = "Invalid arguments.";
				return false;
			}

			bool found = false;
			string steamID = arguments.At(0);
			List<string> reservedSlotsFileRows = File.ReadAllLines(Config.GetReservedSlotPath()).ToList();
			for (int i = 0; i < reservedSlotsFileRows.Count; ++i)
			{
				if (reservedSlotsFileRows[i].Trim().StartsWith(steamID))
				{
					found = true;
					reservedSlotsFileRows.RemoveAt(i);
					--i;

					// Remove SCPDiscord comment if there is one
					if (i >= 0 && reservedSlotsFileRows[i].Trim().StartsWith("# SCPDiscord"))
					{
						reservedSlotsFileRows.RemoveAt(i);
						--i;
					}
				}
			}

			if (found)
			{
				File.WriteAllLines(Config.GetReservedSlotPath(), reservedSlotsFileRows);
				response = "Reserved slot removed.";
				return true;
			}
			else
			{
				response = "Could not find a reserved slot with that Steam ID.";
				return false;
			}
		}
	}
}