using System;
using System.Collections.Generic;
using System.Drawing;

internal class Pattern : IEquatable<Pattern>
{

    private static Pattern LINE_PATTERN = new Pattern("Horizontal", 0, 0, 1, 0, 2, 0);
    private static Pattern SLASH_PATTERN = new Pattern("Slash", 0, 0, 1, 1, 2, 2);

    private string patternName;
    private HashSet<Point> points = new HashSet<Point>();

    public Pattern(HashSet<Point> points) : this("unknown", points) { }

    public Pattern(string patternName, IEnumerable<Point> points)
    {
        this.patternName = patternName;
        this.points.UnionWith(points);
    }

    public Pattern(params int[] pointsArr) : this("unknown", pointsArr) { }

    public Pattern(string patternName, params int[] pointsArr)
    {
        this.patternName = patternName;

        Point[] points = new Point[pointsArr.Length / 2];
        for (int i = 0; i < points.Length; i++)
        {
            Point p = new Point(pointsArr[2 * i], pointsArr[2 * i + 1]);
            this.points.Add(p);
        }
    }

    public HashSet<Point> GetPoints()
    {
        return points;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Pattern);
    }

    public bool Equals(Pattern other)
    {
        if (other == null) return false;
        return (points.SetEquals(other.points));
    }

    public override string ToString()
    {
        string s = string.Join(", ", points);
        return $"{patternName} - {s}";
    }

    // we return 0 so that the pattern comperator will us the equals method.
    public override int GetHashCode() { return 0; }

    public static HashSet<Pattern> GetDefaultPattern()
    {
        HashSet<Pattern> allPatternPermutations = new HashSet<Pattern>();

        HashSet<Pattern> linePatterns = GetAllPermutations(LINE_PATTERN);
        allPatternPermutations.UnionWith(linePatterns);

        HashSet<Pattern> slashPatterns = GetAllPermutations(SLASH_PATTERN);
        allPatternPermutations.UnionWith(slashPatterns);

        return allPatternPermutations;
    }

    public static HashSet<Pattern> GetAllPermutations(Pattern pattern)
    {
        HashSet<Pattern> allPatterns = new HashSet<Pattern>();

        // add the 1st given pattern
        allPatterns.Add(pattern);

        // get the mirrored patterns of the given pattern
        HashSet<Pattern> mirroredPatterns = GetMirroredPermutations(pattern, true, true);
        allPatterns.UnionWith(mirroredPatterns);

        // get all rotations of given + mirrored patterns
        HashSet<Pattern> rotationPatterns = new HashSet<Pattern>();
        foreach (Pattern p in allPatterns)
        {
            HashSet<Pattern> rotationPattern = GetRotationPermutations(p, true, true, true);
            rotationPatterns.UnionWith(rotationPattern);
        }
        allPatterns.UnionWith(rotationPatterns);

        // return all patterns
        return allPatterns;
    }

    public static HashSet<Pattern> GetMirroredPermutations(Pattern pattern, bool mirrorVertically, bool mirrorHorizontally)
    {
        HashSet<Pattern> patterns = new HashSet<Pattern>();

        if (mirrorHorizontally)
            patterns.Add(RotatePattern(pattern, mirrorHorizontallyMatrix));
        if (mirrorVertically)
            patterns.Add(RotatePattern(pattern, mirrorVerticallyMatrix));

        return patterns;
    }

    public static HashSet<Pattern> GetRotationPermutations(Pattern pattern, bool rotateOneQuarter, bool rotateTwoQuarters, bool rotateThreeQuarters)
    {
        HashSet<Pattern> patterns = new HashSet<Pattern>();

        if (rotateOneQuarter)
            patterns.Add(RotatePattern(pattern, rotateOneQuarterMatrix));
        if (rotateTwoQuarters)
            patterns.Add(RotatePattern(pattern, rotateTwoQuarterMatrix));
        if (rotateThreeQuarters)
            patterns.Add(RotatePattern(pattern, rotateThreeQuarterMatrix));

        return patterns;
    }

    private static readonly Tuple<string, int, int, int, int> mirrorHorizontallyMatrix = new Tuple<string, int, int, int, int>("mirror horizontally", 1, 0, 0, -1);
    private static readonly Tuple<string, int, int, int, int> mirrorVerticallyMatrix = new Tuple<string, int, int, int, int>("nirror vertically", -1, 0, 0, 1);
    private static readonly Tuple<string, int, int, int, int> rotateOneQuarterMatrix = new Tuple<string, int, int, int, int>("90° CW", 0, -1, 1, 0);
    private static readonly Tuple<string, int, int, int, int> rotateTwoQuarterMatrix = new Tuple<string, int, int, int, int>("180° CW", -1, 0, 0, -1);
    private static readonly Tuple<string, int, int, int, int> rotateThreeQuarterMatrix = new Tuple<string, int, int, int, int>("90° CCW", 0, 1, -1, 0);
    private static Pattern RotatePattern(Pattern pattern, Tuple<string, int, int, int, int> rotationMatrix)
    {
        HashSet<Point> RotatedPoints = new HashSet<Point>();

        HashSet<Point> points = pattern.GetPoints();
        foreach (Point point in points)
        {
            int newX = point.X * rotationMatrix.Item2 + point.Y * rotationMatrix.Item3;
            int newY = point.X * rotationMatrix.Item4 + point.Y * rotationMatrix.Item5;
            Point rotatedPoint = new Point(newX, newY);
            RotatedPoints.Add(rotatedPoint);
        }

        string newName = pattern.patternName + "-" + rotationMatrix.Item1;
        Pattern RotatedPattern = new Pattern(newName, RotatedPoints);
        Pattern normalizedPattern = NormalizePattern(RotatedPattern);
        return normalizedPattern;
    }

    // this method moves the pattern to the (positive, positive) part of the plane
    private static Pattern NormalizePattern(Pattern pattern)
    {
        int lowestX = int.MaxValue;
        int lowestY = int.MaxValue;

        HashSet<Point> points = pattern.GetPoints();
        foreach (Point point in points)
        {
            if (point.X < lowestX)
            {
                lowestX = point.X;
            }
            if (point.Y < lowestY)
            {
                lowestY = point.Y;
            }
        }
        Pattern movedPattern = addXYToCurrentPattern(pattern, -lowestX, -lowestY);
        return movedPattern;
    }

    public static Pattern addXYToCurrentPattern(Pattern pattern, int x, int y)
    {
        HashSet<Point> recalculatedPoints = new HashSet<Point>();

        HashSet<Point> points = pattern.GetPoints();
        foreach (Point point in points)
        {
            recalculatedPoints.Add(new Point(point.X + x, point.Y + y));
        }
        return new Pattern(pattern.patternName, recalculatedPoints);
    }
}