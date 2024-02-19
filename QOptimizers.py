'''from qiskit_optimization import QuadraticProgram
from qiskit import BasicAer, Aer
from qiskit.algorithms import QAOA, NumPyMinimumEigensolver
from qiskit_optimization.algorithms import MinimumEigenOptimizer
from qiskit.algorithms.optimizers import COBYLA, SLSQP, ADAM'''

from qiskit_optimization import QuadraticProgram
from qiskit_algorithms.minimum_eigensolvers import NumPyMinimumEigensolver
from qiskit_optimization.algorithms import MinimumEigenOptimizer
from qiskit_algorithms.optimizers import ADAM

import sys
import itertools
from QoordinateClasses import Map

def SolveWithNumPyMinimumEigensolver(model: QuadraticProgram):
    exact_mes = NumPyMinimumEigensolver()
    eigen_optimizer = MinimumEigenOptimizer(min_eigen_solver=exact_mes)
    solution = eigen_optimizer.solve(model)
    return solution

def BruteForce(map: Map, teamIndex: int = 0):
    matrix = map.GetDistanceMatrix(teamIndex)
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
    return name_order

def NearestNeighbor(map: Map, teamIndex: int = 0):
    matrix = map.GetDistanceMatrix(teamIndex)
    N = matrix.shape[0]
    visited = [False] * (N - 1)
    order = []
    current_index = 0
    total_distance = 0

    while len(order) < N - 1:
        #find nearest point from current
        min_dist = sys.float_info.max
        min_build_i = -1

        for build_i in range(1, N):
            if visited[build_i - 1]:
                continue
            if min_dist > matrix[current_index, build_i]:
                min_dist = matrix[current_index, build_i]
                min_build_i = build_i

        #append the nearest
        total_distance += min_dist
        current_index = min_build_i
        order.append(min_build_i)
        visited[min_build_i - 1] = True

    print('total distance:', total_distance)
    names = map.GetBuildingNamesList()
    name_order = [names[i - 1] for i in order]

    print('min name order:', name_order)
    return name_order


# def Christofides(map: Map, teamIndex: int = 0):
#     #with Kruskal's for Minimmum Spanning Tree


# '''
# * Generalized name:
#     "(Monotonically?) Increasing Sequential TSP"
# '''