namespace L4D2PlayStats.Ranking.Services;

public interface IRankingTemplateService
{
    string Render<T>(T data);
}