namespace Utility.CustomerAttribute
{
    /// <summary>
    /// 标记属性（只有编辑当前的属性才允许方法注入）
    /// <para>添加属性的好处是防止其他属性也被注册，降低注册效率</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutowiredDependencyPropertyAttribute : Attribute
    {

    }
}
