using EasyWork.Extend.Utilities;
using EasyWork.Utilities;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/1/6 9:59:02
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public abstract class BaseSkill
    {
        public virtual void Excute(BaseUnit source, BaseUnit target, float value) { }
        public virtual void Excute(BaseUnit source, BaseUnit target, params object[] parma) { }
    }

    public class CommonDamage : BaseSkill
    {
        public override void Excute(BaseUnit source, BaseUnit target, float value)
        {
            target.DODamage(value);
        }
    }
}