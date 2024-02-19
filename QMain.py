from QoordinateClasses import *
from QQuadraticProblem import QQuadraticProblem
from QOptimizers import *
from QSolutionTranslator import *

from DWaveQuadraticProblem import *
from DWaveSolver import *

from MapReader import ReadMap
from SolutionWriter import WriteSolution

if __name__ == '__main__':
    myMap = ReadMap('Maps\\test3.map')    
    # problem = QQuadraticProblem(myMap.GetDistanceMatrix(0))
    # model = problem.GetQuadraticProblemModel('my problem')
    # solution = SolveWithNumPyMinimumEigensolver(model)
    
    # all = []
    # print('Optimization Solution:')
    #sol = GetListOfOrder(solution, myMap)
    # all.append(sol)
    #WriteSolution(myMap, all, 'Maps/output.sol')

    # print('Brute Force Solution:')
    # NearestNeighbor(myMap, 0)

    problem = DWaveQuadraticProblem(myMap.GetDistanceMatrix(0))
    bqmModel = problem.CreateBQMfromMap('TPS test')
    solution = SolveDWaveProblem(bqmModel, 'hybrid', 'TPS Hybrid Qoordinate')
    GetListOfOrder(solution, myMap)
