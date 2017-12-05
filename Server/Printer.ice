module Online
{
    sequence<string> strList;

    struct Vector3 {
        float x;
        float y;
        float z;
    }

    class AnimSetValue {}

    sequence<AnimSetValue> AnimatorUpdates;

    class AnimSetFloat extends AnimSetValue {
        string Variable;
        float NewValue;
    }

    struct Transform {
        Vector3 Position;
        Vector3 Rotation;
        Vector3 Scale;
    }

    class DynamicObjectUpdate {
        Vector3 Position;
    }

    class AnimatedDynamicObjectUpdate extends DynamicObjectUpdate {
        AnimatorUpdates AnimUList;
    }

    class Command {}

    class CreateCommand extends Command {
        long Id;
        string Prefab;
        Transform Origin;
    }

    class UpdateCommand extends Command {
        long Id;
        DynamicObjectUpdate Update;
    }

    class DestroyCommand extends Command {
        long Id;
    }

    sequence<Command> CommandList;

    interface Server;

    interface Client {
       void GetInput();
       void Update(CommandList commandList);
       void Notify();
       void NotifyKicked();
       Server* MakeHost();
    }

    sequence<Client*> ClientList;

    struct PlayerStats {
       string Username;
    }

    sequence<PlayerStats> PlayerList;

    struct LobbyInfo {
        string Id;
        PlayerStats Host;
        PlayerList Players;
        bool IsLocked;
    }

    interface Game
    {
        LobbyInfo GetLobbyInfo();
    }

   sequence<Game*> GameList;

     interface GameHost extends Game {
             void StartGame();
             void KickPlayer(string username);
             void SwitchLock();
     }

     interface Server
     {
            void StartGame(ClientList clientList);
     }

    interface Player {
        PlayerStats GetStats();
        bool JoinGame(Client* client, Game* game);
        GameHost* CreateGame(Server* server);
        void LeaveGame();
        void LogOut();
    }

    interface LobbyListener {
        void Update(GameList list);
        bool Ping();
    }

    interface PlayerRegister {
        Player* Login(string username, string password);
        Player* CreateNew(string username, string password);
    }

    interface GameRegister
    {
        PlayerRegister* Connect(LobbyListener* listener);
    }

}