namespace QuantumSimulator.Quantum;

using System.Numerics;

/// <summary>
/// Quantum gates class
/// </summary>
public class QuantumOperator
{
    public Complex[,] OperatorMatrix { get; private set; }

    public QuantumOperator(Complex[,] operatorMatrix)
    {
        // TODO check that the matrix verifies a quantum operator.
    }
    
} 