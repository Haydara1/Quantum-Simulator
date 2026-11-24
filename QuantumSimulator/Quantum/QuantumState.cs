namespace QuantumSimulator.Quantum;

using System.Numerics;

/// <summary>
/// Quantum vector state class.
/// </summary>
public class QuantumState
{
    #region Attributes
    
    // Tolerance rate.
    private const double Epsilon = 1e-10;
    
    /// <summary>
    /// The amplitudes of the quantum state.
    /// </summary>
    private readonly Complex[] _amplitudes;
    public IReadOnlyList<Complex> Amplitudes => _amplitudes;
    
    // Store the number of the Qubits:
    private int _qubitCount;
    
    public int QubitCount => _qubitCount;
    
    #endregion
    
    #region Constructors

    /// <summary>
    /// Main constructor.
    /// </summary>
    /// <param name="amplitudes"></param>
    public QuantumState(Complex[] amplitudes)
    {
        // Assert the amplitudes aren't null:
        if(amplitudes == null)
            throw new ArgumentNullException(nameof(amplitudes));
        
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
    
    #endregion
    
    #region Public Methods

    /// <summary>
    /// Gets the probability of the given qubit to be in the state 1.
    /// To get the probability of it being zero, one can do 1 - GetProbability(index)
    /// </summary>
    /// <param name="qubitIndex">Qubits indexed from 0 to n - 1.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public double GetProbability(int qubitIndex)
    {
        // Check the right index of the qubit.
        if (qubitIndex < 0 || qubitIndex >= _qubitCount)
            throw new ArgumentOutOfRangeException(nameof(qubitIndex));
        
        Complex[] listOfAmplitudes = GetAmplitudes(qubitIndex);

        double probability = 0;
        
        foreach (var amplitude in listOfAmplitudes)
            probability += amplitude.Magnitude * amplitude.Magnitude;
        
        return probability;
    }

    /// <summary>
    /// Gets the probability of the given qubit to be in the state 0.
    /// </summary>
    /// <param name="qubitIndex"></param>
    /// <returns></returns>
    public double GetZeroProbability(int qubitIndex)
        => 1 - GetProbability(qubitIndex);
    
    #endregion

    #region Private methods
    
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

    /// <summary>
    /// Returns the list of the complex amplitudes in which the state of the wanted qubit is one.
    /// </summary>
    /// <param name="qubitIndex"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private Complex[] GetAmplitudes(int qubitIndex)
    {
        // Be aware of integer overflow
        if (qubitIndex < 0 || qubitIndex >= _qubitCount)
            throw new ArgumentOutOfRangeException(nameof(qubitIndex));
        
        int bit = 1; // Start with a bit in this form 00..001
        
        // Shift the bit to match the index of the required qubit:
        for (int i = 0; i < qubitIndex; i++)
            bit <<= 1; // 00..010
        

        Complex[] listOfAmplitudes = new Complex[_amplitudes.Length / 2]; // Only have the states are one.

        int index = 0;
        
        for (int i = 0; i < _amplitudes.Length; i++)
        {
            // Apply logical AND to the index and the bit, if the answer is the bit,
            // then the index contains the required bit as one:

            if ((i & bit) == bit)
            {
                listOfAmplitudes[index] = _amplitudes[i];
                index++;
            }
        }
        
        return listOfAmplitudes;
    }

    #endregion
    
}