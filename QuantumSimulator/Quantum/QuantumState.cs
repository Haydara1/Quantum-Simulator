namespace QuantumSimulator.Quantum;

using System.Numerics;
using QMath;

/// <summary>
/// Quantum vector state class.
/// </summary>
public class QuantumState
{
    #region Attributes
    
    /// <summary>
    /// The amplitudes of the quantum state.
    /// </summary>
    private readonly Complex[] _amplitudes;
    public IReadOnlyList<Complex> Amplitudes => _amplitudes;
    
    // Store the number of the Qubits:

    public int QubitCount { get; private set; }

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
        
        QubitCount = BitOperations.Log2((uint)amplitudes.Length);
        
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
        if (qubitIndex < 0 || qubitIndex >= QubitCount)
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
        
        return Math.Abs(normSquared - 1.0) < Util.Epsilon;
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
        if (qubitIndex < 0 || qubitIndex >= QubitCount)
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
    
    #region Operators

    /// <summary>
    /// Calculates the Dot product of two state vectors.
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    private static Complex Dot(QuantumState q1, QuantumState q2)
    {
        // Check that the quantum states aren't null.
        if(q1 is null || q2 is null)
            throw new ArgumentNullException();
        
        // Check that the quantum states are in the same dimension
        if (q1._amplitudes.Length != q2._amplitudes.Length)
            throw new ArgumentException("Quantum state must have the same dimensionality.");
        
        Complex result = Complex.Zero;

        for (int i = 0; i < q1._amplitudes.Length; i++)
            result += Complex.Conjugate(q1._amplitudes[i]) * q2._amplitudes[i];
        
        return result;
    }
    
    /// <summary>
    /// Calculates the Dot product of two state vectors.
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    public static Complex operator *(QuantumState q1, QuantumState q2)
        => Dot(q1, q2);

    /// <summary>
    /// Returns the outer product of two quantum state vectors as a density matrix.
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static Complex[,] OuterProduct(QuantumState q1, QuantumState q2)
    {
        // Check if the arguments are null.
        if (q1 is null || q2 is null)
            throw new ArgumentNullException();
        
        // Check if the state vectors have the same dimensions
        if(q1.Amplitudes.Count != q2.Amplitudes.Count)
            throw new ArgumentException("Quantum state must have the same dimensionality.");
        
        // Initialize the density matrix
        Complex[,] outputMatrix = new Complex[q1.Amplitudes.Count, q1.Amplitudes.Count];

        for (int i = 0; i < q1.Amplitudes.Count; i++)
        {
            for (int j = 0; j < q1.Amplitudes.Count; j++)
            {
                // Refer to matrix multiplication for this line:
                outputMatrix[i, j] = q1.Amplitudes[i] *
                                      Complex.Conjugate(q2.Amplitudes[j]);
            }
        }

        return outputMatrix;
    }
    
    #endregion
    
    
    #region Comparison operators

    /// <summary>
    /// Checks if the given object is equal to the considered quantum state.
    /// If the object is indeed a quantum state, their amplitudes are compared up to a certain tolerance.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        
        if(obj.GetType() != GetType())
            return false;
        
        return Equals((QuantumState)obj);
    }

    /// <summary>
    /// Compares to quantum states up to a certain Tolerance.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    private bool Equals(QuantumState other)
    {
        // Check that they have the same dimensionality:
        if (this._amplitudes.Length != other._amplitudes.Length)
            return false;

        // Compare amplitudes up to a certain tolerance
        for (int i = 0; i < _amplitudes.Length; i++)
        {
            var c1 = this._amplitudes[i];
            var c2 = other._amplitudes[i];
            
            if(Math.Abs(c1.Real - c2.Real) > Util.Epsilon ||
               Math.Abs(c1.Imaginary - c2.Imaginary) > Util.Epsilon)
                return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// Checks if two quantum states are equal (up to a tolerance Epsilon).
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    public static bool operator ==(QuantumState? q1, QuantumState? q2)
    {
        if (ReferenceEquals(q1, q2))
            return true;

        if (q1 is null || q2 is null)
            return false;

        return q1.Equals(q2);
    }
    
    /// <summary>
    /// Checks if two quantum states are not equal (up to a tolerance Epsilon).
    /// </summary>
    /// <param name="q1"></param>
    /// <param name="q2"></param>
    /// <returns></returns>
    public static bool operator !=(QuantumState? q1, QuantumState? q2)
        => !(q1 == q2);
    
    /// <summary>
    /// We can't implement a hash code for this object, since the equality is true up to a certain tolerance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public override int GetHashCode()
        => throw new NotImplementedException(); 

    #endregion

}