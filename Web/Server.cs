using System;
using Nancy;
using SteamKit2;

namespace SendMessage {
	public class MessageModule : Nancy.NancyModule
	{
		public MessageModule ()
		{
			Get["/"] = _ => "Hello World";

			// Add a friend
			Post["/friends/{id}"] = parameters => {
				SteamID steamId = new SteamID();
				steamId.SetFromString( (string)parameters.id, Steam3.SteamClient.ConnectedUniverse );
				Console.WriteLine ((string)parameters.id);
				var response = new Response();
				if ( steamId.IsValid )
				{
					Steam3.SteamFriends.AddFriend( steamId );
					response.StatusCode = HttpStatusCode.OK;
				}
				else
				{
					response.StatusCode = HttpStatusCode.BadRequest;
				}
				response.ContentType = "application/json";
				return response;
			};
			// Send friend a message
			Post["/friends/{id}/message"] = parameters => {
				SteamID steamId = new SteamID( (ulong)parameters.id );
				var response = new Response();
				var msg = (string)Request.Form.Message; 
				if ( steamId.IsValid && !String.IsNullOrEmpty(msg) )
				{
					EChatEntryType type = EChatEntryType.ChatMsg;
					Steam3.SteamFriends.SendChatMessage( steamId, type, msg );
					response.StatusCode = HttpStatusCode.OK;
				}
				else
				{
					response.StatusCode = HttpStatusCode.BadRequest;
				}
				response.ContentType = "application/json";
				return response;
			};
		}
	}
}