namespace HuffmanDecoder.Objects;

public record Node(string Character, long Frequency, Node? Left, Node? Right)
{

    public bool IsLeft(Node other) => Frequency < other.Frequency;

    public bool IsRight(Node other) => Frequency <= other.Frequency;

    public override string ToString()
    {
        return $"{nameof(Character)}: {Character}, {nameof(Frequency)}: {Frequency}, {nameof(Left)}: {Left}, {nameof(Right)}: {Right}";
    }
}