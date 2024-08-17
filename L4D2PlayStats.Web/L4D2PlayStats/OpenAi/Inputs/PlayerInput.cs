using L4D2PlayStats.Sdk.Matches.Results;

namespace L4D2PlayStats.OpenAi.Inputs;

public class PlayerInput(MatchResult.PlayerResult player)
{
    public string? Nome { get; set; } = player.Name;

    // ReSharper disable once InconsistentNaming
    public int QuantidadeMVP { get; set; } = player.TotalMvp;

    public int QuantidadeComunsQueMatou { get; set; } = player.Common;
    public int QuantidadeInfectadosEspeciaisQueMatou { get; set; } = player.SiKilled;
    public int DanoCausadoAosInfectadosEspeciais { get; set; } = player.SiDamage;
    public int DanoCausadoAoTank { get; set; } = player.TankDamage;

    public int Skeets { get; set; } = player.Skeets;

    public int QuantidadeVezesQueMorreu { get; set; } = player.Died;
    public int QuantidadeVezesQueFoiIncapacitado { get; set; } = player.Incaps;
}