from QoordinateClasses import *
from QQuadraticProblem import QQuadraticProblem
from QOptimizers import *
from QSolutionTranslator import *
from MapReader import ReadMap

if __name__ == '__main__':
    myMap = ReadMap('test0.map')    
    problem = QQuadraticProblem(myMap.GetDistanceMatrix(0))
    model = problem.GetQuadraticProblemModel('my problem')
    solution = SolveWithNumPyMinimumEigensolver(model)
    
    print('Optimization Solution:')
    GetListOfOrder(solution, myMap)

    print('Brute Force Solution:')
    BruteForce(myMap)