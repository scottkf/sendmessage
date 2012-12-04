using System;
using System.Collections.Generic;
using System.Text;
using SteamKit2;

namespace SendMessage
{
    class Friend
    {
		public static SteamClient SteamClient { get; private set; }
		
		public static SteamFriends SteamFriends { get; private set; }
		public static SteamUser SteamUser { get; private set; }

		public ulong SteamID { get; set; }

        public Friend()
        {
        }
        public Friend( ulong steamid )
        {
            this.SteamID = steamid;
        }

        public override bool Equals(object obj)
        {
            return (obj as Friend).SteamID == this.SteamID;
        }

        public override int GetHashCode()
        {
            return SteamID.GetHashCode();
        }

        public string GetGameName()
        {
            if ( !IsInGame() )
                return "";
            try
            {
                return SteamFriends.GetFriendGamePlayedName( this.SteamID );
            }
            catch
            {
                return "";
            }
        }


        public bool IsInGame()
        {
            try
            {
                string gameName = SteamFriends.GetFriendGamePlayedName( this.SteamID );
                return !string.IsNullOrEmpty( gameName );
            }
            catch
            {
                return false;
            }
        }

        public bool IsBlocked()
        {
            EFriendRelationship relationship = SteamFriends.GetFriendRelationship( this.SteamID );
            return ( relationship == EFriendRelationship.Ignored || relationship == EFriendRelationship.IgnoredFriend );
        }

        public bool IsRequestingFriendship()
        {
            EFriendRelationship relationship = SteamFriends.GetFriendRelationship( this.SteamID );
            return ( relationship == EFriendRelationship.RequestRecipient );
        }

        public bool IsAcceptingFriendship()
        {
            return ( SteamFriends.GetFriendRelationship( this.SteamID ) == EFriendRelationship.RequestInitiator );
        }

        public bool IsOnline()
        {
            try
            {
                return SteamFriends.GetFriendPersonaState( this.SteamID ) != EPersonaState.Offline;
            }
            catch { return false; }
        }

        public string GetName()
        {
            try
            {
                return SteamFriends.GetFriendPersonaName( this.SteamID );
            }
            catch
            {
                return "[unknown]";
            }
        }

        public string GetStatus()
        {
            try
            {
                string str = "";

                if ( this.IsInGame() )
                {
                    str = "In-Game";

                    if ( this.IsBlocked() )
                        str += " (Blocked)";

                    return str;
                }

                EPersonaState state = SteamFriends.GetFriendPersonaState( this.SteamID );

                switch ( state )
                {
                    case EPersonaState.Away:
                        str = "Away";
                        break;

                    case EPersonaState.Busy:
                        str = "Busy";
                        break;

                    case EPersonaState.Online:
                        str = "Online";
                        break;

                    case EPersonaState.Snooze:
                        str = "Snooze";
                        break;

                    default:
                        str = "Offline";
                        break;
                }

                if ( this.IsAcceptingFriendship() )
                    str = "Invited";

                if ( this.IsRequestingFriendship() )
                    str = "Req. Friendship";


                if ( this.IsBlocked() )
                    str += " (Blocked)";

                return str;
            }
            catch
            {
                return "Offline";
            }
        }

        public EPersonaState GetState()
        {
            try
            {
                return SteamFriends.GetFriendPersonaState( this.SteamID );
            }
            catch
            {
                return EPersonaState.Offline;
            }
        }
    }
}
