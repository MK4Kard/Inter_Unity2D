using DG.Tweening;
using TMPro;
using UnityEngine;

public static class TMProExtensions
{
    public static Tweener DOText(this TMP_Text textComponent, string targetText, float duration)
    {
        string startText = textComponent.text;

        return DOTween.To(() => textComponent.text,
                          x => textComponent.text = x,
                          targetText,
                          duration)
                     .SetUpdate(true);
    }

    public static Tweener DOTextScramble(this TMP_Text textComponent,
                                          string targetText,
                                          float duration,
                                          bool richTextEnabled = true,
                                          ScrambleMode scrambleMode = ScrambleMode.None,
                                          string scrambleCustom = null)
    {
        textComponent.richText = richTextEnabled;

        return DOTween.To(() => textComponent.text,
                          x => textComponent.text = x,
                          targetText,
                          duration)
                     .SetOptions(true, scrambleMode, scrambleCustom);
    }
}
