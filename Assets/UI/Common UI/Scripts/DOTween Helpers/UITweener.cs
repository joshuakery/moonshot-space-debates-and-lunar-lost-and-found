using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using DG.Tweening;
using EasingCurve;

public class UITweener : MonoBehaviour
{
    public UISequenceManager sequenceManager;

    public bool doAppendSameTweenTypes;
    public bool doInsertSameTweenTypes;

    [System.Serializable]
    public enum TweenType
    {
        Entry = 0,
        Exit = 1
    }

    [System.Serializable]
    public class UITween
    {
        public TweenType tweenType;
        public UIAnimation UIAnimation;
        public GameObject objectToAnimate;

        public UnityEvent onStartEvent;
        public UnityEvent onCompleteEvent;

        public float offset = 0f;
    }

    public List<UITween> UITweens;
    public Dictionary<TweenType, List<UITween>> UITweensDict;

    public void AppendTweener(int i)
    {
        _AppendTweener((TweenType)i);
    }

    public void AppendTweener(TweenType tweenType)
    {
        _AppendTweener(tweenType);
    }

    private void _AppendTweener(TweenType tweenType)
    {
        if (!UITweensDict.ContainsKey(tweenType))
            return;

        List<UITween> UITweens = UITweensDict[tweenType];

        Sequence sequence = GetChildSequence(UITweens);

        if (sequence != null)
        {
            if (sequenceManager.currentSequence == null)
                CreateSequence();

            sequenceManager.currentSequence.Append(sequence);
        }
    }


    public void JoinTweener(int i)
    {
        _JoinTweener((TweenType)i);
    }

    public void JoinTweener(TweenType tweenType)
    {
        _JoinTweener(tweenType);
    }

    private void _JoinTweener(TweenType tweenType)
    {
        if (!UITweensDict.ContainsKey(tweenType))
            return;

        List<UITween> UITweens = UITweensDict[tweenType];

        Sequence sequence = GetChildSequence(UITweens);

        if (sequence != null)
        {
            if (sequenceManager.currentSequence == null)
                CreateSequence();

            sequenceManager.currentSequence.Join(sequence);
        }
    }

    public void InsertTweener(float atPosition, int i)
    {
        _InsertTweener(atPosition, (TweenType)i);
    }

    public void InsertTweener(float atPosition, TweenType tweenType)
    {
        _InsertTweener(atPosition, tweenType);
    }

    private void _InsertTweener(float atPosition, TweenType tweenType)
    {
        if (!UITweensDict.ContainsKey(tweenType))
            return;

        List<UITween> UITweens = UITweensDict[tweenType];

        Sequence sequence = GetChildSequence(UITweens);

        if (sequence != null)
        {
            if (sequenceManager.currentSequence == null)
                CreateSequence();

            sequenceManager.currentSequence.Insert(atPosition, sequence);
        }
    }

    public void BackInsertTweener(float atPosition, int i)
    {
        _BackInsertTweener(atPosition, (TweenType)i);
    }

    public void BackInsertTweener(float atPosition, TweenType tweenType)
    {
        _BackInsertTweener(atPosition, tweenType);
    }

    private void _BackInsertTweener(float atPosition, TweenType tweenType)
    {
        if (!UITweensDict.ContainsKey(tweenType))
            return;

        List<UITween> UITweens = UITweensDict[tweenType];

        Sequence sequence = GetChildSequence(UITweens);

        if (sequence != null)
        {
            if (sequenceManager.currentSequence == null)
                CreateSequence();

            float backPosition = sequenceManager.currentSequence.Duration() - atPosition;
            sequenceManager.currentSequence.Insert(backPosition, sequence);
        }
    }

