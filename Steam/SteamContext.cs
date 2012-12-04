using System;
using System.Net;
using System.Net.Sockets;
using SteamKit2;

namespace SendMessage
{
	class Steam3
	{
		public static SteamClient SteamClient { get; private set; }
		
		public static SteamFriends SteamFriends { get; private set; }
		public static SteamUser SteamUser { get; private set; }
		
		static Steam3 ()
		{
			Initialize ( false );
		}
		public static void Initialize( bool useUdp )
		{
			Console.WriteLine ("Server now running on: ");

			SteamClient = new SteamClient( useUdp ? ProtocolType.Udp : ProtocolType.Tcp );
			
			SteamFriends = SteamClient.GetHandler<SteamFriends>();
			SteamUser = SteamClient.GetHandler<SteamUser>();
			
		}

	}
}
