namespace Krosoft.Extensions.Core.Tools;

public record SequentialGuid
{
    private const int NumberOfBytes = 6;
    private const int PermutationsOfAByte = 256;
    private static readonly Lazy<SequentialGuid> InstanceField = new Lazy<SequentialGuid>(() => new SequentialGuid());
    private readonly long _maximumPermutations = (long)Math.Pow(PermutationsOfAByte, NumberOfBytes);
    private readonly DateTime _sequenceEndDate;
    private readonly DateTime _sequenceStartDate;
    private readonly object _synchronizationObject = new object();
    private long _lastSequence;

    public SequentialGuid(DateTime sequenceStartDate, DateTime sequenceEndDate)
    {
        _sequenceStartDate = sequenceStartDate;
        _sequenceEndDate = sequenceEndDate;
    }

    public SequentialGuid() : this(new DateTime(2011, 10, 15), new DateTime(2100, 1, 1))
    {
    }

    private static SequentialGuid Instance => InstanceField.Value;

    public TimeSpan TimePerSequence
    {
        get
        {
            var ticksPerSequence = TotalPeriod.Ticks / _maximumPermutations;
            var result = new TimeSpan(ticksPerSequence);
            return result;
        }
    }

    public TimeSpan TotalPeriod
    {
        get
        {
            var result = _sequenceEndDate - _sequenceStartDate;
            return result;
        }
    }

    private long GetCurrentSequence(DateTime value)
    {
        var ticksUntilNow = value.Ticks - _sequenceStartDate.Ticks;
        var result = (decimal)ticksUntilNow / TotalPeriod.Ticks * _maximumPermutations - 1;
        return (long)result;
    }

    public Guid GetGuid() => GetGuid(DateTime.Now);

    internal Guid GetGuid(DateTime now)
    {
        if (now < _sequenceStartDate || now > _sequenceEndDate)
        {
            return Guid.NewGuid(); // Outside the range, use regular Guid
        }

        var sequence = GetCurrentSequence(now);
        return GetGuid(sequence);
    }

    internal Guid GetGuid(long sequence)
    {
        lock (_synchronizationObject)
        {
            if (sequence <= _lastSequence)
            {
                // Prevent double sequence on same server
                sequence = _lastSequence + 1;
            }

            _lastSequence = sequence;
        }

        var sequenceBytes = GetSequenceBytes(sequence);
        var guidBytes = GetGuidBytes();
        var totalBytes = guidBytes.Concat(sequenceBytes).ToArray();
        var result = new Guid(totalBytes);
        return result;
    }

    private static IEnumerable<byte> GetGuidBytes()
    {
        var result = Guid.NewGuid().ToByteArray().Take(10).ToArray();
        return result;
    }

    private static IEnumerable<byte> GetSequenceBytes(long sequence)
    {
        var sequenceBytes = BitConverter.GetBytes(sequence);
        var sequenceBytesLongEnough = sequenceBytes.Concat(new byte[NumberOfBytes]);
        var result = sequenceBytesLongEnough.Take(NumberOfBytes).Reverse();
        return result;
    }

    /// <summary>
    /// Permet d'obtenir un nouveau Guid séquentiel.
    /// </summary>
    /// <returns>Guid séquentiel nouvellement généré.</returns>
    public static Guid NewGuid() => Instance.GetGuid();
}