public interface ILevelManager
{
    public int CurrentLevel { get; set; }
    public LevelData GetLevelData();
}