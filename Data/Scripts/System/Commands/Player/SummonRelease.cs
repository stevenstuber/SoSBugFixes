using System;
using Server;
using System.Collections;
using System.Collections.Generic;
using Server.Misc;
using Server.Items;
using Server.Network;
using Server.Commands;
using Server.Commands.Generic;
using Server.Mobiles;
using Server.Accounting;
using Server.Regions;

namespace Server.Misc
{
    class SummonRelease
    {
		public static void Initialize()
		{
            CommandSystem.Register( "releasemagicalsummons", AccessLevel.Player, new CommandEventHandler( SummonRelease_OnCommand ) );
		}
		public static void Register( string command, AccessLevel access, CommandEventHandler handler )
		{
            CommandSystem.Register(command, access, handler);
		}

		[Usage( "releasemagicalsummons" )]
		[Description( "Releases any of your magical summons in the land." )]
		public static void SummonRelease_OnCommand( CommandEventArgs e )
        {
			Mobile from = e.Mobile;

			if (!from.Alive)
			{
				from.SendMessage("You are dead and cannot do that!");
				return;
			}

			if ( Core.SE && from.AllFollowers.Count > 0 )
			{
				for ( int i = from.AllFollowers.Count - 1; i >= 0; --i )
				{
					BaseCreature pet = from.AllFollowers[i] as BaseCreature;

					if (pet == null || pet.ControlMaster == null)
						continue;

					if (pet.Summoned)
					{
						pet.PlaySound( pet.GetAngerSound() );
						Timer.DelayCall( TimeSpan.Zero, new TimerCallback( pet.Delete ) );
					}
				}
			}

			from.SendMessage("Your summons have been released.");
		}
	}
}