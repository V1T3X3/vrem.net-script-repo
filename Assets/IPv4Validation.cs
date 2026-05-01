using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "IPv4 Input Validator", menuName = "TextMeshPro/Input Validators/IPv4")]
public class TMP_IPv4Validator : TMP_InputValidator
{
    public override char Validate(ref string text, ref int pos, char ch)
    {
        // Only allow digits and dot
        if (!char.IsDigit(ch) && ch != '.')
            return '\0';

        // Prevent input if already at max length (e.g., 15 for "255.255.255.255")
        if (text.Length >= 15 && ch != '.')
            return '\0';

        // Handle dot input
        if (ch == '.')
        {
            // Can't start with dot or have consecutive dots
            if (text.Length == 0 || text.EndsWith("."))
                return '\0';

            // Split into octets and validate the last one
            string[] octets = text.Split('.');
            string lastOctet = octets[octets.Length - 1];

            if (!IsValidOctet(lastOctet))
                return '\0';

            // Prevent more than 3 dots (i.e., more than 4 octets)
            if (octets.Length >= 4)
                return '\0';

            // Insert dot at correct position
            text = text.Insert(pos, ch.ToString());
            pos++;
            return ch;
        }

        // Handle digit input
        if (char.IsDigit(ch))
        {
            // Insert digit at correct position
            string newText = text.Insert(pos, ch.ToString());
            pos++;

            // Re-split to get current octet
            string[] octets = newText.Split('.');
            string currentOctet = octets[octets.Length - 1];

            // Prevent octet from being longer than 3 digits
            if (currentOctet.Length > 3)
                return '\0';

            // Validate the octet value
            if (!IsValidOctet(currentOctet))
                return '\0';

            // Apply the change
            text = newText;

            // Auto-insert dot if octet is complete and less than 3 dots exist
            if (currentOctet.Length == 3 && octets.Length < 4)
            {
                text = text.Insert(pos, ".");
                // No need to increment pos again here; method ends
            }

            return ch;
        }

        return '\0';
    }

    private bool IsValidOctet(string octet)
    {
        if (string.IsNullOrEmpty(octet) || octet.Length > 3)
            return false;
        if (!int.TryParse(octet, out int value))
            return false;
        return value >= 0 && value <= 255;
    }
}