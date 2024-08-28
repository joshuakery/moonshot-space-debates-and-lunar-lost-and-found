
public static class UICustomEase
{
    public static float Linear(float time, float duration, float amplitude, float period)
    {
        return time / duration;
    }

    public static float _EaseIn(float t)
    {
        return t * t;
    }

    public static float EaseIn(float time, float duration, float amplitude, float period)
    {
        return _EaseIn(time / duration);
    }

    //Cubic Bezier Math

    private static float _A(float a1, float a2)
    {
        return 1f - 3f * a2 + 3f * a1;
    }

    private static float _B(float a1, float a2)
    {
        return 3f * a2 - 6f * a1;
    }

    private static float _C(float a1)
    {
        return 3f * a1;
    }

    public static float _CubicBezier(float t)
    {
        //only the y values of the control points may be manipulated here
        //hard-coded here

        float p1 = 1f;
        float p3 = 0f;

        return ((_A(p1, p3) * t + _B(p1, p3)) * t + _C(p1)) * t;

    }

    public static float CubicBezier(float time, float duration, float amplitude, float period)
    {
        return _CubicBezier(time / duration);

    }

    //Overshoot

    private static float _Overshoot(float t)
    {
        return t;
    }

    public static float Overshoot(float time, float duration, float amplitude, float period)
    {
        return _Overshoot(time / duration);
    }


}
