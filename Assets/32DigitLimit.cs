using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "DigitLimitValidator.asset", menuName = "TextMeshPro/Validators/Digit Limit (1-32)")]
public class DigitLimitValidator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        // Only allow digits
        if (!char.IsDigit(ch))
            return '\0';

        // Simulate new text with the character inserted
        string newText = text.Insert(pos, ch.ToString());

        // Reject if longer than 32
        if (newText.Length > 32)
            return '\0';

        // Apply change
        text = newText;
        pos++;
        return '\0';  // ← Critical: Prevent TMP from appending again
    }
}