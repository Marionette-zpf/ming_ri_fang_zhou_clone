namespace EasyWork.Utilities
{
    public class ERulderInfo
    {
        public float Weight = 1;
        public float RulerForce = 1;

        public float GetValue()
        {
            if(Weight <= 0 || RulerForce <= 0)
            {
                return 0;
            }
            return Weight * RulerForce;
        }
    }

}

