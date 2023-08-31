namespace BatteOfHerone.Enuns
{
    public enum PlayerState
    {
        PlayerOne,
        PlayerTwo
    }
    public enum State
    {
        PlayerOne,
        PlayerTwo
    }
    public enum EventName
    {
        InitialTurn,
        CurrentTurn,
        EndTurn
    }

    public enum RaceType
    {
        None,
        Human,
        Orc,
        Elf,
        Undead,
        Dwarf
    }

    public enum ButtonType
    {
        Unit,
        Upgrade,
        Action,
        BonusAction,
        StandardAction
    }

    public enum TurnState
    {
        Start,
        Turn,
        End
        // Outros estados do jogo
    }
    public enum GameTurnState
    {
        Waiting,
        SelectObject,
        End
        // Outros estados do jogo
    }
}