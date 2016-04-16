namespace UI.uGui.Animations
{
    public interface IProgressAnimation
    {
        void StartAnimation(float startValue, float endValue,float time);
        void StopAnimation();
    }
}