    private Sequence GetChildSequence(List<UITween> UITweens)
    {
        Sequence sequence = null;

        foreach (UITween UITween in UITweens)
        {
            Tween _tween = CreateTweener(UITween);

            if (_tween != null)
            {
                if (sequence == null)
                    sequence = DOTween.Sequence();

                if (doAppendSameTweenTypes)
                    sequence.Append(_tween);
                else if (doInsertSameTweenTypes)
                {
                    sequence.Insert(UITween.offset, _tween);
                }
                else
                    sequence.Join(_tween);
            }
        }

        //if (sequence != null) Debug.Log("Child sequence duration: " + sequence.Duration().ToString());

        return sequence;
    }

    public void CompleteCurrentSequence()
    {
        sequenceManager.currentSequence.Complete();
    }

    private void CreateSequence()
    {
        //Debug.Log("creating new sequence");
        sequenceManager.currentSequence = DOTween.Sequence();
        sequenceManager.currentSequence.OnKill(() => {
            sequenceManager.currentSequence = null;
        });
    }


    public Tween GetTween(TweenType tweenType)
    {
        if (UITweensDict.ContainsKey(tweenType))
        {
            List<UITween> UITweens = UITweensDict[tweenType];
            Sequence sequence = GetChildSequence(UITweens);
            return sequence;
        }
        else
        {
            return null;
        }
    }

    private void Awake()
    {
        UITweensDict = new Dictionary<TweenType, List<UITween>>();
        foreach (UITween UITween in UITweens)
        {
            if (UITween.objectToAnimate == null)
                UITween.objectToAnimate = gameObject;

            if (!UITweensDict.ContainsKey(UITween.tweenType))
            {
                UITweensDict[UITween.tweenType] = new List<UITween>();
            }

            UITweensDict[UITween.tweenType].Add(UITween);
        }
    }

    private Tween CreateTweener(UITween UITween)
    {
        if (UITween.objectToAnimate == null) { return null; }

        switch (UITween.UIAnimation.animationType)
        {
            case (UIAnimation.AnimationType.Fade):
                return Fade(UITween);
            case (UIAnimation.AnimationType.Move):
                return Move(UITween);
            case (UIAnimation.AnimationType.RelativeMove):
                return RelativeMove(UITween);
            case (UIAnimation.AnimationType.Rotate):
                return Rotate(UITween);
            case (UIAnimation.AnimationType.Scale):
                return Scale(UITween);
            case (UIAnimation.AnimationType.FontSize):
                return FontSize(UITween);
            case (UIAnimation.AnimationType.Move3D):
                return Move3D(UITween);
            case (UIAnimation.AnimationType.RelativeMove3D):
                return RelativeMove3D(UITween);
            default:
                return null;
        }
    }

    private Tween Fade(UITween UITween)
    {
        if (UITween.objectToAnimate.GetComponent<CanvasGroup>() == null)
            UITween.objectToAnimate.AddComponent<CanvasGroup>();

        if (UITween.objectToAnimate.GetComponent<Canvas>() == null)
            UITween.objectToAnimate.AddComponent<Canvas>();

        CanvasGroup canvasGroup = UITween.objectToAnimate.GetComponent<CanvasGroup>();
        Canvas canvas = UITween.objectToAnimate.GetComponent<Canvas>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        //float duration = (canvasGroup.alpha == UIAnimation.to.x && !UIAnimation.useFrom) ? 0 : UIAnimation.duration;

        Tween _tween;

        if (!UIAnimation.useFrom)
            _tween = canvasGroup.DOFade(UIAnimation.to.x, UIAnimation.duration);
        else
        {
            float aux = canvasGroup.alpha;

            canvasGroup.alpha = UIAnimation.to.x; //set alpha to .to.x as the reference for .From()
            _tween = canvasGroup.DOFade(UIAnimation.from.x, UIAnimation.duration).From(); //sets alpha to .from.x

            canvasGroup.alpha = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {

            if (!canvas.enabled && UIAnimation.to.x != 0)
                canvas.enabled = true;

            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.to.x == 0)
                canvas.enabled = false;
            else
                canvas.enabled = true;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();

        });

