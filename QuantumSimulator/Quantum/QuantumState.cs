namespace QuantumSimulator.Quantum;

using System.Numerics;

/// <summary>
/// 
/// </summary>
public class QuantumState
{
    // Tolerance rate.
    private const double Epsilon = 1e-10;
    
    /// <summary>
    /// The amplitudes of the quantum state.
    /// </summary>
    private readonly Complex[] _amplitudes;
    public IReadOnlyList<Complex> Amplitudes => _amplitudes;
    
    // Store the number of the Qubits:
    private int _qubitCount;

    /// <summary>
    /// Main constructor.
    /// </summary>
    /// <param name="amplitudes"></param>
    public QuantumState(Complex[] amplitudes)
    {
        if(amplitudes.Length < 2)
            throw new ArgumentException("Invalid array length, there should be at least 2 amplitudes");
        
        // Quantum states have to be of size 2^n:
        if ((amplitudes.Length & (amplitudes.Length - 1)) != 0)
            throw new ArgumentException("Array length must be a power of 2.");
        
        // Check if the array is normalized:
        if(!IsNormalized(amplitudes))
            throw new ArgumentException("Invalid amplitudes (array should be normalized).");
        
        _qubitCount = BitOperations.Log2((uint)amplitudes.Length);
        
        _amplitudes = (Complex[])amplitudes.Clone(); // Clone the array so it can't be modified using the parameter.
    }

    /// <summary>
    /// Checks whether a list of amplitudes is normalized (up to a certain tolerance).
    /// </summary>
    /// <param name="amplitudes"></param>
    /// <returns></returns>
    private static bool IsNormalized(Complex[] amplitudes)
    {
        double normSquared = 0;

        foreach (var amplitude in amplitudes)
            normSquared += amplitude.Magnitude * amplitude.Magnitude;
        
        return Math.Abs(normSquared - 1.0) < Epsilon;
    }

}