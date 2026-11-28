using System.Numerics;

namespace QuantumSimulator.Quantum;

public static class Gates
{
    /// <summary>
    /// Hadamard gate:
    /// [ 1/sqrt(2) ,  1/sqrt(2)
    ///   1/sqrt(2) , -1/sqrt(2) ]
    /// </summary>
    public static readonly QuantumOperator Hadamard = new QuantumOperator(new Complex[,]
        {
            { 1 / Math.Sqrt(2), 1 / Math.Sqrt(2) },
            { 1 / Math.Sqrt(2), -1 / Math.Sqrt(2) }
        }
    );

    /// <summary>
    /// Pauli X gate:
    /// [ 0 , 1
    ///   1 , 0 ]
    /// </summary>
    public static readonly QuantumOperator PauliX = new QuantumOperator(new Complex[,]
        {
            {0, 1},
            {1, 0}
        }
    );
    
    /// <summary>
    /// Pauli Y gate:
    /// [ 0 , -i
    ///   i , 0 ]
    /// </summary>
    public static readonly QuantumOperator PauliY = new QuantumOperator(new Complex[,]
        {
            {0, -Complex.ImaginaryOne},
            {Complex.ImaginaryOne,  0}
        }
    );
    
    /// <summary>
    /// Pauli Z gate:
    /// [ 1 ,  0
    ///   0 , -1 ]
    /// </summary>
    public static readonly QuantumOperator PauliZ = new QuantumOperator(new Complex[,]
        {
            {1, 0},
            {0, -1}
        }
    );
    
    /// <summary>
    /// Phase:
    /// [ 1 ,  0
    ///   0 , i ]
    /// </summary>
    public static readonly QuantumOperator Phase = new QuantumOperator(new Complex[,]
        {
            {1, 0},
            {0, Complex.ImaginaryOne},
        }
    );
}