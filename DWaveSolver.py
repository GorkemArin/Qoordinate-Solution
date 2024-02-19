import networkx as nx
from dimod import SampleSet, BinaryQuadraticModel
from QoordinateClasses import *

default_token = 'DEV-e7ccc285c1ceebf894bef6ebf1379ded41d2ce8d'
default_topology = 'pegasus'

def __GetDefaultSampler():
    from dwave.system import DWaveSampler, EmbeddingComposite
    return EmbeddingComposite(DWaveSampler(token = default_token, solver=dict(topology__type = default_topology)))

def __ClassicalSolution(bqm: BinaryQuadraticModel) -> SampleSet:
    from dimod.reference.samplers import ExactSolver
    sampler = ExactSolver()
    sampleset = sampler.sample(bqm)
    return sampleset

def __HybridSolution(bqm: BinaryQuadraticModel, name) -> SampleSet:
    import hybrid
    workflow = hybrid.Loop(
    hybrid.RacingBranches(
        hybrid.InterruptableTabuSampler(),
        hybrid.EnergyImpactDecomposer(size=30, rolling=True, rolling_history=0.75)
        | hybrid.QPUSubproblemAutoEmbeddingSampler(qpu_sampler=__GetDefaultSampler())
        | hybrid.SplatComposer()) | hybrid.ArgMin(), convergence=3)
     
    # Convert to dimod sampler and run workflow
    result = hybrid.HybridSampler(workflow).sample(bqm)
    return result
    
def __QuantumSolution(bqm: BinaryQuadraticModel, name = 'default problem') -> SampleSet:
    from dwave.system import DWaveSampler, EmbeddingComposite
    sampler = __GetDefaultSampler()
    sampleset = sampler.sample(bqm, num_reads=100, label=name)
    return sampleset

def SolveDWaveProblem(bqm: BinaryQuadraticModel, solver: str, name: str = '') -> SampleSet:
    if solver == 'quantum':
        return __QuantumSolution(bqm, name)
    elif solver == 'hybrid':
        return __HybridSolution(bqm, name)
    elif solver == 'classical':
        return __ClassicalSolution(bqm)
    else:
        raise Exception(f'Invalid solver name: {solver}')