        return _tween;
    }

    private Tween Move(UITween UITween)
    {
        RectTransform rt = UITween.objectToAnimate.GetComponent<RectTransform>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        Tween _tween;

        if (!UIAnimation.useFrom)
            _tween = rt.DOAnchorPos(UIAnimation.to, UIAnimation.duration);
        else
        {
            Vector2 aux = rt.anchoredPosition;

            rt.anchoredPosition = UIAnimation.to; //set pos to .to as the reference for .From()
            _tween = rt.DOAnchorPos(UIAnimation.from, UIAnimation.duration).From(); //sets pos to .from

            rt.anchoredPosition = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {

            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.useFrom)
                rt.anchoredPosition = UIAnimation.to;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();
        });

        return _tween;
    }

    private Tween RelativeMove(UITween UITween)
    {
        RectTransform rt = UITween.objectToAnimate.GetComponent<RectTransform>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        Tween _tween;

        Vector2 to = rt.anchoredPosition + (Vector2)UIAnimation.to;
        Vector2 from = rt.anchoredPosition + (Vector2)UIAnimation.from;

        if (!UIAnimation.useFrom)
        {
            _tween = rt.DOAnchorPos(to, UIAnimation.duration);
        }
        else
        {
            Vector2 aux = rt.anchoredPosition;

            rt.anchoredPosition = to; //set pos to 'to' as the reference for .From()
            _tween = rt.DOAnchorPos(from, UIAnimation.duration).From(); //sets pos to 'from'

            rt.anchoredPosition = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {

            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.useFrom)
                rt.anchoredPosition = to;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();
        });

        return _tween;
    }

    private Tween Move3D(UITween UITween)
    {
        Transform t = UITween.objectToAnimate.GetComponent<Transform>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        Tween _tween;

        if (!UIAnimation.useFrom)
            _tween = t.DOLocalMove(UIAnimation.to, UIAnimation.duration);
        else
        {
            Vector2 aux = t.localPosition;

            t.localPosition = UIAnimation.to; //set pos to .to as the reference for .From()
            _tween = t.DOLocalMove(UIAnimation.from, UIAnimation.duration).From(); //sets pos to .from

            t.localPosition = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {

            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.useFrom)
                t.localPosition = UIAnimation.to;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();
        });

        return _tween;
    }

    private Tween RelativeMove3D(UITween UITween)
    {
        Transform t = UITween.objectToAnimate.GetComponent<Transform>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        Tween _tween;

        Vector3 to = t.localPosition + UIAnimation.to;
        Vector3 from = t.localPosition + UIAnimation.from;

        if (!UIAnimation.useFrom)
            _tween = t.DOLocalMove(to, UIAnimation.duration);
        else
        {
            Vector2 aux = t.localPosition;

            t.localPosition = to; //set pos to .to as the reference for .From()
            _tween = t.DOLocalMove(from, UIAnimation.duration).From(); //sets pos to .from

            t.localPosition = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {

            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.useFrom)
                t.localPosition = to;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();
        });

        return _tween;
    }

    private Tween Rotate(UITween UITween)
    {
        RectTransform rt = UITween.objectToAnimate.GetComponent<RectTransform>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        Tween _tween;

        if (!UIAnimation.useFrom)
            _tween = rt.DORotate(UIAnimation.to, UIAnimation.duration);
        else
        {
            Vector2 aux = rt.localEulerAngles;

            rt.localEulerAngles = UIAnimation.to; //set rot to .to as the reference for .From()
            _tween = rt.DORotate(UIAnimation.from, UIAnimation.duration).From(); //sets rot to .from

            rt.localEulerAngles = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {
            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.useFrom)
                rt.localEulerAngles = UIAnimation.to;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();
        });

        return _tween;
    }

    private Tween Scale(UITween UITween)
    {
        RectTransform rt = UITween.objectToAnimate.GetComponent<RectTransform>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        Tween _tween;

        if (!UIAnimation.useFrom)
            _tween = rt.DOScale(UIAnimation.to, UIAnimation.duration);
        else
        {
            Vector2 aux = rt.localScale;

            rt.localScale = UIAnimation.to; //set scale to .to as the reference for .From()
            _tween = rt.DOScale(UIAnimation.from, UIAnimation.duration).From(); //sets scale to .from

            rt.localScale = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {
            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.useFrom)
                rt.localScale = UIAnimation.to;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();
        });

        return _tween;
    }

    private Tween FontSize(UITween UITween)
    {
        if (UITween.objectToAnimate.GetComponent<TMP_Text>() == null)
            UITween.objectToAnimate.AddComponent<TMP_Text>();

        TMP_Text tmp_text = UITween.objectToAnimate.GetComponent<TMP_Text>();

        UIAnimation UIAnimation = UITween.UIAnimation;

        Tween _tween;

        if (!UIAnimation.useFrom)
            _tween = DOTween.To(
                () => tmp_text.fontSize,
                x => tmp_text.fontSize = x,
                UIAnimation.to.x,
                UIAnimation.duration
            );
        else
        {
            float aux = tmp_text.fontSize;

            tmp_text.fontSize = UIAnimation.to.x; //set font size to .to as the reference for .From()
            _tween = DOTween.To(
                () => tmp_text.fontSize,
                x => tmp_text.fontSize = x,
                UIAnimation.from.x,
                UIAnimation.duration
            ).From(); //sets font size to .from

            tmp_text.fontSize = aux; //set it back to what it was
        }

        if (UIAnimation.delay > 0)
            _tween.SetDelay(UIAnimation.delay);

        SetEase(_tween, UIAnimation);

        _tween.SetLoops(UIAnimation.loops, UIAnimation.loopType);

        _tween.OnStart(() =>
        {
            if (UITween.onStartEvent != null)
                UITween.onStartEvent.Invoke();

        });

        _tween.OnComplete(() =>
        {
            if (UIAnimation.useFrom)
                tmp_text.fontSize = UIAnimation.to.x;

            if (UITween.onCompleteEvent != null)
                UITween.onCompleteEvent.Invoke();
        });

        return _tween;
    }

    public static void SetEase(Tween _tween, UIAnimation UIAnimation)
    {
        switch (UIAnimation.easingOption)
        {
            case UIAnimation.EasingOption.Ease:
                _tween.SetEase(UIAnimation.ease);
                _tween.easePeriod = UIAnimation.easePeriod;
                _tween.easeOvershootOrAmplitude = UIAnimation.easeOvershootOrAmplitude;
                break;

            case UIAnimation.EasingOption.AnimationCurve:
                _tween.SetEase(UIAnimation.animationCurve);
                break;

            case UIAnimation.EasingOption.CubicBezierCurve:
                Vector2[] controlPointStrips = new Vector2[] {
                    new Vector2(0.0f, 0.0f),
                    new Vector2(UIAnimation.p1.x, UIAnimation.p1.y),
                    new Vector2(UIAnimation.p2.x, UIAnimation.p2.y),
                    new Vector2(1.0f, 1.0f)
                };
                _tween.SetEase(EasingAnimationCurve.BezierToAnimationCurve(controlPointStrips));
                break;

            case UIAnimation.EasingOption.CustomEase:
                _tween.SetEase(GetCustomEase(UIAnimation.customEase));
                _tween.easePeriod = UIAnimation.easePeriod;
                _tween.easeOvershootOrAmplitude = UIAnimation.easeOvershootOrAmplitude;
                break;

            default:
                break;
        }
    }

    private static EaseFunction GetCustomEase(UIAnimation.CustomEase customEase)
    {
        switch (customEase)
        {
            case UIAnimation.CustomEase.Linear:
                return new EaseFunction(UICustomEase.Linear);

            case UIAnimation.CustomEase.EaseIn:
                return new EaseFunction(UICustomEase.EaseIn);

            case UIAnimation.CustomEase.CubicBezier:
                return new EaseFunction(UICustomEase.CubicBezier);

            case UIAnimation.CustomEase.Overshoot:
                return new EaseFunction(UICustomEase.Overshoot);

            default:
                return new EaseFunction(UICustomEase.EaseIn);
        }
    }
}
