namespace TicTacToe;

public class GameManager
{
    private readonly Dictionary<int, Game> _games = new();
    private readonly Dictionary<int, Announcer> _announcers = new();
    private int _nextId;

    public List<int> GetGames()
    {
        return _games.Keys.ToList();
    }

    public int NewGame()
    {
        var id = ++_nextId;
        _games.Add(id, new Game());
        _announcers.Add(id, new Announcer());
        return id;
    }

    public Announcer GetAnnouncer(int id)
    {
        return _announcers[id];
    }

    public GameDto GetGameState(int id)
    {
        var game = _games[id];
        return game.State();
    }

    public void Move(int id, Move move)
    {
        var game = _games[id];
        var announcer = _announcers[id];
        
        game.MakeMove(move);
        announcer.AnnounceState(game.State());
    }
}