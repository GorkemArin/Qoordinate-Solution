from QoordinateClasses import *
from QQuadraticProblem import QQuadraticProblem
from QOptimizers import *
from QSolutionTranslator import *

def CreateMatrix(N):
    res = [[''] * N for i in range(N)]
    return res

def CreateNewMap():
    buildings = []
    
    buildings.append(Building(Point(12, 12), 20, 'A'))
    buildings.append(Building(Point(20, 4), 4, 'C'))
    buildings.append(Building(Point(5, 0), 3, 'B'))
    buildings.append(Building(Point(15, 7), 7, 'D'))

    teams = [Team(Point(5, 5), 15)]

    myMap = Map(teams, buildings)
    return myMap

if __name__ == '__main__':
    myMap = CreateNewMap()
    print(myMap.GetDistanceMatrix(0))
    print('-' * 20)
    
    problem = QQuadraticProblem(myMap.GetDistanceMatrix(0))
    model = problem.GetQuadraticProblemModel('my problem')
    solution = SolveWithNumPyMinimumEigensolver(model)
    
    print('Optimization Solution:')
    GetListOfOrder(solution, myMap)

    print('Brute Force Solution:')
    BruteForce(myMap)

    #print(solution.prettyprint())
    '''print(type(solution))
    print(solution)
    print('-----------')
    print(solution.variables_dict)
    print(solution.prettyprint())'''




