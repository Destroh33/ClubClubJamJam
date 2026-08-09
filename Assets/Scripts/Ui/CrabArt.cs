public static class CrabArt
{
    public static string Banner(int gap)
    {
        string[] left = Title.Split('\n');
        string[] right = Big.Split('\n');

        int width = 0;
        foreach (string line in left)
        {
            if (line.Length > width)
                width = line.Length;
        }

        int rows = left.Length > right.Length ? left.Length : right.Length;
        var text = new System.Text.StringBuilder();

        for (int i = 0; i < rows; i++)
        {
            string a = i < left.Length ? left[i] : "";
            string b = i < right.Length ? right[i] : "";

            if (b.Length == 0)
                text.Append(a.TrimEnd());
            else
                text.Append(a.PadRight(width + gap)).Append(b);

            text.Append('\n');
        }

        return text.ToString();
    }

    public const string Title =
        "\n"+
        "\n"+
        " ######   ######   ##    ##\n" +
        " ##       ##       ##    ##\n" +
        " ######   ######   ########\n" +
        "     ##       ##   ##    ##\n" +
        " ######   ######   ##    ##\n";

    public const string Big =
        "\n"+
        "   /\\\n" +
        "  ( /   @ @    ()\n" +
        "   \\  __| |__  /\n" +
        "    -/   \"   \\-\n" +
        "   /-|       |-\\\n" +
        "  / /-\\     /-\\ \\\n" +
        "   / /-`---'-\\ \\\n" +
        "    /         \\\n";
}
