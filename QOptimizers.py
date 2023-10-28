'''from qiskit_optimization import QuadraticProgram
from qiskit import BasicAer, Aer
from qiskit.algorithms import QAOA, NumPyMinimumEigensolver
from qiskit_optimization.algorithms import MinimumEigenOptimizer
from qiskit.algorithms.optimizers import COBYLA, SLSQP, ADAM'''

from qiskit_optimization import QuadraticProgram
from qiskit.algorithms import NumPyMinimumEigensolver
from qiskit_optimization.algorithms import MinimumEigenOptimizer
from qiskit.algorithms.optimizers import ADAM

import itertools
from QoordinateClasses import Map

def SolveWithNumPyMinimumEigensolver(model: QuadraticProgram):
    exact_mes = NumPyMinimumEigensolver()
    eigen_optimizer = MinimumEigenOptimizer(min_eigen_solver=exact_mes)
    solution = eigen_optimizer.solve(model)
    return solution

def BruteForce(map: Map):
    matrix = map.GetDistanceMatrix(0)
    N = matrix.shape[0]
    perm = list(itertools.permutations(range(1, N)))
    
    alldistances = []
    mindistance = float("inf")
    minorder = None

    for order in perm:
        dist = 0
        for i in range(len(order)):
            if(i == 0):
                dist = dist + matrix[0, order[i]]
                continue
            dist = dist + matrix[order[i], order[i-1]]
        
        if dist < mindistance:
            mindistance = dist
            minorder = order

        alldistances.append(dist)

    print('all distances:', alldistances)
    print('min distance:', mindistance)
    print('min order:', minorder)
    
    names = map.GetBuildingNamesList()
    name_order = [names[i - 1] for i in minorder]

    print('min name order:', name_order)