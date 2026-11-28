namespace QuantumSimulator.Circuit;

/// <summary>
/// The main circuit simulator class:
/// </summary>
public class Circuit
{
    public int QubitNumber { get; private set; }
    
    public Circuit(int qubitNumber)
    {
        if(qubitNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(qubitNumber));
        
        this.QubitNumber = qubitNumber;
    }
}