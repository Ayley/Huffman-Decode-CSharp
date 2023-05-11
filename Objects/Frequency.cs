namespace HuffmanDecoder.Objects;

public record Frequency(char Character, long Frequent)
{
    public override string ToString()
    {
        return $"{nameof(Character)}: {Character}, {nameof(Frequent)}: {Frequent}";
    }
}