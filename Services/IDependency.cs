namespace Services
{
    /// <summary>
    /// 瞬时注入
    /// </summary>
    public interface ITransitDependency;

    /// <summary>
    /// 单例注入标识
    /// </summary>
    public interface ISingletonDependency;

    /// <summary>
    /// 生命周期注入标识
    /// </summary>
    public interface IScopeDependency;

}
